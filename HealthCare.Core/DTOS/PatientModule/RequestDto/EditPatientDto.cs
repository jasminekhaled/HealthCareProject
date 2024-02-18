using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.DTOS.PatientModule.RequestDto
{
    public class EditPatientDto
    {
        public string? PhoneNumber { get; set; }
        public IFormFile? Image { get; set; }

    }
}
