using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.DTOS.HospitalModule.RequestDto
{
    public class EditHospitalAdminDto
    {
        public IFormFile? Image { get; set; }
    }
}
