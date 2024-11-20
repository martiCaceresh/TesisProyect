using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using OptiArroz_Tesis_Proyect.Data.Interfaces;
using OptiArroz_Tesis_Proyect.Models.Entities;
using System.Collections.Concurrent;

namespace OptiArroz_Tesis_Proyect.Services
{
    public class SignalRHub : Hub
    {
        public readonly UserManager<ApplicationUser> userManager;
        public static readonly ConcurrentDictionary<string, HashSet<string>> userConnections = new ConcurrentDictionary<string, HashSet<string>>();
        public readonly INotificationSenderDA notificationDA;
        private readonly IHubContext<SignalRHub> hubContext;

        public SignalRHub(UserManager<ApplicationUser> userManager, INotificationSenderDA notificationDA, IHubContext<SignalRHub> hubContext)
        {
            this.userManager = userManager;
            this.notificationDA = notificationDA;
            this.hubContext = hubContext;
        }

        public async Task SendToRole(string roleName, string title, string message, string messageType, int type, string url, string userId)
        {
            var usersInRole = await userManager.GetUsersInRoleAsync(roleName);
            foreach (var user in usersInRole.Where(x => x.Id != userId))
            {
                Notification notification = new Notification(user.Id, message, messageType, type, url, 1, DateTime.Now, new DateTime());
                await notificationDA.Insert(notification);

                var connections = GetConnectionsForUser(user.Id);
                foreach (var connectionId in connections)
                {
                    await hubContext.Clients.Client(connectionId).SendAsync("SendToRole", title, message, messageType, url, notification.IdNotification);
                }
            }
        }

        public async Task SendToUser(string title, string message, string messageType, int type, string url, string userId)
        {
            Notification notification = new Notification(userId, message, messageType, type, url, 1, DateTime.Now, new DateTime());
            await notificationDA.Insert(notification);

            var connections = GetConnectionsForUser(userId);
            foreach (var connectionId in connections)
            {
                await hubContext.Clients.Client(connectionId).SendAsync("SendToUser", title, message, messageType, url, notification.IdNotification);
            }
        }

        public override Task OnConnectedAsync()
        {
            string userId = Context.UserIdentifier;
            string connectionId = Context.ConnectionId;

            if (!string.IsNullOrEmpty(userId))
            {
                userConnections.AddOrUpdate(userId,
                    new HashSet<string> { connectionId },
                    (_, connections) =>
                    {
                        connections.Add(connectionId);
                        return connections;
                    });
            }

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            string userId = Context.UserIdentifier;
            string connectionId = Context.ConnectionId;

            if (!string.IsNullOrEmpty(userId))
            {
                userConnections.AddOrUpdate(userId,
                    new HashSet<string>(),
                    (_, connections) =>
                    {
                        connections.Remove(connectionId);
                        return connections;
                    });
            }

            return base.OnDisconnectedAsync(exception);
        }

        private IEnumerable<string> GetConnectionsForUser(string userId)
        {
            if (userConnections.TryGetValue(userId, out HashSet<string> connections))
            {
                return connections;
            }

            return Enumerable.Empty<string>();
        }
    }
}
