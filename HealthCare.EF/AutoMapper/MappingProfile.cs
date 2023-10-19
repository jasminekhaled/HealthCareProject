using AutoMapper;
using HealthCare.Core.DTOS.AuthModule.RequestDtos;
using HealthCare.Core.Models.AuthModule;
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
            CreateMap<User, UserTokenDto>();
            CreateMap<SignUpRequestDto, Patient>()
                .ForMember(src => src.PassWord, opt => opt.Ignore());
            CreateMap<SignUpRequestDto, User>()
                .ForMember(src => src.PassWord, opt => opt.Ignore());
        }
    }
}
