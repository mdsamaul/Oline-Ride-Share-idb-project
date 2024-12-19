using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oline_Ride_Share_idb_project.Server.Controllers.vm;
using Oline_Ride_Share_idb_project.Server.Data;
using Oline_Ride_Share_idb_project.Server.Model;
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
        private readonly DistanceService _distanceService;  
        private decimal totalFare;
        private decimal baseFare;
        public RideBooksController(DatabaseDbContext context, DistanceService distanceService)
        {
            _context = context;
            //_firebaseService = firebaseService;
            _distanceService = distanceService;  
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RideBook>>> GetRideBooks()
        {
            return await _context.RideBooks.ToListAsync();
        }
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
            double distance = _distanceService.CalculateDistance(
                rideBook.SourceLatitude, rideBook.SourceLongitude,
                rideBook.DestinationLatitude, rideBook.DestinationLongitude
            );
            var Dv = await _context.DriverVehicles.FindAsync(rideBook.DriverVehicleId);
            var Vehicle = await _context.Vehicles.FindAsync(Dv.VehicleId);
            var Vt = await _context.VehicleTypes.FindAsync(Vehicle.VehicleTypeId);
            var perKmFare = Vt.PerKmFare;
            if (Dv?.Vehicle?.VehicleTypes != null)
            {
                if (perKmFare != null) 
                {
                    totalFare = (decimal)distance * perKmFare;
                    baseFare = totalFare * 0.2m;
                    rideBook.TotalFare = totalFare+baseFare;
                }
                else
                {
                    ModelState.AddModelError("PerKmFare", "PerKmFare is not set for the selected vehicle.");
                    return BadRequest(ModelState); 
                }
            }
            else
            {
                ModelState.AddModelError("DriverVehicle", "Driver vehicle or vehicle types not found.");
                return NotFound(ModelState);
            }
            rideBook.DistanceInMeters = (float)distance;

            var FareDetails = new FareDetail()
            {
                VehicleTypeId = Vehicle.VehicleTypeId,
                BaseFare = baseFare,
                TotalFare=totalFare
            };
            FareDetails.SetCreateInfo();
            _context.FareDetails.Add(FareDetails);
            rideBook.SetCreateInfo();
            _context.RideBooks.Add(rideBook);
            await _context.SaveChangesAsync();
            var rideBookDTO = new RideBookVm()
            {
                RideBookId = rideBook.RideBookId,
                DistanceInMeters = rideBook.DistanceInMeters,
                TotalFare = rideBook.TotalFare
            };
            return CreatedAtAction("GetRideBook", new { id = rideBookDTO.RideBookId }, rideBookDTO);
        }
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
        //private async Task<string> GetAvailableDriverToken(RideBook rideBook)
        //{
        //    var nearestDriver = await _context.Drivers
        //        .Where(d => d.IsAvailable) 
        //        .OrderBy(d => _distanceService.CalculateDistance(rideBook.SourceLatitude, rideBook.SourceLongitude, d.DriverLatitude, d.DriverLongitude)) 
        //        .FirstOrDefaultAsync();
        //    if (nearestDriver != null)
        //    {
        //        return nearestDriver.FcmToken;
        //    }
        //    return null; 
        //}
        private bool RideBookExists(int id)
        {
            return _context.RideBooks.Any(e => e.RideBookId == id);
        }
    }
}
