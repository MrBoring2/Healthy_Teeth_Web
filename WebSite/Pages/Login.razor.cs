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
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
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
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Success,
                    Duration = 2000,
                    Summary = "Оповщение",
                    Detail = "Добро пожаловать"
                });

            }
            else if(result.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Duration = 2000,
                    Summary = "Критическая ошибка",
                    Detail = "Не удаётся соединиться с сервером или с базой данных"
                });
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
