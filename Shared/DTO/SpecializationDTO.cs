using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO
{
    public class SpecializationDTO
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string Title { get; set; }
    }
}
