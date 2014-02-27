namespace UltimateGenSearch.Services.Login
{
    using System.Collections.Generic;
    using System.Net;

    public interface ILogin
    {
        /// <summary>
        /// Logins the specified user.
        /// </summary>
        /// <returns></returns>
        void Login(CookieContainer cookies);
    }
}