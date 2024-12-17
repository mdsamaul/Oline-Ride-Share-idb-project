using Microsoft.AspNetCore.SignalR;

namespace Oline_Ride_Share_idb_project.Server.Hubs
{
    public class ChatHub : Hub
    {
        // Method to send a message to the specific group (Customer-Employee group)
        public async Task SendMessage(string customerId, string employeeId, string message)
        {
            await Clients.Group($"{customerId}-{employeeId}")
                .SendAsync("ReceiveMessage", message);
        }

        // Method for a client to join a group based on Customer and Employee IDs
        public async Task JoinGroup(string customerId, string employeeId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"{customerId}-{employeeId}");
        }

        // Method for a client to leave a group
        public async Task LeaveGroup(string customerId, string employeeId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"{customerId}-{employeeId}");
        }
    }
}
