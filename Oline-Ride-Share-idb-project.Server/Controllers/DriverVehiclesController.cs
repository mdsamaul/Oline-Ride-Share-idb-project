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
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDriverVehicle(int id, DriverVehicle driverVehicle)
        {
            if (id != driverVehicle.DriverVehicleId)
            {
                return BadRequest();
            }

            _context.Entry(driverVehicle).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DriverVehicleExists(id))
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

        // POST: api/DriverVehicles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DriverVehicle>> PostDriverVehicle(DriverVehicle driverVehicle)
        {
            _context.DriverVehicles.Add(driverVehicle);
            await _context.SaveChangesAsync();

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

            _context.DriverVehicles.Remove(driverVehicle);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DriverVehicleExists(int id)
        {
            return _context.DriverVehicles.Any(e => e.DriverVehicleId == id);
        }
    }
}
