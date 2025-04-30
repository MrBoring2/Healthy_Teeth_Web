using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Schedule
    {
        public int Id { get; set; }
        [Required]
        public TimeOnly TimeFrom { get; set; }
        [Required]
        public TimeOnly TimeTo { get; set; }
        [Required]
        public int Cabinet { get; set; }
        [Required]
        public int Weekday { get; set; }
        [Required]
        public int EmployeeId { get; set; }
        public virtual Employee? Employee { get; set; }

    }
}
