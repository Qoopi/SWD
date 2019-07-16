using System;
using RestSharp;

namespace SWD.Utils
{
    public static class RestHelper
    {
        public static IRestResponse SendRequest(Uri url, Method method, string fileName)
        {
            return new RestClient(url).Execute(new RestRequest(fileName, method));
        }
    }
}