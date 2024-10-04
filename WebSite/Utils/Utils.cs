using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;

namespace WebSite.Utils
{
	public static class Utils
	{
		public static IEnumerable<Claim> ParseClaimsFromJwt(string accessToken)
		{
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(accessToken);
            return jwt.Claims;
            //var claims = new List<Claim>();
            //var payload = accessToken.Split('.')[1];
            //var jsonBytes = ParseTokenPayload(payload);
            //var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            //return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
        }
		private static byte[] ParseTokenPayload(string base64TokenPayload)
		{
			switch (base64TokenPayload.Length % 4)
			{
				case 2:
					base64TokenPayload += "==";
					break;
				case 3:
					base64TokenPayload += "=";
					break;
			}
			return Convert.FromBase64String(base64TokenPayload);
		}
	}
}
