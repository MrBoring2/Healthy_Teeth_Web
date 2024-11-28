using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO
{
    public class VisitDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Поле Дата посещения обязательно для заполнения")]
        public DateOnly VisitDate { get; set; }
        [Required(ErrorMessage = "Поле Время посещения обязательно для заполнения")]
        public TimeOnly VisirtTime { get; set; }
        [Required(ErrorMessage = "Поле Причина посещения обязательно для заполнения")]
        [MaxLength(100, ErrorMessage = "Максимальная длина 100 символов")]
        public string VisitPurpose { get; set; }
        public string? VisitDiagnos { get; set; }
        public string? VisitObjectively { get; set; }
        [Required(ErrorMessage = "Поле Сотрудник обязательно для заполнения")]
        public int EmployeeId { get; set; }
        [Required(ErrorMessage = "Поле Пациент обязательно для заполнения")]
        public int PatientId { get; set; }
        public int VisitStatusId { get; set; }
        public DateTime FullDate => VisitDate == null ? new DateTime() : new DateTime(VisitDate, VisirtTime);
        public VisitStatusDTO? VisitStatus { get; set; }
        public EmployeeDTO? Employee { get; set; }
        public PatientDTO? Patient { get; set; }
        public List<ServiceToVisitDTO> ServiceToVisits { get; set; } = [];
    }
}
