using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class DataServiceResult<T> where T : class
    {
        public DataServiceResult(IEnumerable<T> items, int count)
        {
            Items = items;
            Count = count;

        }

        public IEnumerable<T> Items { get; set; }
        public int Count { get; set; }
    }
}
