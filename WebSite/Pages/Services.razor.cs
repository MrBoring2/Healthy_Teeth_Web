using Entities;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using WebSite.Services;

namespace WebSite.Pages
{
    public partial class Services : IDisposable
    {
        [Inject]
        public HttpInterceptorService Interceptor { get; set; }
        private List<Service> list = new List<Service>();

        protected override async Task OnInitializedAsync()
        {
            Interceptor.RegisterEvents();
            await LoadEmployees();
        }


        private async Task LoadEmployees()
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

        public void Dispose() => Interceptor.DisposeEvent();
    }
}
