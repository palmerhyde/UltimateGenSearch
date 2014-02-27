using System.Collections.Generic;

namespace UltimateGenSearch.Services.Connections
{
    using System.Net.Http;

    using UltimateGenSearch.Services.Login;

    public interface IConnectionFactory
    {
        HttpClient CreateClient(ILogin login, Dictionary<string, string> cookies);
    }
}