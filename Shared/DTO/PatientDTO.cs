using Entities;
using Shared.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO
{
    public class PatientDTO
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
        public string? FullName { get; set; }
        public int? Gender { get; set; }
        public string GenderName => Gender == 0 ? "Мужчина" : "Женщина";
        [DateNotInFuture]
        public DateOnly? DateOfBirth { get; set; }

        [MaxLength(40, ErrorMessage = "Максимальная длина 40 символов")]
        public string? City { get; set; }

        [MaxLength(100, ErrorMessage = "Максимальная длина 100 символов")]
        public string? Address { get; set; }
        [MaxLength(18, ErrorMessage = "Максимальная длина 18 символов")]
        [Required(ErrorMessage = "Поле Телефон обязательно для заполнения")]
        [RegularExpression("^\\+\\d{1} \\(\\d{3}\\) \\d{3}-\\d{2}-\\d{2}$", ErrorMessage = "Номер телефона введён неверно")]
        public string Phone { get; set; }

        [MaxLength(6, ErrorMessage = "Максимальная длина 6 символов")]
        [RegularExpression("^\\d{6}$", ErrorMessage = "Номер паспорта введён неверно")]
        public string? PassportNumber { get; set; }

        [MaxLength(4, ErrorMessage = "Максимальная длина 4 символов")]
        [RegularExpression("^\\d{4}$", ErrorMessage = "Код паспорта введён неверно")]
        public string? PassportCode { get; set; }

        [MaxLength(16, ErrorMessage = "Максимальная длина 16 символов")]
        [RegularExpression("^\\d{16}$", ErrorMessage = "Медицинский полис введён неверно")]
        public string? MedicalPolicy { get; set; }
        public virtual List<VisitDTO>? Visits { get; set; }
        public string? Passport { get; set; }
    }
}
