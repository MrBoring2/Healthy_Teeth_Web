using Entities;
using Newtonsoft.Json;
using WebSite.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Shared.DTO;

namespace WebSite.Pages
{
    public partial class Weather : IDisposable
    {
        [Inject]
        public HttpInterceptorService Interceptor { get; set; }
        private List<EmployeeDTO> list = new List<EmployeeDTO>();

        protected override async Task OnInitializedAsync()
        {
            Console.WriteLine("Сервис на списке сотруднриках включён");
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
                list = JsonConvert.DeserializeObject<List<EmployeeDTO>>(response.Content);
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
