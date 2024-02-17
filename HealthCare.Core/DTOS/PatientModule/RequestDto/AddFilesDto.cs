using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.DTOS.PatientModule.RequestDto
{
    public class AddFilesDto
    {
        public IFormFile File { get; set; }
        [MaxLength(length:255)]
        public string? Description { get; set; }
    }
}
