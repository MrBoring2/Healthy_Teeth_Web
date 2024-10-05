using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO
{
    public class AccountDTO
    {
        [Required]
        public int EmploeeId { get; set; }
        [MaxLength(20)]
        [Required]
        public string Login { get; set; }
        [Required]
        public int RoleId { get; set; }
        public RoleDTO Role { get; set; }
    }
}
