using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BankingApi.Models.Identity
{
    public class RefreshTokensModel
    {
        public int Id { get; set; }
        public string RefreshToken { get; set; }

        [ForeignKey("user")]
        public string UserId { get; set; }
        public AppUser User { get; set; }


        public DateTime ExpiresAt { get; set; }
        public DateTime? RevokedAt { get; set; } 
        public DateTime CreatedAt { get; set; }

        public bool IsActive => RevokedAt <= DateTime.Now && DateTime.Now <= ExpiresAt;

    }
}