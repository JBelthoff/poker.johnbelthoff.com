<%@ Page Language="C#" AutoEventWireup="true" CodeFile="default.aspx.cs" Inherits="_default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Poker Project | Texas Holdem | John Belthoff</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
</head>
<body>
    <form id="form1" runat="server">

<asp:ScriptManager ID="smMain" runat="server" />
<asp:UpdatePanel ID="upMain" runat="server">
<ContentTemplate>

    <h1>Poker Table</h1>
    <h2>Texas Holdem</h2>
    <p style="position:absolute; right:15px;top:15px;">&#10132; <a href="/about.aspx">About Poker Project</a> | <a href="https://github.com/JBelthoff/poker.johnbelthoff.com" target="_blank">See the code</a>! | Back to <a href="https://www.johnbelthoff.com">John Belthoff</a> website</p>

<asp:Literal ID="litPlayers" runat="server" />

<asp:Literal ID="litCommunity" runat="server" />

<asp:Literal ID="litWinners" runat="server" />

<p><asp:Button ID="btnDeal" runat="server" Text="Deal" onclick="btnDeal_Click" /><asp:Button ID="btnFlop" runat="server" Text="Deal Flop" Visible="false" onclick="btnFlop_Click" /><asp:Button ID="btnTurn" runat="server" Text="Deal Turn" Visible="false" onclick="btnTurn_Click" /><asp:Button ID="btnRiver" runat="server" Text="Deal River" Visible="false" onclick="btnRiver_Click" /> <asp:Button ID="btnShuffle" runat="server" Text="Shuffle" Visible="false" onclick="btnShuffle_Click" /></p>
    
<asp:Panel ID="pnlTest" runat="server" Visible="false">
    <hr style="margin: 24px auto 0 auto;" />
    <table>
        <tr>
            <th>Deck Shuffled</th>
        </tr>
        <tr>
            <td valign="top"><asp:Literal ID="litTest" runat="server" /></td>
        </tr>
    </table>
</asp:Panel>

</ContentTemplate>
</asp:UpdatePanel>

    </form>
</body>
</html>
