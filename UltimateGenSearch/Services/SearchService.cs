using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UltimateGenSearch.Models;
using UltimateGenSearch.Services.Aggregator;
using UltimateGenSearch.Services.Scrapers;

namespace UltimateGenSearch.Services
{
    public class SearchService : ISearchService
    {
        public SearchService(IScraper[] scrapers, IAggregator aggregator)
        {
            if (scrapers == null)
            {
                throw new ArgumentNullException("scrapers");
            }
            if (aggregator == null)
            {
                throw new ArgumentNullException("aggregator");
            }

            this._aggregator = aggregator;
            this._scrapers = scrapers;
        }

        private readonly IAggregator _aggregator;
        private readonly IScraper[] _scrapers;
        private const int PAGES = 1;

        public IList<Record> Search(Query query)
        {
            IList<IList<Record>> results = new List<IList<Record>>();

            foreach (var scraper in _scrapers)
            {
                results.Add(scraper.Search(query, PAGES));
            }

            return _aggregator.Aggregate(results);
        }
    }
}