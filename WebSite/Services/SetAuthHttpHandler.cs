using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using System.Net;
using Toolbelt.Blazor;
using WebSite.Models;
using WebSite.Providers;

namespace WebSite.Services
{
    public class SetAuthHttpHandler : DelegatingHandler
    {
        private readonly AuthHttpService _authHttpService;

        public SetAuthHttpHandler(AuthHttpService authHttpService)
        {

            _authHttpService = authHttpService;

        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _authHttpService.GetAccessTokenAsync();

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", token);

            return await base.SendAsync(request, cancellationToken);
        }
    }

}
