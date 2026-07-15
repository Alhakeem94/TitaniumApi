using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace BankingApi.Models.Identity
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
        public string Address { get; set; }
        public DateTime RegisteredAt { get; set; }
        public DateTime LastSignIn { get; set; }
    }
}