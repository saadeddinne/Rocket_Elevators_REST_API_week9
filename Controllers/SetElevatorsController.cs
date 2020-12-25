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
    public class SetElevatorsController : ControllerBase
    {
        private readonly RocketContext _context;

        public SetElevatorsController(RocketContext context)
        {
            _context = context;
        }

        // GET: api/SetElevators
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Elevators>>> GetElevators()
        {
            return await _context.Elevators.ToListAsync();
        }

        // GET: api/SetElevators/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Elevators>> GetElevators(long id)
        {
            var elevators = await _context.Elevators.FindAsync(id);

            if (elevators == null)
            {
                return NotFound();
            }

            return elevators;
        }

        // PUT: api/SetElevators/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutElevators(long id, Elevators elevators)
        {
            if (id != elevators.Id)
            {
                return BadRequest();
            }

            _context.Entry(elevators).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ElevatorsExists(id))
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

        [HttpPut("{id}/{status}")]
        public async Task<IActionResult> SetElevatorStatus([FromRoute] long id, [FromRoute] string status)
        {
            // lets get our intervention before update
            var update = await _context.Elevators.FindAsync(id);
            if (update == null)
            {
                // help the user to specify the endpoints
                return Content("hem hem .. something wrong! please use this route format: api/SetElevators/{id}/{Status}");
            }
            else if (status == "Active" || status == "Inactive" || status == "Intervention")
            {
                update.ElevatorStatus = status;
                // update and save
                _context.Elevators.Update(update);
                await _context.SaveChangesAsync();
                // confirmation message
                return Content("Elevator: " + update.Id + ", status has been changed to: " + update.ElevatorStatus);

            }
            else
            {
                // send message to help the user
                return Content("Please send a valid status : Active, Inactive or Intervention and Tray again please !  ");
            }
        }

        // POST: api/SetElevators
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Elevators>> PostElevators(Elevators elevators)
        {
            _context.Elevators.Add(elevators);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetElevators", new { id = elevators.Id }, elevators);
        }

        // DELETE: api/SetElevators/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Elevators>> DeleteElevators(long id)
        {
            var elevators = await _context.Elevators.FindAsync(id);
            if (elevators == null)
            {
                return NotFound();
            }

            _context.Elevators.Remove(elevators);
            await _context.SaveChangesAsync();

            return elevators;
        }

        private bool ElevatorsExists(long id)
        {
            return _context.Elevators.Any(e => e.Id == id);
        }
    }
}
