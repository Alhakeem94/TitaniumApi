using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BankingApi.Models
{
    public class CostumerModel
    {

        public int Id { get; set; }


        [Required(ErrorMessage = "Name is a mandatory field")]
        [StringLength(100,ErrorMessage = "Name should be less than 100 characters")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Date of Birth is a mandatory field")]
        public DateTime CustomerDateOfBirth { get; set; }

        [Required(ErrorMessage = "National ID is a mandatory field")]
        public double CustomerNationalId { get; set; }
        [Required(ErrorMessage = "Gender is a mandatory field")]
        public bool IsMale { get; set; }
        [Required(ErrorMessage = "Grade is a mandatory field")]
        public int Grade { get; set; }
        public string? Notes { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string CustomerEmail { get; set; }
        
        [Required(ErrorMessage = "Phone Number is a mandatory field")]
        public string CustomerPhoenNumber { get; set; }

    }
}