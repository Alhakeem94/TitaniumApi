using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using BankingApi.Models.Identity;

namespace BankingApi.Models.BaseEntities
{
    public class AuditModel
    {

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [ForeignKey("AppUserObject")]
        public string? CreatedBy { get; set; }
        public AppUser? AppUserObject { get; set; }



        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [ForeignKey("UpdatedUser")]
        public string? UpdatedBy { get; set; }
        public AppUser? UpdatedUser { get; set; }




        public DateTime DeletedAt { get; set; } = DateTime.Now;

        [ForeignKey("DeleterUser")]
        public string? DeletedBy { get; set; }
        public AppUser? DeleterUser { get; set; }


    }
}