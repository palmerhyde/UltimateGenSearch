using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltimateGenSearch.Models;

namespace UltimateGenSearch.Services
{
    public interface ISearchService
    {
        IList<Record> Search(Query query);
    }
}
