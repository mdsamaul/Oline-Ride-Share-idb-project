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
    public class VehicleTypesController : ControllerBase
    {
        private readonly DatabaseDbContext _context;
        public VehicleTypesController(DatabaseDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VehicleType>>> GetVehicleTypes()
        {
            return await _context.VehicleTypes.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<VehicleType>> GetVehicleType(int id)
        {
            var vehicleType = await _context.VehicleTypes.FindAsync(id);
            if (vehicleType == null)
            {
                return NotFound();
            }
            return vehicleType;
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVehicleType(int id, VehicleType vehicleType)
        {
            if (id != vehicleType.VehicleTypeId)
            {
                return BadRequest();
            }
            var existingVehicleType = await _context.VehicleTypes.FindAsync(id);
            if (existingVehicleType == null)
            {
                return NotFound("Vehicle Type not found.");
            }
            existingVehicleType.SetUpdateInfo();
            existingVehicleType.VehicleTypeName = vehicleType.VehicleTypeName; 
            existingVehicleType.PerKmFare = vehicleType.PerKmFare;
            try
            {
                _context.Entry(existingVehicleType).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VehicleTypeExists(id))
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
        public async Task<ActionResult<VehicleType>> PostVehicleType(VehicleType vehicleType)
        {
            vehicleType.SetCreateInfo();
            _context.VehicleTypes.Add(vehicleType);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetVehicleType", new { id = vehicleType.VehicleTypeId }, vehicleType);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicleType(int id)
        {
            var vehicleType = await _context.VehicleTypes.FindAsync(id);
            if (vehicleType == null)
            {
                return NotFound();
            }
            _context.VehicleTypes.Remove(vehicleType);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        private bool VehicleTypeExists(int id)
        {
            return _context.VehicleTypes.Any(e => e.VehicleTypeId == id);
        }
    }
}
