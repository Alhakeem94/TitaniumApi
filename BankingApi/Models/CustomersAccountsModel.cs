using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using BankingApi.Models.BaseEntities;
using BankingApi.Shared.Enums;

namespace BankingApi.Models
{
    public class CustomersAccountsModel : AuditModel
    {
        public int Id { get; set; }



        [ForeignKey("CustomerModel")]
        public int CustomerId { get; set; }
        public CostumerModel? CustomerModel { get; set; }



        [ForeignKey("AccountType")]
        public int AccountId { get; set; }
        public AccountsTypesModel? AccountType { get; set; }


        public DateTime DateOfAccountActivation { get; set; } = DateTime.Now;
        public double StartingBalance { get; set; }
        public double CurrentBalance { get; set; }
        
        public AccountStatuses AccountStatus { get; set; }

        public DateTime CreatedAt { get; set; }




    }
}