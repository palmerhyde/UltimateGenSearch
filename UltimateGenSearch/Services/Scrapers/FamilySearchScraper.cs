using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Web;
using System.Xml;

using HtmlAgilityPack;

using UltimateGenSearch.Models;
using UltimateGenSearch.Services.Connections;
using UltimateGenSearch.Services.Login;

namespace UltimateGenSearch.Services.Scrapers
{
    // 
    public class FamilySearchScraper : BaseScraper
    {
        /*
         * {0} FirstName
         * {1} LastName
         * {2} Place
         * {3} Date */

        private const string SEARCH_TEMPLATE = "http://familysearch.org/searchapi/search?count=20&query=%2Bgivenname%3A{0}~%20%2Bsurname%3A{1}~%20%2Bany_place%3A{2}~%20%2Bany_year%3A{3}-{3}~";

        private const string REFERRER_TEMPLATE =
            "https://familysearch.org/search/record/results?count=20&query=%2Bgivenname%3A{0}~%20%2Bsurname%3A{1}~%20%2Bany_place%3A{2}~%20%2Bany_year%3A{3}-{3}~";

        public FamilySearchScraper(IConnectionFactory factory, ILogin login)
            : base(factory, login)
        {

        }

        public override IList<Record> Search(Query query, int pages)
        {
            var results = new List<Record>();

            var names = this.GetFirstAndLastNames(query.Name);

            var firstName = names[0];
            var lastName = names[1];
            var date = HttpUtility.UrlEncode(query.Date);
            var place = HttpUtility.UrlEncode(query.Place);


            var queryString = string.Format(SEARCH_TEMPLATE, firstName, lastName, place, date);
            var referrer = string.Format(REFERRER_TEMPLATE, firstName, lastName, place, date);

            var cookieList = new List<Cookie>()
                                                 {
                                                   new Cookie("ADRUM", "s=1393547433031&r=https%3A%2F%2Ffamilysearch.org%2Fsearch%2Frecord%2Fresults%3Fcount%3D20%26query%3D%252Bgivenname%253Ajohn~%2520%252Bsurname%253Akennedy~%2520%252Bany_place%253Aboston~%2520%252Bany_year%253A1917-1917~"),
                                                new Cookie("__utma", "17181222.323854518.1393528790.1393537698.1393544642.3"),
                                                new Cookie("__utmc", "17181222"),
                                                new Cookie("__utmz", "17181222.1393544642.3.2.utmcsr=localhost:50985|utmccn=(referral)|utmcmd=referral|utmcct=/"),
                                                new Cookie("aam_uuid", "28007998870696928524186830345581174031"),
                                                new Cookie("fs-highconf", "false%24%24USYSA2FAEFE675"),
                                                new Cookie("fs_ex_home", "%7B%22stamp%22%3A%22b429891e2da67342d26c9b6d8b6043d4%22%2C%22bucket%22%3A61%2C%22features%22%3A%7B%22fanChartSlideEx%22%3A1%2C%22loggedInHomeEx%22%3A0%2C%22myFamilyBookletEx%22%3A1%2C%22mBoxHomeBannersEx%22%3A1%2C%22threeMBoxEx%22%3A1%2C%22clickTaleHomeOnlyEx%22%3A0%2C%22boomrRum%22%3Atrue%7D%2C%22dirtyFeatures%22%3A%5B%5D%7D"),
                                                new Cookie("fs_ex_search", "%7B%22stamp%22%3A%2297b497d00b06e8aa83e94e4ce4464766%22%2C%22bucket%22%3A61%2C%22features%22%3A%7B%22allCollectionsTest%22%3A0%2C%22oneCallThatsAll%22%3A0%2C%22detailsExpansion%22%3A0%2C%22hrDetailsComponent%22%3A1%2C%22pushState%22%3A1%2C%22responsiveMobile%22%3A1%2C%22catalogOclcLink%22%3A1%2C%22catalogSubjectColumn%22%3A1%2C%22SourceWalker%22%3A0%2C%22SourceWalkerExt%22%3A0%2C%22SourceWalkerOrg%22%3A0%2C%22SourceWalkerDetach%22%3A0%2C%22exImageLocales%22%3A0%2C%22newFeatureAlert%22%3A0%2C%22alternateCDSprod%22%3A0%2C%22alternateCDSstage%22%3A0%2C%22alternateCDSstageData%22%3A0%2C%22addSourceBoxTitle%22%3A1%7D%2C%22dirtyFeatures%22%3A%5B%5D%7D"),
                                                new Cookie("fs_experiments", "%7B%22bucket%22%3A61%2C%22apps%22%3A%7B%22shared-ui%22%3A%7B%22stamp%22%3A%22c5a4b23e53c17c2103a07b274b6e33a9%22%2C%22bucket%22%3A61%2C%22features%22%3A%7B%22wwwRedirect%22%3A0%2C%22templeLink%22%3A1%2C%22showVolunteerExperiment%22%3A0%2C%22accelleratorDropDownExperiment%22%3A0%2C%22helpMenuExperiment%22%3A1%2C%22myCasesExperiment%22%3A1%2C%22chromeFrameExperiment%22%3A0%2C%22mobileDrawerExperiment%22%3A0%2C%22accelleratorDropDownTnTExperiment%22%3A1%2C%22feedbackExperiment%22%3A1%2C%22feedbackInHeaderExperiment%22%3A0%2C%22salesForceExperiment%22%3A1%2C%22bootstrap232%22%3A0%2C%22bootstrap3%22%3A0%2C%22jQuery191%22%3A0%2C%22jQueryMigrate%22%3A0%2C%22jQueryMigrateDebug%22%3A0%2C%22appDynamicsEx%22%3A1%2C%22parterProductsEx%22%3A0%2C%22newHeaderFooterEx%22%3A1%2C%22adminRoleEx%22%3A0%2C%22dropdownNavEx%22%3A1%2C%22mBoxAlwaysOnEx%22%3A1%2C%22registerLinkEx%22%3A0%2C%22injectFiveTwo%22%3A0%2C%22newRelCalEndpointEx%22%3A1%2C%22frMemoriesEx%22%3A1%2C%22netPromoterEx%22%3A1%2C%22boomrRum%22%3Afalse%7D%2C%22dirtyFeatures%22%3A%5B%5D%7D%7D%7D"),
                                                new Cookie("fs_search_history", HttpUtility.UrlEncode(referrer)),
                                                new Cookie("fssessionid", "USYSA2FAEFE6758DDE7BB3223A7CA26F1606_idses-prod02.a.fsglobal.net"),
                                                new Cookie("mbox", "PC#1393528788905-476309.21_16#1394757035|check#true#1393547495|session#1393547434090-842093#1393549295"),
                                                new Cookie("optimizelyBuckets", "%7B%22589211729%22%3A%22609190609%22%2C%22592710112%22%3A%22597080020%22%2C%22593911883%22%3A%22589946296%22%7D"),
                                                new Cookie("optimizelyEndUserId", "oeu1393528795727r0.17253762506879866"),
                                                new Cookie("optimizelySegments", "%7B%22536411379%22%3A%22none%22%2C%22536691475%22%3A%22direct%22%2C%22544410221%22%3A%22gc%22%2C%22549330157%22%3A%22false%22%7D"),
                                                new Cookie("s_cc", "TRUE"),
                                                new Cookie("s_fid", "048F4749832F6518-0E26E2010A7359F7"),
                                                new Cookie("s_ppv", "FamilySearch%253A%2520Search%253A%2520Records%2520%253A%2520Results"),
                                                new Cookie("s_sq", "%5B%5BB%5D%5D"),
                                                new Cookie("s_vi", "[CS]v1|2987C7EB85012E98-600001322001F1E7[CE]")
                                                 };

            var cookies = new Dictionary<Uri, IList<Cookie>>()
                          {
                              {new Uri("http://familysearch.org"), cookieList }
                          };


            using (var client = this.Factory.CreateClient(this.Login, cookies))
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/32.0.1700.107 Safari/537.36");
                client.DefaultRequestHeaders.Referrer = new Uri(referrer);
                client.DefaultRequestHeaders.Add("ADRUM", "isAjax:true");


                var response = client.GetAsync(queryString).Result;
                if (response.IsSuccessStatusCode)
                {

                    var doc = new XmlDocument();
                    doc.LoadXml(response.Content.ReadAsStringAsync().Result);

                    XmlNamespaceManager nsMgr = new XmlNamespaceManager(doc.NameTable);
                    nsMgr.AddNamespace("dc", "http://purl.org/dc/terms/");



                    var resultRows =
                        doc.SelectNodes(
                            "//personHit ");

                    foreach (XmlNode row in resultRows)
                    {
                        var person = row.SelectSingleNode("./person");

                        var record = new Record() { Vendor = "FamilySearch" };

                        var name = row.SelectSingleNode("./person/name");
                        var firstLast = this.GetFirstAndLastNames(name.InnerText);
                        record.FirstName = firstLast[0];
                        record.LastName = firstLast[1];

                        var events = row.SelectNodes("./person/event");

                        foreach (XmlNode evt in events)
                        {
                            var e = new Event();

                            if (evt.Attributes["date"] != null)
                                e.Date = this.GetTextValue(evt.Attributes["date"].Value);

                            if (evt.Attributes["place"] != null)
                                e.Place = this.GetTextValue(evt.Attributes["place"].Value);

                            if (evt.Attributes["type"] != null)
                                e.Name = this.GetTextValue(evt.Attributes["type"].Value);

                            record.Events.Add(e);
                        }

                        var sourceTitle = row.SelectSingleNode("./person/isPartOf/dc:title", nsMgr);

                        record.Source = new Source();

                        if (person != null)
                        {
                            record.Source.Link = person.Attributes["url"].Value;
                        }

                        if (sourceTitle != null)
                        {
                            record.Source.Name = sourceTitle.InnerText;
                        }

                        results.Add(record);
                    }
                }
                else
                {
                    throw new ApplicationException("FamilySearch.org returned an error");
                }
            }

            return results;
        }


    }
}