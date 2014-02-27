using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using UltimateGenSearch.Models;

namespace UltimateGenSearch.Services.Aggregator
{
    public class SimpleAggregator : IAggregator
    {
        public IList<Record> Aggregate(IEnumerable<IList<Record>> records)
        {
            var result = new List<Record>();
            foreach (var recordList in records)
            {
                result.AddRange(recordList);
            }

            return result;
        }
    }
}