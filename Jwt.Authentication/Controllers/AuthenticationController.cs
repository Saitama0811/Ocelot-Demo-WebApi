using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Jwt.Authentication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private IConfiguration Configuration { get; }
        public AuthenticationController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        [HttpGet, Route("")]
        public IActionResult Login(string username, string password)
        {
            if (username == null)
            {
                return BadRequest("Invalid Client Request");
            }

            if (username == "saurabh.goel@xceedance.com" && password == "12345678")
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]));
                var signInCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                var tokenOptions = new JwtSecurityToken(
                    issuer: "http://localhost:4200",
                    audience: "http://localhost:4200",
                    claims: new List<Claim>(),
                    expires: DateTime.Now.AddMinutes(5),
                    signingCredentials: signInCredentials
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                return Ok(new { Token = tokenString, User = username, Date = DateTime.Now });
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}
