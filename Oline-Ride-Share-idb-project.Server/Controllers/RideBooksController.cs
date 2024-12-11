using System.Collections.Generic;
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
