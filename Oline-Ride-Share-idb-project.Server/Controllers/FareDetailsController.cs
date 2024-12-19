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
    public class FareDetailsController : ControllerBase
    {
        private readonly DatabaseDbContext _context;
        public FareDetailsController(DatabaseDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FareDetail>>> GetFareDetails()
        {
            return await _context.FareDetails.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<FareDetail>> GetFareDetail(int id)
        {
            var fareDetail = await _context.FareDetails.FindAsync(id);

            if (fareDetail == null)
            {
                return NotFound();
            }
            return fareDetail;
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFareDetail(int id, FareDetail fareDetail)
        {
            if (id != fareDetail.FareDetailId)
            {
                return BadRequest();
            }
            _context.Entry(fareDetail).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FareDetailExists(id))
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
        public async Task<ActionResult<FareDetail>> PostFareDetail(FareDetail fareDetail)
        {
            _context.FareDetails.Add(fareDetail);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetFareDetail", new { id = fareDetail.FareDetailId }, fareDetail);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFareDetail(int id)
        {
            var fareDetail = await _context.FareDetails.FindAsync(id);
            if (fareDetail == null)
            {
                return NotFound();
            }
            _context.FareDetails.Remove(fareDetail);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        private bool FareDetailExists(int id)
        {
            return _context.FareDetails.Any(e => e.FareDetailId == id);
        }
    }
}
