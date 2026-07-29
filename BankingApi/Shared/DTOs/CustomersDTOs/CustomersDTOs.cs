using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingApi.Shared.DTOs.CustomersDTOs
{
    public class CustomersDTOs
    {
        public record CustomersDTO(string CustomerName,DateTime customerDateOfBirth,
                                    double CustomerNationalId,string CustomerEmail,string customerPhoenNumber);
    }
}