using Entities;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using Radzen;
using Radzen.Blazor;
using Shared.DTO;
using Shared.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        [Inject]
        NotificationService NotificationService { get; set; }

        private IApiService<RoleDTO> RoleApiService { get; set; }
        private IApiService<SpecializationDTO> SpecializaionApiService { get; set; }
        private IApiService<EmployeeDTO> EmployeeApiService { get; set; }
        private RadzenDataGrid<ScheduleDTO> grid;
        private List<RoleDTO> Roles { get; set; }
        private List<Gender> Genders;
        private List<SpecializationDTO> Specializations { get; set; }
        protected string Title = "Add";
        protected EmployeeViewModel employee = new();
        protected ObservableCollection<ScheduleDTO> schedules = new ObservableCollection<ScheduleDTO>();
        protected override async Task OnParametersSetAsync()
        {
            await LoadEmployee();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {


        }

        protected override async Task OnInitializedAsync()
        {
            employee.Schedules = new List<ScheduleDTO>();
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
            employee.Schedules = schedules.ToList();
            if (employee.Id != 0)
            {
                responseModel = await EmployeeApiService.PutAsync(employee.Id, employee);
            }
            else
            {
                responseModel = await EmployeeApiService.PostAsync(employee);
            }

            if (responseModel.StatusCode == System.Net.HttpStatusCode.Created || responseModel.StatusCode == System.Net.HttpStatusCode.OK)
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

        private async Task LoadEmployee()
        {
            if (EmployeeId != 0)
            {
                var response = await EmployeeApiService.GetAsync(EmployeeId);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    employee.FirstName = response.Content.FirstName;
                    employee.LastName = response.Content.LastName;
                    employee.MiddleName = response.Content.MiddleName;
                    employee.DateOfBirth = response.Content.DateOfBirth;
                    employee.Gender = response.Content.Gender;
                    employee.Login = response.Content.Account.Login;
                    employee.Phone = response.Content.Phone;
                    employee.RoleId = response.Content.Account.RoleId;
                    employee.SpecializationId = response.Content.SpecializationId;
                    employee.Id = response.Content.Id;
                    employee.Password = "";
                    if (response.Content.Schedules == null)
                        schedules = new ObservableCollection<ScheduleDTO>();
                    else schedules = new ObservableCollection<ScheduleDTO>(response.Content.Schedules);
                }
            }
        }

        private async Task LoadSpecializations()
        {
            var response = await SpecializaionApiService.GetAsync();
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Specializations = response.Content.ToList();
            }
        }
        public async Task AddSchedule()
        {
            ScheduleDTO schedule = await DialogService.OpenAsync<AddSchedule>($"Добавление",
              new Dictionary<string, object>() { { "ScheduleId", 0 } },
              new DialogOptions()
              {
                  Resizable = true,
                  Draggable = true,

                  Width = "300px",
                  Height = "500px"
              });

            if (schedules.FirstOrDefault(p => p.Weekday == schedule.Weekday) != null)
            {
                NotificationService.Notify(NotificationSeverity.Warning, summary: "Оповещение", detail: "Рассписание этого дня недели уже существует");
                return;
            }
            schedules.Add(schedule);
            schedules = new ObservableCollection<ScheduleDTO>(schedules.OrderBy(p => p.Weekday));
        }
        public async Task RemoveSchedule(int weekday)
        {
            schedules.Remove(schedules.FirstOrDefault(p => p.Weekday == weekday));
            schedules = new ObservableCollection<ScheduleDTO>(schedules.OrderBy(p => p.Weekday));
        }

        public void Cancel()
        {
            NavigationManager.NavigateTo("/");
        }

    }
}
