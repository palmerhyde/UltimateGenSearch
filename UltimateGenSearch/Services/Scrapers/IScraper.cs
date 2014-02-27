using System.Collections.Generic;

using UltimateGenSearch.Models;

namespace UltimateGenSearch.Services.Scrapers
{
    public interface IScraper
    {
        IList<Record> Search(Query query, int pages);
    }
}
