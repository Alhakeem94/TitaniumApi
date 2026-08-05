using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingApi.Shared.DTOs.FilesDTOs
{
    public class UploadFileDTO
    {
        public string OriginalName { get; set; } = "";   // shown to users
        public string StoredName { get; set; } = "";     // the GUID name on disk
        public string ContentType { get; set; } = "";
        public long Size { get; set; }
        public string SavedFilePath { get; set; } = "";
        public DateTime UploadedAt { get; set; }

        public bool IsSaved { get; set; } = false;
        public string StatusMessage { get; set; } = "";

    }
}