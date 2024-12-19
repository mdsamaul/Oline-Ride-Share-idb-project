using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oline_Ride_Share_idb_project.Server.Data;
using Oline_Ride_Share_idb_project.Server.Model;
using Oline_Ride_Share_idb_project.Server.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oline_Ride_Share_idb_project.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatsController : ControllerBase
    {
        private readonly DatabaseDbContext _context;
        private readonly IHubContext<ChatHub> _hubContext;
        public ChatsController(DatabaseDbContext context, IHubContext<ChatHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }
        [HttpPost]
        public async Task<ActionResult<Chat>> PostChat(Chat chat)
        {
            chat.ChatTime = DateTime.Now;
            chat.SetCreateInfo();
            _context.Chats.Add(chat);
            await _context.SaveChangesAsync();
            await _hubContext.Clients
                .Group($"{chat.CustomerId}-{chat.DriverId}") 
                .SendAsync("ReceiveMessage", chat.Message);
            return CreatedAtAction("GetChat", new { id = chat.ChatId }, chat);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Chat>> GetChat(int id)
        {
            var chat = await _context.Chats.FindAsync(id);
            if (chat == null)
            {
                return NotFound();
            }
            return chat;
        }
        [HttpGet("Conversation/{customerId}/{employeeId}")]
        public async Task<ActionResult<IEnumerable<Chat>>> GetConversation(int customerId, int employeeId)
        {
            var chats = await _context.Chats
                .Where(c => (c.CustomerId == customerId && c.DriverId == employeeId) ||
                            (c.CustomerId == employeeId && c.DriverId == customerId))
                .OrderBy(c => c.ChatTime)
                .ToListAsync();
            return chats;
        }
        [HttpPost("Join/{customerId}/{employeeId}")]
        public async Task<IActionResult> JoinGroup(int customerId, int employeeId)
        {
            var connectionId = HttpContext.Connection.Id; 
            await _hubContext.Groups.AddToGroupAsync(connectionId, $"{customerId}-{employeeId}");
            return Ok();
        }
        [HttpPost("Leave/{customerId}/{employeeId}")]
        public async Task<IActionResult> LeaveGroup(int customerId, int employeeId)
        {
            var connectionId = HttpContext.Connection.Id; 
            await _hubContext.Groups.RemoveFromGroupAsync(connectionId, $"{customerId}-{employeeId}");
            return Ok();
        }
    }
}
