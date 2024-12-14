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
    public class RideTracksController : ControllerBase
    {
        private readonly DatabaseDbContext _context;

        public RideTracksController(DatabaseDbContext context)
        {
            _context = context;
        }

        // GET: api/RideTracks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RideTrack>>> GetRideTracks()
        {
            return await _context.RideTracks.ToListAsync();
        }

        // GET: api/RideTracks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RideTrack>> GetRideTrack(int id)
        {
            var rideTrack = await _context.RideTracks.FindAsync(id);

            if (rideTrack == null)
            {
                return NotFound();
            }

            return rideTrack;
        }

        // PUT: api/RideTracks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRideTrack(int id, RideTrack rideTrack)
        {
            if (id != rideTrack.RideTrackId)
            {
                return BadRequest();
            }

            _context.Entry(rideTrack).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RideTrackExists(id))
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

        // POST: api/RideTracks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RideTrack>> PostRideTrack(RideTrack rideTrack)
        {
            _context.RideTracks.Add(rideTrack);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRideTrack", new { id = rideTrack.RideTrackId }, rideTrack);
        }

        // DELETE: api/RideTracks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRideTrack(int id)
        {
            var rideTrack = await _context.RideTracks.FindAsync(id);
            if (rideTrack == null)
            {
                return NotFound();
            }

            _context.RideTracks.Remove(rideTrack);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RideTrackExists(int id)
        {
            return _context.RideTracks.Any(e => e.RideTrackId == id);
        }
    }
}
