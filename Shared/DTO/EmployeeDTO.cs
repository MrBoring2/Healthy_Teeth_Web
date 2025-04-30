using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO
{
    public class EmployeeDTO
    {
        public EmployeeDTO()
        {
            Account = new();
        }

        public int Id { get; set; }
        [MaxLength(30, ErrorMessage = "Максимальная длина 30 символов")]
        [Required(ErrorMessage = "Поле Фамилия обязательно для заполнения")]
        public string FirstName { get; set; }
        [MaxLength(30, ErrorMessage = "Максимальная длина 30 символов")]
        [Required(ErrorMessage = "Поле Имя обязательно для заполнения")]
        public string LastName { get; set; }
        [MaxLength(30, ErrorMessage = "Максимальная длина 30 символов")]
        [Required(ErrorMessage = "Поле Отчество обязательно для заполнения")]
        public string MiddleName { get; set; }
        public string FullName { get; set; }
        [Required(ErrorMessage = "Поле Пол обязательно для заполнения")]
        public int Gender { get; set; }
        public string GenderName => Gender == 0 ? "Мужчина" : "Женщина";
        [Required(ErrorMessage = "Поле Дата рождения обязательно для заполнения")]
        public DateOnly DateOfBirth { get; set; }
        [MaxLength(18, ErrorMessage = "Максимальная длина 18 символов")]
        [Required(ErrorMessage = "Поле Номер телефона обязательно для заполнения")]
        [RegularExpression("^\\+7 \\(\\d{3}\\) \\d{3}-\\d{2}-\\d{2}$", ErrorMessage = "Номер телефона введён неверно")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Поле Специализация обязательна для заполнения")]
        public int SpecializationId { get; set; }
        public List<ScheduleDTO> Schedules {  get; set; } 
        public List<VisitDTO> Visits {  get; set; } 
        public SpecializationDTO? Specialization { get; set; }
        public AccountDTO? Account { get; set; }
    }
}
