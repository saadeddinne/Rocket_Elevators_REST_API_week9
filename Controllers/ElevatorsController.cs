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
    public class ElevatorsController : ControllerBase
    {
        private readonly RocketContext _context;

        public ElevatorsController(RocketContext context)
        {
            _context = context;
        }

        // GET: api/Elevators : All elevators
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Elevators>>> GetElevators()
        {
            return await _context.Elevators.ToListAsync();
        }

        // Retrieving a specific Elevator
        // GET: api/Elevators/10 
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
        // Get columns for specific Customers

        [HttpGet("{email}/elevator")]
        public IEnumerable<Elevators> ElevatorsCostumer([FromRoute] string email)
        {


            var cost = _context.Customers.Where(c => c.CompanyContactEmail.Equals(email));
            Customers costume = cost.FirstOrDefault();
            // var costx = costume.Id;

            IEnumerable<Elevators> Elevators = (from elevator in _context.Elevators
                                                join buildings in _context.Buildings on elevator.Id equals buildings.Id
                                                join costumer in _context.Customers on buildings.Id equals
                                                costumer.Id
                                                select elevator).Take(4);



            return Elevators.Distinct().ToList();

        }

        // Retrieving the current status of a specific Elevator
        // GET: api/Elevators/{id}/status
        [HttpGet("{id}/Status")]
        public async Task<ActionResult<string>> GetElevatorStatus([FromRoute] long id)
        {
            var cc = await _context.Elevators.FindAsync(id);

            if (cc == null)
                return NotFound();
            Console.WriteLine("Get Elevator status", cc.ElevatorStatus);

            return cc.ElevatorStatus;

        }

        // Changing the status of a specific Elevator
        [HttpPut("{id}/Status")]
        public async Task<IActionResult> UpdateElevatorStatus([FromRoute] long id, Elevators elevator)
        {


            if (id != elevator.Id)
            {
                Console.WriteLine(elevator.Id);
                return Content("Wrong id ! please check and try again");
            }

            if (elevator.ElevatorStatus == "Active" || elevator.ElevatorStatus == "Inactive" || elevator.ElevatorStatus == "Intervention")
            {
                _context.Entry(elevator).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Content("Elevator: " + elevator.Id + ", status as been change to: " + elevator.ElevatorStatus);
            }

            return Content("Please insert a valid status : Intervention, Inactive, Active, Tray again !  ");
        }


        // DELETE: api/Elevators/5
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
