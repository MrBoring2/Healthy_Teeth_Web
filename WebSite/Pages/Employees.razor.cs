using Entities;
using Newtonsoft.Json;
using WebSite.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Shared.DTO;
using Radzen;
using Radzen.Blazor;
using Microsoft.AspNetCore.SignalR.Client;
using WebSite.Services.ApiServices;

namespace WebSite.Pages
{
    public partial class Employees
    {
        [Inject]
        private IApiServiceFactory ApiServiceFatory { get; set; }
        [Inject]
        private HubConnection HubConnection { get; set; }
        [Inject]
        private DialogService DialogService { get; set; }
        private IApiService EmployeeApiService { get; set; }
        private IApiService RoleApiService { get; set; }
        private IApiService SpecificationApiService { get; set; }
        private ODataEnumerable<EmployeeDTO> list;
        private IList<EmployeeDTO> selectedEmployees;
        private RadzenDataGrid<EmployeeDTO> grid;
        private IList<RoleDTO> selectedRoles;
        private List<RoleDTO> Roles { get; set; }
        private List<SpecializationDTO> Specializations { get; set; }
        private bool isLoading;
        private LoadDataArgs lastArgs;
        private int count;

        protected override async Task OnInitializedAsync()
        {
            EmployeeApiService = ApiServiceFatory.GetEmployeeApiService();
            RoleApiService = ApiServiceFatory.GetRoleApiService();
            SpecificationApiService = ApiServiceFatory.GetSpecificationApiService();
            HubConnection.On<string>("EmployeeAdded", async mes =>
            {
                await LoadData(lastArgs);
            });
         
        }



        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await LoadRoles();
                
                await LoadSpecializations();
            }
        }

        private async Task LoadData(LoadDataArgs args)
        {
            lastArgs = args;
            isLoading = true;
            var filterNames = new string[]
            {
                "search",
                "orderBy",
                ""
            };
            var response = await EmployeeApiService.GetAsync();
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
        private async Task LoadRoles()
        {
            var response = await RoleApiService.GetAsync();
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Roles = JsonConvert.DeserializeObject<List<RoleDTO>>(response.Content);
            }
            else
            {
                return;
            }
        }

        private async Task LoadSpecializations()
        {
            var response = await SpecificationApiService.GetAsync();
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Specializations = JsonConvert.DeserializeObject<List<SpecializationDTO>>(response.Content);
            }
        }
        public async Task OpenEmployeeWindow()
        {
            await DialogService.OpenAsync<AddEmployee>($"Добавление",
               new Dictionary<string, object>() { { "EmployeeId", 0 } },
               new DialogOptions()
               {
                   Resizable = true,
                   Draggable = true,

                   Width = "500px",
                   Height = "720px"
               });

        }
       
    }
}
