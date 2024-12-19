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
        [HttpPost("start")]
        public async Task<IActionResult> StartRideTracking(int rideBookId, float latitude, float longitude)
        {
            try
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
                if (_timer != null)
                {
                    return BadRequest("Tracking is already in progress.");
                }
                _timer = new Timer(UpdateRideTrack, new { Latitude = latitude, Longitude = longitude }, TimeSpan.Zero, TimeSpan.FromSeconds(2));
                 Console.WriteLine($"Timer Status: {_timer?.ToString()}");
                return Ok("Ride started and tracking initiated.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
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
                    distance = _distanceService.CalculateDistance(
                        currentLatitude,
                        currentLongitude,
                        _currentRideBook.SourceLatitude,
                        _currentRideBook.SourceLongitude) * 1000;
                    Console.WriteLine($"Distance to Destination: {distance} meters");
                    var lastRideTrack = _currentRideBook.RideTracks?
                        .OrderByDescending(rt => rt.Timestamp)
                        .FirstOrDefault();

                    if (lastRideTrack != null)
                    {
                        lastRideTrack.RideTrackLatitude = currentLatitude;
                        lastRideTrack.RideTrackLongitude = currentLongitude;
                        lastRideTrack.Distance = (int)distance;
                        lastRideTrack.TrackTime = DateTime.Now;
                        context.Entry(lastRideTrack).State = EntityState.Modified;
                        Console.WriteLine($"Updated RideTrack at {lastRideTrack.TrackTime}");
                    }
                    else
                    {
                        var rideTrack = new RideTrack
                        {
                            RideBookId = _currentRideBook.RideBookId,
                            RideTrackLatitude = currentLatitude,
                            RideTrackLongitude = currentLongitude,
                            Timestamp = DateTime.Now,
                            Distance = (int)distance,
                            TrackTime = DateTime.Now
                        };
                        rideTrack.SetCreateInfo();
                        _currentRideBook.RideTracks?.Add(rideTrack);
                        context.RideTracks.Add(rideTrack);
                        Console.WriteLine($"Added new RideTrack at {rideTrack.TrackTime}");
                    }
                    await context.SaveChangesAsync();
                    var totaldistance = _distanceService.CalculateDistance(
                        _currentRideBook.SourceLatitude,
                        _currentRideBook.SourceLongitude,
                        _currentRideBook.DestinationLatitude,
                        _currentRideBook.DestinationLongitude) * 1000;
                    if (distance >= totaldistance - 50) 
                    {
                        await StopTracking(_currentRideBook.CustomerId); 
                        Console.WriteLine("Destination Reached! Stopping tracking.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateRideTrack: {ex.Message}");
            }
        }
        private async Task StopTracking(int customerId)
        {
            if (_timer != null)
            {
                try
                {
                    var customer = await _context.Customers.FindAsync(customerId);
                    var rideBook = await _context.RideBooks
                        .Where(r => r.CustomerId == customer.CustomerId)
                        .OrderByDescending(r => r.RideBookId)
                        .FirstOrDefaultAsync();
                    if (rideBook == null) return;
                    var driverVehicle = _context.DriverVehicles.FirstOrDefault(dv => dv.DriverVehicleId == rideBook.DriverVehicleId);
                    var vehicle = _context.Vehicles.FirstOrDefault(vid => vid.VehicleId == driverVehicle.VehicleId);
                    var vehicleType = _context.VehicleTypes.FirstOrDefault(vtid => vtid.VehicleTypeId == vehicle.VehicleTypeId);
                    var farePerKm = vehicleType.PerKmFare;
                    var amount = (int)(distance / 1000) * farePerKm;
                    var invoice = new Invoice
                    {
                        PaymentTime = DateTime.Now,
                        Amount = amount,
                        Particular = "",
                        CustomerId = customer.CustomerId,
                        PaymentMethodId = 1
                    };
                    invoice.SetCreateInfo();
                    _context.Invoices.Add(invoice);                  
                    await _context.SaveChangesAsync();
                    var payment = new Payment()
                    {
                        InvoiceId= invoice.InvoiceId,
                        Amount = amount,
                        PaymentDate= DateTime.Now,
                    };
                    payment.SetCreateInfo();
                    _context.Payments.Add(payment);
                    await _context.SaveChangesAsync();
                    _timer.Dispose();
                    _timer = null;
                    Console.WriteLine("Tracking stopped.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in StopTracking: {ex.Message}");
                }
            }
        }
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
        [HttpPost("stop")]
        public async Task<IActionResult> StopRideTracking(int customerId)
        {
            try
            {
                if (_timer != null)
                {
                    await StopTracking(customerId); 
                    return Ok("Tracking stopped successfully.");
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
