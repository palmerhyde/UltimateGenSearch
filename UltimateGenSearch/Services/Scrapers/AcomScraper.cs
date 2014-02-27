namespace UltimateGenSearch.Services.Scrapers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    using HtmlAgilityPack;

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

        private const string SEARCH_TEMPLATE = "http://search.ancestry.co.uk/cgi-bin/sse.dll?gl=ROOT_CATEGORY&rank=1&new=1&so=3&MSAV=1&msT=1&gss=ms_r_f-2_s&gsfn={0}&gsln={1}&msbdy={2}&msypn__ftp={3}&cpxt=1&catBucket=rstp&uidh=000&cp=0";

        public IConnectionFactory Factory { get; set; }

        public ILogin Login { get; set; }

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

        public IList<Record> Search(Query query, int pages)
        {
            var results = new List<Record>();

            var firstName = HttpUtility.UrlEncode(query.Name.Split(' ').First());
            var lastName = HttpUtility.UrlEncode(query.Name.Split(' ').Last());
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

                    var resultRows = doc.DocumentNode.SelectNodes("//table[@id='gsresults']/tbody//tr[@class='tblrow record']");

                    foreach (var row in resultRows)
                    {
                        var record = new Record();

                        var sourceCell = row.ChildNodes[1];

                        var sourceName = sourceCell.SelectSingleNode("span[@class='srchFoundCat']").InnerText;
                        var sourceLink = sourceCell.SelectSingleNode("a").GetAttributeValue("href", "");
                        
                        var recordCell = row.ChildNodes[2];

                        var recordNames = recordCell.SelectSingleNode("table/tbody[0]/span[@class='srchSelfName']/span[@class='srchMatch']");
                        record.FirstName = recordNames.FirstChild.InnerText;
                        record.LastName = recordNames.LastChild.InnerText;
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