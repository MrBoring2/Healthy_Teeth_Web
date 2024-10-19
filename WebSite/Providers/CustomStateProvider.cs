using Microsoft.AspNetCore.Components.Authorization;
using Shared.Models;
using System.Security.Claims;
using WebAPI.Identity;
using WebSite.Services;

namespace WebSite.Providers
{
    public class CustomStateProvider : AuthenticationStateProvider
	{
        private readonly HttpClient _httpClient;
		private readonly AuthHttpService _authHttpService;

		public CustomStateProvider(AuthHttpService authHttpService, HttpClient httpClient)
		{
			_authHttpService = authHttpService;
            _httpClient = httpClient;
		}

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var accessToken = await _authHttpService.GetAccessTokenAsync();
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(Utils.Utils.ParseClaimsFromJwt(accessToken), "jwt")));
        }

        public async Task<LoginResponse> LoginAsync(LoginModel loginViewModel)
        {
            var result = await _authHttpService.LoginAsync(loginViewModel);
            if (result.Success)
            {
                var authUser = new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, loginViewModel.Login)
                }, "apiauth"));
                var authState = Task.FromResult(new AuthenticationState(authUser));
                NotifyAuthenticationStateChanged(authState);
            }

            return result;
        }

        public async Task<LogoutResponse> LogoutAsync()
        {
            var result = await _authHttpService.LogoutAsync();
            if (result.Success)
            {
                var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
                var authState = Task.FromResult(new AuthenticationState(anonymousUser));
                NotifyAuthenticationStateChanged(authState);
            }

            return result;
        }
    }
}
