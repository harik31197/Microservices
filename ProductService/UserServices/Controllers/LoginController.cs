using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using UserServices.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UserServices.Controllers
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


    // POST api/<LoginController>
    [AllowAnonymous]
    [HttpPost]
    public IActionResult Login([FromBody] UserLogin userLogin)
    {
      try
      {
        var user = Authenticate(userLogin);
        if (user != null)
        {
          var token = Generate(user);
          return Ok(token);
        }        

      }
      catch(Exception ex)
      {
        throw ex;
      }
      return NotFound();
    }

    private string Generate(UserModel user)
    {
      var h = _config["Jwt:Key"];
      var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
      var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
      var claims = new[]
      {
        new Claim(ClaimTypes.NameIdentifier,user.Username),
        new Claim(ClaimTypes.GivenName,user.Name),
        new Claim(ClaimTypes.Role,user.Role)

      };

      var token = new JwtSecurityToken(_config["Jwt:Issuer"],
        _config["Jwt:Audience"],
        claims,expires: DateTime.Now.AddHours(1),
        signingCredentials:credentials);
      return new JwtSecurityTokenHandler().WriteToken(token); 
    }

    private UserModel Authenticate(UserLogin userLogin)
    {
      var currentUser = UserConstant.Users.FirstOrDefault(a => a.Username.ToLower() == userLogin.Username.ToLower() && a.Password == userLogin.Password);
      if (currentUser != null)
      {
        return currentUser;
      }
      return null;
    }

  }
}
