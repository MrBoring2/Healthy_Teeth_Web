using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO
{
    public class EmployeeDTO
    {
        public EmployeeDTO()
        {
            Account = new();
        }

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
        public string FullName { get; set; }
        [Required]
        [MaxLength(1)]
        public string Gender { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [MaxLength(11)]
        [Required]
        public string Phone { get; set; }
        [Required]
        public int SpecializationId { get; set; }
        public SpecializationDTO Specialization { get; set; }
        public AccountDTO Account { get; set; }
    }
}
