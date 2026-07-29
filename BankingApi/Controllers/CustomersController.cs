using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingApi.Data;
using BankingApi.Models;
using BankingApi.Shared.Repositories;
using BankingApi.Shared.responses;
using Microsoft.AspNetCore.Mvc;
using static BankingApi.Shared.DTOs.CustomersDTOs.CustomersDTOs;

namespace BankingApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {

        private readonly ApplicationDbContext _Db;
        private readonly ICustomers _customers;

        public CustomersController(ApplicationDbContext Db, ICustomers customers)
        {
            _Db = Db;
            _customers = customers;
        }


        [HttpPost("AddNewCustomer")]
        public ActionResult<string> AddCustomer([FromBody] CostumerModel NewCustomer)
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


        [HttpGet("GetAllCustomers")]
        public async Task<ActionResult<PaginatedResult<CustomersDTO>>> GetAllCustomers([FromQuery] int Page = 1, [FromQuery] int pageSize = 25)
        {
            var ListOfAllCustomers = await _customers.GetAllCustomers(Page,pageSize);
            return Ok(ListOfAllCustomers);
        }




    }
}