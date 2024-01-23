using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.DTOS.ClinicModule.ResponseDto
{
    public class AddClinicResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
    }
}
