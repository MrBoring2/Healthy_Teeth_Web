using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO
{
    public class ServiceDTO
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public float Price { get; set; }
        [Required]
        public int SpecializationId { get; set; }
        public SpecializationDTO? Specialization { get; set; }
    }
}
