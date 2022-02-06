using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;


/// <summary>
/// Summary description for DBHelper
/// </summary>
public class DBHelper
{
    public static List<Card> RawDeck()
    {
        System.Web.HttpContext.Current.Trace.Write("DBHelper.RawDeck()", "Start");

        // Create a List of Cards
        List<Card> l = new List<Card>();

        if (System.Web.HttpContext.Current.Cache["DBHelper.RawDeck()"] == null)
        {
            // Create Instance of Connection and Command Object
            SqlConnection objConn = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString);
            SqlCommand objCmd = new SqlCommand("dbo.GameDeck_GetRawDeck", objConn);
            SqlDataReader objRdr;
            Card c;

            // Mark the Command as a SPROC
            objCmd.CommandType = CommandType.StoredProcedure;

            // Return Value
            SqlParameter ParamReturn = new SqlParameter("@Return", SqlDbType.Int);
            ParamReturn.Direction = ParameterDirection.ReturnValue;
            objCmd.Parameters.Add(ParamReturn);

            objConn.Open();
            objRdr = objCmd.ExecuteReader();
            while (objRdr.Read())
            {
                c = new Card();
                c.ID = Convert.ToInt32(objRdr["ID"]);
                c.Color = objRdr["Color"].ToString();
                c.Face = objRdr["Face"].ToString();
                c.Suit = objRdr["Suit"].ToString();
                c.Value = Convert.ToInt32(objRdr["Value"]);
                l.Add(c);

            }
            objRdr.Close();
            objConn.Close();

            System.Web.HttpContext.Current.Cache.Insert("DBHelper.RawDeck()", l, null, DateTime.Now.AddHours(24), TimeSpan.Zero);
            System.Web.HttpContext.Current.Trace.Warn("DBHelper.RawDeck()", "Returning from DBase");

        }
        else
        {
            l = (List<Card>)System.Web.HttpContext.Current.Cache["DBHelper.RawDeck()"];
            System.Web.HttpContext.Current.Trace.Write("DBHelper.RawDeck()", "Returning from Cache");
        }

        return l;

    }

    public String RecordNewGame(String sArray, String sIP)
    {
        System.Web.HttpContext.Current.Trace.Write("DBHelper.RecordNewGame()", "Start");

        // Create Instance of Connection and Command Object
        SqlConnection objConn = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString);
        SqlCommand objCmd = new SqlCommand("dbo.Game_InsertNewGame2", objConn);

        // Mark the Command as a SPROC
        objCmd.CommandType = CommandType.StoredProcedure;

        // Return Value
        SqlParameter ParamReturn = new SqlParameter("@Return", SqlDbType.Int);
        ParamReturn.Direction = ParameterDirection.ReturnValue;
        objCmd.Parameters.Add(ParamReturn);

        // Input Params
        SqlParameter ParamCreateIP = new SqlParameter("@CreateIP", SqlDbType.VarChar, 100);
        ParamCreateIP.Direction = ParameterDirection.Input;
        ParamCreateIP.Value = sIP;
        objCmd.Parameters.Add(ParamCreateIP);

        SqlParameter ParamArray = new SqlParameter("@Array", SqlDbType.VarChar, 8000);
        ParamArray.Direction = ParameterDirection.Input;
        ParamArray.Value = sArray;
        objCmd.Parameters.Add(ParamArray);

        // Output Params
        SqlParameter ParamGameID = new SqlParameter("@GameID", SqlDbType.UniqueIdentifier);
        ParamGameID.Direction = ParameterDirection.Output;
        objCmd.Parameters.Add(ParamGameID);

        objConn.Open();
        objCmd.ExecuteNonQuery();
        objConn.Close();

        System.Web.HttpContext.Current.Trace.Write("DBHelper.RecordNewGame()", "Returning");
        return ParamGameID.Value.ToString();

    }
}
