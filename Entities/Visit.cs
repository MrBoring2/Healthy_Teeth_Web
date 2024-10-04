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
        public required DateTime VisitDate { get; set; }
        public required TimeSpan VisirtTime { get; set; }
        [MaxLength(100)]
        public required string VisitPurpose { get; set; }
        public string? VisitDiagnos { get; set; }
        public string? VisitObjectively { get; set; }
        public required int EmployeeId { get; set; }
        public required int PatientId { get; set; }
        public required int VisitStatusId { get; set; }
        public virtual VisitStatus? VisitStatus { get; set; }
        public virtual Employee? Employee { get; set; }
        public virtual Patient? Patient { get; set; }
        public virtual List<Service> Services { get; set; } = [];
        public virtual List<ServiceToVisit> ServiceToVisits { get; set; } = [];

    }
}
