using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingApi.Data;
using BankingApi.Models;
using BankingApi.Shared.Repositories;
using BankingApi.Shared.requests;
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
        private readonly IFilesManager _FileManager;

        public CustomersController(ApplicationDbContext Db, ICustomers customers, IFilesManager fileManager)
        {
            _Db = Db;
            _customers = customers;
            _FileManager = fileManager;
        }


        [HttpPost("AddNewCustomer")]

        public async Task<ActionResult<string>> AddCustomer([FromForm] AddNewCustomerRequest NewCustomer)
        {
            if (ModelState.IsValid)
            {
                var DoesCustomerExist = _Db.CustomersTable.FirstOrDefault(a => a.CustomerNationalId == NewCustomer.CustomerNationalId);
                if (DoesCustomerExist is null)
                {
                    var UploadedFileResult = await _FileManager.UploadFileAsync($"{DateTime.Now.Year}/{DateTime.Now.Month}/{DateTime.Now.Day}", NewCustomer.CustomerProfileImage);
                    var CustomerModelObject = new CostumerModel
                    {
                        CustomerName = NewCustomer.CustomerName,
                        CustomerDateOfBirth = NewCustomer.CustomerDateOfBirth,
                        CustomerNationalId = NewCustomer.CustomerNationalId,
                        IsMale = NewCustomer.IsMale,
                        Grade = NewCustomer.Grade,
                        Notes = NewCustomer.Notes,
                        CustomerEmail = NewCustomer.CustomerEmail,
                        CustomerPhoenNumber = NewCustomer.CustomerPhoenNumber,
                        Adress = NewCustomer.Adress,
                        CustomerProfileImagePath = UploadedFileResult.SavedFilePath
                    };


                    await _Db.CustomersTable.AddAsync(CustomerModelObject);
                    await _Db.SaveChangesAsync();
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
            var ListOfAllCustomers = await _customers.GetAllCustomers(Page, pageSize);
            return Ok(ListOfAllCustomers);
        }


        [HttpGet("GetCustomerAccounts")]
        public async Task<ActionResult<List<CustomersAccountsModel>>> GetCustomerAccounts([FromQuery] int CustomerId)
        {
            var CustomerAccounts = await _customers.customersAccountsModel(CustomerId);
            return Ok(CustomerAccounts);
        }


        [HttpPost("AddAccount")]
        public async Task<ActionResult<string>> AddAccount([FromBody] AccountsTypesModel NewAccountType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var Result = await _customers.AddAccount(NewAccountType);

            if (Result == "Account Type Already Exists")
            {
                return BadRequest(Result);
            }

            return Ok(Result);
        }


        [HttpPost("AddCustomerAccount")]
        public async Task<ActionResult<string>> AddCustomerAccount([FromBody] CustomersAccountsModel NewCustomerAccount)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var Result = await _customers.AddCustomerAccount(NewCustomerAccount);

            if (Result != "Customer Account Added Successfully")
            {
                return BadRequest(Result);
            }

            return Ok(Result);
        }








    }
}