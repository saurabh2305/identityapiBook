using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BookIdentityAPI.Infrastructure;
using BookIdentityAPI.Models;
using BookIdentityAPI.Models.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BookIdentityAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private IdentityDbContext db;
        private IConfiguration config;
        public IdentityController(IdentityDbContext dbcontext, IConfiguration configuration)
        {
            db = dbcontext;
            config = configuration;
        }
        [HttpPost("register", Name = "RegisterUser")]
        public async Task<ActionResult<dynamic>> Register(User user)
        {
            string username = user.UserName;
            TryValidateModel(user);
            if (ModelState.IsValid)
            {

                var userObject = db.Users.SingleOrDefault(s => s.UserName == username);
                if (userObject == null)
                {
                    user.Status = true;
                    user.CreatedDate = DateTime.Now;
                    await db.Users.AddAsync(user);
                    await db.SaveChangesAsync();

                    return Created("", new
                    {
                        user.Id,
                        user.Name,
                        user.UserName,
                        user.Email,
                        user.UserType

                    });
                }
                else
                {
                    return "User already Exists";
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost("token", Name = "GetToken")]
        public ActionResult<dynamic> GetToken(LoginModel model)
        {
            TryValidateModel(model);
            if (ModelState.IsValid)
            {
                var user = db.Users.SingleOrDefault(s => s.UserName == model.Username && s.Password == model.Password && s.Status == true);
                if (user != null)
                {
                    var token = GenerateToken(user);
                    return Ok(new { user.Name, user.Email, user.UserName, Token = token, user.UserType });
                }
                else
                {
                    return Unauthorized();
                }
            }
            else
            {
                return BadRequest(ModelState);
            }

        }

        [NonAction]
        private string GenerateToken(User user)
        {
            var claims = new List<Claim>   //additional info need to add about token 
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.Name),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };
            claims.Add(new Claim(JwtRegisteredClaimNames.Aud, "catalogapi"));
            claims.Add(new Claim(JwtRegisteredClaimNames.Aud, "paymentapi"));
            claims.Add(new Claim(JwtRegisteredClaimNames.Aud, "basketapi"));
            claims.Add(new Claim(ClaimTypes.Role, user.UserType));
            //if (user.Username == "john")
            //{
            // claims.Add(new Claim(ClaimTypes.Role, user.Role));
            //}

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetValue<string>("Jwt:secret")));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: config.GetValue<string>("Jwt:issuer"),
                //audience: config.GetValue<string>("Jwt:audiance"),
                audience: null,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
             );
            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenString;
        }
    }
}