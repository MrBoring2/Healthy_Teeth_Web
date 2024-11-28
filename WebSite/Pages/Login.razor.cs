using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Radzen;
using Shared.Models;
using System.Security.Claims;
using WebSite.Providers;

namespace WebSite.Pages
{
    public partial class Login
    {
        [Inject] NavigationManager Navigation { get; set; }
        [Inject] CustomStateProvider AuthStateProvider { get; set; }
        [Inject] NotificationService NotificationService { get; set; }
        private string ErrorMessage { get; set; }
        private EditContext EditContext { get; set; }
        /// <summary>
        /// User name
        /// </summary>
        LoginModel loginModel = new LoginModel();
        string message = string.Empty;
        bool isDisabled = false;
        public Login()
        {
            EditContext = new EditContext(this);
        }
        private async void Authenticate()
        {
            if (!EditContext.Validate())
                return;
            var result = await AuthStateProvider.LoginAsync(loginModel);
            if (result.Success)
            {
                var user = await AuthStateProvider.GetAuthenticationStateAsync();
                if ((user.User.Claims?.FirstOrDefault(p => p.Type == ClaimTypes.Role)?.Value == "Администратор" || user.User.Claims?.FirstOrDefault(p => p.Type == ClaimTypes.Role)?.Value == "Регистратор"))
                {
                    Navigation.NavigateTo("/doctors-schedule");
                }
                else
                {
                    Navigation.NavigateTo("/doctor-schedule");
                }

            }
            else
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Warning,
                    Duration = 2000,
                    Summary = "Оповещение",
                    Detail = "Неверный логин или пароль"
                });
            }
        }
    }
}
