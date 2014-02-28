using System;
using System.Collections.Generic;
using System.Net;

namespace UltimateGenSearch.Services.Connections
{
    using System.Net;
    using System.Net.Http;

    using UltimateGenSearch.Services.Login;

    public class ConnectionFactory : IConnectionFactory
    {

        public ConnectionFactory()
        {
            
        }

        public HttpClient CreateClient(ILogin login, Dictionary<Uri, IList<Cookie>> cookies)
        {
            var cookieContainer = new CookieContainer();
            if (login != null)
            {
                login.Login(cookieContainer);
            }


            // http://stackoverflow.com/questions/12373738/how-do-i-set-a-cookie-on-httpclients-httprequestmessage
            var handler = new HttpClientHandler() { CookieContainer = cookieContainer };
           
            if (cookies != null)
            {
                foreach (var k in cookies)
                {
                    foreach (var cookie in k.Value)
                    {
                        handler.CookieContainer.Add(k.Key, cookie);
                    }
                }
               
            }
            return new HttpClient(handler);
        }
    }
}