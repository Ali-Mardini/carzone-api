using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using carzone_api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace carzone_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {

        [HttpGet("booking")]
        public IActionResult GetAllBooks()
        {
            try
            {
                var jsonData = System.IO.File.ReadAllText("api.json");

                if (!String.IsNullOrEmpty(jsonData))
                {
                    var bookingList = JsonConvert.DeserializeObject<List<BookDTO>>(jsonData);

                    return Ok(new { success = true, response = bookingList });
                }
                else
                {
                    return NotFound(new { success = true, response = "No booking found!" });
                }
                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("book")]
        public IActionResult BookAnAppointment([FromBody] BookDTO bookingPayload)
        {
            try
            {
                var jsonData = System.IO.File.ReadAllText("api.json");

                var bookingList = JsonConvert.DeserializeObject<List<BookDTO>>(jsonData)
                      ?? new List<BookDTO>();

                bookingList.Add(bookingPayload);

                // Update json data string
                jsonData = JsonConvert.SerializeObject(bookingList);
                System.IO.File.WriteAllText("api.json", jsonData);

                return Ok(new { success = true, response = "Your book have been recevied!"});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}