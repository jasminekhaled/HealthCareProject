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

        [MinLength(length: 10)]
        [MaxLength(length: 100)]
        public string? UserName { get; set; }
        public string? PhoneNumber { get; set; }

    }
}
