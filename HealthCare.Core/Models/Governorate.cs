using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthCare.Core.Models.HospitalModule;

namespace HealthCare.Core.Models
{
    public class Governorate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<HospitalGovernorate> HospitalGovernorates { get; set; }
    }
}
