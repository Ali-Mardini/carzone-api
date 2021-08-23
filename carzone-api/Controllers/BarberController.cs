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
    }
}