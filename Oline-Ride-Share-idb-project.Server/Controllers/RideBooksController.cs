<<<<<<< HEAD
﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oline_Ride_Share_idb_project.Server.Data;
using Oline_Ride_Share_idb_project.Server.Model;
using Oline_Ride_Share_idb_project.Server.Utilities;

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
            rideBook.StartTime = TimeZoneInfo.ConvertTimeFromUtc(rideBook.StartTime, TimeZoneInfo.FindSystemTimeZoneById("Bangladesh Standard Time"));
            rideBook.EndTime = TimeZoneInfo.ConvertTimeFromUtc(rideBook.EndTime, TimeZoneInfo.FindSystemTimeZoneById("Bangladesh Standard Time"));
            return rideBook;
        }
        // POST: api/RideBooks
        [HttpPost]
        public async Task<ActionResult<RideBook>> PostRideBook(RideBook rideBook)
        {
            rideBook.StartTime = DateTime.UtcNow;
            rideBook.StartTime = TimeZoneInfo.ConvertTimeFromUtc(rideBook.StartTime, TimeZoneInfo.FindSystemTimeZoneById("Bangladesh Standard Time"));
            double distance = rideBook.DistanceInMeter;
            double hours = distance / 1000;
            TimeSpan duration = TimeSpan.FromHours(hours);
            rideBook.EndTime = rideBook.StartTime.Add(duration);
            _context.RideBooks.Add(rideBook);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetRideBook", new { id = rideBook.RideBookId }, rideBook);
        }
        // POST: api/RideBooks/requestRide
        [HttpPost("requestRide")]
        public async Task<IActionResult> RequestRide([FromBody] RideBook rideBook)
        {
            _context.RideBooks.Add(rideBook);
            await _context.SaveChangesAsync();
            var drivers = await _context.Drivers.ToListAsync();
            var closestDrivers = drivers
                .Select(driver => new
                {
                    Driver = driver,
                    Distance = DistanceCalculator.CalculateDistance(
                        rideBook.SourceLaitude,
                        rideBook.SourceLongtiude,
                        driver.DriverLaitude,
                        driver.DriverLongtiude)
                })
                .OrderBy(d => d.Distance)
                .Take(5)
                .ToList();
            return Ok(closestDrivers);
        }
    }
}
=======
﻿//using System;
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
//    public class RideBooksController : ControllerBase
//    {
//        private readonly DatabaseDbContext _context;

//        public RideBooksController(DatabaseDbContext context)
//        {
//            _context = context;
//        }

//        // GET: api/RideBooks
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<RideBook>>> GetRideBooks()
//        {
//            return await _context.RideBooks.ToListAsync();
//        }

//        // GET: api/RideBooks/5
//        [HttpGet("{id}")]
//        public async Task<ActionResult<RideBook>> GetRideBook(int id)
//        {
//            var rideBook = await _context.RideBooks.FindAsync(id);

//            if (rideBook == null)
//            {
//                return NotFound();
//            }

//            return rideBook;
//        }

//        // PUT: api/RideBooks/5
//        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//        [HttpPut("{id}")]
//        public async Task<IActionResult> PutRideBook(int id, RideBook rideBook)
//        {
//            if (id != rideBook.RideBookId)
//            {
//                return BadRequest();
//            }

//            _context.Entry(rideBook).State = EntityState.Modified;

//            try
//            {
//                await _context.SaveChangesAsync();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!RideBookExists(id))
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

//        // POST: api/RideBooks
//        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//        [HttpPost]
//        public async Task<ActionResult<RideBook>> PostRideBook(RideBook rideBook)
//        {
//            _context.RideBooks.Add(rideBook);
//            await _context.SaveChangesAsync();

//            return CreatedAtAction("GetRideBook", new { id = rideBook.RideBookId }, rideBook);
//        }

//        // DELETE: api/RideBooks/5
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteRideBook(int id)
//        {
//            var rideBook = await _context.RideBooks.FindAsync(id);
//            if (rideBook == null)
//            {
//                return NotFound();
//            }

//            _context.RideBooks.Remove(rideBook);
//            await _context.SaveChangesAsync();

//            return NoContent();
//        }

//        private bool RideBookExists(int id)
//        {
//            return _context.RideBooks.Any(e => e.RideBookId == id);
//        }
//    }
//}

//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Oline_Ride_Share_idb_project.Server.Data;
//using Oline_Ride_Share_idb_project.Server.Model;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//namespace Oline_Ride_Share_idb_project.Server.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class RideBooksController : ControllerBase
//    {
//        private readonly DatabaseDbContext _context;

//        // Constructor to inject the database context
//        public RideBooksController(DatabaseDbContext context)
//        {
//            _context = context;
//        }

//        // GET: api/RideBooks
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<RideBook>>> GetRideBooks()
//        {
//            // Use Eager Loading to include related entities
//            var rideBooks = await _context.RideBooks
//                                           .Include(r => r.Customers) // Include Customer details
//                                           .Include(r => r.DriverVehicles) // Include DriverVehicle details
//                                           .Include(r => r.RideTracks) // Include RideTrack details
//                                           .ToListAsync();

//            return rideBooks;
//        }

//        // GET: api/RideBooks/5
//        [HttpGet("{id}")]
//        public async Task<ActionResult<RideBook>> GetRideBook(int id)
//        {
//            // Use Eager Loading to include related entities for a specific RideBook
//            var rideBook = await _context.RideBooks
//                                          .Include(r => r.Customers)
//                                          .Include(r => r.DriverVehicles)
//                                          .Include(r => r.RideTracks)
//                                          .FirstOrDefaultAsync(r => r.RideBookId == id);

//            if (rideBook == null)
//            {
//                return NotFound();
//            }

//            return rideBook;
//        }

//        // PUT: api/RideBooks/5
//        [HttpPut("{id}")]
//        public async Task<IActionResult> PutRideBook(int id, RideBook rideBook)
//        {
//            if (id != rideBook.RideBookId)
//            {
//                return BadRequest();
//            }

//            _context.Entry(rideBook).State = EntityState.Modified;

//            try
//            {
//                await _context.SaveChangesAsync();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!RideBookExists(id))
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

//        // POST: api/RideBooks
//        [HttpPost]
//        public async Task<ActionResult<RideBook>> PostRideBook(RideBook rideBook)
//        {
//            _context.RideBooks.Add(rideBook);
//            await _context.SaveChangesAsync();

//            return CreatedAtAction("GetRideBook", new { id = rideBook.RideBookId }, rideBook);
//        }

//        // DELETE: api/RideBooks/5
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteRideBook(int id)
//        {
//            var rideBook = await _context.RideBooks.FindAsync(id);
//            if (rideBook == null)
//            {
//                return NotFound();
//            }

//            _context.RideBooks.Remove(rideBook);
//            await _context.SaveChangesAsync();

//            return NoContent();
//        }

//        // Helper function to check if RideBook exists
//        private bool RideBookExists(int id)
//        {
//            return _context.RideBooks.Any(e => e.RideBookId == id);
//        }
//    }
//}


//using Microsoft.AspNetCore.Mvc;
//using Oline_Ride_Share_idb_project.Server.Data;
//using Oline_Ride_Share_idb_project.Server.Model;
//using Oline_Ride_Share_idb_project.Server.Model.vm;
//using Oline_Ride_Share_idb_project.Server.Services; // Service ক্লাসটি এখানে include করবেন

//[Route("api/[controller]")]
//[ApiController]
//public class RideBooksController : ControllerBase
//{
//    private readonly DatabaseDbContext _context;
//    private readonly GeoLocationService _geoLocationService;

//    public RideBooksController(DatabaseDbContext context, GeoLocationService geoLocationService)
//    {
//        _context = context;
//        _geoLocationService = geoLocationService;
//    }

//    [HttpPost]
//    public async Task<ActionResult<RideBook>> PostRideBook(RideBookVm rideBookVm)
//    {
//        // Find Customer by ID to get their location
//        var customer = await _context.Customers.FindAsync(rideBookVm.CustomerId);

//        if (customer == null)
//        {
//            return BadRequest("Customer not found.");
//        }
//        var s = customer.CustomerLongitude, customer.cusermerlatued;
//        // If SourceLocation is provided, set the latitude and longitude for Source
//        if (!string.IsNullOrEmpty(customer.SourceLocation))
//        {
//            var sourceCoordinates = await _geoLocationService.GetCoordinatesFromLocation(customer.SourceLocation);
//            rideBookVm.SourceLaitude = sourceCoordinates.Latitude;
//            rideBookVm.SourceLongtiude = sourceCoordinates.Longitude;
//        }

//        // If DestinationLocation is provided, set the latitude and longitude for Destination
//        if (!string.IsNullOrEmpty(customer.DestinationLocation))
//        {
//            var destinationCoordinates = await _geoLocationService.GetCoordinatesFromLocation(customer.DestinationLocation);
//            rideBookVm.DestinationLatitude = destinationCoordinates.Latitude;
//            rideBookVm.DestinationLongtiude = destinationCoordinates.Longitude;
//        }

//        // Add RideBook data to the database
//        _context.RideBooks.Add(rideBookVm);
//        await _context.SaveChangesAsync();

//        return CreatedAtAction("GetRideBook", new { id = rideBook.RideBookId }, rideBook);
//    }
//}

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

>>>>>>> 59ad6d4544f47f00f5b8c1b8726896d593a07457
