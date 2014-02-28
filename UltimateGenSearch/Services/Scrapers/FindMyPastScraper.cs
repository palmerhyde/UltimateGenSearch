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
    public class FindMyPastScraper : BaseScraper
    {
        /*
         * {0} FirstName
         * {1} LastName
         * {2} Place
         * {3} Date */

        private const string SEARCH_TEMPLATE = "http://search.findmypast.com/search/world-records?firstname={0}&lastname={1}&keywordsplace={2}&yearofbirth={3}&yearofbirth_offset=2";

        private const string RECORD_DOMAIN = "http://search.findmypast.com";

     
        public FindMyPastScraper(IConnectionFactory factory, ILogin login) : base(factory, login)
        {
        
        }

        private readonly  string[] DocumentTypes = new string[]{"IMG", "PDF", "TXT" };

        public override IList<Record> Search(Query query, int pages)
        {
            var results = new List<Record>();

            var names = this.GetFirstAndLastNames(query.Name);

            var firstName = names[0];
            var lastName = names[1];
            var date = HttpUtility.UrlEncode(query.Date);
            var place = HttpUtility.UrlEncode(query.Place);


            var queryString = string.Format(SEARCH_TEMPLATE, firstName, lastName, place, date);

            using (var client = this.Factory.CreateClient(this.Login, null))
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
                        var record = new Record() { Vendor = "FindMyPast" }; 

                        var cells = row.SelectNodes("./td");
                        if (cells.Count > 7)
                        {

                            var ln = GetTextValue(cells[0].InnerText);
                            var fn = GetTextValue(cells[1].InnerText);

                            var born = GetTextValue(cells[2].InnerText);
                            var died = GetTextValue(cells[3].InnerText);


                            var sourceName = GetTextValue(cells[5].InnerText);
                            string sourceLink = null;


                            HtmlNode sourceCell = null;
                            foreach (var type in DocumentTypes)
                            {
                                sourceCell = cells[7].SelectSingleNode(string.Format(".//div[@class='{0}']/a", type));
                                if (sourceCell != null) 
                                    break;
                            }


                            if (sourceCell != null)
                            {
                                sourceLink = RECORD_DOMAIN + this.GetTextValue(sourceCell.Attributes["href"].Value);
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


    }
}