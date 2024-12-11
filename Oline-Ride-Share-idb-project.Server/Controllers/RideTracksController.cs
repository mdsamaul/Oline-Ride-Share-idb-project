<<<<<<< HEAD
﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oline_Ride_Share_idb_project.Server.Data;
using Oline_Ride_Share_idb_project.Server.Model;
using Oline_Ride_Share_idb_project.Server.Utilities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
=======
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oline_Ride_Share_idb_project.Server.Data;
using Oline_Ride_Share_idb_project.Server.Model;
>>>>>>> 59ad6d4544f47f00f5b8c1b8726896d593a07457

namespace Oline_Ride_Share_idb_project.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RideTracksController : ControllerBase
    {
        private readonly DatabaseDbContext _context;
<<<<<<< HEAD
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private Timer? _timer;
        private RideBook? _currentRideBook;
        public RideTracksController(DatabaseDbContext context, IServiceScopeFactory serviceScopeFactory)
        {
            _context = context;
            _serviceScopeFactory = serviceScopeFactory;
        }

        // POST: api/RideTracks/start
        [HttpPost("start")]
        public async Task<IActionResult> StartRideTracking(int rideBookId)
        {
            _currentRideBook = await _context.RideBooks
                .Include(rb => rb.RideTracks)
                .FirstOrDefaultAsync(rb => rb.RideBookId == rideBookId);

            if (_currentRideBook == null)
            {
                return NotFound("RideBook not found.");
            }
            _currentRideBook.StartTime = DateTime.Now;
            await _context.SaveChangesAsync();
            _timer = new Timer(UpdateRideTrack, null, TimeSpan.Zero, TimeSpan.FromSeconds(2));
            return Ok("Ride started and tracking initiated.");
        }
        private async void UpdateRideTrack(object? state)
        {
            if (_currentRideBook == null) return;
            var currentLatitude = 23.8103f; 
            var currentLongitude = 90.4125f;
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseDbContext>();
                var distance = DistanceCalculator.CalculateDistance(
                    currentLatitude,
                    currentLongitude,
                    _currentRideBook.DestinationLatitude,
                    _currentRideBook.DestinationLongitude);
                var lastRideTrack = _currentRideBook.RideTracks?.OrderByDescending(rt => rt.Timestamp).FirstOrDefault();
                if (lastRideTrack != null)
                {
                    lastRideTrack.RideTrackLatitude = currentLatitude;
                    lastRideTrack.RideTrackLongitude = currentLongitude;
                    lastRideTrack.Timestamp = DateTime.Now;
                    lastRideTrack.Distance = (float)distance;
                    lastRideTrack.TrackTime = DateTime.Now;
                    context.RideTracks.Update(lastRideTrack);
                }
                else
                {
                    var rideTrack = new RideTrack
                    {
                        RideBookId = _currentRideBook.RideBookId,
                        RideTrackLatitude = currentLatitude,
                        RideTrackLongitude = currentLongitude,
                        Timestamp = DateTime.Now,
                        Distance = (float)distance,
                        TrackTime = DateTime.Now
                    };
                    _currentRideBook.RideTracks?.Add(rideTrack);
                    context.RideTracks.Add(rideTrack);
                }
                await context.SaveChangesAsync();
                if (distance <= 100)  
                {
                    _timer?.Dispose(); 
                }
            }
        }
        // GET: api/RideTracks/{rideBookId}
        [HttpGet("{rideBookId}")]
        public async Task<IActionResult> GetRideTracks(int rideBookId)
        {
            var rideTracks = await _context.RideTracks
                .Where(rt => rt.RideBookId == rideBookId)
                .OrderBy(rt => rt.Timestamp)
                .ToListAsync();

            if (!rideTracks.Any())
            {
                return NotFound("No Ride Tracks found for the given RideBook.");
            }

            return Ok(rideTracks);
        }
        [HttpPost("stop")]
        public IActionResult StopRideTracking()
        {
            if (_timer != null)
            {
                _timer.Dispose();
                _timer = null;
                return Ok("Tracking stopped successfully.");
            }

            return BadRequest("No active tracking found.");
=======

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
>>>>>>> 59ad6d4544f47f00f5b8c1b8726896d593a07457
        }
    }
}
