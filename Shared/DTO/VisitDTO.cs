using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO
{
    public class VisitDTO
    {
        public int Id { get; set; }
        [Required]
        public DateOnly VisitDate { get; set; }
        [Required]
        public TimeOnly VisirtTime { get; set; }
        [Required]
        [MaxLength(100)]
        public string VisitPurpose { get; set; }
        public string? VisitDiagnos { get; set; }
        public string? VisitObjectively { get; set; }
        [Required]
        public int EmployeeId { get; set; }
        [Required]
        public int PatientId { get; set; }
        public int VisitStatusId { get; set; }
        public virtual VisitStatusDTO? VisitStatus { get; set; }
        public virtual EmployeeDTO? Employee { get; set; }
        public virtual PatientDTO? Patient { get; set; }
        public virtual List<ServiceDTO> Services { get; set; } = [];
        //public virtual List<ServiceToVisit> ServiceToVisits { get; set; } = [];
    }
}
