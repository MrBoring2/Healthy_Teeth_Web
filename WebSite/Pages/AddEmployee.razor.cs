using Entities;
using Microsoft.AspNetCore.Components;
using Shared.Models;
using System.Net.Http.Json;
using WebSite.Models;
using WebSite.Services;

namespace WebSite.Pages
{
    public partial class AddEmployee : IDisposable
    {
        [Parameter]
        public int userId { get; set; }
        [Inject]
        public HttpInterceptorService Interceptor { get; set; }
        protected string Title = "Add";
        protected EmployeeDTO employee = new();
        protected override async Task OnParametersSetAsync()
        {

        }
        protected override async Task OnInitializedAsync()
        {
            Console.WriteLine("Сервис на добавлении сотруднриках включён");
            Interceptor.RegisterEvents();
            //var a = await _authStateProvider.GetAuthenticationStateAsync();
            //Console.WriteLine(a.User.Identity.IsAuthenticated.ToString());
            //await JsRuntime.InvokeVoidAsync("alert", $"{a.User.Identity.IsAuthenticated.ToString()}"); // Alert
        }
        protected async Task SaveUser()
        {
            ResponseModel response;

            if (employee.Id != 0)
            {
                response = await _apiService.PutAsync("api/employees", employee.Id, employee);
            }
            else
            {
                response = await _apiService.PostAsync("api/employees", employee);
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
            Console.WriteLine("Сервис на добавлении сотруднриках отключён");
            Interceptor.DisposeEvent();
        }
    }
}
