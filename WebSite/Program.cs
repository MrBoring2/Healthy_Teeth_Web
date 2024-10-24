using Blazored.LocalStorage;
using Entities;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WebSite;
using WebSite.Providers;
using WebSite.Services;
using Toolbelt.Blazor.Extensions.DependencyInjection;
using Radzen;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using WebSite.Services.ApiServices;



var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<SetAuthHttpHandler>();
builder.Services.AddHttpClient("authapi", httpClient =>
{
    httpClient.BaseAddress = new Uri("https://localhost:8082");
});
builder.Services.AddHttpClient("api", httpClient =>
{
    httpClient.BaseAddress = new Uri("https://localhost:8082");
}).AddHttpMessageHandler<SetAuthHttpHandler>();

builder.Services.AddHttpClientInterceptor();
builder.Services.AddRadzenComponents();
builder.Services.AddRadzenCookieThemeService(options =>
{
    options.Name = "MyApp";
    options.Duration = TimeSpan.FromDays(7);
});
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddTransient<IEmployeeApiService, EmployeeApiService>();
builder.Services.AddTransient<IRoleApiService, RoleApiService>();
builder.Services.AddTransient<IServiceApiService, ServiceApiService>();
builder.Services.AddTransient<ISpecializationApiService, SpecializationApiService>();
builder.Services.AddScoped<AuthHttpService>();
builder.Services.AddScoped(
    sp => sp.GetService<IHttpClientFactory>().CreateClient("api"));
builder.Services.AddScoped<CustomStateProvider>();
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
var a = builder.Build();
await a.RunAsync();
