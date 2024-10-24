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
                    case 1:
                        return "Понедельник";
                    case 2:
                        return "Вторник";
                    case 3:
                        return "Среда";
                    case 4:
                        return "Четверг";
                    case 5:
                        return "Пятница";
                    case 6:
                        return "Суббота";
                    case 0:
                        return "Воскресенье";
                    default:
                        return "Понедельник";
                }
            }
        }
    }
}
