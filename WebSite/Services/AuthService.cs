using Shared.Models;
using System.Net.Http.Json;

namespace WebSite.Services
{
    public class AuthService : IAuthService
	{
		private readonly HttpClient _httpClient;
		public AuthService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}
		public async Task Login(LoginModel loginRequest)
		{
			var result = await _httpClient.PostAsJsonAsync("api/auth/login", loginRequest);
			if (result.StatusCode == System.Net.HttpStatusCode.BadRequest) throw new Exception(await result.Content.ReadAsStringAsync());
			result.EnsureSuccessStatusCode();
		}
		public async Task Logout()
		{
			var result = await _httpClient.PostAsync("api/auth/logout", null);
			result.EnsureSuccessStatusCode();
		}
	}
}
