using InventoryService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace InventoryService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly InventoryContext _context;

        public TokenController(IConfiguration config, InventoryContext context)
        {
            _configuration = config;
            _context = context;
        }

        [HttpPost]
        public IActionResult Post(UserInfo _userData)
        {

            if (_userData != null && _userData.Email != null)
            {
                var user = GetUser(_userData.Email, _userData.Password);

                if (user != null)
                {
                    //create claims details based on the user information
                    var claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("Id", user.UserId.ToString()),
                    new Claim("FirstName", user.FirstName),
                    new Claim("LastName", user.LastName),
                    new Claim("UserName", user.UserName),
                    new Claim("Email", user.Email)
                   };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims, expires: DateTime.UtcNow.AddMinutes(1), signingCredentials: signIn);
                   
                    var sdssd = new JwtSecurityTokenHandler().WriteToken(token);
                    HttpContext.Request.Headers.Add("authToken", sdssd);
                    return Ok(sdssd);
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }
            }
            else
            {
                return BadRequest();
            }
        }

        private UserInfo GetUser(string email, string password)
        {
            UserInfo lora = new UserInfo();
            lora.UserId = 1;
            lora.FirstName = "Syed";
            lora.LastName = "talha";
            lora.UserName = "syedtalha30";
            lora.Email = "syedtalha30@gmail.com";
            return lora;
        }
    }
}