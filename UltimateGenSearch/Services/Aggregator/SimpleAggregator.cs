using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

using UltimateGenSearch.Models;

namespace UltimateGenSearch.Services.Aggregator
{
    public class SimpleAggregator : IAggregator
    {
        private readonly MD5CryptoServiceProvider hasher = new MD5CryptoServiceProvider();

        public IList<Record> Aggregate(IEnumerable<IList<Record>> records)
        {
            var result = new List<Tuple<string, Record>>();
            foreach (var recordList in records)
            {
                foreach (var record in recordList)
                {
                    result.Add(new Tuple<string, Record>(ComputeHash(record.Source.Link), record));
                }
            }

            return (from r in result orderby r.Item1 ascending select r.Item2).ToList();
        }


        private string ComputeHash(string link)
        {
            byte[] data = hasher.ComputeHash(Encoding.Default.GetBytes(link));
            var sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
    }
}