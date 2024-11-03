using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Radzen.Blazor;
using Radzen;
using Shared.DTO;
using WebSite.Services.ApiServices;

namespace WebSite.Pages
{
    public partial class Patients
    {
        [Inject]
        private HubConnection HubConnection { get; set; }
        [Inject]
        private DialogService DialogService { get; set; }
        [Inject]
        private NotificationService NotificationService { get; set; }
        [Inject]

        private IPatientApiService PatientApiService { get; set; }
        private ODataEnumerable<PatientDTO> list;
        private IList<PatientDTO> SelectedPatients { get; set; }
        private RadzenDataGrid<PatientDTO> grid;
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
        private bool isLoading;
        private LoadDataArgs lastArgs;
        private int count;
        private string[] FilterNames { get; set; } = new string[]
            {
                "search",
                "orderBy"
            };

        protected override async Task OnInitializedAsync()
        {
            search = string.Empty;

            count = 10;

            HubConnection.On<string>("PatientsChanged", async mes =>
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

        private async Task LoadData(LoadDataArgs args)
        {
            lastArgs = args;
            isLoading = true;

            var queryParameters = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(Search))
            {
                queryParameters.Add("search", Search);
            }
           
            if (!string.IsNullOrEmpty(args.OrderBy))
            {
                string[] filterOrderBy = args.OrderBy.Split(' ');
                string orderby = "";
                string move = filterOrderBy[1];
                if (filterOrderBy[0].Contains("Id"))
                {
                    orderby = "Id";
                }
                else if (filterOrderBy[0].Contains("FullName"))
                {
                    orderby = "FullName";
                }
                else if (filterOrderBy[0].Contains("Specialization"))
                {
                    orderby = "Specialization.Title";
                }
                else if (filterOrderBy[0].Contains("Role"))
                {
                    orderby = "Account.Role.Title";
                }
                else if (filterOrderBy[0].Contains("Login"))
                {
                    orderby = "Account.Login";
                }
                else if (filterOrderBy[0].Contains("Gender"))
                {
                    orderby = "Gender";
                }

                orderby += " " + move;
                queryParameters.Add("orderby", orderby);
            }
            queryParameters.Add("top", args.Top.ToString());
            queryParameters.Add("skip", args.Skip.ToString());
            var response = await PatientApiService.GetAsync(queryParameters);
            list = response.Content.Items.AsODataEnumerable();
            count = response.Content.Count;
            Console.WriteLine(count);
            isLoading = false;
            StateHasChanged();
        }
        public async Task DeletePatient(int id)
        {
            var confirm = await DialogService.Confirm("Подтвердите удаление пациента", "Подтверждение", new ConfirmOptions() { OkButtonText = "Да", CancelButtonText = "Нет" });
            if (confirm == true)
            {
                var response = await PatientApiService.DeleteAsync(id);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    NotificationService.Notify(new NotificationMessage
                    {
                        Severity = NotificationSeverity.Success,
                        Duration = 2000,
                        Summary = "Оповещение",
                        Detail = "Пациент успешно удалён"
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
        public async Task OpenPatientWindow()
        {
            await DialogService.OpenAsync<AddPatient>($"Добавление",
               new Dictionary<string, object>() { { "PatientId", 0 } },
               new DialogOptions()
               {
                   Resizable = true,
                   Draggable = true,

                   Width = "500px",
                   Height = "720px"
               });

        }

        public async Task OpenEditPatientWindow(int id)
        {
            await DialogService.OpenAsync<AddPatient>($"Редактирование",
             new Dictionary<string, object>() { { "PatientId", id } },
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
