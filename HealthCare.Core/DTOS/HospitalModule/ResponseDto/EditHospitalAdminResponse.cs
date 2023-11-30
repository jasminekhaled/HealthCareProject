using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.DTOS.HospitalModule.ResponseDto
{
    public class EditHospitalAdminResponse
    {
        public string UserName { get; set; }
        public string NationalId { get; set; }
        public string Email { get; set; }
        public string ImagePath { get; set; }
    }
}
