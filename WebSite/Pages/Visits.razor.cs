using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Radzen.Blazor;
using Radzen;
using Shared.DTO;
using WebSite.Services.ApiServices;
using Entities;

namespace WebSite.Pages
{
    public partial class Visits
    {
        [Inject]
        private HubConnection HubConnection { get; set; }
        [Inject]
        private DialogService DialogService { get; set; }
        [Inject]
        private NotificationService NotificationService { get; set; }
        [Inject]

        private IVisitApiService VisitApiService { get; set; }
        [Inject]
        private IVisitStatusApiService VisitStatusApiService { get; set; }
        private ODataEnumerable<VisitDTO> list;
        private IList<VisitDTO> SelectedVisits { get; set; }
        private RadzenDataGrid<VisitDTO> grid;
        private List<VisitStatusDTO> Statuses { get; set; }
        private List<VisitStatusDTO> selectedStatuses;
        private List<VisitStatusDTO> SelectedStatuses
        {
            get => selectedStatuses;
            set
            {
                selectedStatuses = value;
                LoadData(lastArgs);
            }
        }
        private string doctor;
        private string Doctor
        {
            get => doctor;
            set
            {
                doctor = value;
                LoadData(lastArgs);
            }
        }
        private string patient;
        private string Patient
        {
            get => patient;
            set
            {
                patient = value;
                LoadData(lastArgs);
            }
        }
        private DateOnly startDate;
        private DateOnly StartDate
        {
            get => startDate;
            set
            {
                if (value <= EndDate)
                {
                    startDate = value;
                    LoadData(lastArgs);
                }
                startDate = value;
                LoadData(lastArgs);
            }
        }
        private DateOnly endDate;
        private DateOnly EndDate
        {
            get => endDate;
            set
            {
                if (value >= StartDate)
                {
                    endDate = value;
                    LoadData(lastArgs);
                }
            }
        }

        private bool isLoading;
        private LoadDataArgs lastArgs;
        private int count;
        protected override async Task OnInitializedAsync()
        {
            doctor = string.Empty;
            patient = string.Empty;
            startDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-7));
            endDate = DateOnly.FromDateTime(DateTime.Now);
            count = 10;

            await LoadVisitStatuses();
            HubConnection.On<string>("VisitsChanged", async mes =>
            {
                await LoadData(lastArgs);
            });
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {

            }
        }
        private async Task LoadVisitStatuses()
        {
            var response = await VisitStatusApiService.GetAsync();
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Statuses = response.Content.ToList();
                selectedStatuses = Statuses;
            }
        }
        private async Task LoadData(LoadDataArgs args)
        {
            lastArgs = args;
            isLoading = true;

            var queryParameters = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(Patient))
            {
                queryParameters.Add("patient", Patient);

            }
            if (!string.IsNullOrEmpty(Doctor))
            {
                queryParameters.Add("doctor", Doctor);
            }
            if (SelectedStatuses != null && SelectedStatuses.Count > 0)
            {
                queryParameters.Add("statusesIds", string.Join(',', SelectedStatuses.Select(p => p.Id)));
            }
            queryParameters.Add("startDate", StartDate.ToShortDateString());
            queryParameters.Add("endDate", EndDate.ToShortDateString());
           

            if (!string.IsNullOrEmpty(args.OrderBy))
            {
                string[] filterOrderBy = args.OrderBy.Split(' ');
                string orderby = "";
                string move = filterOrderBy[1];
                if (filterOrderBy[0].Contains("Id"))
                {
                    orderby = "Id";
                }
                else if (filterOrderBy[0].Contains("Patient"))
                {
                    orderby = "Patient.FullName";
                }
                else if (filterOrderBy[0].Contains("Doctor"))
                {
                    orderby = "Doctor.FullName";
                }
                else if (filterOrderBy[0].Contains("VisitDate"))
                {
                    orderby = "VisitDate";
                }
                else if (filterOrderBy[0].Contains("VisirtTime"))
                {
                    orderby = "VisirtTime";
                }
                else if (filterOrderBy[0].Contains("VisitStatus"))
                {
                    orderby = "VisitStatus.Title";
                }

                orderby += " " + move;
                queryParameters.Add("orderby", orderby);
            }
            queryParameters.Add("top", args.Top.ToString());
            queryParameters.Add("skip", args.Skip.ToString());
            var response = await VisitApiService.GetAsync(queryParameters);
            list = response.Content.Items.AsODataEnumerable();
            count = response.Content.Count;
            isLoading = false;
            StateHasChanged();
        }
        public async Task DeleteVisit(int id)
        {
            var confirm = await DialogService.Confirm("Подтвердите удаление записи", "Подтверждение", new ConfirmOptions() { OkButtonText = "Да", CancelButtonText = "Нет" });
            if (confirm == true)
            {
                var response = await VisitApiService.DeleteAsync(id);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    NotificationService.Notify(new NotificationMessage
                    {
                        Severity = NotificationSeverity.Success,
                        Duration = 2000,
                        Summary = "Оповещение",
                        Detail = "Запись успешно удалена"
                    });
                }
                else
                {
                    NotificationService.Notify(new NotificationMessage
                    {
                        Severity = NotificationSeverity.Warning,
                        Duration = 2000,
                        Summary = "Оповещение",
                        Detail = response.Content
                    });
                }
            }
        }
        public async Task OpenVisitWindow()
        {
            await DialogService.OpenAsync<AddService>($"Добавление",
               new Dictionary<string, object>() { { "VisitId", 0 } },
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
