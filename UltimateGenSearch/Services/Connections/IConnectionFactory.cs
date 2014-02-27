namespace UltimateGenSearch.Services.Connections
{
    using System.Net.Http;

    using UltimateGenSearch.Services.Login;

    public interface IConnectionFactory
    {
        HttpClient CreateClient(ILogin login);
    }
}