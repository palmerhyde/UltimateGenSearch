namespace UltimateGenSearch.Services.Connections
{
    using System.Net;
    using System.Net.Http;

    using UltimateGenSearch.Services.Login;

    public class ConnectionFactory : IConnectionFactory
    {
        public HttpClient CreateClient(ILogin login)
        {
            var cookieContainer = new CookieContainer();
            if (login != null)
            {
                login.Login(cookieContainer);
            }

            var handler = new HttpClientHandler() { CookieContainer = cookieContainer };

            return new HttpClient(handler);
        }
    }
}