using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO
{
    public class PatientDTO
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(30)]
        public string LastName { get; set; }
        [Required]
        [MaxLength(30)]
        public string MiddleName { get; set; }
        public string? FullName { get; set; }

        [MaxLength(10)]
        public string? Gender { get; set; }

        public DateOnly? DateOfBirth { get; set; }

        [MaxLength(40)]
        public string? City { get; set; }

        [MaxLength(100)]
        public string? Address { get; set; }
        [Required]
        [MaxLength(11)]
        public string Phone { get; set; }

        [MaxLength(4)]
        public string? PassportNumber { get; set; }

        [MaxLength(6)]
        public string? PassportCode { get; set; }

        [MaxLength(16)]
        public string? MedicalPolicy { get; set; }
        public virtual List<VisitDTO>? Visits { get; set; }
        public string Passport => PassportNumber + " " + PassportCode;
    }
}
