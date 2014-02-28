namespace UltimateGenSearch.Services
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Web;
    using System.Web.Script.Serialization;

    using Microsoft.Ajax.Utilities;

    using UltimateGenSearch.Services.Login;

    public class AccountService : IAccountService
    {
        private static object lockObject = new Object();

        public AccountService()
        {
            this.EnsureDataStore();
        }

        /// <summary>
        /// Ensures the data store. This will create a JSON file for every account configured in the web.config
        /// </summary>
        private void EnsureDataStore()
        {
            lock (lockObject)
            {
                var types = ConfigurationManager.AppSettings["accountTypes"].Split(';');
                foreach (var t in types)
                {
                    var path = GetAccountPath(t);

                    if (File.Exists(path))
                    {
                        continue;
                    }

                    var a = new Account { Name = t };

                    using (var sw = File.CreateText(path))
                    {
                        var ser = new JavaScriptSerializer();
                        sw.Write(ser.Serialize(a));
                    }
                }
            }
        }

        /// <summary>
        /// Gets the accounts.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Account> GetAccounts()
        {
            lock (lockObject)
            {
                var accounts = new List<Account>();
                var types = ConfigurationManager.AppSettings["accountTypes"].Split(';');
                foreach (var t in types)
                {
                    var path = GetAccountPath(t);

                    if (!File.Exists(path))
                    {
                        continue;
                    }

                    var data = File.ReadAllText(path);
                    var ser = new JavaScriptSerializer();
                    var a = ser.Deserialize<Account>(data);
                    accounts.Add(a);
                }
                return accounts;
            }
        }

        /// <summary>
        /// Gets the account path.
        /// </summary>
        /// <param name="accountName">Name of the account.</param>
        /// <returns></returns>
        private static string GetAccountPath(string accountName)
        {
            return HttpContext.Current.Server.MapPath(string.Format("~/App_Data/{0}.json", accountName));
        }

        /// <summary>
        /// Updates the account.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <returns></returns>
        public bool UpdateAccount(Account account)
        {
            try
            {
                lock (lockObject)
                {
                    var path = GetAccountPath(account.Name);
                    if (File.Exists(path))
                    {
                        using (var sw = File.CreateText(path))
                        {
                            var ser = new JavaScriptSerializer();
                            sw.Write(ser.Serialize(account));
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}