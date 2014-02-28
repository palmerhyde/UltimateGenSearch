using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UltimateGenSearch.Services.Login
{
    public class User : IUser
    {
        public IAccountService AccountService { get; set; }

        public User(IAccountService accountService)
        {
            if (accountService == null)
            {
                throw new ArgumentNullException("accountService");
            }

            this.AccountService = accountService;
        }

        public IEnumerable<Account> GetAccounts()
        {
            return AccountService.GetAccounts();
        }
    }
}