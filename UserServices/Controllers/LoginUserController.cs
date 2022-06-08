using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UserServices.Usermodel;

namespace UserServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginUserController : ControllerBase
    {
        private IConfiguration _config;
        public LoginUserController(IConfiguration config)
        {
            _config = config;
        }

        [Route("Users")]
        [HttpGet]
        public IList<UserCredentials> GetUsers()
        {
            using (var db = new User_DatabaseContext())
            {
                return db.UserCredentials.ToList();
            }
        }

        [Route("UserLogin")]
        [HttpPost]
        public IActionResult UserLogin([FromBody] UserCredentials credentialsObj)
        {
            if (credentialsObj == null)
            {
                return NotFound(new
                {
                    success = 0,
                    message = "Please check your Credentials"
                });
            }
            else
            {
                var db = new User_DatabaseContext();
                var user = db.UserCredentials.Where(a =>
                    a.Email == credentialsObj.Email
                    && a.Password == credentialsObj.Password
                    ).FirstOrDefault();
                var token = GenerateTokenUsingJwt(credentialsObj.Email);
                if (user != null)
                {
                    return Ok(new
                    {
                        success = 1,
                        message = "Logged In Successfully",
                        emailId = user.Email,
                        token = token
                    });
                }
                else
                {
                    return NotFound(new
                    {
                        success = 0,
                        message = "Please check your Credentials"
                    });
                }
            }
        }

        /// <summary>
        /// JWT Authentication
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public string GenerateTokenUsingJwt(string userName)
        {
            var signinngKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var signingCredentials = new SigningCredentials(signinngKey, SecurityAlgorithms.HmacSha256);

            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var jwt = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Issuer"],
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.Now.AddMinutes(3)
            );

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }
    }
}
