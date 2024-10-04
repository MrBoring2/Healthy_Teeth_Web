using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Schedule
    {
        public int Id { get; set; }
        public required TimeSpan TimeFrom { get; set; }
        public required TimeSpan TimeTo { get; set; }
        public required int Cabinet { get; set; }
        public required int WeekdayId { get; set; }
        public required int EmployeeId { get; set; }
        public virtual Weekday? Weekday { get; set; }
        public virtual Employee? Employee { get; set; }

    }
}
