using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace WebAPI.Identity
{
    public class AuthOptions
    {
        public const string ISSUER = "HealthyTeethAPI";
        public const string AUDIENCE = "HealthyTeethClient";
        private const string KEY = "524C1F22-6115-4E16-9B6A-3FBF185308F2";
        public const int LIFETIME = 1;
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
        }
    }
}
