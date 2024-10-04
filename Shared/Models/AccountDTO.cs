using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class AccountDTO
    {
        public int EmploeeId { get; set; }
        [MaxLength(20)]
        [Required]
        public string Login { get; set; }
        [Required]
        [MaxLength(20)]
        public string Password { get; set; }
        [Required]
        public int RoleId { get; set; }
    }
}
