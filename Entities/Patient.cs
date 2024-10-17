using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Patient
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(30)]
        public  string LastName { get; set; }
        [Required]
        [MaxLength(30)]
        public  string MiddleName { get; set; }
        public string FullName { get; private set; }
        [Required]
        [MaxLength(10)]
        public  string Gender { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        [MaxLength(40)]
        public string City { get; set; }
        [Required]
        [MaxLength(100)]
        public string Address { get; set; }
        [Required]
        [MaxLength(11)]
        public string Phone { get; set; }
        [Required]
        [MaxLength(4)]
        public string PassportNumber { get; set; }
        [Required]
        [MaxLength(6)]
        public string PassportCode { get; set; }
        [Required]
        [MaxLength(16)]
        public string MedicalPolicy { get; set; }
        public virtual List<Visit>? Visits { get; set; }
    }
}
