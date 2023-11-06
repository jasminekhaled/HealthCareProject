using AutoMapper;
using HealthCare.Core.DTOS.AuthModule.RequestDtos;
using HealthCare.Core.DTOS.AuthModule.ResponseDtos;
using HealthCare.Core.DTOS.HospitalModule.RequestDto;
using HealthCare.Core.DTOS.HospitalModule.ResponseDto;
using HealthCare.Core.DTOS.PatientModule.ResponseDto;
using HealthCare.Core.Models.AuthModule;
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
            CreateMap<Hospital, HospitalDto>();


        }
    }
}
