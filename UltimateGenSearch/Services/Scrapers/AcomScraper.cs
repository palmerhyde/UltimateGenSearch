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

    public class AcomScraper : BaseScraper
    {
        /*
         * {0} FirstName
         * {1} LastName
         * {2} Date
         * {3} Place */

        /// <summary>
        /// The search template
        /// </summary>
        private const string SEARCH_TEMPLATE = "http://search.ancestry.com/cgi-bin/sse.dll?gl=ROOT_CATEGORY&rank=1&new=1&so=3&MSAV=1&msT=1&gss=ms_r_f-2_s&gsfn={0}&gsln={1}&msbdy={2}&msypn__ftp={3}&cpxt=1&catBucket=rstp&uidh=000&cp=0";


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
        public AcomScraper(IConnectionFactory factory, ILogin login) : base(factory, login)
        {
        
        }

        /// <summary>
        /// Searches the specified query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="pages">The pages.</param>
        /// <returns></returns>
        /// <exception cref="System.ApplicationException">Ancestry.com returned an error</exception>
        public override IList<Record> Search(Query query, int pages)
        {
            var results = new List<Record>();

            var flNames = GetFirstAndLastNames(query.Name);

            var firstName = HttpUtility.UrlEncode(flNames[0]);
            var lastName = HttpUtility.UrlEncode(flNames[1]);
            var date = HttpUtility.UrlEncode(query.Date);
            var place = HttpUtility.UrlEncode(query.Place);


            var queryString = string.Format(SEARCH_TEMPLATE, firstName, lastName, date, place);

            using (var client = this.Factory.CreateClient(this.Login, null))
            {
                var response = client.GetAsync(queryString).Result;
                if (response.IsSuccessStatusCode)
                {
                    var doc = new HtmlDocument();
                    doc.LoadHtml(response.Content.ReadAsStringAsync().Result);

                    var resultRows = doc.DocumentNode.SelectNodes("//table[@id='gsresults']//tr[@class='tblrow record']");

                    foreach (var row in resultRows)
                    {
                        var record = new Record() { Vendor = "Ancestry" };

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
                            record.Events.Add(e);
                        }

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
    }
}