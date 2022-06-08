using System;
using System.Collections.Generic;

namespace BookingServices.Usermodel
{
    public partial class UserCredentials
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ContactNo { get; set; }
        public string Gender { get; set; }
        public string Age { get; set; }
        public string Address { get; set; }
    }
}
