using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace WebAPI.SignalR
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        public string? GetUserId(HubConnectionContext connection)
        {
            var userId = connection.User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Name).Value;
            Console.WriteLine("Найден USERID: " + userId);
            return userId;
        }
    }
}
