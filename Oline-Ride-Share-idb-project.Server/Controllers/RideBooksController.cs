using Microsoft.AspNetCore.Mvc;
using Oline_Ride_Share_idb_project.Server.Data;
using Oline_Ride_Share_idb_project.Server.Model;
using Oline_Ride_Share_idb_project.Server.Model.vm;
using Oline_Ride_Share_idb_project.Server.Services; // Service ক্লাসটি এখানে include করবেন

[Route("api/[controller]")]
[ApiController]
public class RideBooksController : ControllerBase
{
    private readonly DatabaseDbContext _context;
    private readonly GeoLocationService _geoLocationService;

    public RideBooksController(DatabaseDbContext context, GeoLocationService geoLocationService)
    {
        _context = context;
        _geoLocationService = geoLocationService;
    }

    // POST: api/RideBooks
    [HttpPost]
    public async Task<ActionResult<RideBook>> PostRideBook(RideBookVm rideBookVm)
    {
        // Find Customer by ID to get their location
        var customer = await _context.Customers.FindAsync(rideBookVm.CustomerId);

        if (customer == null)
        {
            return BadRequest("Customer not found.");
        }

        // If SourceLocation is provided, set the latitude and longitude for Source
        if (!string.IsNullOrEmpty(rideBookVm.SourceLocation))
        {
            var sourceCoordinates = await _geoLocationService.GetCoordinatesFromLocation(rideBookVm.SourceLocation);
            rideBookVm.SourceLaitude = sourceCoordinates.Latitude;
            rideBookVm.SourceLongtiude = sourceCoordinates.Longitude;
        }

        // If DestinationLocation is provided, set the latitude and longitude for Destination
        if (!string.IsNullOrEmpty(rideBookVm.DestinationLocation))
        {
            var destinationCoordinates = await _geoLocationService.GetCoordinatesFromLocation(rideBookVm.DestinationLocation);
            rideBookVm.DestinationLatitude = destinationCoordinates.Latitude;
            rideBookVm.DestinationLongtiude = destinationCoordinates.Longitude;
        }

        // Convert RideBookVm to RideBook
        var rideBook = new RideBook
        {
            CustomerId = rideBookVm.CustomerId,
            DriverVehicleId = rideBookVm.DriverVehicleId,
            SourceLaitude = rideBookVm.SourceLaitude,
            SourceLongtiude = rideBookVm.SourceLongtiude,
            DestinationLatitude = rideBookVm.DestinationLatitude,
            DestinationLongtiude = rideBookVm.DestinationLongtiude,
            StartTime = rideBookVm.StartTime,
            EndTime = rideBookVm.EndTime,
            TotalFare = rideBookVm.TotalFare,
            IsPaid = rideBookVm.IsPaid,
            DriverRating = rideBookVm.DriverRating,
            CustomerRating = rideBookVm.CustomerRating,
            DistanceInMeter = rideBookVm.DistanceInMeter
        };

        // Add RideBook data to the database
        _context.RideBooks.Add(rideBook);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetRideBook", new { id = rideBook.RideBookId }, rideBook);
    }
}
