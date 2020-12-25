using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RocketApi.Models;

namespace RocketApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdressesController : ControllerBase
    {
        private readonly RocketContext _context;

        public AdressesController(RocketContext context)
        {
            _context = context;
        }

        // GET: api/Adresses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Addresses>>> GetAddresses()
        {
            return await _context.Addresses.ToListAsync();
        }

        // Get buildings adresses for specific Customers

        [HttpGet("{email}/address")]
        public IEnumerable<Addresses> BuildingsAddressesCostumer([FromRoute] string email)
        {


            var cost = _context.Customers.Where(c => c.CompanyContactEmail.Equals(email));
            var costume = 3;
            // var costx = costume.Id;

            IEnumerable<Addresses> Addresses = (from addresses in _context.Addresses where addresses.CustomerId == costume select addresses);



            return Addresses.Distinct().ToList();

        }
        // GET: api/Adresses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Addresses>> GetAddresses(long id)
        {
            var addresses = await _context.Addresses.FindAsync(id);

            if (addresses == null)
            {
                return NotFound();
            }

            return addresses;
        }

        // PUT: api/Adresses/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAddresses(long id, Addresses addresses)
        {
            if (id != addresses.Id)
            {
                return BadRequest();
            }

            _context.Entry(addresses).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AddressesExists(id))
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

        // POST: api/Adresses
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Addresses>> PostAddresses(Addresses addresses)
        {
            _context.Addresses.Add(addresses);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAddresses", new { id = addresses.Id }, addresses);
        }

        // DELETE: api/Adresses/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Addresses>> DeleteAddresses(long id)
        {
            var addresses = await _context.Addresses.FindAsync(id);
            if (addresses == null)
            {
                return NotFound();
            }

            _context.Addresses.Remove(addresses);
            await _context.SaveChangesAsync();

            return addresses;
        }

        private bool AddressesExists(long id)
        {
            return _context.Addresses.Any(e => e.Id == id);
        }
    }
}
