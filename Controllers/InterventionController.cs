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
                return Content("No data or Wrong id ! please check and try again");
            }

            return interventions;
        }

        // the same method handle 2 endpoint :
        //** /api/Intervention/1/inprogress and
        //** /api/Intervention/1/completed
        // /api/Intervention/1/completed
        [HttpPut("{id}/{status}")]
        public async Task<IActionResult> CompletedInterventions([FromRoute] long id, [FromRoute] string status, Interventions intervention)
        {
            // verify the status from the route and handle the response
            if (status == "inprogress")
            {
                var update = await _context.Interventions.FindAsync(id);
                // verify if the id in the route is the same in the Body
                if (update == null || id != intervention.Id)
                {
                    return Content("Wrong id ! please check and try again");
                }
                // verify the spelling 
                if (intervention.Status == "InProgress")
                {
                    // update date and status
                    update.StartIntervention = DateTime.Now;
                    update.Status = "InProgress";
                }

                else
                {
                    // send message to help the user
                    return Content("Please insert a valid status : InProgress. Tray again please !  ");
                }
                // update and save
                _context.Interventions.Update(update);
                await _context.SaveChangesAsync();
                // confirmation message
                return Content("Intervention: " + intervention.Id + ", status has been changed to: " + intervention.Status);
            }
            // 2 case completed
            else if (status == "completed")
            {
                // check the id
                var update = await _context.Interventions.FindAsync(id);
                if (update == null || id != intervention.Id)
                {
                    return Content("Wrong id ! please check and try again");
                }
                // check the sppelling to prevent and standardize data
                if (intervention.Status == "Completed")
                {
                    update.EndIntervention = DateTime.Now;
                    update.Status = "Completed";
                }

                else
                {
                    // help the user to understand whats wrong !
                    return Content("Please insert a valid status : Completed. Tray again please !  ");
                }
                // update and save
                _context.Interventions.Update(update);
                await _context.SaveChangesAsync();

                return Content("Intervention: " + intervention.Id + ", status has been changed to: " + intervention.Status);

            }

            // help the user to specify the endpoints
            else return Content("hem hem .. something wrong! please use this route format: [api/Intervention/{id}/inprogress] or  [api/Intervention/{id}/completed] and change the status in the body of the request");


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
