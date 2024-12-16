using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oline_Ride_Share_idb_project.Server.Data;
using Oline_Ride_Share_idb_project.Server.Model;
using Oline_Ride_Share_idb_project.Server.Services;
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

        private DistanceService _distanceService { get; }

        public RideTracksController(DatabaseDbContext context, IServiceScopeFactory serviceScopeFactory, DistanceService distanceService)
        {
            _context = context;
            _serviceScopeFactory = serviceScopeFactory;
            _distanceService = distanceService;
        }

        // POST: api/RideTracks/start
        [HttpPost("start")]
        public async Task<IActionResult> StartRideTracking(int rideBookId, float latitude, float longitude)
        {
            // Search for the RideBook
            _currentRideBook = await _context.RideBooks
                .Include(rb => rb.RideTracks)
                .FirstOrDefaultAsync(rb => rb.RideBookId == rideBookId);

            if (_currentRideBook == null)
            {
                return NotFound("RideBook not found.");
            }

            // Set StartTime
            _currentRideBook.StartTime = DateTime.Now;
            await _context.SaveChangesAsync();

            // Start the timer to update the ride track every 2 seconds
            _timer = new Timer(UpdateRideTrack, new { Latitude = latitude, Longitude = longitude }, TimeSpan.Zero, TimeSpan.FromSeconds(2));

            return Ok("Ride started and tracking initiated.");
        }

        // Method to update the ride track every 2 seconds
        private async void UpdateRideTrack(object? state)
        {
            if (_currentRideBook == null) return;

            // Extract latitude and longitude from the state object
            var data = (dynamic)state;
            var currentLatitude = data.Latitude;
            var currentLongitude = data.Longitude;

            Console.WriteLine($"Updating Track: Latitude = {currentLatitude}, Longitude = {currentLongitude}");

            // Create a new scope to resolve a fresh instance of the context
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseDbContext>();

                // Calculate the distance to the destination
                var distance = _distanceService.CalculateDistance(
                    currentLatitude,
                    currentLongitude,
                    _currentRideBook.DestinationLatitude,
                    _currentRideBook.DestinationLongitude) * 1000;

                // Log the calculated distance
                Console.WriteLine($"Distance to Destination: {distance} meters");

                // Check if there's already a recent RideTrack (e.g., within the last 2 seconds)
                var lastRideTrack = _currentRideBook.RideTracks?
                    .OrderByDescending(rt => rt.Timestamp)
                    .FirstOrDefault();

                if (lastRideTrack != null)
                {
                    // Update the last ride track (if any)
                    lastRideTrack.RideTrackLatitude = currentLatitude;
                    lastRideTrack.RideTrackLongitude = currentLongitude;
                    lastRideTrack.Distance = (int)distance;
                    lastRideTrack.TrackTime = DateTime.Now;

                    // Explicitly mark the entity as modified
                    context.Entry(lastRideTrack).State = EntityState.Modified;

                    // Log update
                    Console.WriteLine($"Updated RideTrack at {lastRideTrack.TrackTime}");

                    await context.SaveChangesAsync();
                }
                else
                {
                    // If no previous track exists, add a new one
                    var rideTrack = new RideTrack
                    {
                        RideBookId = _currentRideBook.RideBookId,
                        RideTrackLatitude = currentLatitude,
                        RideTrackLongitude = currentLongitude,
                        Timestamp = DateTime.Now,
                        Distance = (int)(double)distance,
                        TrackTime = DateTime.Now
                    };
                    rideTrack.SetCreateInfo();

                    // Log new entry
                    Console.WriteLine($"Added new RideTrack at {rideTrack.TrackTime}");

                    // Add the new RideTrack to both the current RideBook and the context
                    _currentRideBook.RideTracks?.Add(rideTrack);
                    context.RideTracks.Add(rideTrack);
                }

                await context.SaveChangesAsync();

                // Stop tracking if the destination is reached (within 100 meters)
                if (distance <= 100)  // Within 100 meters of the destination
                {
                    _timer?.Dispose();  // Stop the timer
                    Console.WriteLine("Destination Reached! Stopping tracking.");
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
                Console.WriteLine("Stopping tracking...");
                _timer.Dispose(); // Dispose the timer to stop updates
                _timer = null;    // Set the timer to null to avoid reusing it

                // Log that tracking was stopped
                Console.WriteLine("Tracking stopped successfully.");
                return Ok("Tracking stopped successfully.");
            }

            // Log if no active tracking is found
            Console.WriteLine("No active tracking found.");
            return BadRequest("No active tracking found.");
        }
    }
}
