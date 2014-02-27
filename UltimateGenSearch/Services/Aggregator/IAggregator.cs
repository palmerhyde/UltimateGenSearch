using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UltimateGenSearch.Models;

namespace UltimateGenSearch.Services.Aggregator
{
    public interface IAggregator
    {
        IList<Record> Aggregate(IEnumerable<IList<Record>> records);
    }
}
