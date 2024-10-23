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

        private IApiService<RoleDTO> RoleApiService { get; set; }
        private IApiService<SpecializationDTO> SpecializaionApiService { get; set; }
        private IApiService<EmployeeDTO> EmployeeApiService { get; set; }

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
            SpecializaionApiService = ApiServiceFactory.GetSpecializationApiService();
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
            ResponseModel<string> responseModel;
            if (employee.Id != 0)
            {
                responseModel = await EmployeeApiService.PutAsync(employee.Id, employee);
            }
            else
            {
                responseModel = await EmployeeApiService.PostAsync(employee);
            }

            if (responseModel.StatusCode == System.Net.HttpStatusCode.Created)
            {
                DialogService.Close(true);
            }
            else
            {

            }
        }
        private async Task LoadRoles()
        {
            var response = await RoleApiService.GetAsync();
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Roles = response.Content.ToList();
            }
            else
            {
                return;
            }
            StateHasChanged();
        }

        private async Task LoadSpecializations()
        {
            var response = await SpecializaionApiService.GetAsync();
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Specializations = response.Content.ToList();
            }
        }

        public void Cancel()
        {
            NavigationManager.NavigateTo("/");
        }
 
    }
}
