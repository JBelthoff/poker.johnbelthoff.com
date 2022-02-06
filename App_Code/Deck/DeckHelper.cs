using System;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;
/// <summary>
/// Summary description for DeckHelper
/// </summary>
public class DeckHelper
{
    public void Shuffle(List<Card> l)
    {
        System.Web.HttpContext.Current.Trace.Write("DeckHelper.Shuffle()", "Started");
        var provider = new RNGCryptoServiceProvider();
        int n = l.Count;
        while (n > 1)
        {
            var box = new byte[1];
            do provider.GetBytes(box);
            while (!(box[0] < n * (Byte.MaxValue / n)));
            var k = (box[0] % n);
            n--;
            var value = l[k];
            l[k] = l[n];
            l[n] = value;
        }

        System.Web.HttpContext.Current.Trace.Write("DeckHelper.Shuffle()", "Ended");
        //return l;
    }

    public String AssembleDeckIDsIntoString(List<Card> l)
    {
        StringBuilder objStr = new StringBuilder();
        for (Int32 i = 0; i < l.Count; i++)
        {
            objStr.Append(l[i].ID);
            if (i < l.Count - 1)
                objStr.Append("|");
        }
        return objStr.ToString();

    }

    public List<Card> GetDeepCopyOfDeck(List<Card> l)
    {
        List<Card> lCopy = new List<Card>();
        for (Int32 i = 0; i < l.Count; i++)
            lCopy.Add(l[i]);
        return lCopy;
    }
}