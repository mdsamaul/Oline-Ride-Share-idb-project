using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oline_Ride_Share_idb_project.Server.Data;
using Oline_Ride_Share_idb_project.Server.Model;

namespace Oline_Ride_Share_idb_project.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BanksController : ControllerBase
    {
        private readonly DatabaseDbContext _context;

        public BanksController(DatabaseDbContext context)
        {
            _context = context;
        }

        // GET: api/Banks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bank>>> GetBanks()
        {
            return await _context.Banks.ToListAsync();
        }

        // GET: api/Banks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Bank>> GetBank(int id)
        {
            var bank = await _context.Banks.FindAsync(id);

            if (bank == null)
            {
                return NotFound();
            }

            return bank;
        }

        // PUT: api/Banks/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBank(int id, Bank bank)
        {
            if (id != bank.BankId)
            {
                return BadRequest("Bank ID does not match.");
            }

            var existingBank = await _context.Banks.FindAsync(id);
            if (existingBank == null)
            {
                return NotFound("Bank not found.");
            }
            existingBank.BankName = bank.BankName; // Update other properties as needed
            existingBank.SetUpdateInfo(); // Automatically update UpdateBy and UpdateDate

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BankExists(id))
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

        // POST: api/Banks
        [HttpPost]
        public async Task<ActionResult<Bank>> PostBank(Bank bank)
        {
            bank.SetCreateInfo();
            // BaseEntity constructor will handle CreateBy and CreateDate
            _context.Banks.Add(bank);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBank), new { id = bank.BankId }, bank);
        }

        // DELETE: api/Banks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBank(int id)
        {
            var bank = await _context.Banks.FindAsync(id);
            if (bank == null)
            {
                return NotFound("Bank not found.");
            }

            _context.Banks.Remove(bank);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BankExists(int id)
        {
            return _context.Banks.Any(e => e.BankId == id);
        }
    }
}
