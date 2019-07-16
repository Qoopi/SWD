using System;
using RestSharp;
using static SWD.Utils.RestHelper;

namespace SWD.Selenoid
{
    public static class SelenoidHelper
    {
        public static void DeleteVideo(Uri uri, string fileName)
        {
            SendRequest(uri, Method.DELETE, fileName);
        }
        public static void DeleteLog(Uri uri, string fileName)
        {
            SendRequest(uri, Method.DELETE, fileName);
        }
    }
}