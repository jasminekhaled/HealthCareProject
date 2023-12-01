using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.DTOS.DoctorModule.RequestDtos
{
    public class RateRequestDto
    {
        [MaxLength(length: 2)]
        public int Rate { get; set; }
    }
}
