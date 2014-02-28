namespace UltimateGenSearch.Services.Connections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;

    using UltimateGenSearch.Services.Login;

    public class ConnectionFactory : IConnectionFactory
    {
        private static IDictionary<string, CookieContainer> cachedCookies = new Dictionary<string, CookieContainer>();
        private static object lockObject = new object();

        public HttpClient CreateClient(ILogin login, Dictionary<Uri, IList<Cookie>> cookies)
        {
            var handler = new HttpClientHandler();

            if (login != null)
            {
                CookieContainer cookieContainer;
                string loginName = login.GetType().FullName;
                lock (lockObject)
                {
                    cachedCookies.TryGetValue(loginName, out cookieContainer);
                }

                if (cookieContainer == null)
                {
                    cookieContainer = new CookieContainer();
                    login.Login(cookieContainer);
                    lock (lockObject)
                    {
                        cachedCookies[loginName] = cookieContainer;
                    }

                }
                handler.CookieContainer = cookieContainer;
            }

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