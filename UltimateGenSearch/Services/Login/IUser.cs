namespace UltimateGenSearch.Services.Login
{
    using System.Collections.Generic;

    public interface IUser
    {
        IEnumerable<Account> GetAccounts();
    }
}
