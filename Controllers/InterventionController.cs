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
    public class InterventionController : ControllerBase
    {
        private readonly RocketContext _context;

        public InterventionController(RocketContext context)
        {
            _context = context;
        }

        // GET: Returns all fields of all Service Request records that do not have a start date and are in "Pending" status.
        // GET: api/Intervention
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Interventions>>> GetInterventions()
        {
            return await _context.Interventions.Where(intervention => intervention.StartIntervention == null && intervention.Status == "Pending").ToListAsync();
        }

        // GET: Returns a specific intervention by id
        // GET: api/Intervention/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Interventions>> GetInterventions(long id)
        {
            var interventions = await _context.Interventions.FindAsync(id);

            if (interventions == null)
            {
                return NotFound();
            }

            return interventions;
        }

        // /api/Intervention/1/update
        // [HttpPut("{id}/update")]
        // public async Task<IActionResult> UpdateInterventions([FromRoute] long id, Interventions intervention)
        // {
        //     var update = await _context.Interventions.FindAsync(id);
        //     if (update == null || id != intervention.Id)
        //     {
        //         return Content("Wrong id ! please check and try again");
        //     }
        //     if (intervention.Status == "InProgress")
        //     {
        //         update.StartIntervention = DateTime.Now;
        //         update.Status = "InProgress";
        //     }
        //     else if (intervention.Status == "Completed")
        //     {
        //         update.EndIntervention = DateTime.Now;
        //         update.Status = "Completed";
        //     }
        //     else
        //     {
        //         return Content("Please insert a valid status : InProgress, Completed. Tray again please !  ");
        //     }
        //     _context.Interventions.Update(update);
        //     await _context.SaveChangesAsync();

        //     return Content("Intervention: " + intervention.Id + ", status has been changed to: " + intervention.Status);
        // }



        [HttpPut("{id}/inprogress")]
        public async Task<IActionResult> InProgressInterventions([FromRoute] long id, Interventions intervention)
        {
            var update = await _context.Interventions.FindAsync(id);
            if (update == null || id != intervention.Id)
            {
                return Content("Wrong id ! please check and try again");
            }
            if (intervention.Status == "InProgress")
            {
                update.StartIntervention = DateTime.Now;
                update.Status = "InProgress";
            }

            else
            {
                return Content("Please insert a valid status : InProgress. Tray again please !  ");
            }
            _context.Interventions.Update(update);
            await _context.SaveChangesAsync();

            return Content("Intervention: " + intervention.Id + ", status has been changed to: " + intervention.Status);
        }
        // /api/Intervention/1/completed
        [HttpPut("{id}/completed")]
        public async Task<IActionResult> CompletedInterventions([FromRoute] long id, [FromRoute] string status, Interventions intervention)
        {
            if (status == "completed")
            {

                var update = await _context.Interventions.FindAsync(id);
                if (update == null || id != intervention.Id)
                {
                    return Content("Wrong id ! please check and try again");
                }
                if (intervention.Status == "Completed")
                {
                    update.EndIntervention = DateTime.Now;
                    update.Status = "Completed";
                }

                else
                {
                    return Content("Please insert a valid status : Completed. Tray again please !  ");
                }
                _context.Interventions.Update(update);
                await _context.SaveChangesAsync();

                return Content("Intervention: " + intervention.Id + ", status has been changed to: " + intervention.Status);

            }
            else return Content("status is wrong");


        }





        // DELETE: api/Intervention/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Interventions>> DeleteInterventions(long id)
        {
            var interventions = await _context.Interventions.FindAsync(id);
            if (interventions == null)
            {
                return NotFound();
            }

            _context.Interventions.Remove(interventions);
            await _context.SaveChangesAsync();

            return interventions;
        }

        private bool InterventionsExists(long id)
        {
            return _context.Interventions.Any(e => e.Id == id);
        }
    }
}
