using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.DTOS.ClinicModule.RequestDto
{
    public class SpecializationRequestDto
    {
        public string Name { get; set; }
        public IFormFile Image { get; set; }
    }
}
