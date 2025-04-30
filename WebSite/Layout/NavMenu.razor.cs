using Microsoft.AspNetCore.Components;
using WebSite.Providers;
using WebSite.Services;

namespace WebSite.Layout
{
    public partial class NavMenu
    {
        [Inject]
        private CustomStateProvider AuthStatePrivder { get; set; }
        [Inject]
        public NavigationManager Nagivation { get; set; }

        private bool collapseNavMenu = true;

        private string? NavMenuCssClass => collapseNavMenu ? "collapse" : null;

        private void ToggleNavMenu()
        {
            collapseNavMenu = !collapseNavMenu;
        }
        private async Task OnExitClicked()
        {
            var result = await AuthStatePrivder.LogoutAsync();
            if (result.Success)
                Nagivation.NavigateTo("/login");
        }
    }
}
