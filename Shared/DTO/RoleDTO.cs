using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO
{
    public class RoleDTO
    {
        [Required]
        public int Id { get; set; }
        public string Title { get; set; }
    }
}
