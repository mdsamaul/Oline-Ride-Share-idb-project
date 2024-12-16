using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oline_Ride_Share_idb_project.Server.Data;
using Oline_Ride_Share_idb_project.Server.Model;
using Oline_Ride_Share_idb_project.Server.Model.vm;
using Oline_Ride_Share_idb_project.Server.Services;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Oline_Ride_Share_idb_project.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RideBooksController : ControllerBase
    {
        private readonly DatabaseDbContext _context;
        //private readonly FirebaseService _firebaseService;
        private readonly DistanceService _distanceService;  // Injecting DistanceService
        private decimal totalFare;
        private decimal baseFare;
        public RideBooksController(DatabaseDbContext context, DistanceService distanceService)
        {
            _context = context;
            //_firebaseService = firebaseService;
            _distanceService = distanceService;  // Initializing DistanceService
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

        [HttpPost]
        public async Task<ActionResult<RideBook>> PostRideBook(RideBook rideBook)
        {
            // Distance calculation logic using DistanceService
            double distance = _distanceService.CalculateDistance(
                rideBook.SourceLatitude, rideBook.SourceLongitude,
                rideBook.DestinationLatitude, rideBook.DestinationLongitude
            );

            // DriverVehicle এবং Vehicle সম্পর্ক সন্নিবেশ করা
            //var driverVehicle = await _context.DriverVehicles
            //    .Include(dv => dv.Vehicle)  // শুধুমাত্র Vehicle সম্পর্ক সন্নিবেশ করা
            //    .ThenInclude(v => v.VehicleTypes)  // VehicleTypes সম্পর্ক সন্নিবেশ করা
            //    .FirstOrDefaultAsync(dv => dv.DriverVehicleId == rideBook.DriverVehicleId);
            var Dv = await _context.DriverVehicles.FindAsync(rideBook.DriverVehicleId);
            var Vehicle = await _context.Vehicles.FindAsync(Dv.VehicleId);
            var Vt = await _context.VehicleTypes.FindAsync(Vehicle.VehicleTypeId);
            var perKmFare = Vt.PerKmFare;

            // DriverVehicle এবং VehicleTypes চেক করা
            if (Dv?.Vehicle?.VehicleTypes != null)
            {
                //var perKmFare = driverVehicle.Vehicle.VehicleTypes.PerKmFare; // VehicleTypes থেকে PerKmFare নেওয়া
                if (perKmFare != null) // Ensure PerKmFare is not null
                {
                    // PerKmFare দিয়ে মোট ফেয়ার হিসাব করা
                    totalFare = (decimal)distance * perKmFare;
                    baseFare = totalFare * 0.2m;
                    rideBook.TotalFare = totalFare+baseFare;
                }
                else
                {
                    ModelState.AddModelError("PerKmFare", "PerKmFare is not set for the selected vehicle.");
                    return BadRequest(ModelState); // BadRequest যদি PerKmFare না থাকে
                }
            }
            else
            {
                ModelState.AddModelError("DriverVehicle", "Driver vehicle or vehicle types not found.");
                return NotFound(ModelState); // NotFound যদি ড্রাইভার ভেহিকল বা ভেহিকল টাইপ না পাওয়া যায়
            }

            // Set the calculated distance in the RideBook model
            rideBook.DistanceInMeters = (float)distance;

            var FareDetails = new FareDetail()
            {
                VehicleTypeId = Vehicle.VehicleTypeId,
                BaseFare = baseFare,
                TotalFare=totalFare
            };
            FareDetails.SetCreateInfo();
            _context.FareDetails.Add(FareDetails);
            // Save the ride request to the database
            rideBook.SetCreateInfo(); // যেটি হয়তো আপনার custom method, সময় ও user info সেট করার জন্য
            _context.RideBooks.Add(rideBook);
            await _context.SaveChangesAsync();

            // Create the DTO (RideBookVm) to return the relevant information
            var rideBookDTO = new RideBookVm
            {
                RideBookId = rideBook.RideBookId,
                DistanceInMeters = rideBook.DistanceInMeters,
                TotalFare = rideBook.TotalFare
            };

            // Return the newly created RideBook object with status code 201 (Created)
            return CreatedAtAction("GetRideBook", new { id = rideBookDTO.RideBookId }, rideBookDTO);
        }





        // PUT: api/RideBooks/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRideBook(int id, RideBook rideBook)
        {
            if (id != rideBook.RideBookId)
            {
                return BadRequest();
            }
            var exRidebook = await _context.RideBooks.FindAsync(id);
            if (exRidebook == null)
            {
                return NotFound("Ride book not found");
            }
            // Set update information
            exRidebook.SetUpdateInfo();
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

        // Helper method to get the available driver token (find nearest driver)
        private async Task<string> GetAvailableDriverToken(RideBook rideBook)
        {
            // Find nearest driver based on source latitude and longitude
            var nearestDriver = await _context.Drivers
                .Where(d => d.IsAvailable)  // Only get drivers that are available
                .OrderBy(d => _distanceService.CalculateDistance(rideBook.SourceLatitude, rideBook.SourceLongitude, d.DriverLatitude, d.DriverLongitude))  // Sort by distance
                .FirstOrDefaultAsync();

            if (nearestDriver != null)
            {
                return nearestDriver.FcmToken; // Return the driver's Firebase Cloud Messaging token
            }

            return null; // Return null if no available driver is found
        }

        // Helper method to check if a ride book exists
        private bool RideBookExists(int id)
        {
            return _context.RideBooks.Any(e => e.RideBookId == id);
        }
    }
}
