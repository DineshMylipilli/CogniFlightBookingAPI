using System;
using System.Collections.Generic;

namespace UserServices.Usermodel
{
    public partial class TicketBooking
    {
        public int UserId { get; set; }
        public string AirlineId { get; set; }
        public string BoardingTime { get; set; }
        public string UserName { get; set; }
        public string EmailId { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }
        public string MealTpe { get; set; }
        public string SeatNumbers { get; set; }
        public string Pnr { get; set; }
        public int IsCancelled { get; set; }
    }
}
