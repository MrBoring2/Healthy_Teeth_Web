using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO
{
    public class ServiceToVisitDTO
    {
        public int VisitId { get; set; }
        public VisitDTO? Visit { get; set; }
        public int ServiceId { get; set; }
        public ServiceDTO? Service { get; set; }
        public required int Count { get; set; }
    }
}
