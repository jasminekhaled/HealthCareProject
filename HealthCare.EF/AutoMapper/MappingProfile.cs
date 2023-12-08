using AutoMapper;
using HealthCare.Core.DTOS.AuthModule.RequestDtos;
using HealthCare.Core.DTOS.AuthModule.ResponseDtos;
using HealthCare.Core.DTOS.DoctorModule.RequestDtos;
using HealthCare.Core.DTOS.DoctorModule.ResponseDtos;
using HealthCare.Core.DTOS.HospitalModule.RequestDto;
using HealthCare.Core.DTOS.HospitalModule.ResponseDto;
using HealthCare.Core.DTOS.PatientModule.ResponseDto;
using HealthCare.Core.Models;
using HealthCare.Core.Models.AuthModule;
using HealthCare.Core.Models.DoctorModule;
using HealthCare.Core.Models.HospitalModule;
using HealthCare.Core.Models.PatientModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.EF.AutoMapper
{
    public class MappingProfile : Profile
    {

        public MappingProfile()
        {
            CreateMap<User ,UserTokenDto>();
            CreateMap<SignUpRequestDto, Patient>()
                .ForMember(src => src.PassWord, opt => opt.Ignore());
            CreateMap<Patient, SignUpResponse>();
            CreateMap<Patient, User>()
                .ForMember(src => src.Id, opt => opt.Ignore());
            CreateMap<Patient, VerifyResponse>();
            CreateMap<Doctor, LogInResponse>();
            CreateMap<User, LogInResponse>();
            CreateMap<Patient, PatientDto>();
            CreateMap<HospitalRequestDto, Hospital>();
            CreateMap<Hospital, HospitalDto>()
                .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => src.UploadedFile.FilePath))
                .ForMember(dest => dest.Governorate, opt => opt.MapFrom(src => src.HospitalGovernorate.Governorate.Name));
            CreateMap<Hospital, ListOfHospitalDto>()
                .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => src.UploadedFile.FilePath))
                .ForMember(dest => dest.Governorate, opt => opt.MapFrom(src => src.HospitalGovernorate.Governorate.Name));
            CreateMap<HospitalAdmin, HospitalAdminDto>()
                .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => src.UploadedFile.FilePath));
            CreateMap<HospitalAdminRequestDto, HospitalAdmin>();
            CreateMap<HospitalAdmin, User>()
                .ForMember(src => src.Id, opt => opt.Ignore());
            CreateMap<HospitalAdmin, EditHospitalAdminResponse>()
                .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => src.UploadedFile.FilePath));
            CreateMap<HospitalAdmin, ListOfHospitalAdminDto>()
                .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => src.UploadedFile.FilePath));
            CreateMap<Specialization, SpecializationDto>();
            CreateMap<Governorate, GovernorateDto>();
            CreateMap<DoctorRequestDto, Doctor>()
                .ForMember(src => src.Id, opt => opt.Ignore())
                .ForMember(src => src.hospitalDoctors, opt => opt.Ignore())
                .ForMember(src => src.RateDoctor, opt => opt.Ignore())
                .ForMember(src => src.clinicLabDoctors, opt => opt.Ignore())
                .ForMember(src => src.DoctorSpecialization, opt => opt.Ignore());
            CreateMap<Doctor, User>()
                .ForMember(src => src.Id, opt => opt.Ignore());
            CreateMap<Doctor, AddDoctorResponseDto>();
            CreateMap<Hospital, HospitalIdDto>();
            CreateMap<ClinicLab, ClinicIdDto>();
        }
    }
}
