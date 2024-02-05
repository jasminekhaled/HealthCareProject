using HealthCare.Core.Models.AppointmentModule;
using HealthCare.Core.Models.BandModule;
using HealthCare.Core.Models.DoctorModule;
using HealthCare.Core.Models.HospitalModule;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.Models.PatientModule
{
    [Table("Patients")]
    public class Patient : GeneralUser
    {
        public bool IsEmailConfirmed { get; set; }
        [StringLength(11)]
        public string PhoneNumber { get; set; }
        public string FullName { get; set; }
        public string? VerificationCode { get; set; }
        public List<RateDoctor> RateDoctor { get; set; }
        public List<ClinicReservation> ClinicReservations { get; set; }
        public List<XrayReservation> XrayReservations { get; set; }
        public List<LabReservation> LabReservations { get; set; }
        public List<AllReservations> AllReservations { get; set; }
        public List<Band> Bands { get; set; }

    }
}
