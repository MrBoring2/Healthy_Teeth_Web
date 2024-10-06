using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Newtonsoft.Json;
using Shared.Models;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Security.Claims;
using System.Text;
using WebAPI.Identity;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebSite.Services
{
    public class AuthHttpService
    {
        protected HttpClient _httpClient;
        protected readonly ILocalStorageService _localStorage;
        public AuthHttpService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
            _httpClient = httpClient;

            //_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", localStorage.GetItemAsync<string>("accessToken").Result);
        }

        private async Task SetAccessTokenAsync(string accessToken, string refreshToken)
        {
            if (!string.IsNullOrEmpty(accessToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", accessToken);
                await _localStorage.SetItemAsync("accessToken", accessToken);
                await _localStorage.SetItemAsync("refreshToken", refreshToken);
            }
            else
            {
                _httpClient.DefaultRequestHeaders.Authorization = null;
                await _localStorage.RemoveItemAsync("accessToken");
                await _localStorage.RemoveItemAsync("refreshToken");
            }
        }

        public async Task<string> GetAccessTokenAsync()
        {
            if (_httpClient.DefaultRequestHeaders.Authorization != null)
            {
                if (!string.IsNullOrEmpty(_httpClient.DefaultRequestHeaders.Authorization.Parameter))
                {
                    return _httpClient.DefaultRequestHeaders.Authorization.Parameter;
                }
            }

            var accessToken = "";
            accessToken = await _localStorage.GetItemAsync<string>("accessToken");

            return accessToken;
        }

        public async Task<string> RefreshToken()
        {
            var token = await _localStorage.GetItemAsync<string>("accessToken");
            var refreshToken = await _localStorage.GetItemAsync<string>("refreshToken");

            //Console.WriteLine($"Токен: {token}");
           // Console.WriteLine($"Токен обновления: {refreshToken}");

            var response = await _httpClient.PostAsJsonAsync("api/Authentication/RefreshToken",
                 new RefreshTokenRequest { Token = token, RefreshToken = refreshToken });
            var result = JsonConvert.DeserializeObject<AuthResponse>(await response.Content.ReadAsStringAsync());

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Что-то пошло не так при обновлении токена");
            }

            await _localStorage.SetItemAsync("accessToken", result.Token);
            await _localStorage.SetItemAsync("refreshToken", result.RefreshToken);

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", result.Token);
            return result.Token;
        }
        public async Task<LoginResponse> LoginAsync(LoginModel loginViewModel)
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

            if (result != null && result.Success)
            {
                await SetAccessTokenAsync(result.JwtBearer, result.RefreshJwtBearer);
            }

            return result;
        }

        public async Task<LogoutResponse> LogoutAsync()
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
            Console.WriteLine("Результат выхода: " + result.Success);

            if (result != null && result.Success)
                await SetAccessTokenAsync(null, null);

            return result;
        }
    }
}
