using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oline_Ride_Share_idb_project.Server.Data;
using Oline_Ride_Share_idb_project.Server.Model;
using Oline_Ride_Share_idb_project.Server.Utilities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Oline_Ride_Share_idb_project.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RideTracksController : ControllerBase
    {
        private readonly DatabaseDbContext _context;
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
        }
    }
}
