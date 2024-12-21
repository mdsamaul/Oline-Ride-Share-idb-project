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
    public class CompaniesController : ControllerBase
    {
        private readonly DatabaseDbContext _context;

        public CompaniesController(DatabaseDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Company>>> GetCompanys()
        {
            return await _context.Companys.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Company>> GetCompany(int id)
        {
            var company = await _context.Companys.FindAsync(id);
            if (company == null)
            {
                return NotFound();
            }

            return company;
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompany(int id, Company company)
        {
            if (id != company.CompanyId)
            {
                return BadRequest();
            }
            var exCompany = await _context.Companys.FindAsync(id);
            if(exCompany == null)
            {
                return NotFound("compnay not found");
            }
            exCompany.CompanyName = company.CompanyName;
            exCompany.Address = company.Address;
            exCompany.PhoneNumber = company.PhoneNumber;
            exCompany.Email = company.Email;
            exCompany.SetUpdateInfo();
            _context.Entry(exCompany).State = EntityState.Modified;
            try
            {
               
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyExists(id))
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
        public async Task<ActionResult<Company>> PostCompany(Company company)
        {
            company.SetCreateInfo();
            _context.Companys.Add(company);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetCompany", new { id = company.CompanyId }, company);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            var company = await _context.Companys.FindAsync(id);
            if (company == null)
            {
                return NotFound();
            }
            _context.Companys.Remove(company);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        private bool CompanyExists(int id)
        {
            return _context.Companys.Any(e => e.CompanyId == id);
        }
    }
}
