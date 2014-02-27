namespace UltimateGenSearch.Services.Scrapers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    using HtmlAgilityPack;

    using Microsoft.Ajax.Utilities;

    using UltimateGenSearch.Models;
    using UltimateGenSearch.Services.Connections;
    using UltimateGenSearch.Services.Login;

    public class AcomScraper : IScraper
    {
        /*
         * {0} FirstName
         * {1} LastName
         * {2} Date
         * {3} Place */

        /// <summary>
        /// The search template
        /// </summary>
        private const string SEARCH_TEMPLATE = "http://search.ancestry.co.uk/cgi-bin/sse.dll?gl=ROOT_CATEGORY&rank=1&new=1&so=3&MSAV=1&msT=1&gss=ms_r_f-2_s&gsfn={0}&gsln={1}&msbdy={2}&msypn__ftp={3}&cpxt=1&catBucket=rstp&uidh=000&cp=0";

        /// <summary>
        /// Gets or sets the factory.
        /// </summary>
        /// <value>
        /// The factory.
        /// </value>
        public IConnectionFactory Factory { get; set; }

        /// <summary>
        /// Gets or sets the login.
        /// </summary>
        /// <value>
        /// The login.
        /// </value>
        public ILogin Login { get; set; }

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
        public AcomScraper(IConnectionFactory factory, ILogin login)
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

        /// <summary>
        /// Searches the specified query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="pages">The pages.</param>
        /// <returns></returns>
        /// <exception cref="System.ApplicationException">Ancestry.com returned an error</exception>
        public IList<Record> Search(Query query, int pages)
        {
            var results = new List<Record>();

            var flNames = GetFirstAndLastNames(query.Name);

            var firstName = HttpUtility.UrlEncode(flNames[0]);
            var lastName = HttpUtility.UrlEncode(flNames[1]);
            var date = HttpUtility.UrlEncode(query.Date);
            var place = HttpUtility.UrlEncode(query.Place);


            var queryString = string.Format(SEARCH_TEMPLATE, firstName, lastName, date, place);

            using (var client = this.Factory.CreateClient(this.Login))
            {
                var response = client.GetAsync(queryString).Result;
                if (response.IsSuccessStatusCode)
                {
                    var doc = new HtmlDocument();
                    doc.LoadHtml(response.Content.ReadAsStringAsync().Result);

                    var resultRows = doc.DocumentNode.SelectNodes("//table[@id='gsresults']//tr[@class='tblrow record']");

                    foreach (var row in resultRows)
                    {
                        var record = new Record();

                        var sourceCell = row.SelectSingleNode("./td[2]");

                        var sourceName = sourceCell.SelectSingleNode("./span[@class='srchFoundCat']").InnerText;
                        var sourceLink = sourceCell.SelectSingleNode("./a").GetAttributeValue("href", "");

                        var source = new Source() { Link = sourceLink, Name = sourceName };

                        record.Source = source;

                        var recordCell = row.SelectSingleNode("./td[3]");

                        var recordNames = recordCell.SelectSingleNode("./table/tr[1]/td[2]/span[@class='srchSelfName']");

                        if (recordNames != null)
                        {
                            var names = GetFirstAndLastNames(GetTextValue(recordNames.InnerText));
                            record.FirstName = names[0];
                            record.LastName = names[1];
                        }

                        var events = recordCell.SelectNodes("./table/tr[position()>1]");

                        var eventList = new List<Event>();
                        foreach (var evt in events.Where(e => e.ChildNodes.Count() > 1))
                        {
                            var e = new Event { Name = GetTextValue(evt.FirstChild.InnerText, ":") };

                            var eventData = GetTextValue(evt.LastChild.InnerText).Split('-');

                            if (eventData.Length > 0)
                            {
                                if (eventData.Length > 1)
                                {
                                    e.Place = eventData[1];
                                    e.Date = eventData[0];
                                }
                                else
                                {
                                    e.Place = eventData[0];
                                }
                            }
                            eventList.Add(e);
                        }

                        record.Events = eventList;

                        results.Add(record);
                    }
                }
                else
                {
                    throw new ApplicationException("Ancestry.com returned an error");
                }
            }

            return results;
        }


        /// <summary>
        /// Gets the text value from an HTML string
        /// </summary>
        /// <param name="htmlValue">The HTML value.</param>
        /// <param name="stripChars">The characters to strip out from the resulting string.</param>
        /// <returns></returns>
        public string GetTextValue(string htmlValue, params string[] stripChars)
        {
            var result = HttpUtility.HtmlDecode(htmlValue).Trim();
            return stripChars == null ? result : stripChars.Aggregate(result, (current, c) => current.Replace(c, ""));
        }

        /// <summary>
        /// Gets the first and last names.
        /// </summary>
        /// <param name="firstlastname">The firstlastname.</param>
        /// <returns></returns>
        public string[] GetFirstAndLastNames(string firstlastname)
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