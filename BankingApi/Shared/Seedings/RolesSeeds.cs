using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingApi.Shared.Seedings
{
    public static class RolesSeeds
    {
        public const string Admin = "SuperAdmin";
        public const string Employee = "Admin";
        public const string BranchManager = "BranchManager";
        public const string Customer = "Customer";
        public const string Teller = "Teller";

        public static readonly string[] All = { Admin, Employee, BranchManager, Customer,Teller }; 
    }
}