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

namespace WebSite.Pages
{
    public partial class AddEmployee : IDisposable
    {
        [Parameter]
        public int EmployeeId { get; set; }
        [Inject]
        public DialogService DialogService { get; set; }
        [Inject]
        public HttpInterceptorService Interceptor { get; set; }
        private List<RoleDTO> Roles { get; set; }
        private List<string> Genders = new List<string>() { "Мужчина", "Женщина" };
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
            await Task.WhenAll(Task.Run(() => Interceptor.RegisterEvents()), LoadRoles());
          
           // await LoadRoles();
           // await LoadSpecializations();
            //Interceptor.RegisterEvents();

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
                DialogService.Close(true);
            }
        }
        private async Task LoadRoles()
        {
            var response = await _apiService.GetAsync("api/roles");
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Roles = JsonConvert.DeserializeObject<List<RoleDTO>>(response.Content);
                await LoadSpecializations();

            }
            else
            {
                return;
            }
            StateHasChanged();
        }

        private async Task LoadSpecializations()
        {
            var response = await _apiService.GetAsync("api/specializations");
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Specializations = JsonConvert.DeserializeObject<List<SpecializationDTO>>(response.Content);
            }
        }

        public void Cancel()
        {
            NavigationManager.NavigateTo("/");
        }
        public void Dispose()
        {
            //Console.WriteLine("Сервис на добавлении сотруднриках отключён");
            Interceptor.DisposeEvent();
        }
    }
}
