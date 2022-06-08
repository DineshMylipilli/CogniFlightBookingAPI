using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Net.Http;
using System.Web.Http;
using AdminServices.Usermodel;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;

namespace AdminServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration _config;
        public LoginController(IConfiguration config)
        {
            _config = config;
        }

        [Route("Admins")]
        [HttpGet]
        public IList<AdminCredentials> GetAdmins()
        {
            using (var db = new Admin_DatabaseContext())
            {
                return db.AdminCredentials.ToList();
            }
        }

        [Route("AdminLogin")]
        [HttpPost]
        public IActionResult LoginAdmin([FromBody] AdminCredentials credObj)
        {
            if (credObj == null)
                return NotFound(new
                {
                    success = 0,
                    message = "LogIn Failed"
                });
            else
            {
                var db = new Admin_DatabaseContext();
                var admin = db.AdminCredentials.Where(a =>
                    a.Email == credObj.Email
                    && a.Password == credObj.Password).FirstOrDefault();
                if (admin != null)
                {
                    var token = GenerateTokenUsingJwt(credObj.Email);

                    return Ok(new
                    {
                        success = 1,
                        message = "Logged In Successfully",
                        token = token
                    });
                }
                else
                    return NotFound(new
                    {
                        success = 0,
                        message = "LogIn Failed"
                    });
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
