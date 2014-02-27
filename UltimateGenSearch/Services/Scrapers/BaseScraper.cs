using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using UltimateGenSearch.Services.Connections;
using UltimateGenSearch.Services.Login;

namespace UltimateGenSearch.Services.Scrapers
{
    public abstract class BaseScraper : IScraper
    {
        
        /// <summary>
        /// Gets or sets the factory.
        /// </summary>
        /// <value>
        /// The factory.
        /// </value>
        protected IConnectionFactory Factory { get; private set; }

        /// <summary>
        /// Gets or sets the login.
        /// </summary>
        /// <value>
        /// The login.
        /// </value>
        protected ILogin Login { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AcomScraper"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="login">The login.</param>
        /// <exception cref="System.ArgumentNullException">
        /// factory
        /// or
        /// login
        /// </exception>
        public BaseScraper(IConnectionFactory factory, ILogin login)
        {
            if (factory == null)
            {
                throw new ArgumentNullException("factory");
            }

            if (login == null)
            {
                throw new ArgumentNullException("login");
            }

            this.Factory = factory;

            this.Login = login;
        }

        public abstract IList<Models.Record> Search(Models.Query query, int pages);

        /// <summary>
        /// Gets the text value from an HTML string
        /// </summary>
        /// <param name="htmlValue">The HTML value.</param>
        /// <param name="stripChars">The characters to strip out from the resulting string.</param>
        /// <returns></returns>
        public virtual string GetTextValue(string htmlValue, params string[] stripChars)
        {
            var result = HttpUtility.HtmlDecode(htmlValue).Trim();
            return stripChars == null ? result : stripChars.Aggregate(result, (current, c) => current.Replace(c, ""));
        }

        /// <summary>
        /// Gets the first and last names.
        /// </summary>
        /// <param name="firstlastname">The firstlastname.</param>
        /// <returns></returns>
        public virtual string[] GetFirstAndLastNames(string firstlastname)
        {
            var lastName = "";
            var firstName = "";
            if (!string.IsNullOrEmpty(firstlastname))
            {
                var pieces = firstlastname.Split(' ');

                if (pieces.Length == 1)
                {
                    firstName = pieces[0];
                }
                else
                {
                    firstName = string.Join(" ", pieces.Take(pieces.Count() - 1));
                    lastName = pieces.Last();
                }
            }
            return new[] { firstName, lastName };
        }
    }
}