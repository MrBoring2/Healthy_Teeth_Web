using Blazored.LocalStorage;
using Newtonsoft.Json;
using Shared.Models;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Security.Claims;
using System.Text;
using WebAPI.Identity;

namespace WebSite.Services
{
    public class AuthHttpService
    {
        protected HttpClient _httpClient;
        protected readonly ILocalStorageService _localStorage;
        private readonly static SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        public AuthHttpService(IHttpClientFactory httpClient, ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
            _httpClient = httpClient.CreateClient("authapi");
        }

        private async Task SetAccessTokenAsync(string? accessToken, string? refreshToken)
        {
            if (!string.IsNullOrEmpty(accessToken))
            {
                await _localStorage.SetItemAsync("accessToken", accessToken);
            }
            else
            {
                await _localStorage.RemoveItemAsync("accessToken");
            }


            if (!string.IsNullOrEmpty(refreshToken))
            {
                await _localStorage.SetItemAsync("refreshToken", refreshToken);
            }
            else
            {
                await _localStorage.RemoveItemAsync("refreshToken");
            }
        }

        public async Task<string?> GetAccessTokenAsync()
        {

            var accessToken = await _localStorage.GetItemAsync<string?>("accessToken");

            var expTime = await GetExpirationTime(accessToken);

            if (expTime is null)
                return null;

            var now = DateTime.UtcNow;

            var diff = expTime - now;
            if (diff?.TotalSeconds <= 10)
            {
                await _semaphore.WaitAsync();
                try
                {                 
                    accessToken = await _localStorage.GetItemAsync<string>("accessToken");
                    expTime = await GetExpirationTime(accessToken);
                    now = DateTime.UtcNow;
                    diff = expTime - now;
                    if (diff?.TotalSeconds <= 10)
                    {
                        accessToken = await RefreshToken();
                    }
                }
                finally
                {
                    _semaphore.Release();
                }

            }

            return accessToken;
        }


        private async Task<DateTimeOffset?> GetExpirationTime(string? token)
        {
            if (string.IsNullOrEmpty(token))
                return null;

            var claims = Utils.Utils.ParseClaimsFromJwt(token);
            var exp = claims.First(p => p.Type.Equals("exp")).Value;
            var expTime = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(exp));
            return expTime;
        }

        private async Task<string> RefreshToken()
        {
            var token = await _localStorage.GetItemAsync<string>("accessToken");
            var refreshToken = await _localStorage.GetItemAsync<string>("refreshToken");

            var response = await _httpClient.PostAsJsonAsync("api/Authentication/RefreshToken",
                 new RefreshTokenRequest { Token = token, RefreshToken = refreshToken });
            var result = JsonConvert.DeserializeObject<AuthResponse>(await response.Content.ReadAsStringAsync());

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Что-то пошло не так при обновлении токена");
            }

            await _localStorage.SetItemAsync("accessToken", result.Token);
            await _localStorage.SetItemAsync("refreshToken", result.RefreshToken);
          
            return result.Token;
        }
        public async Task<LoginResponse?> LoginAsync(LoginModel loginViewModel)
        {
            LoginResponse result = null;

            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Authentication/Login", loginViewModel);
                result = JsonConvert.DeserializeObject<LoginResponse>(await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


            await SetAccessTokenAsync(result?.JwtBearer, result?.RefreshJwtBearer);


            return result;
        }

        public async Task<LogoutResponse?> LogoutAsync()
        {

            LogoutRequest request = new LogoutRequest();
            LogoutResponse result = null;
            try
            {
                var token = await _localStorage.GetItemAsync<string>("accessToken");
                var claims = Utils.Utils.ParseClaimsFromJwt(token);
                request.Login = claims.FirstOrDefault(p => p.Type == ClaimTypes.Name).Value;
                var response = await _httpClient.PostAsync("api/Authentication/Logout", new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, MediaTypeNames.Application.Json));
                result = await Task.Run(async () => JsonConvert.DeserializeObject<LogoutResponse>(await response.Content.ReadAsStringAsync()));
            }
            catch (Exception ex) { }

            await SetAccessTokenAsync(null, null);

            return result;
        }
    }
}
