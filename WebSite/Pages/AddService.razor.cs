using Entities;
using Microsoft.AspNetCore.Components;
using Shared.Models;
using System.Net.Http.Json;
using WebSite.Services;

namespace WebSite.Pages
{
    public partial class AddService : IDisposable
    {
        [Parameter]
        public int userId { get; set; }
        [Inject]
        public HttpInterceptorService Interceptor { get; set; }
        protected string Title = "Add";
        protected Service service = new();
        protected override async Task OnParametersSetAsync()
        {

        }
        protected override async Task OnInitializedAsync()
        {
            // Interceptor.RegisterEvents();
            var a = await _authStateProvider.GetAuthenticationStateAsync();
            Console.WriteLine(a.User.Identity.IsAuthenticated.ToString());
            //await JsRuntime.InvokeVoidAsync("alert", $"{a.User.Identity.IsAuthenticated.ToString()}"); // Alert
        }
        protected async Task SaveUser()
        {
            if (service.Id != 0)
            {
                await Http.PutAsJsonAsync("api/User", service);
            }
            else
            {
                await _apiService.PostAsync("api/Services", service);
            }
            Cancel();
        }
        public void Cancel()
        {
            NavigationManager.NavigateTo("/");
        }
        public void Dispose() => Interceptor.DisposeEvent();
    }
}
