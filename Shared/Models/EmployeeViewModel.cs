using Shared.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class EmployeeViewModel
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
        [Required]
        public int Gender { get; set; }
        [Required]
        public DateOnly DateOfBirth { get; set; }
        [MaxLength(11)]
        [Required]
        public string Phone { get; set; }
        public List<ScheduleDTO> Schedules { get; set; }
        [Required]
        public int SpecializationId { get; set; }
        public int RoleId { get; set; }
        public string Password { get; set; }
        public string Login { get; set; }
        public bool ChangePassword { get; set; }
    }
}
