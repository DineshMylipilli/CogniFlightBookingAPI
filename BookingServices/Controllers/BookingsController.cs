using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookingServices.Usermodel;
using Microsoft.AspNetCore.Authorization;

namespace BookingServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        [Route("GetAllFlights")]
        public IList<Flights> GetAllFlights()
        {
            using (var db = new Admin_DatabaseContext())
            {
                return db.Flights.ToList();
            }
        }

        [Authorize]
        [HttpPost]
        [Route("GetFlightsBySearch")]
        public IList<Flights> GetFlightsBySearch([FromBody] Flights obj)
        {
            var db = new Admin_DatabaseContext();
            var flights = db.Flights.Where(t => t.Source == obj.Source && t.Destination == obj.Destination && t.RoundTrip == obj.RoundTrip).ToList();

            //if (flights != null)
            //    return flights;
            //else
            return flights;
        }

        [Authorize]
        [HttpPost]
        [Route("FlightTicketBooking")]
        public IActionResult InsertTicketBooking([FromBody] TicketBooking ticket)
        {
            string pnr = string.Empty;
            if (string.IsNullOrEmpty(ticket.AirlineId) || string.IsNullOrEmpty(ticket.UserName) || string.IsNullOrEmpty(ticket.Name)
                                                       || string.IsNullOrEmpty(ticket.Gender) || string.IsNullOrEmpty(ticket.SeatNumbers))
                return NotFound(new
                {
                    success = 0,
                    message = "Please enter valid details"
                });
            else
            {
                var db = new User_DatabaseContext();
                var admindb = new Admin_DatabaseContext();
                
                var flight = admindb.Flights.Where(x => x.FlightId == ticket.AirlineId).FirstOrDefault();
                if (flight != null)
                {
                    ticket.BoardingTime = flight.StartTime.ToString();
                    ticket.Source = flight.Source;
                    ticket.Destination = flight.Destination;
                    
                    Random random = new Random();
                    //int seatNo = random.Next(00,99);
                    int pnrID = random.Next(000000, 999999);
                    pnr = "PNR" + pnrID;
                    ticket.Pnr = pnr;
                    ticket.IsCancelled = 0;
                    db.TicketBooking.Add(ticket);
                    db.SaveChanges();
                    return Ok(new
                    {
                        success = 1,
                        message = "Ticket booked Successfully",
                        pnr = pnr
                    });
                }
                else
                {
                    return NotFound(new
                    {
                        success = 0,
                        message = "Please enter valid AirlineId"
                    });

                }
            }

        }

        [Authorize]
        [HttpGet]
        [Route("SearchTicket")]
        public IList<TicketBooking> TicketSeach(string emailId)
        {
            var db = new User_DatabaseContext();
            var tickets = new List<TicketBooking>();

            if (emailId == null)
                return tickets;
            else
            {
                tickets = db.TicketBooking.Where(t => t.EmailId == emailId).ToList();

                return tickets;
            }
        }

        [Authorize]
        [HttpPut]
        [Route("CancelTicket")]
        public IActionResult CancelTicket([FromBody] TicketBooking cancelTicket)
        {
            if (string.IsNullOrEmpty(cancelTicket.Pnr))
                return Ok(new
                {
                    success = 0,
                    Message = "Please enter PNR number properly",
                });
            else
            {
                var db = new User_DatabaseContext();
                var ticket = db.TicketBooking.Where(t => t.Pnr == cancelTicket.Pnr).FirstOrDefault();

                if (ticket != null)
                {
                    if (ticket.IsCancelled == 1)
                    {
                        return Ok(new
                        {
                            success = 0,
                            Message = "Ticket with this PNR is already cancelled",
                        });
                    }
                    ticket.IsCancelled = 1;
                    db.SaveChanges();

                    return Ok(new
                    {
                        success = 1,
                        Message = "Ticket is cancelled",
                        pnr = ticket.Pnr
                    });
                }
                else
                {
                    return Ok(new
                    {
                        success = 0,
                        Message = "Please enter valid PNR number",
                    });
                }
            }
        }
    }
}
