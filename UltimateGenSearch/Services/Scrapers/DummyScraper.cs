using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UltimateGenSearch.Models;

namespace UltimateGenSearch.Services.Scrapers
{
    public class DummyScraper : IScraper
    {
        public IList<Models.Record> Search(Models.Query query, int pages)
        {
            return new List<Record>() { 
                new Record(){ 
                    FirstName = "Shane", 
                    LastName = "Burke", 
                    Events = new List<Event>(){ new Event(){Name = "Birth", Date = "26/07/1988" }},
                    Source = new Source(){ Name = "Google", Link = "http://www.google.com"}
                },
                new Record(){ 
                    FirstName = "Shane", 
                    LastName = "Burke", 
                    Events = new List<Event>(){ new Event(){Name = "Birth", Date = "26 Jul 1988" }},
                    Source = new Source(){ Name = "Ancestry", Link = "http://www.ancestry.com"}
                },
            };
        }
    }
}