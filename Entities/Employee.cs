using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Employee
    {
        public int Id { get; set; }
        [MaxLength(30)]
        [Required]
        public string FirstName { get; set; }
        [MaxLength(30)]
        [Required]
        public string LastName { get; set; }
        [MaxLength(30)]
        [Required]
        public string MiddleName { get; set; }
        public string FullName { get; private set; }
        [Required]
        [MaxLength(10)]
        public string Gender { get; set; }
        [Required]
        public  DateTime DateOfBirth { get; set; }
        [MaxLength(11)]
        [Required]
        public string Phone { get; set; }
        [Required]
        public int SpecializationId { get; set; }
        public virtual Account? Account { get; set; }    
        public virtual Specialization? Specialization { get; set; }
        public virtual List<Visit>? Visits { get; set; }
        public virtual List<Schedule>? Schedules { get; set; }
    }
}
