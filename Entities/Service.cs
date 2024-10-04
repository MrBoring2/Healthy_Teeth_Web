using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Service
    {
        public int Id { get; set; }
        [MaxLength(80)]
        [Required]
        public string Title { get; set; }
        [Required]
        public float Price { get; set; }
        [Required]
        public  int SpecializationId { get; set; }
        public virtual Specialization? Specialization { get; set; }
        public virtual List<Visit> Visits { get; set; } = [];
        public virtual List<ServiceToVisit> ServiceToVisits { get; set; } = [];
    }
}
