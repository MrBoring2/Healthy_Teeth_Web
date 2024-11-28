using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using Radzen;
using Shared.DTO;
using Shared.Models;
using System.Collections.ObjectModel;
using WebSite.Models;
using WebSite.Services.ApiServices;

namespace WebSite.Pages
{
    public partial class AddPatient  
    {
        [Parameter]
        public int PatientId{ get; set; }
        [Inject]
        public DialogService DialogService { get; set; }
        [Inject]
        NotificationService NotificationService { get; set; }
        [Inject]
        private IPatientApiService PatientApiService { get; set; }
        private List<Gender> Genders;
        protected string Title = "Add";
        protected PatientDTO patient = new();
        protected ObservableCollection<ScheduleDTO> schedules = new ObservableCollection<ScheduleDTO>();
        protected override async Task OnParametersSetAsync()
        {
            await LoadPatient();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {


        }

        protected override async Task OnInitializedAsync()
        {
            Genders = new List<Gender>
            {
                new Gender{ Id = 0, Title = "Мужской"},
                new Gender{ Id = 1, Title = "Женский"}
            };
        }

        protected async Task SaveUser()
        {
            ResponseModel<string> responseModel;
            if (patient.Id != 0)
            {
                responseModel = await PatientApiService.PutAsync(patient.Id, patient);
            }
            else
            {
                responseModel = await PatientApiService.PostAsync(patient);
            }

            if (responseModel.StatusCode == System.Net.HttpStatusCode.Created || responseModel.StatusCode == System.Net.HttpStatusCode.OK)
            {
                DialogService.Close(true);
            }
            else
            {

            }
        }
      
        private async Task LoadPatient()
        {
            if (PatientId != 0)
            {
                var response = await PatientApiService.GetAsync(PatientId);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    patient.FirstName = response.Content.FirstName;
                    patient.LastName = response.Content.LastName;
                    patient.MiddleName = response.Content.MiddleName;
                    patient.DateOfBirth = response.Content.DateOfBirth;
                    patient.Gender = response.Content.Gender;
                    patient.Address = response.Content.Address;
                    patient.Phone = response.Content.Phone;
                    patient.DateOfBirth = response.Content.DateOfBirth;
                    patient.City = response.Content.City;
                    patient.MedicalPolicy = response.Content.MedicalPolicy;
                    patient.PassportCode = response.Content.PassportCode;
                    patient.PassportNumber = response.Content.PassportNumber;                  
                    patient.Id = response.Content.Id;                  
                }
            }
        }

       
        public void Cancel()
        {

            DialogService.Close();
        }
    }
}
