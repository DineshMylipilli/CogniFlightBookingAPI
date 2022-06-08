using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserServices.Usermodel;

namespace UserServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        [HttpGet]
        [Route("GetAllUsers")]
        public List<UserCredentials> AllUserDetails()
        {
            using (var db = new User_DatabaseContext())
            {
                return db.UserCredentials.ToList();
            }
        }

        [HttpPost]
        [Route("RegisterUser")]
        public IActionResult UserRegistration([FromBody] UserCredentials userObj)
        {
            if (userObj == null || string.IsNullOrEmpty(userObj.UserName) || string.IsNullOrEmpty(userObj.Email) || string.IsNullOrEmpty(userObj.Password))
            {
                return BadRequest();
            }
            else
            {
                var db = new User_DatabaseContext();
                var userExists = db.UserCredentials.Where(x => x.Email == userObj.Email || x.UserName == userObj.UserName).FirstOrDefault();
                if (userExists == null)
                {
                    db.UserCredentials.Add(userObj);
                    db.SaveChanges();
                    return Ok(new
                    {
                        success = 1,
                        Message = "User Registered Successfully",
                        userData = userObj.UserName
                    });
                }
                else
                {
                    return Ok(new
                    {
                        success = 0,
                        Message = "You are already registered, Please login"
                    });
                }
            }
        }
    }
}
