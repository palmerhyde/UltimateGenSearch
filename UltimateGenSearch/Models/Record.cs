using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UltimateGenSearch.Models
{
    public class Record
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
        
        public IList<Event> Events { get; set; }

        public Source Source { get; set; }
    }
}