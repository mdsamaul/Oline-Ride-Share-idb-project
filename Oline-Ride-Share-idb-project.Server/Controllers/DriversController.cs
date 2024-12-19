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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Driver>>> GetDrivers()
        {
            return await _context.Drivers.ToListAsync();
        }
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
            exDriver.SetUpdateInfo();
            exDriver.DriverName = driver.DriverName;
            exDriver.PhoneNumber = driver.PhoneNumber;
            exDriver.Email = driver.Email;
            exDriver.DrivingLicenseNo = driver.DrivingLicenseNo;
            exDriver.DriverNid = driver.DriverNid;
            exDriver.DriverImage = driver.DriverImage;
            exDriver.CompanyId = driver.CompanyId;
            exDriver.IsAvailable = driver.IsAvailable; 
            exDriver.FcmToken = driver.FcmToken;     
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
        [HttpPost]
        public async Task<ActionResult<Driver>> PostDriver(Driver driver)
        {
            driver.SetCreateInfo();
            if (driver.DriverLatitude == 0 || driver.DriverLongitude == 0)
            {
                driver.DriverLatitude = 0; 
                driver.DriverLongitude = 0; 
            }
            driver.IsAvailable = true; 
            driver.FcmToken = driver.FcmToken ?? string.Empty; 
            _context.Drivers.Add(driver);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetDriver", new { id = driver.DriverId }, driver);
        }
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
