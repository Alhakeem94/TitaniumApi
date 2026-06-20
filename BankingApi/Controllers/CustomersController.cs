using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingApi.Data;
using BankingApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace BankingApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {

        private readonly ApplicationDbContext _Db;
        public CustomersController(ApplicationDbContext Db)
        {
            _Db = Db;
        }


        [HttpPost("AddNewCustomer")]
        public ActionResult<string> AddCustomer([FromBody]CostumerModel NewCustomer)
        {
            if (ModelState.IsValid)
            {
                var DoesCustomerExist = _Db.CustomersTable.FirstOrDefault(a => a.CustomerNationalId == NewCustomer.CustomerNationalId);
                if (DoesCustomerExist is null)
                {
                    _Db.CustomersTable.Add(NewCustomer);
                    _Db.SaveChanges();
                    return Ok("Customer Added Successfully");
                }
                else
                {
                    return BadRequest("Customer Already Exists");
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }






    }
}