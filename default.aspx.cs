using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

public partial class _default : System.Web.UI.Page
{
    #region Variables

    Int32[] h, r;
    Int32 iDealer;
    Boolean bHideTest = false;

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            iDealer = 8;
            ViewState["DealerID"] = iDealer;
        }
        else
            iDealer = Convert.ToInt32(ViewState["DealerID"]);
    }

    #region Page Events

    protected void btnDeal_Click(object sender, EventArgs e)
    {
        // Get a new Shuffled Deck
        List<Card> l = new DeckHelper().GetDeepCopyOfDeck(DBHelper.RawDeck());
        new DeckHelper().Shuffle(l);
        ViewState["GameDeck"] = l;

        // Record the Game Deal
        StringBuilder objStr = new StringBuilder();
        for (Int32 i = 0; i < l.Count; i++)
        {
            objStr.Append(l[i].ID);
            if(i < l.Count - 1)
                objStr.Append("|");
        }
        String sGuid = new DBHelper().RecordNewGame(objStr.ToString(), new CommonFunction().GetIPAddress());
       
        // Deal the Cards
        List<List<Card>> lPlayerHands = new List<List<Card>>();
        List<Card> lTemp;
        for (Int32 i = 0; i < 9; i++)
        {
            lTemp = new List<Card>();
            lTemp.Add(l[i]);
            lTemp.Add(l[i + 9]);
            for (Int32 x = 18; x < 23; x++)
                lTemp.Add(l[x]);
            lPlayerHands.Add(lTemp);
        }

        // Display the Deal
        litPlayers.Text = GetMainDisplay(lPlayerHands);

        btnDeal.Visible = false;
        btnFlop.Visible = true;
        btnTurn.Visible = false;
        btnRiver.Visible = false;
        btnShuffle.Visible = true;

        // For Testinging
        if (!bHideTest)
            DoTest(l);

    }

    protected void btnShuffle_Click(object sender, EventArgs e)
    {
        iDealer = (Int32)ViewState["DealerID"];
        if (iDealer.Equals(8))
        {
            iDealer = 0;
            ViewState["DealerID"] = iDealer;
        }
        else
        {
            iDealer++;
            ViewState["DealerID"] = iDealer;
        }

        btnDeal.Visible = true;
        btnFlop.Visible = false;
        btnTurn.Visible = false;
        btnRiver.Visible = false;
        btnShuffle.Visible = false;

        litPlayers.Text = String.Empty;
        litCommunity.Text = String.Empty;
        litWinners.Text = String.Empty;
        litTest.Text = String.Empty;
        //litTest2.Text = String.Empty;

    }

    protected void btnFlop_Click(object sender, EventArgs e)
    {
        litCommunity.Text = GetCommunityCards(21);
        btnDeal.Visible = false;
        btnFlop.Visible = false;
        btnTurn.Visible = true;
        btnRiver.Visible = false;
        btnShuffle.Visible = true;
    }

    protected void btnTurn_Click(object sender, EventArgs e)
    {
        litCommunity.Text = GetCommunityCards(22);
        btnDeal.Visible = false;
        btnFlop.Visible = false;
        btnTurn.Visible = false;
        btnRiver.Visible = true;
        btnShuffle.Visible = true;
    }

    protected void btnRiver_Click(object sender, EventArgs e)
    {
        List<Card> l = (List<Card>)ViewState["GameDeck"];

        List<List<Card>> lPlayerHands = new List<List<Card>>();
        List<Card> lTemp;
        for (Int32 i = 0; i < 9; i++)
        {
            lTemp = new List<Card>();
            lTemp.Add(l[i]);
            lTemp.Add(l[i + 9]);
            for (Int32 x = 18; x < 23; x++)
                lTemp.Add(l[x]);
            lPlayerHands.Add(lTemp);
        }

        // The Index of the winnign hands from each players hand
        Int32[] iWinIndex = GetPlayersHandWinIndexes(lPlayerHands);

        // Get Table Winner
        h = new Int32[9];
        r = new Int32[9];
        for (Int32 i = 0; i < lPlayerHands.Count; i++)
        {
            h[i] = PokerLib.eval_5hand_fast_jb(GetSubHand(lPlayerHands[i], iWinIndex[i]));
            r[i] = PokerLib.hand_rank_jb(h[i]);
        }

        // Display the Best Hands
        litWinners.Text = GetRiverDisplay(lPlayerHands, iWinIndex);

        litCommunity.Text = GetCommunityCards(23);

        btnDeal.Visible = false;
        btnFlop.Visible = false;
        btnTurn.Visible = false;
        btnRiver.Visible = false;
        btnShuffle.Visible = true;
    }

    #endregion

    #region Helpers

    private List<Card> GetSubHand(List<Card> l, Int32 iIndex)
    {
        List<Card> lSubHand = new List<Card>();
        for (Int32 j = 0; j < 5; j++)
            lSubHand.Add(l[PokerLib.perm7[iIndex, j]]);

        return lSubHand;
    }

    private Int32[] GetPlayersHandWinIndexes(List<List<Card>> l)
    {
        Trace.Write("GetPlayersHandWinIndexes()", "Started");
        Int32[] iWinIndex = new Int32[l.Count];
        for (Int32 x = 0; x < l.Count; x++)
        {
            UInt16[] iHandValues = new UInt16[21];
            List<List<Card>> lSubHands = new List<List<Card>>();
            for (Int32 i = 0; i < 21; i++)
            {
                List<Card> lSubHand = new List<Card>();
                for (Int32 j = 0; j < 5; j++)
                    lSubHand.Add(l[x][PokerLib.perm7[i, j]]);
                lSubHands.Add(lSubHand);
                iHandValues[i] = PokerLib.eval_5hand_fast_jb(lSubHand);
            }
            iWinIndex[x] = iHandValues.ToList().IndexOf(iHandValues.Min());
        }
        Trace.Write("GetPlayersHandWinIndexes()", "Returning");
        return iWinIndex;
    }

    #endregion

    #region Display

    private String GetRiverDisplay(List<List<Card>> l, Int32[] iWIndex)
    {
        // Get Only the Winning Hands of Each Player
        List<List<Card>> lWinners = new List<List<Card>>();
        for (Int32 i = 0; i < l.Count; i++)
            lWinners.Add(GetSubHand(l[i], iWIndex[i]));

        // Get the Winning Value of the Winning Hand. There maybe ties...!
        Int32 iWinValue = h.Min();

        // Loop the results into a string and return it.
        StringBuilder objStr = new StringBuilder();
        objStr.AppendLine("<table border=\"1\" cellpadding=\"4\" cellspacing=\"0\">");

        objStr.AppendLine("  <tr>");
        for (Int32 i = 0; i < lWinners.Count; i++)
        {
            if (h[PokerLib.dealorder[iDealer, i]].Equals(iWinValue))
                objStr.Append("    <th style=\"background-color: #bfb;\">");
            else
                objStr.Append("    <th>");

            if (h[PokerLib.dealorder[iDealer, i]].Equals(iWinValue))
                objStr.Append("WINNER!");
            else
            {
                objStr.Append("Player");
                objStr.Append(" ");
                objStr.Append((i + 1));
            }

            objStr.AppendLine("</th>");
        }
        objStr.AppendLine("  </tr>");

        objStr.AppendLine("  <tr>");
        for (Int32 i = 0; i < 9; i++)
        {
            objStr.Append("    <td align=\"center\">");
            for (Int32 x = 0; x < lWinners[i].Count; x++)
            {
                objStr.Append("<span style=\"color:#");
                objStr.Append(lWinners[PokerLib.dealorder[iDealer, i]][x].Color);
                objStr.Append(";\">");
                objStr.Append(lWinners[PokerLib.dealorder[iDealer, i]][x].Face);
                objStr.Append(lWinners[PokerLib.dealorder[iDealer, i]][x].Suit);
                objStr.Append("</span>");
                if (x < lWinners[i].Count - 1)
                    objStr.Append(" ");
            }
            objStr.AppendLine("</td>");
        }
        objStr.AppendLine("  </tr>");

        objStr.AppendLine("  <tr>");
        for (Int32 i = 0; i < 9; i++)
        {
            if (h[PokerLib.dealorder[iDealer, i]].Equals(iWinValue))
                objStr.Append("    <td align=\"center\" style=\"background-color: #bfb;\">");
            else
                objStr.Append("    <td align=\"center\">");
            if (h[PokerLib.dealorder[iDealer, i]].Equals(iWinValue))
                objStr.Append("<strong>");
            if (h[PokerLib.dealorder[iDealer, i]].Equals(1))
                objStr.Append("Royal Flush");
            else
            {
                if (!h[PokerLib.dealorder[iDealer, i]].Equals(iWinValue))
                    objStr.Append("<em>");
                objStr.Append(GetNameOfHand(r[PokerLib.dealorder[iDealer, i]]));
                if (!h[PokerLib.dealorder[iDealer, i]].Equals(iWinValue))
                    objStr.Append("</em>");
            }
            if (h[PokerLib.dealorder[iDealer, i]].Equals(iWinValue))
                objStr.Append("</strong>");
            objStr.AppendLine("</td>");
        }
        objStr.AppendLine("  </tr>");

        objStr.AppendLine("</table>");

        return objStr.ToString();

    }

    private String GetMainDisplay(List<List<Card>> l)
    {
        StringBuilder objStr = new StringBuilder();
        objStr.AppendLine("<table border=\"1\" cellpadding=\"4\" cellspacing=\"0\">");

        objStr.AppendLine("  <tr>");
        for (Int32 i = 0; i < 9; i++)
        {
            if (iDealer.Equals(0))
            {
                if (i.Equals(8))
                {
                    objStr.Append("    <th style=\"background-color: #bbf;\">");
                    objStr.Append("Dealer");
                }
                else
                {
                    objStr.Append("    <th>");
                    objStr.Append("Player");
                }
            }
            else
            {
                if (iDealer.Equals(i + 1))
                {
                    objStr.Append("    <th style=\"background-color: #bbf;\">");
                    objStr.Append("Dealer");
                }
                else
                {
                    objStr.Append("    <th>");
                    objStr.Append("Player");
                }
            }
            objStr.Append(" ");
            objStr.Append((i + 1));
            objStr.AppendLine("</th>");
        }
        objStr.AppendLine("  </tr>");

        objStr.AppendLine("  <tr>");
        for (Int32 i = 0; i < 9; i++)
        {
            objStr.Append("    <td align=\"center\">");
            for (Int32 x = 0; x < 2; x++)
            {
                objStr.Append("<span style=\"color:#");
                objStr.Append(l[PokerLib.dealorder[iDealer, i]][x].Color);
                objStr.Append(";\">");
                objStr.Append(l[PokerLib.dealorder[iDealer, i]][x].Face);
                objStr.Append(l[PokerLib.dealorder[iDealer, i]][x].Suit);
                objStr.Append("</span>");
                if (x < 1)
                    objStr.Append(" ");
            }
            objStr.AppendLine("</td>");
        }
        objStr.AppendLine("  </tr>");

        objStr.AppendLine("</table>");

        return objStr.ToString();

    }

    private String GetCommunityCards(Int32 iStreet)
    {
        List<Card> l = (List<Card>)ViewState["GameDeck"];
        StringBuilder objStr = new StringBuilder();
        objStr.Append("<p>Community Cards: ");
        for (Int32 i = 18; i < iStreet; i++)
        {
            objStr.Append("<span style=\"color:#");
            objStr.Append(l[i].Color);
            objStr.Append(";\">");
            objStr.Append(l[i].Face);
            objStr.Append(l[i].Suit);
            objStr.Append("</span>");
            if (i < iStreet - 1)
                objStr.Append(", ");
            else
                objStr.Append("</p>");
        }
        return objStr.ToString();
    }

    private String GetNameOfHand(Int32 iRank)
    {
        String sReturn = String.Empty;
        switch (iRank)
        {
            case 1:
                sReturn = ("Straight Flush");
                break;
            case 2:
                sReturn = ("4 of a Kind");
                break;
            case 3:
                sReturn = ("Full House");
                break;
            case 4:
                sReturn = ("Flush");
                break;
            case 5:
                sReturn = ("Straight");
                break;
            case 6:
                sReturn = ("3 of a Kind");
                break;
            case 7:
                sReturn = ("2 Pair");
                break;
            case 8:
                sReturn = ("Pair");
                break;
            case 9:
                sReturn = ("High Card");
                break;
        }
        return sReturn;
    }

    #endregion

    #region Test Display

    private void DoTest(List<Card> l)
    {
        pnlTest.Visible = bHideTest ? false : true;
        List<Card> l2 = new List<Card>();
        for (Int32 i = 0; i < 9; i++)
        {
            l2.Add(l[i]);
            l2.Add(l[i + 9]);
        }

        for (Int32 i = 0; i < l.Count; i++)
        {
            litTest.Text += (i).ToString();
            litTest.Text += ": ";
            litTest.Text += "<span style=\"color:#";
            litTest.Text += l[i].Color;
            litTest.Text += ";\">";
            litTest.Text += l[i].Face;
            litTest.Text += l[i].Suit;
            litTest.Text += "</span>";
            if (i < l.Count - 1)
            {
                litTest.Text += "<br />";
            }
        }

        //for (Int32 i = 0; i < l2.Count; i++)
        //{
        //    litTest2.Text += (i).ToString();
        //    litTest2.Text += ": ";
        //    //litTest2.Text += l2[i].ID.ToString();
        //    //litTest2.Text += ": ";
        //    litTest2.Text += "<span style=\"color:#";
        //    litTest2.Text += l2[i].Color;
        //    litTest2.Text += ";\">";
        //    litTest2.Text += l2[i].Face;
        //    litTest2.Text += l2[i].Suit;
        //    litTest2.Text += "</span>";
        //    if (i < l2.Count - 1)
        //    {
        //        litTest2.Text += "<br />";
        //    }
        //}
    }

    #endregion

}