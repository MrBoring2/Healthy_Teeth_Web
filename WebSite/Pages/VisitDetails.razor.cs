using Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Radzen;
using Radzen.Blazor;
using Shared.Constants;
using Shared.DTO;
using WebSite.Models;
using WebSite.Services.ApiServices;

namespace WebSite.Pages
{
    public partial class VisitDetails
    {
        [Parameter]
        public int VisitId { get; set; }
        [Inject]
        public DialogService DialogService { get; set; }
        [Inject]
        private NotificationService NotificationService { get; set; }
        [Inject]

        private IServiceApiService ServiceApiService { get; set; }
        [Inject]

        private IVisitApiService VisitApiService { get; set; }
        public List<ServiceDTO> Services { get; set; }
        public List<ServiceInVisit> SelectedServices { get; set; } = new List<ServiceInVisit>();
        private ServiceDTO selectedService;
        RadzenDataGrid<ServiceInVisit> servicesGrid;
        public double TotalPrice => SelectedServices.Sum(p => p.Service == null ? 0 : p.Quantity * p.Service.Price);
        public ServiceDTO SelectedService
        {
            get => selectedService;
            set
            {
                selectedService = value;
            }
        }
        protected VisitDTO visit = new();

        protected override async Task OnParametersSetAsync()
        {
            await LoadVisit();
            await LoadServices();
        }

        private async Task LoadVisit()
        {
            var response = await VisitApiService.GetAsync(VisitId);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                visit = response.Content;
                if (visit.ServiceToVisits.Count != 0)
                {
                    foreach (var item in visit.ServiceToVisits)
                    {
                        SelectedServices.Add(new ServiceInVisit { Service = item.Service, Quantity = item.Count });
                    }
                }
            }
            else
            {
                return;
            }
            StateHasChanged();
        }
        private async Task LoadServices()
        {
            var response = await ServiceApiService.GetForSpesializationAsync(visit.Employee.SpecializationId);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Services = response.Content.ToList();
            }
            else
            {
                return;
            }
            StateHasChanged();
        }

        private async Task Submit(VisitDTO arg)
        {
            var confirm = await DialogService.Confirm("Завершить посещение?", "Подтверждение", new ConfirmOptions() { OkButtonText = "Да", CancelButtonText = "Нет" });
            if (confirm == false)
                return;

            if (SelectedServices.Count == 0)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Warning,
                    Duration = 2000,
                    Summary = "Оповещение",
                    Detail = "Количество услуг не должно быть 0"
                });
                return;
            }

            if (SelectedServices.Any(p => p.Quantity <= 0))
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Warning,
                    Duration = 2000,
                    Summary = "Оповещение",
                    Detail = "Все выбранные услуги должны быть проведены хотя бы 1 раз"
                });
                return;
            }


            arg.ServiceToVisits = SelectedServices.Select(p => new ServiceToVisitDTO { ServiceId = p.Service.Id, Count = p.Quantity }).ToList();
            arg.VisitStatusId = (int)VisitStatuses.Compleated;

            Console.WriteLine(arg.VisitPurpose);
            Console.WriteLine(arg.VisitDiagnos);
            ResponseModel<string> responseModel = await VisitApiService.PutAsync(arg.Id, arg);

            if (responseModel.StatusCode == System.Net.HttpStatusCode.Created || responseModel.StatusCode == System.Net.HttpStatusCode.OK)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Success,
                    Duration = 2000,
                    Summary = "Оповещение",
                    Detail = "Посещение успешно проведено"
                });
                DialogService.Close(true);
            }
            else
            {

            }
        }
        async Task InsertRow()
        {
            var service = new ServiceInVisit();
            SelectedServices.Add(service);
            await servicesGrid.InsertRow(service);
        }
        async Task EditRow(ServiceInVisit service)
        {

            await servicesGrid.EditRow(service);
        }
        async Task DeleteRow(ServiceInVisit service)
        {
            if (service.Service == null)
                return;

            if (SelectedServices.Any(p => p.Service.Id == service.Service.Id))
            {
                SelectedServices.Remove(SelectedServices.FirstOrDefault(p => p.Service.Id == service.Service.Id));
                servicesGrid.CancelEditRow(service);
                await servicesGrid.Reload();
                StateHasChanged();
            }
            else
            {

            }
        }
        async Task SaveRow(ServiceInVisit service)
        {
            if (service.Service == null)
                return;

            if (SelectedServices.Count(p => p.Service.Id == service.Service.Id) > 1)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Warning,
                    Duration = 2000,
                    Summary = "Оповещение",
                    Detail = "Данная услуга уже есть в списке"
                });
                return;
            }


            await servicesGrid.UpdateRow(service);
        }
        void Cancel()
        {

            DialogService.Close();
        }

        void CancelEdit(ServiceInVisit service)
        {
            servicesGrid.CancelEditRow(service);
        }
    }
}
