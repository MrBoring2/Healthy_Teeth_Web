using Toolbelt.Blazor;

namespace WebSite.Services
{
    public class HttpInterceptorService
    {
        private readonly HttpClientInterceptor _interceptor;
        private readonly RefreshTokenService _refreshTokenService;

        public HttpInterceptorService(HttpClientInterceptor interceptor, RefreshTokenService refreshTokenService)
        {
            _interceptor = interceptor;
            _refreshTokenService = refreshTokenService;
        }

        public void RegisterEvents() => _interceptor.BeforeSendAsync += InterceptorBeforeSendAsync;

        public async Task InterceptorBeforeSendAsync(object sender, HttpClientInterceptorEventArgs e)
        {
            var absPath = e.Request.RequestUri.AbsolutePath;

            if(!absPath.Contains("Token") && !absPath.Contains("Authentication"))
            {
                var token = await _refreshTokenService.TryRefreshToken();
                if (!string.IsNullOrEmpty(token))
                {
                    e.Request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", token);
                }
            }
        }

        public void DisposeEvent() => _interceptor.BeforeSendAsync -= InterceptorBeforeSendAsync;
    }
}
