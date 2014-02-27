using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using HtmlAgilityPack;

using UltimateGenSearch.Models;
using UltimateGenSearch.Services.Connections;
using UltimateGenSearch.Services.Login;

namespace UltimateGenSearch.Services.Scrapers
{
    public class FindMyPastScraper : IScraper
    {
        /*
         * {0} FirstName
         * {1} LastName
         * {2} Place
         * {3} Date */

        private const string SEARCH_TEMPLATE = "http://search.findmypast.com/search/world-records?firstname={0}&lastname={1}&keywordsplace={2}&yearofbirth={3}&yearofbirth_offset=2";

        private const string RECORD_DOMAIN = "http://search.findmypast.com";

        public IConnectionFactory Factory { get; set; }

        public ILogin Login { get; set; }

        public FindMyPastScraper(IConnectionFactory factory, ILogin login)
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

        public IList<Record> Search(Query query, int pages)
        {
            var results = new List<Record>();

            var firstName = HttpUtility.UrlEncode(query.Name.Split(' ').First());
            var lastName = HttpUtility.UrlEncode(query.Name.Split(' ').Last());
            var date = HttpUtility.UrlEncode(query.Date);
            var place = HttpUtility.UrlEncode(query.Place);


            var queryString = string.Format(SEARCH_TEMPLATE, firstName, lastName, place, date);

            using (var client = this.Factory.CreateClient(this.Login))
            {
                var response = client.GetAsync(queryString).Result;
                if (response.IsSuccessStatusCode)
                {
                    var doc = new HtmlDocument();
                    doc.LoadHtml(response.Content.ReadAsStringAsync().Result);

                    var resultRows =
                        doc.DocumentNode.SelectNodes(
                            "//div[@id='activeResultsContainer']/table[@class='Single']/tbody/tr");

                    foreach (var row in resultRows)
                    {
                        var record = new Record();

                        var cells = row.SelectNodes("./td");
                        if (cells.Count > 7)
                        {

                            var ln = CleanupString(cells[0].InnerText);
                            var fn = CleanupString(cells[1].InnerText);

                            var born = CleanupString(cells[2].InnerText);
                            var died = CleanupString(cells[3].InnerText);


                            var sourceName = CleanupString(cells[5].InnerText);
                            string sourceLink = null;

                            var sourceCell = cells[7].SelectSingleNode(".//div[@class='IMG']/a");
                            if (sourceCell == null)
                            {
                                // link to transcription
                                sourceCell = cells[7].SelectSingleNode(".//div[@class='TXT']/a");
                            }

                            if (sourceCell != null)
                            {
                                sourceLink = RECORD_DOMAIN + sourceCell.Attributes["href"].Value;
                            }

                            record.FirstName = fn;
                            record.LastName = ln;

                            if (!string.IsNullOrEmpty(born))
                            {
                                record.Events.Add(new Event() { Name = "Birth", Date = born });
                            }

                            if (!string.IsNullOrEmpty(died))
                            {
                                record.Events.Add(new Event() { Name = "Death", Date = died });
                            }

                            record.Source = new Source() { Name = sourceName, Link = sourceLink };

                            results.Add(record);
                        }
                    }
                }
                else
                {
                    throw new ApplicationException("FindMyPast.com returned an error");
                }
            }

            return results;
        }

        private string CleanupString(string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            return value.Trim();
        }
    }
}