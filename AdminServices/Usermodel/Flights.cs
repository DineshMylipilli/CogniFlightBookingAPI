using System;
using System.Collections.Generic;

namespace AdminServices.Usermodel
{
    public partial class Flights
    {
        public int Id { get; set; }
        public string FlightId { get; set; }
        public string FlightName { get; set; }
        public string FlightLogo { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string ScheduledDays { get; set; }
        public string Bcseats { get; set; }
        public string Nbcseats { get; set; }
        public string Price { get; set; }
        public int? MealType { get; set; }
        public int? RoundTrip { get; set; }
    }
}
