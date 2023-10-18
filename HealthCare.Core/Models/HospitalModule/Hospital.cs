using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.Models.HospitalModule
{
    public class Hospital
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Government { get; set; }
        public string ImagePath { get; set; }
        public List<HospitalDoctor> hospitalDoctors { get; set; }

    }
}
