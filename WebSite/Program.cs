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

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<ApiService>();
builder.Services.AddScoped<RefreshTokenService>();
builder.Services.AddScoped<AuthHttpService>();
builder.Services.AddScoped<CustomStateProvider>();
builder.Services.AddScoped<HttpInterceptorService>();
builder.Services.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<CustomStateProvider>());

builder.Services.AddAuthorizationCore();
await builder.Build().RunAsync();
