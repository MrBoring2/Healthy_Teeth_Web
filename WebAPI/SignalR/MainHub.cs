using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace WebAPI.SignalR
{
    [Authorize]
    public class MainHub : Hub<IMainHub>
    {
        public override Task OnConnectedAsync()
        {
           // var context = this.Context.GetHttpContext();
            var userName = Context.User.Identity.Name;
            var roleName = Context.User.FindFirst(p => p.Type == ClaimTypes.Role).Value;
            Console.WriteLine($"Подключен: {userName}/{roleName}");
            if (!string.IsNullOrWhiteSpace(userName) && !string.IsNullOrWhiteSpace(roleName))
            {
                AddGroupsToClient(roleName);
            }

            return base.OnConnectedAsync();
        }

        private void AddGroupsToClient(string roleName)
        {
            string connectionId = Context.ConnectionId;
            Groups.AddToGroupAsync(connectionId, roleName);
        }
    }
}
