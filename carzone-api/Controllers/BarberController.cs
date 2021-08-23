using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using carzone_api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNet.Identity;

namespace carzone_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BarberController : ControllerBase
    {
        private readonly PasswordHasher _passwordHasher;

        public BarberController()
        {
            _passwordHasher = new PasswordHasher();
        }

        [HttpPost("user")]
        public IActionResult CreateNewUser([FromBody] UserDTO userPayload)
        {
            try
            {
                var jsonData = System.IO.File.ReadAllText("barberApi.json");

                var userList = JsonConvert.DeserializeObject<List<UserDTO>>(jsonData)
                      ?? new List<UserDTO>();

                var user = new UserDTO()
                {
                    FirstName = userPayload.FirstName,
                    LastName = userPayload.LastName,
                    Email = userPayload.Email,
                    CompanyName = userPayload.CompanyName,
                    Password = _passwordHasher.HashPassword(userPayload.Password)
                };

                userList.Add(user);

                // Update json data string
                jsonData = JsonConvert.SerializeObject(userList);
                System.IO.File.WriteAllText("barberApi.json", jsonData);

                return Ok(new { success = true, response = "The user have been created successfully!" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDTO userPayload)
        {
            try
            {
                var jsonData = System.IO.File.ReadAllText("barberApi.json");

                if (!String.IsNullOrEmpty(jsonData))
                {
                    var usersList = JsonConvert.DeserializeObject<List<UserDTO>>(jsonData);

                    string hashedPassword = _passwordHasher.HashPassword(userPayload.Password);
                    var isValidPassword = _passwordHasher.VerifyHashedPassword(hashedPassword, userPayload.Password);

                    var user = (from u in usersList
                               where u.Email == userPayload.Username &&
                                     isValidPassword.ToString() == "Success"
                                select u).FirstOrDefault();

                    if (user != null)
                    {
                        return Ok(new { success = true, response = new {user.Email, message = "You have loggedin successfully!"} });
                    }
                    else
                    {
                        return NotFound(new { success = true, response = "Invalid username or password!" });
                    }
                    
                }
                else
                {
                    return NotFound(new { success = true, response = "Invalid username or password!" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}