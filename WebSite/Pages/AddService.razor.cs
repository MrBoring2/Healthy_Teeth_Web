using Entities;
using Microsoft.AspNetCore.Components;
using Shared.Models;
using System.Net.Http.Json;
using WebSite.Models;
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
            Console.WriteLine("Сервис на добавлении сервисах включён");
            Interceptor.RegisterEvents();
            //await JsRuntime.InvokeVoidAsync("alert", $"{a.User.Identity.IsAuthenticated.ToString()}"); // Alert
        }
        protected async Task SaveUser()
        {
            ResponseModel response;
            if (service.Id != 0)
            {
                response = await _apiService.PutAsync("api/Services", service.Id, service);
            }
            else
            {
                response = await _apiService.PostAsync("api/Services", service);
            }
            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                Cancel();
            }

        }
        public void Cancel()
        {
            NavigationManager.NavigateTo("/");
        }
        public void Dispose()
        {
            Console.WriteLine("Сервис на добавлении сервисах отключён");
            Interceptor.DisposeEvent();
        }
    }
}
