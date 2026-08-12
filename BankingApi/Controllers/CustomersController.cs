using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using BankingApi.Data;
using BankingApi.Models;
using BankingApi.Shared.Repositories;
using BankingApi.Shared.requests;
using BankingApi.Shared.responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
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
        private readonly IMemoryCache _Cache;

        private readonly IDistributedCache _RedisCache;


        public CustomersController(ApplicationDbContext Db, ICustomers customers, IFilesManager fileManager, IMemoryCache cache, IDistributedCache redisCache)
        {
            _Db = Db;
            _customers = customers;
            _FileManager = fileManager;
            _Cache = cache;
            _RedisCache = redisCache;
        }


        [HttpPost("AddNewCustomer")]
        [RequestSizeLimit(5 * 1024 * 1024)]
        public async Task<ActionResult<string>> AddCustomer([FromForm] AddNewCustomerRequest NewCustomer)
        {
            if (ModelState.IsValid)
            {
                var DoesCustomerExist = _Db.CustomersTable.FirstOrDefault(a => a.CustomerNationalId == NewCustomer.CustomerNationalId);
                if (DoesCustomerExist is null)
                {
                    var UploadedFileResult = await _FileManager.UploadFileAsync($"{DateTime.Now.Year}/{DateTime.Now.Month}/{DateTime.Now.Day}", NewCustomer.CustomerProfileImage);
                    if (UploadedFileResult.IsSaved == false)
                    {
                        return BadRequest(UploadedFileResult.StatusMessage);
                    }
                    else
                    {
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


        [HttpGet("GetCustomerProfileImage")]
        public async Task<ActionResult<string>> GetCustomerProfileImage([FromQuery] int CustomerId)
        {
            var Customer = await _Db.CustomersTable.FirstOrDefaultAsync(a => a.Id == CustomerId);
            if (Customer is null)
            {
                return NotFound("Customer Not Found");
            }

            var ImagePath = Customer.CustomerProfileImagePath;

            return Ok(ImagePath);
        }



        [HttpGet("GetAllAccountTypes")]
        public async Task<ActionResult<List<AccountsTypesModel>>> GetAllAccountTypes()
        {

            return (await _Cache.GetOrCreateAsync($"accountsTypes :", async entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromSeconds(30);
                entry.SetSize(1024);
                return await _Db.AccountTypesTable.AsNoTracking().ToListAsync();   // the expensive queries
            }))!;
        }

        [HttpGet("GetAllAccountTypesRedis")]
        public async Task<ActionResult<List<AccountsTypesModel>>> GetAllAccountTypesRedis()
        {

             var key = $"accountsTypes:";
 
                var cached = await _RedisCache.GetStringAsync(key);
                if (cached is not null)
                    return System.Text.Json.JsonSerializer.Deserialize<List<AccountsTypesModel>>(cached)!;
            
                var stats = await _Db.AccountTypesTable.AsNoTracking().ToListAsync();
            
                await _RedisCache.SetStringAsync(key, JsonSerializer.Serialize(stats),
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
                    });
            
                return stats;
        }






    }
}