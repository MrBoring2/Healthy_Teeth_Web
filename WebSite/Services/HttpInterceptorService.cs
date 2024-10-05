using Microsoft.AspNetCore.Components;
using System.Net;

using Toolbelt.Blazor;
using WebSite.Models;

namespace WebSite.Services
{
    public class HttpInterceptorService
    {
        private readonly HttpClientInterceptor _interceptor;
        private readonly RefreshTokenService _refreshTokenService;
        private readonly NavigationManager _navManager;
        private readonly AuthHttpService _authHttpService;

        public HttpInterceptorService(HttpClientInterceptor interceptor, RefreshTokenService refreshTokenService, NavigationManager navManager, AuthHttpService authHttpService)
        {
            _interceptor = interceptor;
            _refreshTokenService = refreshTokenService;
            _navManager = navManager;
            _authHttpService = authHttpService;
        }

        public void RegisterEvents()
        {
            _interceptor.BeforeSendAsync += InterceptorBeforeSendAsync;
            _interceptor.AfterSendAsync += _interceptor_AfterSendAsync;
        }

        private async Task _interceptor_AfterSendAsync(object sender, HttpClientInterceptorEventArgs e)
        {
            string message = string.Empty;
            if (!e.Response.IsSuccessStatusCode)
            {
                var statusCode = e.Response.StatusCode;
                switch (statusCode)
                {
                    case HttpStatusCode.NotFound:
                        _navManager.NavigateTo("/404");
                        message = "Запрашиваемый ресурс не найден.";
                        break;
                    case HttpStatusCode.Unauthorized:
                        //await _authHttpService.LogoutAsync();
                        _navManager.NavigateTo("/login");
                        message = "Пользователь не авторизован.";
                        break;
                    default:
                        _navManager.NavigateTo("/500");
                        message = "Что-то пошло не так, позовите администратора.";
                        break;
                }
                throw new HttpResponseException(message);
            }
        }

        public async Task InterceptorBeforeSendAsync(object sender, HttpClientInterceptorEventArgs e)
        {
            var absPath = e.Request.RequestUri.AbsolutePath;

            Console.WriteLine("Путь: " + absPath);
            if (absPath.Contains("Token"))
            {
                Console.WriteLine("Хуйня не прошла!!!: " + absPath);
                return;
            }

            else
            {
                Console.WriteLine("Хуйня прошла!!!: " + absPath);
                var token = await _refreshTokenService.TryRefreshToken();
                if (!string.IsNullOrEmpty(token))
                {
                    e.Request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", token);
                }
            }
        }

        public void DisposeEvent()
        {
            _interceptor.BeforeSendAsync -= InterceptorBeforeSendAsync;
            _interceptor.AfterSendAsync -= _interceptor_AfterSendAsync;
        }
    }
}
