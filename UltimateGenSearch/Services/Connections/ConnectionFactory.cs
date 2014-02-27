using System;
using System.Collections.Generic;
using System.Net;

namespace UltimateGenSearch.Services.Connections
{
    using System.Net.Http;

    using UltimateGenSearch.Services.Login;

    public class ConnectionFactory : IConnectionFactory
    {
        public ConnectionFactory()
        {
            
        }

        public HttpClient CreateClient(ILogin login, Dictionary<string, string> cookies)
        {
            // TODO: perform login to a service and return an http client with cookies

            // http://stackoverflow.com/questions/12373738/how-do-i-set-a-cookie-on-httpclients-httprequestmessage
            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = new CookieContainer();
            if (cookies != null)
            {
                foreach (var cookie in cookies)
                {
                    handler.CookieContainer.Add(new Uri("http://familysearch.org"), new Cookie(cookie.Key, cookie.Value));
                }
            }

            return new HttpClient();
        }
    }
}