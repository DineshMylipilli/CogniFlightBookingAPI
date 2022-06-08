using System;
using System.Collections.Generic;

namespace AdminServices.Usermodel
{
    public partial class AdminCredentials
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
