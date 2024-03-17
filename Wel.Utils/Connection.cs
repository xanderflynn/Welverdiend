using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Wel.Utils
{

    public static class Connection
    {
        public static bool CheckInternetConnection()
        {
            string CheckUrl = "http://www.google.co.za";

            try
            {
                HttpWebRequest iNetRequest = (HttpWebRequest)WebRequest.Create(CheckUrl);

                iNetRequest.Timeout = 45000;

                WebResponse iNetResponse = iNetRequest.GetResponse();

                // Console.WriteLine ("...connection established..." + iNetRequest.ToString ());
                iNetResponse.Close();

                return true;

            }
            catch (WebException ex)
            {
                // Console.WriteLine (".....no connection..." + ex.ToString ());

                return false;
            }
        }
    }
}

