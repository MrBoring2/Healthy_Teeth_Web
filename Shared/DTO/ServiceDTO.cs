using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO
{
    public class ServiceDTO
    {
        public int Id { get; set; }
        [MaxLength(80, ErrorMessage = "Максимальная длина 80 символов")]
        [Required(ErrorMessage = "Поле Название обязательно для заполнения")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Поле Цена обязательно для заполнения")]
        [Range(1, 1000000, ErrorMessage = "Поле Цена должна быть впределах 1-1000000")]
        public float Price { get; set; }
        [Required(ErrorMessage = "Поле Специализация обязательно для заполнения")]
        public int SpecializationId { get; set; }
        public SpecializationDTO? Specialization { get; set; }
    }
}
