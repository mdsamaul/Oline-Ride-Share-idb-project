using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oline_Ride_Share_idb_project.Server.Data;
using Oline_Ride_Share_idb_project.Server.Model;

namespace Oline_Ride_Share_idb_project.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriversController : ControllerBase
    {
        private readonly DatabaseDbContext _context;

        public DriversController(DatabaseDbContext context)
        {
            _context = context;
        }

        // GET: api/Drivers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Driver>>> GetDrivers()
        {
            return await _context.Drivers.ToListAsync();
        }

        // GET: api/Drivers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Driver>> GetDriver(int id)
        {
            var driver = await _context.Drivers.FindAsync(id);

            if (driver == null)
            {
                return NotFound();
            }

            return driver;
        }

        // PUT: api/Drivers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDriver(int id, Driver driver)
        {
            if (id != driver.DriverId)
            {
                return BadRequest();
            }

            var exDriver = await _context.Drivers.FindAsync(id);
            if (exDriver == null)
            {
                return NotFound("Driver Not Found");
            }

            // Update existing driver details
            exDriver.SetUpdateInfo();
            exDriver.DriverName = driver.DriverName;
            exDriver.PhoneNumber = driver.PhoneNumber;
            exDriver.Email = driver.Email;
            exDriver.DrivingLicenseNo = driver.DrivingLicenseNo;
            exDriver.DriverNid = driver.DriverNid;
            exDriver.DriverImage = driver.DriverImage;
            exDriver.CompanyId = driver.CompanyId;
            exDriver.IsAvailable = driver.IsAvailable; // Update IsAvailable
            exDriver.FcmToken = driver.FcmToken;       // Update FcmToken

            // Update Latitude and Longitude if provided
            if (driver.DriverLatitude != 0 && driver.DriverLongitude != 0)
            {
                exDriver.DriverLatitude = driver.DriverLatitude;
                exDriver.DriverLongitude = driver.DriverLongitude;
            }

            _context.Entry(exDriver).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DriverExists(id))
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

        // POST: api/Drivers
        [HttpPost]
        public async Task<ActionResult<Driver>> PostDriver(Driver driver)
        {
            driver.SetCreateInfo();

            // Default values for new drivers
            if (driver.DriverLatitude == 0 || driver.DriverLongitude == 0)
            {
                driver.DriverLatitude = 0; // Default Latitude
                driver.DriverLongitude = 0; // Default Longitude
            }

            driver.IsAvailable = true; // Default to available
            driver.FcmToken = driver.FcmToken ?? string.Empty; // Default to empty string if null

            _context.Drivers.Add(driver);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDriver", new { id = driver.DriverId }, driver);
        }

        // DELETE: api/Drivers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDriver(int id)
        {
            var driver = await _context.Drivers.FindAsync(id);
            if (driver == null)
            {
                return NotFound();
            }

            _context.Drivers.Remove(driver);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DriverExists(int id)
        {
            return _context.Drivers.Any(e => e.DriverId == id);
        }
    }
}
