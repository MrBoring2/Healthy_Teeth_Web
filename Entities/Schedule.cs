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
        public TimeSpan TimeFrom { get; set; }
        [Required]
        public TimeSpan TimeTo { get; set; }
        [Required]
        public int Cabinet { get; set; }
        [Required]
        public int WeekdayId { get; set; }
        [Required]
        public int EmployeeId { get; set; }
        public virtual Weekday? Weekday { get; set; }
        public virtual Employee? Employee { get; set; }

    }
}
