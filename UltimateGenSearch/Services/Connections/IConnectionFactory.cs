using System;
using System.Collections.Generic;
using System.Net;

namespace UltimateGenSearch.Services.Connections
{
    using System.Net.Http;

    using UltimateGenSearch.Services.Login;

    public interface IConnectionFactory
    {
        HttpClient CreateClient(ILogin login, Dictionary<Uri, IList<Cookie>> cookies);
    }
}