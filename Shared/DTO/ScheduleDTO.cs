using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO
{
    public class ScheduleDTO
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
        public string WeekdayName
        {
            get
            {
                switch (Weekday)
                {
                    case 0:
                        return "Понедельник";
                    case 1:
                        return "Вторник";
                    case 2:
                        return "Среда";
                    case 3:
                        return "Четверг";
                    case 4:
                        return "Пятница";
                    case 5:
                        return "Суббота";
                    case 6:
                        return "Воскресенье";
                    default:
                        return "Понедельник";
                }
            }
        }
    }
}
