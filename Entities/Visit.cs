using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Visit
    {
        public int Id { get; set; }
        [Required]
        public DateTime VisitDate { get; set; }
        [Required]
        public TimeSpan VisirtTime { get; set; }
        [Required]
        [MaxLength(100)]
        public string VisitPurpose { get; set; }
        public string? VisitDiagnos { get; set; }
        public string? VisitObjectively { get; set; }
        [Required]
        public int EmployeeId { get; set; }
        [Required]
        public int PatientId { get; set; }
        public required int VisitStatusId { get; set; }
        public virtual VisitStatus? VisitStatus { get; set; }
        public virtual Employee? Employee { get; set; }
        public virtual Patient? Patient { get; set; }
        public virtual List<Service> Services { get; set; } = [];
        public virtual List<ServiceToVisit> ServiceToVisits { get; set; } = [];

    }
}
