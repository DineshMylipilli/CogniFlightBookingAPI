using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminServices.Usermodel;
using Microsoft.AspNetCore.Authorization;

namespace AdminServices.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        [Authorize]
        [HttpPost]
        [Route("ScheduleFlight")]
        public IActionResult AddFlight([FromBody] Flights flight)
        {
            if (flight == null)
                return BadRequest();
            else
            {
                var db = new Admin_DatabaseContext();
                var airlines = db.AirlineaddBlock.ToList();
                var existingAirline = airlines.Where(x => x.AirlineId == flight.FlightId).FirstOrDefault();

                if (existingAirline == null)
                {
                    return Ok(new
                    {
                        success = 0,
                        message = "Airline doesnot exist",
                    });
                }
                else
                {
                    db.Flights.Add(flight);
                    db.SaveChanges();
                    return Ok(new {
                        success = 1,
                        message = flight.FlightName + "added Successfully"
                    });
                }
            }
        }
    }
}
