using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class VisitStatus
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }
        public virtual List<Visit>? Visits { get; set; }
    }
}
