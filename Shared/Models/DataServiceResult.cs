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
        public DataServiceResult(IEnumerable<T> result, int count)
        {
            Result = result;
            Count = count;
        }

        public IEnumerable<T> Result { get; set; }
        public int Count { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }
        public string Message { get; set; } 
    }
}
