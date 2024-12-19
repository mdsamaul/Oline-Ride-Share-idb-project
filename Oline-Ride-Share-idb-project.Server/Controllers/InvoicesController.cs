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
    public class InvoicesController : ControllerBase
    {
        private readonly DatabaseDbContext _context;
        public InvoicesController(DatabaseDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Invoice>>> GetInvoices()
        {
            return await _context.Invoices.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Invoice>> GetInvoice(int id)
        {
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }
            return invoice;
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInvoice(int id, Invoice invoice)
        {
            if (id != invoice.InvoiceId)
            {
                return BadRequest();
            }
            _context.Entry(invoice).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InvoiceExists(id))
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
        public async Task<ActionResult<Invoice>> PostInvoice(Invoice invoice)
        {
            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetInvoice", new { id = invoice.InvoiceId }, invoice);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvoice(int id)
        {
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }
            _context.Invoices.Remove(invoice);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        private bool InvoiceExists(int id)
        {
            return _context.Invoices.Any(e => e.InvoiceId == id);
        }
    }
}
