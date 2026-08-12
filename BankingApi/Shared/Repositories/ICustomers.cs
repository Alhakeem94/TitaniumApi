using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingApi.Data;
using BankingApi.Models;
using BankingApi.Shared.responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static BankingApi.Shared.DTOs.CustomersDTOs.CustomersDTOs;

namespace BankingApi.Shared.Repositories
{
    public interface ICustomers
    {
        public Task<PaginatedResult<CustomersDTO>> GetAllCustomers(int Page = 1, int pageSize = 25);
        public Task<List<CustomersAccountsModel>> customersAccountsModel(int CustomerId);
        public Task<string> AddAccount(AccountsTypesModel accountsTypesModel);
        public Task<string> AddCustomerAccount(CustomersAccountsModel customersAccountsModel);

    }

    public class CustomersRepository : ICustomers
    {

        private readonly ApplicationDbContext _Db;

        public CustomersRepository(ApplicationDbContext db)
        {
            _Db = db;
        }

        public async Task<List<CustomersAccountsModel>> customersAccountsModel(int CustomerId)
        {
            var customerAccounts = await _Db.CustomersAccountsTable.Include(a => a.CustomerModel)
                                   .Include(a => a.AccountType).Include(a => a.AppUserObject)
                                   .Where(a => a.CustomerId == CustomerId).ToListAsync();

            return customerAccounts;
        }

        // Page 10000, PageSize 1000000
        public async Task<PaginatedResult<CustomersDTO>> GetAllCustomers(int Page = 1, int pageSize = 25)
        {

            var MaxPageSize = Math.Min(pageSize, 25);
            var CostomersCount = await _Db.CustomersTable.CountAsync();
            var TotalPages = (int)Math.Ceiling(CostomersCount / (double)MaxPageSize);


            var Query = _Db.CustomersTable.OrderBy(a => a.Id)
                        .Select(t => new CustomersDTO(t.CustomerName, t.CustomerDateOfBirth,
                             t.CustomerNationalId, t.CustomerEmail, t.CustomerPhoenNumber));

            var Items = await Query.Skip((Page - 1) * MaxPageSize).Take(MaxPageSize).ToListAsync();

          
            return new PaginatedResult<CustomersDTO>
            {
                Items = Items,
                Page = Page,
                PageSize = MaxPageSize,
                TotalCount = CostomersCount
            };

        }


        public async Task<string> AddAccount(AccountsTypesModel accountsTypesModel)
        {
            var DoesAccountTypeExist = await _Db.AccountTypesTable
                                       .FirstOrDefaultAsync(a => a.AccountName == accountsTypesModel.AccountName);

            if (DoesAccountTypeExist is not null)
            {
                return "Account Type Already Exists";
            }

            _Db.AccountTypesTable.Add(accountsTypesModel);
            await _Db.SaveChangesAsync();
            return "Account Type Added Successfully";
        }


        public async Task<string> AddCustomerAccount(CustomersAccountsModel customersAccountsModel)
        {
            var DoesCustomerExist = await _Db.CustomersTable
                                    .FirstOrDefaultAsync(a => a.Id == customersAccountsModel.CustomerId);

            if (DoesCustomerExist is null)
            {
                return "Customer Does Not Exist";
            }

            var DoesAccountTypeExist = await _Db.AccountTypesTable
                                       .FirstOrDefaultAsync(a => a.Id == customersAccountsModel.AccountId);

            if (DoesAccountTypeExist is null)
            {
                return "Account Type Does Not Exist";
            }

            _Db.CustomersAccountsTable.Add(customersAccountsModel);
            await _Db.SaveChangesAsync();
            return "Customer Account Added Successfully";
        }

       

    }
}