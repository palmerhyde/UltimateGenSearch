using System.Collections.Generic;

using UltimateGenSearch.Models;

namespace UltimateGenSearch.Services.Scrapers
{
    interface IScraper
    {
        IList<Record> Search(Query query, int pages);
    }
}
