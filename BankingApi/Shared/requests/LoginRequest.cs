using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BankingApi.Shared.requests
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Email is a mandatory field")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string UserEmail { get; set; }
        
        [Required(ErrorMessage = "Password is a mandatory field")]
        public string  Password{ get; set; }
    }
}