using Entities;
using Newtonsoft.Json;
using WebSite.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Shared.DTO;
using Radzen;
using Radzen.Blazor;
using Microsoft.AspNetCore.SignalR.Client;

namespace WebSite.Pages
{
    public partial class Employees : IDisposable
    {
        [Inject]
        private DialogService DialogService { get; set; }
        [Inject]
        public HttpInterceptorService Interceptor { get; set; }
        private ODataEnumerable<EmployeeDTO> list;
        private IList<EmployeeDTO> selectedEmployees;
        private RadzenDataGrid<EmployeeDTO> grid;
        private IList<RoleDTO> selectedRoles;
        private bool isLoading;
        private int count;
        protected override async Task OnInitializedAsync()
        {
            //Console.WriteLine("Сервис на списке сотруднриках включён");
            Interceptor.RegisterEvents();
        }



        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {

                //await LoadData(null);
            }


        }
       
        private async Task LoadData(LoadDataArgs args)
        {
            isLoading = true;
            var response = await _apiService.GetAsync("api/employees");
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                list = JsonConvert.DeserializeObject<ODataEnumerable<EmployeeDTO>>(response.Content);
                count = 10;
            }
            else
            {
                return;
            }
            isLoading = false;
            StateHasChanged();
        }

        public async Task OpenEmployeeWindow()
        {
            await DialogService.OpenAsync<AddEmployee>($"Добавление",
               new Dictionary<string, object>() { { "EmployeeId", 0 } },
               new DialogOptions()
               {
                   Resizable = true,
                   Draggable = true,
                   
                   Width =  "500px",
                   Height = "720px"
               });

        }
        public void Dispose()
        {
            //Console.WriteLine("Сервис на списке сотруднриках отключён");
            Interceptor.DisposeEvent();
        }
    }
}
