using Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using WebSite.Services;

namespace WebSite.Pages
{
    public partial class Services : IDisposable
    {
        //[Inject]
        //HubConnection HubConnection { get; set; }
        [Inject]
        HubConnection HubConnection { get; set; }   
        [Inject]
        public HttpInterceptorService Interceptor { get; set; }
        private List<Service> list = new List<Service>();

        protected override async Task OnInitializedAsync()
        {
            Console.WriteLine("Статус хаба: " + HubConnection.State);
            Console.WriteLine("Сервис на списке сервисов включёг");
            await RegisterEvents(); 
            await LoadServices();

            //HubConnection.On<Service>("ServiceAdded", async service =>
            //{
            //    await LoadServices();
            //});
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                
            }
        }


        private async Task LoadServices()
        {
            var response = await _apiService.GetAsync("api/services");
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                list = JsonConvert.DeserializeObject<List<Service>>(response.Content);
            }
            else
            {
                return;
            }
            StateHasChanged();
        }
        public async Task RegisterEvents()
        {
            await Task.Run(() => Interceptor.RegisterEvents());
            await Task.Run(() => 
            {
                HubConnection.On<string>("ServiceAdded", async mes =>
                {
                    await LoadServices();
                });
            });
        }

        public void Dispose()
        {
            Console.WriteLine("Сервис на списке сервисов отключён");
            Interceptor.DisposeEvent();
            //HubConnection.Remove("ServiceAdded");
        }
    }
}
