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

    }

    public class CustomersRepository : ICustomers
    {

        private readonly ApplicationDbContext _Db;

        public CustomersRepository(ApplicationDbContext db)
        {
            _Db = db;
        }





        // Page 10000, PageSize 1000000
        public async Task<PaginatedResult<CustomersDTO>> GetAllCustomers(int Page = 1, int pageSize = 25)
        {

            var MaxPageSize = Math.Min(pageSize, 25);
            var CostomersCount = await _Db.CustomersTable.CountAsync();
            var TotalPages = (int)Math.Ceiling(CostomersCount / (double)MaxPageSize);


            var Query = _Db.CustomersTable.OrderBy(a => a.Id).Select(t => new CustomersDTO(t.CustomerName, t.CustomerDateOfBirth,
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

       

    }
}