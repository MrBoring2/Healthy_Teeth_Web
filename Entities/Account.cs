using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Account
    {
        public int EmployeeId { get; set; }
        [MaxLength(20)]
        [Required]
        public string Login { get; set; }
        [Required]
        public byte[] PasswordHash { get; set; }
        [Required]
        public byte[] PasswordSalt { get; set; }
        [Required]
        public int RoleId { get; set; }
        public virtual Role? Role { get; set; }
        public virtual Employee? Employee { get; set; }
        public virtual EmployeeRefreshToken EmployeeRefreshToken { get; set; }
    }
}
