using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebAPI.Identity;

namespace WebAPI.Services
{
    public class JwtTokenGenerator
    {
        public static JwtTokenResult GenerateToken(ClaimsIdentity identity)
        {
            if (identity == null)
            {
                return null;
            }

            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                claims: identity.Claims,
                expires: DateTime.Now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(),
                SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var result = new JwtTokenResult
            {
                access_token = encodedJwt,
                user_name = identity.FindFirst(ClaimTypes.Name).Value.ToString(),
                role_name = identity.FindFirst(ClaimTypes.Role).Value.ToString(),
            };

            return result;
        }
    }
}
