//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Oline_Ride_Share_idb_project.Server.Data;
//using Oline_Ride_Share_idb_project.Server.Model;

//namespace Oline_Ride_Share_idb_project.Server.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class ChatsController : ControllerBase
//    {
//        private readonly DatabaseDbContext _context;

//        public ChatsController(DatabaseDbContext context)
//        {
//            _context = context;
//        }

//        // GET: api/Chats
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<Chat>>> GetChats()
//        {
//            return await _context.Chats.ToListAsync();
//        }

//        // GET: api/Chats/5
//        [HttpGet("{id}")]
//        public async Task<ActionResult<Chat>> GetChat(int id)
//        {
//            var chat = await _context.Chats.FindAsync(id);

//            if (chat == null)
//            {
//                return NotFound();
//            }

//            return chat;
//        }

//        // PUT: api/Chats/5
//        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//        [HttpPut("{id}")]
//        public async Task<IActionResult> PutChat(int id, Chat chat)
//        {
//            if (id != chat.ChatId)
//            {
//                return BadRequest();
//            }

//            _context.Entry(chat).State = EntityState.Modified;

//            try
//            {
//                await _context.SaveChangesAsync();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!ChatExists(id))
//                {
//                    return NotFound();
//                }
//                else
//                {
//                    throw;
//                }
//            }

//            return NoContent();
//        }

//        // POST: api/Chats
//        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//        [HttpPost]
//        public async Task<ActionResult<Chat>> PostChat(Chat chat)
//        {
//            _context.Chats.Add(chat);
//            await _context.SaveChangesAsync();

//            return CreatedAtAction("GetChat", new { id = chat.ChatId }, chat);
//        }

//        // DELETE: api/Chats/5
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteChat(int id)
//        {
//            var chat = await _context.Chats.FindAsync(id);
//            if (chat == null)
//            {
//                return NotFound();
//            }

//            _context.Chats.Remove(chat);
//            await _context.SaveChangesAsync();

//            return NoContent();
//        }

//        private bool ChatExists(int id)
//        {
//            return _context.Chats.Any(e => e.ChatId == id);
//        }
//    }
//}

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

        // Constructor
        public ChatsController(DatabaseDbContext context, IHubContext<ChatHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        // POST: api/Chats
        [HttpPost]
        public async Task<ActionResult<Chat>> PostChat(Chat chat)
        {
            // Save the chat message in the database
            chat.ChatTime = DateTime.Now; // Set the current timestamp for the chat message
            chat.SetCreateInfo();
            _context.Chats.Add(chat);
            await _context.SaveChangesAsync();

            // Send the message to the SignalR group
            await _hubContext.Clients
                .Group($"{chat.CustomerId}-{chat.EmployeeId}")  // Send to the Customer-Employee group
                .SendAsync("ReceiveMessage", chat.Message);

            return CreatedAtAction("GetChat", new { id = chat.ChatId }, chat);
        }

        // GET: api/Chats/5
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

        // GET: api/Chats/Conversation/{customerId}/{employeeId}
        [HttpGet("Conversation/{customerId}/{employeeId}")]
        public async Task<ActionResult<IEnumerable<Chat>>> GetConversation(int customerId, int employeeId)
        {
            var chats = await _context.Chats
                .Where(c => (c.CustomerId == customerId && c.EmployeeId == employeeId) ||
                            (c.CustomerId == employeeId && c.EmployeeId == customerId))
                .OrderBy(c => c.ChatTime)
                .ToListAsync();

            return chats;
        }

        // SignalR group join API
        [HttpPost("Join/{customerId}/{employeeId}")]
        public async Task<IActionResult> JoinGroup(int customerId, int employeeId)
        {
            // Use the connection ID from the SignalR hub connection
            var connectionId = HttpContext.Connection.Id; // Access the connection ID via HttpContext

            // Add the user to the SignalR group
            await _hubContext.Groups.AddToGroupAsync(connectionId, $"{customerId}-{employeeId}");
            return Ok();
        }

        // SignalR group leave API
        [HttpPost("Leave/{customerId}/{employeeId}")]
        public async Task<IActionResult> LeaveGroup(int customerId, int employeeId)
        {
            // Use the connection ID from the SignalR hub connection
            var connectionId = HttpContext.Connection.Id; // Access the connection ID via HttpContext

            // Remove the user from the SignalR group
            await _hubContext.Groups.RemoveFromGroupAsync(connectionId, $"{customerId}-{employeeId}");
            return Ok();
        }
    }
}
