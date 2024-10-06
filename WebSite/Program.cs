using Blazored.LocalStorage;
using Entities;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using System.Text.Json.Serialization;
using System.Text.Json;
using WebSite;
using WebSite.Providers;
using WebSite.Services;
using Toolbelt.Blazor.Extensions.DependencyInjection;
using Radzen;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:8082") }
        .EnableIntercept(sp));
//builder.Services.AddScoped<IApiService, ApiService>();
builder.Services.AddHttpClientInterceptor();
//builder.Services.AddHttpClient<ApiService>(options =>
//{
//    options.BaseAddress = new Uri("https://localhost:8082");
//});
builder.Services.AddRadzenComponents();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<ApiService>();
builder.Services.AddScoped<RefreshTokenService>();
builder.Services.AddScoped<AuthHttpService>();
builder.Services.AddScoped<CustomStateProvider>();
builder.Services.AddScoped<HttpInterceptorService>();
builder.Services.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<CustomStateProvider>());
builder.Services.AddScoped(sp =>
{
    var navManager = sp.GetRequiredService<NavigationManager>();
   // var a = sp.GetService<AuthHttpService>();
    var authHttpService = sp.GetRequiredService<AuthHttpService>();
    return new HubConnectionBuilder()
    .WithUrl("https://localhost:8082/healthy_teeth_hub", async options =>
    {
        options.AccessTokenProvider = async () =>
        {
            Console.WriteLine(authHttpService is null);

            var accessTokenResult = await authHttpService.GetAccessTokenAsync();

            Console.WriteLine("ТОкен получен: " + accessTokenResult);
            return accessTokenResult;
           // return "";
            
        };
    })
    .WithAutomaticReconnect()
    .Build();

});
builder.Services.AddAuthorizationCore();
await builder.Build().RunAsync();
