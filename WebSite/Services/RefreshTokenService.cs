using Microsoft.AspNetCore.Components.Authorization;
using WebSite.Providers;

namespace WebSite.Services
{
    public class RefreshTokenService
    {
        private readonly CustomStateProvider _customStateProvider;
        private readonly AuthHttpService _authService;

        public RefreshTokenService(CustomStateProvider customStateProvider, AuthHttpService authService)
        {
            _customStateProvider = customStateProvider;
            _authService = authService;
        }

        public async Task<string> TryRefreshToken()
        {
            var authState = await _customStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            var exp = user.FindFirst(p => p.Type.Equals("exp")).Value;
            var expTime = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(exp));

            var timeUTC = DateTime.UtcNow;

            var diff = expTime - timeUTC;
            if (diff.TotalSeconds <= 10)
                return await _authService.RefreshToken();

            return string.Empty;
        }
    }
}
