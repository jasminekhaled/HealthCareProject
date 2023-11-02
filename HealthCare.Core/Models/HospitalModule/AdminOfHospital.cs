using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.Models.HospitalModule
{
    public class AdminOfHospital
    {
        public int Id { get; set; }
        public int HospitalId { get; set; }
        public Hospital Hospital { get; set; }
        public int HospitalAdminId { get; set; }
        public HospitalAdmin HospitalAdmin { get; set; }

    }
}
