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
    public class BuildingsOffController : ControllerBase
    {
        private readonly RocketContext _context;

        public BuildingsOffController(RocketContext context)
        {
            _context = context;
        }


        // Retrieving a list of Buildings that contain at least one battery, column or elevator requiring intervention
        // GET: api/BuildingsOff

        [HttpGet]
        public IEnumerable<Buildings> GetBuildings()
        {



            IEnumerable<Buildings> Building =

            from building in _context.Buildings
            join battery in _context.Batteries on building.Id equals battery.BuildingId
            join column in _context.Columns on battery.Id equals column.BatteryId
            join elevator in _context.Elevators on column.Id equals elevator.ColumnId
            where
            battery.BatteryStatus == "Intervention" || battery.BatteryStatus == "Inactive"
            || column.ColumnStatus == "Intervention" || column.ColumnStatus == "Inactive"
            || elevator.ElevatorStatus == "Intervention" || elevator.ElevatorStatus == "Inactive"
            select building;

            return Building.Distinct().ToList();

        }
        // Get buildings for specific Customers

        [HttpGet("{email}/buildings")]
        public IEnumerable<Buildings> BuildingsCostumer([FromRoute] string email)
        {


            var cost = _context.Customers.Where(c => c.CompanyContactEmail.Equals(email));
            Customers costume = cost.FirstOrDefault();
            // var costx = costume.Id;

            IEnumerable<Buildings> Buildings = (from buildings in _context.Buildings join customer in _context.Customers on buildings.CustomerId equals customer.Id select buildings).Take(2);



            return Buildings.Distinct().ToList();

        }

        private bool BuildingsExists(long id)
        {
            return _context.Buildings.Any(e => e.Id == id);
        }
    }
}
