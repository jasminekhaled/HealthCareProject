using HealthCare.Core.Models.AppointmentModule;
using HealthCare.Core.Models.AuthModule;
using HealthCare.Core.Models.BandModule;
using HealthCare.Core.Models.ClinicModule;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.Models.DoctorModule
{
    [Table("Doctors")]
    public class Doctor : GeneralUser
    {
        public string FullName { get; set; }
        public string Description { get; set; }
        public float Rate { get; set; }
        public List<DoctorSpecialization> DoctorSpecialization { get; set; }
        public List<HospitalDoctor> hospitalDoctors { get; set; }
        public List<ClinicLabDoctor> clinicLabDoctors { get; set; }
        public List<LabDoctor> LabDoctors { get; set; }
        public List<XrayDoctor> XrayDoctors { get; set; }
        public List<RateDoctor> RateDoctor { get; set; }
        public List<ClinicAppointment> ClinicAppointments { get; set; }
        public List<XrayAppointment> XrayAppointments { get; set; }
        public List<LabAppointment> LabAppointments { get; set; }
        public List<AllReservations> AllReservations { get; set; }
        public List<SavedBand> SavedBands { get; set; }


    }
}
