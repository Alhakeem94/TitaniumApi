using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingApi.Models
{
    public class AccountsTypesModel
    {
        public int Id { get; set; }
        public string AccountName { get; set; }
        public int YearsOfDeposit { get; set; }
        public bool HasInterist { get; set; } = false;
        public int IntrestRate { get; set; }
        public double MaxAccountLimit { get; set; }
    }
}