using Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using Radzen.Blazor;
using Radzen;
using Shared.DTO;
using WebSite.Services;
using WebSite.Services.ApiServices;

namespace WebSite.Pages
{
    public partial class Services
    {
        [Inject]
        private HubConnection HubConnection { get; set; }
        [Inject]
        private DialogService DialogService { get; set; }
        [Inject]
        private NotificationService NotificationService { get; set; }
        [Inject]
        private ISpecializationApiService SpecalizationApiService { get; set; }
        [Inject]

        private IServiceApiService ServiceApiService { get; set; }
        private ODataEnumerable<ServiceDTO> list;
        private IList<ServiceDTO> SelectedPatients { get; set; }
        private RadzenDataGrid<ServiceDTO> grid;
        private List<SpecializationDTO> Specializations { get; set; }
        private List<SpecializationDTO> selectedSpecializations;
        private List<SpecializationDTO> SelectedSpecializations
        {
            get => selectedSpecializations;
            set
            {

                selectedSpecializations = value;
                LoadData(lastArgs);
            }
        }
        private string searchTitle;
        private string SearchTitle
        {
            get => searchTitle;
            set
            {
                searchTitle = value;
                LoadData(lastArgs);
            }
        }

        private bool isLoading;
        private LoadDataArgs lastArgs;
        private int count;
        protected override async Task OnInitializedAsync()
        {
            searchTitle = string.Empty;

            count = 10;
            await LoadSpecializations();
            HubConnection.On<string>("ServicesChanged", async mes =>
            {
                await LoadData(lastArgs);
            });
        }

        private async Task LoadData(LoadDataArgs args)
        {
            lastArgs = args;
            isLoading = true;

            var queryParameters = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(SearchTitle))
            {
                queryParameters.Add("search", SearchTitle);
            }
            if (SelectedSpecializations != null && SelectedSpecializations.Count > 0)
            {
                queryParameters.Add("spesializationIds", string.Join(',', SelectedSpecializations.Select(p => p.Id)));
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
                else if (filterOrderBy[0].Contains("Specialization"))
                {
                    orderby = "Specialization.Title";
                }
                else if (filterOrderBy[0].Contains("Title"))
                {
                    orderby = "Title";
                }
                else if (filterOrderBy[0].Contains("Price"))
                {
                    orderby = "Price";
                }


                orderby += " " + move;
                queryParameters.Add("orderby", orderby);
            }
            queryParameters.Add("top", args.Top.ToString());
            queryParameters.Add("skip", args.Skip.ToString());
            var response = await ServiceApiService.GetAsync(queryParameters);
            list = response.Content.Items.AsODataEnumerable();
            count = response.Content.Count;
            if (grid.CurrentPage * 10 >= count)
                await grid.FirstPage();
            isLoading = false;
            StateHasChanged();
        }

        private async Task LoadSpecializations()
        {
            var response = await SpecalizationApiService.GetAsync();
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Specializations = response.Content.ToList();
                selectedSpecializations = Specializations;
            }
        }


        public async Task DeleteService(int id)
        {
            var confirm = await DialogService.Confirm("Подтвердите удаление услуги", "Подтверждение", new ConfirmOptions() { OkButtonText = "Да", CancelButtonText = "Нет" });
            if (confirm == true)
            {
                var response = await ServiceApiService.DeleteAsync(id);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    NotificationService.Notify(new NotificationMessage
                    {
                        Severity = NotificationSeverity.Success,
                        Duration = 2000,
                        Summary = "Оповещение",
                        Detail = "Услуга успешно удалена"
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
        public async Task OpenServiceWindow()
        {
            await DialogService.OpenAsync<AddService>($"Добавление",
               new Dictionary<string, object>() { { "ServiceId", 0 } },
               new DialogOptions()
               {
                   Resizable = true,
                   Draggable = true,

                   Width = "500px",
                   Height = "720px"
               });

        }

        public async Task OpenEditServiceWindow(int id)
        {
            await DialogService.OpenAsync<AddService>($"Редактирование",
             new Dictionary<string, object>() { { "ServiceId", id } },
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
