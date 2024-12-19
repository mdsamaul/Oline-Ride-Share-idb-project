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
    public class CustomersController : ControllerBase
    {
        private readonly DatabaseDbContext _context;
        public CustomersController(DatabaseDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            return await _context.Customers.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return customer;
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return BadRequest();
            }
            var exCustomer = await _context.Customers.FindAsync(id);
            if (exCustomer == null)
            {
                return NotFound("Customer not found");
            }
            exCustomer.SetUpdateInfo();
            exCustomer.CustomerName = customer.CustomerName;
            exCustomer.CustomerPhoneNumber = customer.CustomerPhoneNumber;
            exCustomer.CustomerEmail = customer.CustomerEmail;
            exCustomer.CustomerNID = customer.CustomerNID;
            exCustomer.CustomerImage = customer.CustomerImage;
            exCustomer.CustomerLatitude = customer.CustomerLatitude;  // Automatically sent from front-end
            exCustomer.CustomerLongitude = customer.CustomerLongitude;  // Automatically sent from front-end
            _context.Entry(exCustomer).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
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
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            customer.SetCreateInfo(); 
            if (customer.CustomerLatitude == 0 || customer.CustomerLongitude == 0)
            {
                return BadRequest("Latitude and Longitude are required.");
            }
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetCustomer", new { id = customer.CustomerId }, customer);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.CustomerId == id);
        }
    }
}
