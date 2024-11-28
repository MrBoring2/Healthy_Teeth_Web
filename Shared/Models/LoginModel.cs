using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class LoginModel
    {
        [MaxLength(20, ErrorMessage = "Максимальная длина 30 символов")]
        [Required(ErrorMessage = "Поле Имя обязательно для заполнения")]
        public string Login { get; set; }

        [MaxLength(20, ErrorMessage = "Максимальная длина 30 символов")]
        [Required(ErrorMessage = "Поле Имя обязательно для заполнения")]
        public string Password { get; set; }
    }
}
