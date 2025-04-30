using Shared.DTO;
using Shared.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class EmployeeViewModel
    {
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
        [Required(ErrorMessage = "Поле Пол обязательно для заполнения")]
        public int Gender { get; set; }
        [Required(ErrorMessage = "Поле Дата рождения обязательно для заполнения")]
        [DateNotInFuture]
        public DateOnly DateOfBirth { get; set; }
        [MaxLength(18, ErrorMessage = "Максимальная длина 18 символов")]
        [Required(ErrorMessage = "Поле Номер телефона обязательно для заполнения")]
        [RegularExpression("^\\+\\d{1} \\(\\d{3}\\) \\d{3}-\\d{2}-\\d{2}$", ErrorMessage = "Номер телефона введён неверно")]
        public string Phone { get; set; }
        public List<ScheduleDTO> Schedules { get; set; }
        [Required(ErrorMessage = "Поле Специализация обязательна для заполнения")]
        public int SpecializationId { get; set; }
        [Required(ErrorMessage = "Поле Роль обязательна для заполнения")]
        public int RoleId { get; set; }
        [MaxLength(20, ErrorMessage = "Максимальная длина 20 символов")]
        public string Password { get; set; }
        [MaxLength(20, ErrorMessage = "Максимальная длина 20 символов")]
        [Required(ErrorMessage = "Поле Логин обязательно для заполнения")]
        public string Login { get; set; }
        public bool ChangePassword { get; set; }
    }
}
