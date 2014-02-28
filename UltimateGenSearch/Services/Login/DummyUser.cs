namespace UltimateGenSearch.Services.Login
{
    using System;
    using System.Collections.Generic;

    public class DummyUser : IUser
    {
        public IEnumerable<Account> GetAccounts()
        {
            return new[] { new Account() { Name = Enum.GetName(typeof(VendorNames), VendorNames.Ancestry), Username = "ClaudioDuranti", Password = "ancestrytemp" } };
        }
    }
}