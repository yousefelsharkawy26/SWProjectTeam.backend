using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Models;

namespace DentalManagementSystem.SignalRHubs
{
    public class NotificationHub : Hub
    {
        private readonly UserManager<User> _userManager;
        public NotificationHub(UserManager<User> userManager) 
        {
            _userManager = userManager;
        }
        public async Task SendNotificationToAll(string message)
        {
            await Clients.All.SendAsync("ReceiveNotification", message);
        }

        public async Task SendNotificationToUser(string clientId, string message)
        {
            await Clients.User(clientId).SendAsync("ReceiveNotification", message);
        }

        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }

        public override async Task OnConnectedAsync()
        {
            Console.WriteLine($"Client connected: {Context.ConnectionId}");
            await base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            Console.WriteLine($"Client disconnected: {Context.ConnectionId}");
            return base.OnDisconnectedAsync(exception);
        }
    }
}
