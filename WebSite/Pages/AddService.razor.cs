using Entities;
using Microsoft.AspNetCore.Components;
using Radzen;
using Shared.DTO;
using Shared.Models;
using System.Collections.ObjectModel;
using System.Net.Http.Json;
using WebSite.Models;
using WebSite.Services;
using WebSite.Services.ApiServices;

namespace WebSite.Pages
{
    public partial class AddService
    {
        [Parameter]
        public int ServiceId { get; set; }
        [Inject]
        public DialogService DialogService { get; set; }
        [Inject]
        NotificationService NotificationService { get; set; }
        [Inject]
        private IServiceApiService ServiceApiService { get; set; }
        [Inject]
        private ISpecializationApiService SpecializaionApiService { get; set; }
        protected string Title = "Add";
        private List<SpecializationDTO> Specializations { get; set; }
        protected ServiceDTO service = new();
        protected override async Task OnParametersSetAsync()
        {
            await LoadService();
        }
        protected override async Task OnInitializedAsync()
        {
            await LoadSpecializations();
        }
        private async Task LoadSpecializations()
        {
            var response = await SpecializaionApiService.GetAsync();
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Specializations = response.Content.ToList();
                Specializations.Remove(Specializations.FirstOrDefault(p => p.Title == "Нет"));
            }
        }
        protected async Task SaveService()
        {
            ResponseModel<string> responseModel;
            if (service.Id != 0)
            {
                responseModel = await ServiceApiService.PutAsync(service.Id, service);
            }
            else
            {
                responseModel = await ServiceApiService.PostAsync(service);
            }

            if (responseModel.StatusCode == System.Net.HttpStatusCode.Created || responseModel.StatusCode == System.Net.HttpStatusCode.OK)
            {
                DialogService.Close(true);
            }
            else
            {

            }
        }

        private async Task LoadService()
        {
            if (ServiceId != 0)
            {
                var response = await ServiceApiService.GetAsync(ServiceId);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    service.Title = response.Content.Title;
                    service.Price = response.Content.Price;
                    service.SpecializationId = response.Content.SpecializationId;
                    service.Id = response.Content.Id;
                }
            }
        }


        public void Cancel()
        {
            NavigationManager.NavigateTo("/");
        }

    }
}
