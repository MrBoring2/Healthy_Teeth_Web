using Entities;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using Radzen;
using Shared.DTO;
using Shared.Models;
using System.Collections.Generic;
using System.Net.Http.Json;
using WebSite.Models;
using WebSite.Services;
using WebSite.Services.ApiServices;

namespace WebSite.Pages
{
    public partial class AddEmployee
    {
        [Parameter]
        public int EmployeeId { get; set; }
        [Inject]
        private IApiServiceFactory ApiServiceFactory { get; set; }
        [Inject]
        public DialogService DialogService { get; set; }

        private IApiService RoleApiService { get; set; }
        private IApiService SpecificaionApiService { get; set; }
        private IApiService EmployeeApiService { get; set; }

        private List<RoleDTO> Roles { get; set; }
        private List<Gender> Genders;
        private List<SpecializationDTO> Specializations { get; set; }
        protected string Title = "Add";
        protected EmployeeViewModel employee = new();
        protected override async Task OnParametersSetAsync()
        {

        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {


        }

        protected override async Task OnInitializedAsync()
        {

            RoleApiService = ApiServiceFactory.GetRoleApiService();
            SpecificaionApiService = ApiServiceFactory.GetSpecificationApiService();
            EmployeeApiService = ApiServiceFactory.GetEmployeeApiService();

            Genders = new List<Gender>
            {
                new Gender{ Id = 0, Title = "Мужской"},
                new Gender{ Id = 1, Title = "Женский"}
            };
            // await Task.WhenAll(Task.Run(() => Interceptor.RegisterEvents()), LoadRoles());

            await LoadRoles();
            await LoadSpecializations();
            //Interceptor.RegisterEvents();

        }

        protected async Task SaveUser()
        {
            ResponseModel response;

            if (employee.Id != 0)
            {
                response = await EmployeeApiService.PutAsync(employee.Id, employee);
            }
            else
            {
                response = await EmployeeApiService.PostAsync(employee);
            }

            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                DialogService.Close(true);
            }
        }
        private async Task LoadRoles()
        {
            var response = await RoleApiService.GetAsync();
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Roles = JsonConvert.DeserializeObject<List<RoleDTO>>(response.Content);
                // await LoadSpecializations();

            }
            else
            {
                return;
            }
            StateHasChanged();
        }

        private async Task LoadSpecializations()
        {
            var response = await SpecificaionApiService.GetAsync();
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Specializations = JsonConvert.DeserializeObject<List<SpecializationDTO>>(response.Content);
            }
        }

        public void Cancel()
        {
            NavigationManager.NavigateTo("/");
        }
 
    }
}
