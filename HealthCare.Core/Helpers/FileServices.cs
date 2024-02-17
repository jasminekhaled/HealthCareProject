using HealthCare.Core.DTOS;
using HealthCare.Core.DTOS.DoctorModule.ResponseDtos;
using HealthCare.Core.IRepositories;
using HealthCare.Core.Models.AuthModule;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.Helpers
{
    public class FileServices
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;

        public FileServices(IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
        }
        public GeneralResponse<UploadedFile> ImageValidations(IFormFile image, string imageFile)
        {
            if(image != null)
            {
                var MaxAllowedPosterSize = _configuration.GetValue<long>("MaxAllowedPosterSize");
                List<string> AllowedExtenstions = _configuration.GetSection("AllowedExtenstions").Get<List<string>>();

                if (!AllowedExtenstions.Contains(Path.GetExtension(image.FileName).ToLower()))
                {
                    return new GeneralResponse<UploadedFile>
                    {
                        IsSuccess = false,
                        Message = "Only .jpg and .png Images Are Allowed."
                    };
                }

                if (image.Length > MaxAllowedPosterSize)
                {
                    return new GeneralResponse<UploadedFile>
                    {
                        IsSuccess = false,
                        Message = "Max Allowed Size Is 1MB."
                    };
                }
                var fakeFileName = Path.GetRandomFileName();
                var uploadedFile = new UploadedFile()
                {
                    FileName = image.FileName,
                    ContentType = image.ContentType,
                    StoredFileName = fakeFileName
                };
                var directoryPath = Path.Combine(_webHostEnvironment.ContentRootPath, "Uploads", imageFile);
                var path = Path.Combine(directoryPath, fakeFileName);
                using FileStream fileStream = new(path, FileMode.Create);
                image.CopyTo(fileStream);
                uploadedFile.FilePath = path;
                return new GeneralResponse<UploadedFile>
                {
                    IsSuccess = true,
                    Data = uploadedFile
                };
            }
            else
            {
                var DefaultFile = new UploadedFile()
                {
                    FileName = "DefaultImage.png",
                    StoredFileName = "DefaultImage",
                    ContentType = "image/png",
                    FilePath = "G:\\WEB DEVELOPMENT\\HealthCareProject\\HealthCareAPIs\\HealthCare\\Uploads\\DefaultImage"

                };
                return new GeneralResponse<UploadedFile>
                {
                    IsSuccess = true,
                    Data = DefaultFile
                };
            }
                
        }
    }
}
