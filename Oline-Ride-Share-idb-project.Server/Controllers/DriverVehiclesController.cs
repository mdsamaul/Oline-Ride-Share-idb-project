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
    public class DriverVehiclesController : ControllerBase
    {
        private readonly DatabaseDbContext _context;

        public DriverVehiclesController(DatabaseDbContext context)
        {
            _context = context;
        }

        // GET: api/DriverVehicles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DriverVehicle>>> GetDriverVehicles()
        {
            // Retrieves all DriverVehicle records asynchronously
            return await _context.DriverVehicles.ToListAsync();
        }

        // GET: api/DriverVehicles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DriverVehicle>> GetDriverVehicle(int id)
        {
            var driverVehicle = await _context.DriverVehicles.FindAsync(id);

            if (driverVehicle == null)
            {
                return NotFound();
            }

            return driverVehicle;
        }

        // PUT: api/DriverVehicles/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDriverVehicle(int id, DriverVehicle driverVehicle)
        {
            if (id != driverVehicle.DriverVehicleId)
            {
                return BadRequest();
            }

            // Find existing DriverVehicle
            var exDriverVehicle = await _context.DriverVehicles.FindAsync(id);
            if (exDriverVehicle == null)
            {
                return NotFound("DriverVehicle not found");
            }

            // Update only the relevant fields
            exDriverVehicle.DriverId = driverVehicle.DriverId;
            exDriverVehicle.VehicleId = driverVehicle.VehicleId;

            // Set the updated information (like timestamp)
            exDriverVehicle.SetUpdateInfo();

            // Mark the entity state as modified for the update operation
            _context.Entry(exDriverVehicle).State = EntityState.Modified;

            try
            {
                // Save the changes to the database asynchronously
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // If the entity does not exist in the database anymore, return a NotFound response
                if (!DriverVehicleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw; // Rethrow the exception if something else goes wrong
                }
            }

            return NoContent(); // Return status code 204 for successful update
        }

        // POST: api/DriverVehicles
        [HttpPost]
        public async Task<ActionResult<DriverVehicle>> PostDriverVehicle(DriverVehicle driverVehicle)
        {
            // Set the creation info (like timestamp)
            driverVehicle.SetCreateInfo();

            // Add the new DriverVehicle record to the context
            _context.DriverVehicles.Add(driverVehicle);

            // Save the new record asynchronously
            await _context.SaveChangesAsync();

            // Return a response with status code 201 (Created), including the new DriverVehicle
            return CreatedAtAction("GetDriverVehicle", new { id = driverVehicle.DriverVehicleId }, driverVehicle);
        }

        // DELETE: api/DriverVehicles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDriverVehicle(int id)
        {
            var driverVehicle = await _context.DriverVehicles.FindAsync(id);
            if (driverVehicle == null)
            {
                return NotFound();
            }

            // Remove the DriverVehicle from the context
            _context.DriverVehicles.Remove(driverVehicle);

            // Save the changes asynchronously
            await _context.SaveChangesAsync();

            return NoContent(); // Return status code 204 for successful deletion
        }

        // Helper method to check if a DriverVehicle exists
        private bool DriverVehicleExists(int id)
        {
            return _context.DriverVehicles.Any(e => e.DriverVehicleId == id);
        }
    }
}
