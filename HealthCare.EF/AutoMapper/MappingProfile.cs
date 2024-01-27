﻿using AutoMapper;
using HealthCare.Core.DTOS.AppointmentModule.RequestDto;
using HealthCare.Core.DTOS.AppointmentModule.ResponseDto;
using HealthCare.Core.DTOS.AuthModule.RequestDtos;
using HealthCare.Core.DTOS.AuthModule.ResponseDtos;
using HealthCare.Core.DTOS.ClinicModule.ResponseDto;
using HealthCare.Core.DTOS.DoctorModule.RequestDtos;
using HealthCare.Core.DTOS.DoctorModule.ResponseDtos;
using HealthCare.Core.DTOS.HospitalModule.RequestDto;
using HealthCare.Core.DTOS.HospitalModule.ResponseDto;
using HealthCare.Core.DTOS.PatientModule.ResponseDto;
using HealthCare.Core.Models;
using HealthCare.Core.Models.AppointmentModule;
using HealthCare.Core.Models.AuthModule;
using HealthCare.Core.Models.ClinicModule;
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
            CreateMap<Doctor, DoctorDto>()
                .ForMember(src => src.Hospitals, opt => opt.Ignore())
                .ForMember(src => src.Specializations, opt => opt.Ignore())
                .ForMember(src => src.Rate, opt => opt.Ignore());
            CreateMap<Doctor, EditDoctorResponseDto>()
                .ForMember(src => src.ImagePath, opt => opt.Ignore())
                .ForMember(src => src.specializations, opt => opt.Ignore());
            CreateMap<Specialization, AddClinicResponseDto>()
                .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => src.UploadedFile.FilePath));
            CreateMap<XraySpecialization, AddClinicResponseDto>()
                .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => src.UploadedFile.FilePath));
            CreateMap<LabSpecialization, AddClinicResponseDto>()
                .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => src.UploadedFile.FilePath));
            CreateMap<ClinicLab, AddClinicResponseDto>()
                .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => src.Specialization.UploadedFile.FilePath))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Specialization.Name));
            CreateMap<Xray, AddClinicResponseDto>()
                .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => src.XraySpecialization.UploadedFile.FilePath))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.XraySpecialization.Name));
            CreateMap<Lab, AddLabResponseDto>();
            CreateMap<LabSpecialization, SpecializationDto>();
            CreateMap<ClinicAppointment, AppointmentResponseDto>()
                .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Doctor.FullName))
                .ForMember(dest => dest.DoctorRate, opt => opt.MapFrom(src => src.Doctor.Rate))
                .ForMember(dest => dest.DoctorDiscription, opt => opt.MapFrom(src => src.Doctor.Description));
            CreateMap<ClinicAppointmentDate, AppointmentDateResponseDto>()
                .ForMember(dest => dest.DayName, opt => opt.MapFrom(src => src.Day.DayName));
        }
    }
}
