namespace UltimateGenSearch.Services.Login
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;

    public class AcomLogin : ILogin
    {
        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        public IUser User { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AcomLogin"/> class.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <exception cref="System.ArgumentNullException">user</exception>
        public AcomLogin(IUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            this.User = user;
        }

        public void Login(CookieContainer cookies)
        {
            var accountName = Enum.GetName(typeof(VendorNames), VendorNames.Ancestry);

            if (accountName == null)
            {
                return;
            }

            var account = this.User.GetAccounts().FirstOrDefault(a => a.Name == accountName);

            if (account == null)
            {
                return;
            }

            var baseAddress = new Uri("https://secure.ancestry.com/");

            using (var handler = new HttpClientHandler() { CookieContainer = cookies })
            {
                using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
                {
                    var content =
                        new FormUrlEncodedContent(
                            new[] { new KeyValuePair<string, string>("username", account.Username), new KeyValuePair<string, string>("password", account.Password), });

                    // perfoming login. This will fill the cookie container with the cookis for authentication
                    var result = client.PostAsync("/login?ti.si=0&ti=0", content).Result;

                    //var uri = new Uri("http://ancestry.com");

                    //cookies.GetCookies(uri);
                }
            }
        }
    }
}