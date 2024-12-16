using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oline_Ride_Share_idb_project.Server.Data;
using Oline_Ride_Share_idb_project.Server.Model;
using Oline_Ride_Share_idb_project.Server.Model.vm;
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
        private static Timer? _timer;
        private RideBook? _currentRideBook;
        private readonly DistanceService _distanceService;
        private static double distance;
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
            try
            {
                // রাইডবুক খুঁজে বের করা
                _currentRideBook = await _context.RideBooks
                    .Include(rb => rb.RideTracks)
                    .FirstOrDefaultAsync(rb => rb.RideBookId == rideBookId);

                if (_currentRideBook == null)
                {
                    return NotFound("RideBook not found.");
                }

                // রাইড শুরু করার সময় নির্ধারণ
                _currentRideBook.StartTime = DateTime.Now;
                await _context.SaveChangesAsync();

                // যদি ট্যামার আগে থেকেই চলতে থাকে তবে এটি বন্ধ করে দেওয়া
                if (_timer != null)
                {
                    return BadRequest("Tracking is already in progress.");
                }

                // রাইড ট্র্যাকিং শুরু করার জন্য ট্যামার ইনিশিয়ালাইজ করা
                _timer = new Timer(UpdateRideTrack, new { Latitude = latitude, Longitude = longitude }, TimeSpan.Zero, TimeSpan.FromSeconds(2));

                // ট্যামারের স্ট্যাটাস লগ করা
                Console.WriteLine($"Timer Status: {_timer?.ToString()}");
                return Ok("Ride started and tracking initiated.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // রাইড ট্র্যাক আপডেট করার জন্য ট্যামার থেকে কল হওয়া মেথড
        private async void UpdateRideTrack(object? state)
        {
            try
            {
                if (_currentRideBook == null) return;

                var data = (dynamic)state;
                var currentLatitude = data.Latitude;
                var currentLongitude = data.Longitude;

                Console.WriteLine($"Updating Track: Latitude = {currentLatitude}, Longitude = {currentLongitude}");

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<DatabaseDbContext>();

                    // বর্তমান অবস্থান থেকে গন্তব্যে পৌঁছানোর দূরত্ব গণনা করা
                    distance = _distanceService.CalculateDistance(
                        currentLatitude,
                        currentLongitude,
                        _currentRideBook.DestinationLatitude,
                        _currentRideBook.DestinationLongitude) * 1000;

                    Console.WriteLine($"Distance to Destination: {distance} meters");

                    // শেষ রাইড ট্র্যাক খুঁজে বের করা
                    var lastRideTrack = _currentRideBook.RideTracks?
                        .OrderByDescending(rt => rt.Timestamp)
                        .FirstOrDefault();

                    if (lastRideTrack != null)
                    {
                        // পূর্বের রাইড ট্র্যাক আপডেট করা
                        lastRideTrack.RideTrackLatitude = currentLatitude;
                        lastRideTrack.RideTrackLongitude = currentLongitude;
                        lastRideTrack.Distance = (int)distance;
                        lastRideTrack.TrackTime = DateTime.Now;

                        context.Entry(lastRideTrack).State = EntityState.Modified;
                        Console.WriteLine($"Updated RideTrack at {lastRideTrack.TrackTime}");
                    }
                    else
                    {
                        // নতুন রাইড ট্র্যাক তৈরি করা
                        var rideTrack = new RideTrack
                        {
                            RideBookId = _currentRideBook.RideBookId,
                            RideTrackLatitude = currentLatitude,
                            RideTrackLongitude = currentLongitude,
                            Timestamp = DateTime.Now,
                            Distance = (int)distance,
                            TrackTime = DateTime.Now,
                            Status=true
                        };

                        rideTrack.SetCreateInfo();

                        _currentRideBook.RideTracks?.Add(rideTrack);
                        context.RideTracks.Add(rideTrack);
                        Console.WriteLine($"Added new RideTrack at {rideTrack.TrackTime}");
                    }

                    await context.SaveChangesAsync();

                    // গন্তব্যে পৌঁছালে ট্র্যাকিং বন্ধ করা
                    if (distance <= 100) // 100 মিটার বা কম দূরত্বে পৌঁছালে
                    {
                        StopTracking();
                        Console.WriteLine("Destination Reached! Stopping tracking.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateRideTrack: {ex.Message}");
            }
        }

        // ট্র্যাকিং থামানোর জন্য মেথড
        private void StopTracking()
        {
            if (_timer != null)
            {
                _timer.Dispose();
                _timer = null;
                Console.WriteLine("Tracking stopped.");
            }
        }

        // GET: api/RideTracks/{rideBookId}
        [HttpGet("{rideBookId}")]
        public async Task<IActionResult> GetRideTracks(int rideBookId)
        {
            try
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
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/RideTracks/stop
        [HttpPost("stop")]
        public async Task<IActionResult> StopRideTracking(int customerId)
        {
            try
            {
                // ট্যামার স্ট্যাটাস চেক করা
                Console.WriteLine($"Stopping Timer: {_timer?.ToString()}");
                if (_timer != null)
                {
                    // কাস্টমার এর রাইডবুক বের করা যেখানে ট্র্যাকিং চলছে
                    var customer = await _context.Customers.FindAsync(customerId);
                    var rideBook = await _context.RideBooks.Where(r => r.CustomerId == customer.CustomerId).OrderByDescending(r => r.RideBookId).FirstOrDefaultAsync(); 
                    var driverVehicle = _context.DriverVehicles.FirstOrDefault(dv=>dv.DriverVehicleId == rideBook.DriverVehicleId);
                    var vehicle =  _context.Vehicles.FirstOrDefault(vid => vid.VehicleId == driverVehicle.VehicleId);
                    var vehicleType =  _context.VehicleTypes.FirstOrDefault(vtid => vtid.VehicleTypeId == vehicle.VehicleTypeId);
                    var farePerKm = vehicleType.PerKmFare;
                    var amount = (int)(distance/1000) * farePerKm;

                    // ইনভয়েস তৈরি করা
                    var invoice = new Invoice
                    {
                        //RideBookId = activeRideBook.RideBookId,
                        //CustomerId = activeRideBook.CustomerId,
                        PaymentTime = DateTime.Now,
                        Amount = amount,
                        Particular="",
                        CustomerId=customer.CustomerId,
                        PaymentMethodId = 1
                    };

                   

                    // ডাটাবেস আপডেট করা
                    //

                    // ট্যামার বন্ধ করা
                    StopTracking();
                    _context.Invoices.Add(invoice);
                    await _context.SaveChangesAsync();

                    return Ok($"Tracking stopped successfully. Total fare: .");
                }
                else
                {
                    return BadRequest("No active tracking found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
