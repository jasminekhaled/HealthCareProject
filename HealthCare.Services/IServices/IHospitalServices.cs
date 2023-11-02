using HealthCare.Core.DTOS.PatientModule.ResponseDto;
using HealthCare.Core.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthCare.Core.DTOS.HospitalModule.ResponseDto;
using HealthCare.Core.DTOS.HospitalModule.RequestDto;
using HealthCare.Core.Models.HospitalModule;

namespace HealthCare.Services.IServices
{
    public interface IHospitalServices
    {
        Task<GeneralResponse<List<HospitalDto>>> ListOfHospitals();
        Task<GeneralResponse<List<HospitalDto>>> GetHospitalByGovernment(string GovernmentName);
        Task<GeneralResponse<List<PatientDto>>> GetHospitalByName(string Name);
        Task<GeneralResponse<HospitalDto>> AddHospital(HospitalRequestDto dto);
        Task<GeneralResponse<string>> DeleteHospital(int hospitalId);
        Task<GeneralResponse<HospitalDto>> HospitalDetails(int hospitalId);
        Task<GeneralResponse<HospitalDto>> EditHospital(EditHospitalDto dto);
        Task<GeneralResponse<HospitalAdminDto>> AddHospitalAdmin(int OldHospitalAdminId, HospitalAdminRequestDto dto);
        Task<GeneralResponse<List<HospitalAdminDto>>> ListOfSpecificHospitalAdmins(int HospitalId);
        Task<GeneralResponse<string>> DeleteHospitalAdmin(int hospitalAdminId);
        Task<GeneralResponse<HospitalAdminDto>> HospitalAdminDetails(int hospitalAdminId);
        Task<GeneralResponse<HospitalAdminDto>> EditHospitalAdmin(EditHospitalAdminDto dto);

    }
}
