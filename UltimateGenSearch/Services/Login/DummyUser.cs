namespace UltimateGenSearch.Services.Login
{
    using System.Collections.Generic;

    public class DummyUser : IUser
    {
        public IEnumerable<Account> GetAccounts()
        {
            return new[] { new Account() { Name = VendorNames.ACOM, Username = "ClaudioDuranti", Password = "26bambino" } };
        }
    }
}