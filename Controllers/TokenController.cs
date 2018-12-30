using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiJwt.Controllers{
    [Route("api/token")]
    public class TokenController : Controller
    {
        private IConfiguration _configuration;

        public TokenController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [AllowAnonymous] // Forventer at der medfølger et "LoginModel" objekt i form af user/pwd
        [HttpPost]
        public IActionResult CreateToken([FromBody]LoginModel login)
        {
            IActionResult response = Unauthorized();
            var user = Authenticate(login);

            if (user!=null){
                var tokenString = BuildToken(user);
                response = Ok(new { token = tokenString});
            }

            return response;
        }

        private string BuildToken(UserModel user)
        {
            // Anvender "key" fra appsettings.json som krypteringsnøgle
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Konfigurerer token ift. vores appsettings.json
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: creds);
            
            return new JwtSecurityTokenHandler().WriteToken(token);    
        }

        private UserModel Authenticate(LoginModel login)
        {
            UserModel user = null;

            if (login.Username == "Anders" && login.Password == "1234")
            {
                user = new UserModel { Name = "Anders And", Email = "Anders@andeby.dk"};
            }

            return user;
        }



        public class LoginModel
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        private class UserModel
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public DateTime Birthdate { get; set; }
        }
    }


}