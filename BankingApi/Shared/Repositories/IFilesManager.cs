using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using BankingApi.Shared.DTOs.FilesDTOs;

namespace BankingApi.Shared.Repositories
{
    public interface IFilesManager
    {
        public Task<UploadFileDTO> UploadFileAsync(string filePath, IFormFile UploadedFile);

    }


    public class FilesRepo : IFilesManager
    {

        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _Configuration;
        private static readonly string[] AllowedExtensions = [".jpg", ".jpeg", ".png", ".pdf", ".docx"];


        public FilesRepo(IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _webHostEnvironment = webHostEnvironment;
            _Configuration = configuration;
        }





        public async Task<UploadFileDTO> UploadFileAsync(string filePath, IFormFile UploadedFile)
        {
            if (UploadedFile is null || UploadedFile.Length == 0)
            {
                return new UploadFileDTO
                {
                    IsSaved = false,
                    StatusMessage = "No file uploaded."
                };
            }
            {

                if (AllowedExtensions.Contains(UploadedFile.FileName.ToLower()))
                {

                    // BankingApi/wwwroot/2026/8/5/981723bads8y19uihedigasty87d
                    var UploadedFilePath = Path.Combine(_Configuration["Storage:StoragePath"], filePath);
                    Directory.CreateDirectory(UploadedFilePath); // Ensure the directory exists
                    Console.WriteLine(UploadedFilePath);
                    var newFileName = Guid.NewGuid().ToString();

                    var FullPath = Path.Combine(UploadedFilePath, newFileName + Path.GetExtension(UploadedFile.FileName));
                    Console.WriteLine(FullPath);
                    await using (var stream = File.Create(FullPath))
                        await UploadedFile.CopyToAsync(stream);

                    return new UploadFileDTO
                    {
                        IsSaved = true,
                        StatusMessage = "File uploaded successfully.",
                        SavedFilePath = FullPath,
                        ContentType = UploadedFile.ContentType,
                        OriginalName = UploadedFile.FileName,
                        Size = UploadedFile.Length,
                        StoredName = newFileName,
                        UploadedAt = DateTime.Now,
                    };

                }
                else
                {
                    return new UploadFileDTO
                    {
                        IsSaved = false,
                        StatusMessage = "Invalid file format. Allowed formats are: .jpg, .jpeg, .png, .pdf, .docx"
                    };
                }
            }
        }



    }

}