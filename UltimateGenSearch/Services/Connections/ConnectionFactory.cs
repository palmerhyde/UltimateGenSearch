using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

using UltimateGenSearch.Services.Login;

namespace UltimateGenSearch.Services
{
    public static class ConnectionFactory
    {
        public static HttpClient CreateClient(ILogin login)
        {
            // TODO: perform login to a service and return an http client with cookies

            // http://stackoverflow.com/questions/12373738/how-do-i-set-a-cookie-on-httpclients-httprequestmessage

            return new HttpClient();
        }
    }
}