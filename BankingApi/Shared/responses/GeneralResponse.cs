using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingApi.Shared.responses
{
    public class GeneralResponse
    {
        public bool IsSuccessful { get; set; } 
        public string Message { get; set; } 
    }
}