using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class RefreshTokenRequest
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}
