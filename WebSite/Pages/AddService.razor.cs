using Entities;
using Microsoft.AspNetCore.Components;
using Shared.Models;
using System.Net.Http.Json;
using WebSite.Models;
using WebSite.Services;

namespace WebSite.Pages
{
    public partial class AddService
    {
        [Parameter]
        public int userId { get; set; }
        protected string Title = "Add";
        protected ServiceViewModel service = new();
        protected override async Task OnParametersSetAsync()
        {

        }
        protected override async Task OnInitializedAsync()
        {
            
        }
        protected async Task SaveUser()
        {
            ResponseModel response;
            if (service.Id != 0)
            {
                response = await _apiService.PutAsync(service.Id, service);
            }
            else
            {
                response = await _apiService.PostAsync( service);
            }
            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                Cancel();
            }

        }
        public void Cancel()
        {
            NavigationManager.NavigateTo("/");
        }
 
    }
}
