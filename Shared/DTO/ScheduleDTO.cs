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
        [Required(ErrorMessage = "Поле Начало смены обязательно для заполнения")]
        public TimeOnly TimeFrom { get; set; }
        [Required(ErrorMessage = "Поле Конец смены обязательно для заполнения")]
        public TimeOnly TimeTo { get; set; }
        [Required(ErrorMessage = "Поле Кабинет обязательно для заполнения")]
        [Range(0, 1000, ErrorMessage = "Кабинет должен быть в ределах 0-1000")]
        public int Cabinet { get; set; }
        [Required(ErrorMessage = "Поле День недели обязательно для заполнения")]
        public int Weekday { get; set; }
        [Required(ErrorMessage = "Поле Сотрудник обязательно для заполнения")]
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
