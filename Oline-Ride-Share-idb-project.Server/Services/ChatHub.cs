using Microsoft.AspNetCore.SignalR;

namespace Oline_Ride_Share_idb_project.Server.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string customerId, string employeeId, string message)
        {
            await Clients.Group($"{customerId}-{employeeId}")
                .SendAsync("ReceiveMessage", message);
        }
        public async Task JoinGroup(string customerId, string employeeId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"{customerId}-{employeeId}");
        }
        public async Task LeaveGroup(string customerId, string employeeId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"{customerId}-{employeeId}");
        }
    }
}
