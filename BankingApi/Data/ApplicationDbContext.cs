using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BankingApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<CostumerModel> CustomersTable { get; set; }
        //           DataSource      DestinationName


    }
}