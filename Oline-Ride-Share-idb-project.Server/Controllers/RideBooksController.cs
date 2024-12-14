using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oline_Ride_Share_idb_project.Server.Data;
using Oline_Ride_Share_idb_project.Server.Model;

namespace Oline_Ride_Share_idb_project.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RideBooksController : ControllerBase
    {
        private readonly DatabaseDbContext _context;

        public RideBooksController(DatabaseDbContext context)
        {
            _context = context;
        }

        // GET: api/RideBooks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RideBook>>> GetRideBooks()
        {
            return await _context.RideBooks.ToListAsync();
        }

        // GET: api/RideBooks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RideBook>> GetRideBook(int id)
        {
            var rideBook = await _context.RideBooks.FindAsync(id);

            if (rideBook == null)
            {
                return NotFound();
            }

            return rideBook;
        }

        // PUT: api/RideBooks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRideBook(int id, RideBook rideBook)
        {
            if (id != rideBook.RideBookId)
            {
                return BadRequest();
            }

            _context.Entry(rideBook).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RideBookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/RideBooks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RideBook>> PostRideBook(RideBook rideBook)
        {
            _context.RideBooks.Add(rideBook);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRideBook", new { id = rideBook.RideBookId }, rideBook);
        }

        // DELETE: api/RideBooks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRideBook(int id)
        {
            var rideBook = await _context.RideBooks.FindAsync(id);
            if (rideBook == null)
            {
                return NotFound();
            }

            _context.RideBooks.Remove(rideBook);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RideBookExists(int id)
        {
            return _context.RideBooks.Any(e => e.RideBookId == id);
        }
    }
}
