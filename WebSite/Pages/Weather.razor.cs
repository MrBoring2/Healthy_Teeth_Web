using Entities;
using Newtonsoft.Json;
using WebSite.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;

namespace WebSite.Pages
{
    public partial class Weather : IDisposable
    {
        [Inject]
        public HttpInterceptorService Interceptor { get; set; }
        private List<Employee> list = new List<Employee>();

        protected override async Task OnInitializedAsync()
        {
            Interceptor.RegisterEvents();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await LoadEmployees();
            }
        }

        private async Task LoadEmployees()
        {
            var response = await _apiService.GetAsync("api/employees");
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                list = JsonConvert.DeserializeObject<List<Employee>>(response.Content);
            }
            else
            {
                return;
            }
            StateHasChanged();
        }

        public void Dispose()
        {
            Console.WriteLine("Сервис на списке сотруднриках отключён");
            Interceptor.DisposeEvent();
        }
    }
}
