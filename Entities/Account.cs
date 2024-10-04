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
        public int EmploeeId { get; set; }
        [MaxLength(20)]
        [Required]
        public string Login { get; set; }
        [Required]
        public byte[] PasswordHash { get; set; }
        [Required]
        public byte[] PasswordSalt { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryDate { get; set; }
        public required int RoleId { get; set; }
        public virtual Role? Role { get; set; }
        public virtual Employee? Employee { get; set; }
    }
}
