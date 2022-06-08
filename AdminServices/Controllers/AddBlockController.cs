using AdminServices.Usermodel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminServices.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AddBlockController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        [Route("AllFlightDetails")]
        public List<AirlineaddBlock> AllFlightDetails()
        {
            using (var db = new Admin_DatabaseContext())
            {
                return db.AirlineaddBlock.ToList();
            }
        }


        [Authorize]
        [HttpPost]
        [Route("AddFlight")]
        public int AddFlight([FromBody] AirlineaddBlock data) 
        {
            if (string.IsNullOrEmpty(data.AirlineId) || string.IsNullOrEmpty(data.Airlinename))
                return 0;
            else
            {
                using (var db = new Admin_DatabaseContext())
                {
                    db.AirlineaddBlock.Add(data);
                    db.SaveChanges();
                    return 1;
                }
            }
        }

        [Authorize]
        [HttpDelete]
        [Route("BlockFlight")]
        public int BlockFlight(string Airline_ID)
        {
            using (var db = new Admin_DatabaseContext())
            {
                var airline = db.AirlineaddBlock.Find(Airline_ID);
                db.AirlineaddBlock.Remove(airline);
                return db.SaveChanges();
            }
        }
    }
}
