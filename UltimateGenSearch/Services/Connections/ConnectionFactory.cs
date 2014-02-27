namespace UltimateGenSearch.Services.Connections
{
    using System.Net.Http;

    using UltimateGenSearch.Services.Login;

    public class ConnectionFactory : IConnectionFactory
    {
        public ConnectionFactory()
        {
            
        }

        public HttpClient CreateClient(ILogin login)
        {
            // TODO: perform login to a service and return an http client with cookies

            // http://stackoverflow.com/questions/12373738/how-do-i-set-a-cookie-on-httpclients-httprequestmessage

            return new HttpClient();
        }
    }
}