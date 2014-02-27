using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UltimateGenSearch.Models
{
    public class Record
    {
        public Record()
        {
            Events = new List<Event>();
        }

        public string Vendor { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        
        public IList<Event> Events { get; set; }

        public Source Source { get; set; }
    }
}