using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using UserServices.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UserServices.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class UserController : ControllerBase
  {
    [HttpGet]
    public IActionResult GetAllUsers()
    {
      var user = UserConstant.Users.ToList();
      if (user == null) return NotFound();
      return Ok(user);

    }
    [HttpGet("Admins")]
    public IActionResult AdminEndpoint()
    {
      var currentUser = GetCurrentuser();
      return Ok($"Hi {currentUser.Name}, You are an {currentUser.Role}.");
    }
    // GET: api/<UserController>
    [HttpGet("Public")]
    public UserConstant[] Public()
    {
      return new UserConstant[0];

    }
    [HttpPost("adduser")]
    public IActionResult AddUser([FromBody] UserModel user)
    {
      try
      {
         var users = addNewUser(user);
        return Ok(users);
         
      }
      catch(Exception ex)
      {
        return BadRequest(ex.Message);
      }     

    }

    [HttpPut("editUser")]
    public IActionResult EditUser([FromBody] UserModel user)
    {
      try
      {

        editUser(user);
        return Ok(user);      

      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }

    }

    private IActionResult editUser(UserModel user)
    {

      var isUserExistng = UserConstant.Users.Where(a => a.Username == user.Username).Any();
      if (isUserExistng)
      {
        var getuser = UserConstant.Users.Where(a => a.Username == user.Username).FirstOrDefault();
        getuser.Name = user.Name;
        getuser.Role = user.Role;
        return Ok("Update Successful");
      }
      return BadRequest("Username does not exist");
    }

    private IActionResult addNewUser(UserModel user)
    {
     
        var dupUser = UserConstant.Users.Where(a => a.Username == user.Username).Any();
        if (dupUser)
        {
          return BadRequest("Username already exists");

        }
        UserConstant.Users.Add(user);
        return Ok(user);
      
    }
    
    private UserModel GetCurrentuser()
    {
      var identity = HttpContext.User.Identity as ClaimsIdentity;
      if (identity != null)
      {
        var userClaims = identity.Claims;
        return new UserModel
        {
          Username = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value,
          Name = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.GivenName)?.Value,
          Role = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value,

        };
      }
      return null;

    }
  
  }
}
