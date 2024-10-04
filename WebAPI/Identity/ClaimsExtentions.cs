using Entities;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace WebAPI.Identity
{
    public static class ClaimsExtentions
    {
        public static ClaimsIdentity BuildClaimsForUser<T>(this T user)
            where T : Employee
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Account.Login),
                new Claim(ClaimTypes.Role, user.Account.Role.Title)
            };
            //ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            return new ClaimsIdentity(claims);
        }
    }
}
