using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Specialization
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public required string Title { get; set; }
        public virtual List<Employee>? Employees { get; set; }
        public virtual List<Service>? Services { get; set; }
    }
}
