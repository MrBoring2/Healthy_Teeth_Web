using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Radzen.Blazor;
using Radzen;
using Shared.DTO;
using WebSite.Services.ApiServices;
using WebSite.Models;

namespace WebSite.Pages
{
    public partial class AddVisit
    {
        [Parameter]
        public int EmployeeId { get; set; }
        [Parameter]
        public DateOnly VisitDate { get; set; }
        [Parameter]
        public TimeOnly VisitTime { get; set; }
        [Inject]
        private HubConnection HubConnection { get; set; }
        [Inject]
        private DialogService DialogService { get; set; }
        [Inject]
        private NotificationService NotificationService { get; set; }
        [Inject]

        private IPatientApiService PatientApiService { get; set; }
        [Inject]
        private IEmployeeApiService EmployeeApiService { get; set; }
        [Inject]
        private IVisitApiService VisitApiService { get; set; }
        private ODataEnumerable<PatientDTO> list;
        public string VisitPurpose { get; set; }
        private IList<PatientDTO> selectedPatients;
        private VisitDTO visit = new VisitDTO();
        private IList<PatientDTO> SelectedPatients
        {
            get => selectedPatients;
            set
            {
                selectedPatients = value;
                StateHasChanged();
            }
        }
        public EmployeeDTO Employee { get; set; }
        private RadzenDataGrid<PatientDTO> grid;
        private string fullName;
        private string FullName
        {
            get => fullName;
            set
            {
                fullName = value;
                LoadData(lastArgs);
            }
        }
        private string phoneNumber;
        private string PhoneNumber
        {
            get => phoneNumber;
            set
            {
                phoneNumber = value;
                LoadData(lastArgs);
            }
        }
        private string passport;
        private string Passport
        {
            get => passport;
            set
            {
                passport = value;
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
            visit.VisirtTime = VisitTime;
            visit.VisitDate = VisitDate;
          
            selectedPatients = new List<PatientDTO>();
            fullName = string.Empty;
            count = 10;
            await LoadEmployee();
            HubConnection.On<string>("PatientsChanged", async mes =>
            {
                await LoadData(lastArgs);
            });
        }

        private async Task LoadEmployee()
        {
            var response = await EmployeeApiService.GetAsync(EmployeeId);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Employee = response.Content;
                visit.Employee = Employee;
                StateHasChanged();
            }
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

            if (!string.IsNullOrEmpty(FullName))
            {
                queryParameters.Add("fullname", FullName);
            }
            if (!string.IsNullOrEmpty(PhoneNumber))
            {
                queryParameters.Add("phonenumber", PhoneNumber);
            }
            if (!string.IsNullOrEmpty(Passport))
            {
                queryParameters.Add("passport", passport);
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
                else if (filterOrderBy[0].Contains("Phone"))
                {
                    orderby = "Phone";
                }
                else if (filterOrderBy[0].Contains("Passport"))
                {
                    orderby = "Passport";
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
            if (grid.CurrentPage * 10 >= count)
                await grid.FirstPage();
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

        public async Task OnSubmit(VisitDTO visit)
        {
 

            visit.Employee = null;
            visit.PatientId = SelectedPatients.FirstOrDefault().Id;
            visit.EmployeeId = EmployeeId;
            var response = await VisitApiService.PostAsync(visit);
            if (response.StatusCode == System.Net.HttpStatusCode.Created || response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Success,
                    Duration = 2000,
                    Summary = "Оповещение",
                    Detail = "Услуга успешно сохранена"
                });
                DialogService.Close(true);
                DialogService.Close(true);
            }
            else
            {
                visit.Employee = Employee;
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Warning,
                    Duration = 2000,
                    Summary = "Оповещение",
                    Detail = response.Content
                });
            }
        }
        public async Task OnInvalidSubmit(FormInvalidSubmitEventArgs args)
        {
            NotificationService.Notify(new NotificationMessage
            {
                Severity = NotificationSeverity.Warning,
                Duration = 2000,
                Summary = "Оповещение",
                Detail = "Неверно заполнены данные"
            });
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
