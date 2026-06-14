using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace BankingApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {

        [HttpGet("HelloWorld")]
        public ActionResult<string> GetHelloWorld()
        {
            return Ok("Hello World!");  // Ok = تدلل
        }


        [HttpGet("GetStudentName")]
        public ActionResult<string> GetStudentNameFromList([FromQuery] string Name, [FromQuery] int age)
        {
            return Ok($"Hello {Name} whos age is {age}");
        }



        [HttpPost("AddNewCustomer")]
        public ActionResult<string> AddNewCustomerEndPoint([FromBody] CostumerModel costumer)
        {
            var TrimmedName = costumer.CustomerName.Trim();
            return Ok(costumer);  
        }









    }
}