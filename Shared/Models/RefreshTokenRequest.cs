using System.ComponentModel.DataAnnotations;

namespace Shared.Models
{
    public class RefreshTokenRequest
    {
        [Required]
        public string Token { get; set; }   
        [Required]
        public string RefreshToken { get; set; }
    }
}
