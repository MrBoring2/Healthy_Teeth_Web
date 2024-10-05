using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class ServiceViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public float Price { get; set; }
        public int SpecializationId { get; set; }
    }
}
