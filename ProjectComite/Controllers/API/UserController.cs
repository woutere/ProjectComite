using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectComite.Models;
using ProjectComite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using ProjectComite.Areas.Identity.Data;
using ProjectComite.Helpers;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HelloCore.Controllers.api
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly SignInManager<CustomUser> _signInManager;
        private readonly UserManager<CustomUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly AppSettings _appSettings;

        public UserController(
            UserManager<CustomUser> userManager,
            SignInManager<CustomUser> signInManager,
            IConfiguration configuration,
            IOptions<AppSettings> appSettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _appSettings = appSettings.Value;
        }

        [HttpPost("authenticate")]
        public async Task<Object> Authenticate([FromBody]User userParam)
        {
            var result = await _signInManager.PasswordSignInAsync(userParam.Username, userParam.Password, false, false);

            if (result.Succeeded)
            {
                CustomUser appUser = _userManager.Users.SingleOrDefault(r => r.Email == userParam.Username);
                userParam.Token = GenerateJwtToken(userParam.Email, appUser).ToString();

                return userParam;
            }

            return BadRequest(new { message = "Username or password is incorrect" });
        }

        private String GenerateJwtToken(String email, CustomUser appUser)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, appUser.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}