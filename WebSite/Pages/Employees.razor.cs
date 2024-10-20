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
        private IApiService<EmployeeDTO> EmployeeApiService { get; set; }
        private IApiService<RoleDTO> RoleApiService { get; set; }
        private IApiService<SpecializationDTO> SpecalizationApiService { get; set; }
        private ODataEnumerable<EmployeeDTO> list;
        private IList<EmployeeDTO> selectedEmployees;
        private RadzenDataGrid<EmployeeDTO> grid;
        private List<RoleDTO> Roles { get; set; }
        private List<SpecializationDTO> Specializations { get; set; }
        private string search;
        private string Search
        {
            get => search;
            set
            {
                search = value;
                LoadData(lastArgs);
            }
        }
        private List<RoleDTO> selectedRoles;
        private List<RoleDTO> SelectedRoles
        {
            get => selectedRoles;
            set
            {

                selectedRoles = value;
                LoadData(lastArgs);
            }
        }
        private List<SpecializationDTO> selectedSpecializations;
        private List<SpecializationDTO> SelectedSpecializations
        {
            get => selectedSpecializations;
            set
            {

                selectedSpecializations = value;
                LoadData(lastArgs);
            }
        }
        private bool isLoading;
        private LoadDataArgs lastArgs;
        private int count;
        private string[] FilterNames { get; set; } = new string[]
            {
                "search",
                "orderBy",
                "rolesIds",
                "specializationIds",
            };

        protected override async Task OnInitializedAsync()
        {
            search = string.Empty;

            count = 10;
            EmployeeApiService = ApiServiceFatory.GetEmployeeApiService();
            RoleApiService = ApiServiceFatory.GetRoleApiService();
            SpecalizationApiService = ApiServiceFatory.GetSpecializationApiService();

            HubConnection.On<string>("EmployeeAdded", async mes =>
            {
                await LoadData(lastArgs);
            });
            await LoadRoles();
            await LoadSpecializations();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {

            }
        }

        private async Task LoadData(LoadDataArgs args)
        {
            lastArgs = args;
            isLoading = true;

            var queryParameters = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(Search))
            {
                queryParameters.Add("search", Search);
            }
            if (SelectedRoles != null && SelectedRoles.Count > 0)
            {
                queryParameters.Add("rolesIds", string.Join(',', SelectedRoles.Select(p => p.Id)));
            }
            if (SelectedSpecializations != null && SelectedSpecializations.Count > 0)
            {
                queryParameters.Add("spesializationIds", string.Join(',', SelectedSpecializations.Select(p => p.Id)));
            }
            if (!string.IsNullOrEmpty(args.OrderBy))
            {
                queryParameters.Add("orderby", args.OrderBy);
            }
            queryParameters.Add("top", args.Top.ToString());
            queryParameters.Add("skip", args.Skip.ToString());
            var response = await EmployeeApiService.GetAsync(queryParameters);
            list = response.Result.AsODataEnumerable();
            count = response.Count;
            Console.WriteLine(count);
            isLoading = false;
            StateHasChanged();
        }
        private async Task LoadRoles()
        {
            var response = await RoleApiService.GetAsync();
            Console.WriteLine(response.Count);
            if (response.Result != null)
            {
                Roles = response.Result.ToList();
                selectedRoles = Roles;
            }
        }

        private async Task LoadSpecializations()
        {
            var response = await SpecalizationApiService.GetAsync();
            if (response.Result != null)
            {
                Specializations = response.Result.ToList();
                selectedSpecializations = Specializations;
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
