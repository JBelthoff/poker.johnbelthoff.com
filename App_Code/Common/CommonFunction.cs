using System;

/// <summary>
/// Summary description for CommonFunction
/// </summary>
public class CommonFunction
{
    public String GetIPAddress()
    {
        string sIPAddress;
        sIPAddress = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        if (sIPAddress == null)
        {
            sIPAddress = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        }
        return sIPAddress;
    }
    
}