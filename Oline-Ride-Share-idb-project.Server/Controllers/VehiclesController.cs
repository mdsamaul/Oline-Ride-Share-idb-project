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
    public class VehiclesController : ControllerBase
    {
        private readonly DatabaseDbContext _context;
        public VehiclesController(DatabaseDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vehicle>>> GetVehicles()
        {
            return await _context.Vehicles.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Vehicle>> GetVehicle(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            return vehicle;
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVehicle(int id, Vehicle vehicle)
        {
            if (id != vehicle.VehicleId)
            {
                return BadRequest();
            }
            var exVehicle = await _context.Vehicles.FindAsync(id);
            if (exVehicle == null)
            {
                return NotFound("Vehicle Not Found");
            }
            exVehicle.VehicleBrand = vehicle.VehicleBrand;
             exVehicle.VehicleModel = vehicle.VehicleModel;
            exVehicle.VehicleCapacity = vehicle.VehicleCapacity;
            exVehicle.VehicleRegistrationNo = vehicle.VehicleRegistrationNo;
            exVehicle.VehicleChassisNo = vehicle.VehicleChassisNo;
            exVehicle.VehicleLicense = vehicle.VehicleLicense;
            exVehicle.VehicleTypeId = vehicle.VehicleTypeId;
            exVehicle.SetUpdateInfo();
            try
            {
                _context.Entry(exVehicle).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VehicleExists(id))
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
        public async Task<ActionResult<Vehicle>> PostVehicle(Vehicle vehicle)
        {
            vehicle.SetCreateInfo();
            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetVehicle", new { id = vehicle.VehicleId }, vehicle);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        private bool VehicleExists(int id)
        {
            return _context.Vehicles.Any(e => e.VehicleId == id);
        }
    }
}
