using AutoMapper;
using HealthCare.Core.DTOS.AppointmentModule.RequestDto;
using HealthCare.Core.DTOS.AppointmentModule.ResponseDto;
using HealthCare.Core.DTOS.AuthModule.RequestDtos;
using HealthCare.Core.DTOS.AuthModule.ResponseDtos;
using HealthCare.Core.DTOS.BandModule.ResponseDtos;
using HealthCare.Core.DTOS.ClinicModule.ResponseDto;
using HealthCare.Core.DTOS.DoctorModule.RequestDtos;
using HealthCare.Core.DTOS.DoctorModule.ResponseDtos;
using HealthCare.Core.DTOS.HospitalModule.RequestDto;
using HealthCare.Core.DTOS.HospitalModule.ResponseDto;
using HealthCare.Core.DTOS.PatientModule.RequestDto;
using HealthCare.Core.DTOS.PatientModule.ResponseDto;
using HealthCare.Core.Models;
using HealthCare.Core.Models.AppointmentModule;
using HealthCare.Core.Models.AuthModule;
using HealthCare.Core.Models.BandModule;
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
            CreateMap<User ,UserTokenDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));

            CreateMap<SignUpRequestDto, Patient>()
                .ForMember(src => src.PassWord, opt => opt.Ignore());

            CreateMap<Patient, SignUpResponse>()
                .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => src.UploadedFile.FilePath))
                .ForMember(dest => dest.PatientId, opt => opt.MapFrom(src => src.Id));

            CreateMap<Patient, User>()
                .ForMember(src => src.Id, opt => opt.Ignore());

            CreateMap<Doctor, LogInResponse>();

            CreateMap<User, LogInResponse>()
                .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => src.UploadedFile.FilePath));

            CreateMap<Patient, PatientDto>()
            .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => src.UploadedFile.FilePath));
            CreateMap<HospitalRequestDto, Hospital>();

            CreateMap<Hospital, HospitalDto>()
                .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => src.UploadedFile.FilePath))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Governorate, opt => opt.MapFrom(src => src.HospitalGovernorate.Governorate.Name))
                .ForMember(dest => dest.GovernorateId, opt => opt.MapFrom(src => src.HospitalGovernorate.Governorate.Id));

            CreateMap<Hospital, ListOfHospitalDto>()
                .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => src.UploadedFile.FilePath))
                .ForMember(dest => dest.Governorate, opt => opt.MapFrom(src => src.HospitalGovernorate.Governorate.Name));

            CreateMap<HospitalAdmin, HospitalAdminDto>()
                .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => src.UploadedFile.FilePath));

            CreateMap<HospitalAdminRequestDto, HospitalAdmin>();

            CreateMap<HospitalAdmin, User>()
                .ForMember(src => src.Id, opt => opt.Ignore());

            CreateMap<HospitalAdmin, EditHospitalAdminResponse>()
                .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => src.UploadedFile.FilePath))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.HospitalId, opt => opt.MapFrom(src => src.AdminOfHospital.Hospital.Id))
                .ForMember(dest => dest.HospitalImagePath, opt => opt.MapFrom(src => src.AdminOfHospital.Hospital.UploadedFile.FilePath))
                .ForMember(dest => dest.HospitalName, opt => opt.MapFrom(src => src.AdminOfHospital.Hospital.Name));

            CreateMap<HospitalAdmin, ListOfHospitalAdminDto>()
                .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => src.UploadedFile.FilePath));

            CreateMap<Specialization, SpecializationDto>()
                .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => src.UploadedFile.FilePath));

            CreateMap<Governorate, GovernorateDto>();

            CreateMap<DoctorRequestDto, Doctor>();

            CreateMap<Doctor, User>()
                .ForMember(src => src.Id, opt => opt.Ignore());

            CreateMap<Day, DayDto>();

            CreateMap<Doctor, AddDoctorResponseDto>()
                .ForMember(dest => dest.DoctorId, opt => opt.MapFrom(src => src.Id));

            CreateMap<Hospital, HospitalIdDto>();

            CreateMap<ClinicLab, ClinicIdDto>();

            CreateMap<Doctor, DoctorDto>()
                .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => src.UploadedFile.FilePath))
                .ForMember(src => src.Hospitals, opt => opt.Ignore())
                .ForMember(src => src.Specializations, opt => opt.Ignore());

            CreateMap<Doctor, EditDoctorResponseDto>()
                .ForMember(src => src.ImagePath, opt => opt.Ignore())
                .ForMember(src => src.specializations, opt => opt.Ignore());

            CreateMap<Specialization, AddClinicResponseDto>()
                .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => src.UploadedFile.FilePath));

            CreateMap<XraySpecialization, AddClinicResponseDto>()
                .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => src.UploadedFile.FilePath));

            CreateMap<LabSpecialization, AddClinicResponseDto>()
                .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => src.UploadedFile.FilePath));
            
            CreateMap<LabSpecialization, ListOfLabSpecializationDto>()
                .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => src.UploadedFile.FilePath));

            CreateMap<ClinicLab, AddClinicResponseDto>()
                .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => src.Specialization.UploadedFile.FilePath))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Specialization.Name));

            CreateMap<Xray, AddClinicResponseDto>()
                .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => src.XraySpecialization.UploadedFile.FilePath))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.XraySpecialization.Name));

            CreateMap<Lab, AddLabResponseDto>();

            CreateMap<LabSpecialization, SpecializationDto>();
            CreateMap<LabSpecialization, LabSpecializationDto>();

            CreateMap<ClinicAppointment, AppointmentResponseDto>()
                .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Doctor.FullName))
                .ForMember(dest => dest.DoctorRate, opt => opt.MapFrom(src => src.Doctor.Rate))
                .ForMember(dest => dest.DoctorDiscription, opt => opt.MapFrom(src => src.Doctor.Description));

            CreateMap<XrayAppointment, AppointmentResponseDto>()
                .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Doctor.FullName))
                .ForMember(dest => dest.DoctorRate, opt => opt.MapFrom(src => src.Doctor.Rate))
                .ForMember(dest => dest.DoctorDiscription, opt => opt.MapFrom(src => src.Doctor.Description));

            CreateMap<LabAppointment, AppointmentResponseDto>()
                .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Doctor.FullName))
                .ForMember(dest => dest.DoctorRate, opt => opt.MapFrom(src => src.Doctor.Rate))
                .ForMember(dest => dest.DoctorDiscription, opt => opt.MapFrom(src => src.Doctor.Description));

            CreateMap<ClinicAppointmentDate, AppointmentDateResponseDto>()
                .ForMember(dest => dest.DayName, opt => opt.MapFrom(src => src.Day.DayName));

            CreateMap<LabAppointmentDate, AppointmentDateResponseDto>()
                .ForMember(dest => dest.DayName, opt => opt.MapFrom(src => src.Day.DayName));

            CreateMap<XrayAppointmentDate, AppointmentDateResponseDto>()
                .ForMember(dest => dest.DayName, opt => opt.MapFrom(src => src.Day.DayName));

            CreateMap<ClinicAppointment, ListAppointmentDto>()
                .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Doctor.FullName))
                .ForMember(dest => dest.DoctorRate, opt => opt.MapFrom(src => src.Doctor.Rate))
                .ForMember(dest => dest.DoctorDiscription, opt => opt.MapFrom(src => src.Doctor.Description));

            CreateMap<XrayAppointment, ListAppointmentDto>()
                .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Doctor.FullName))
                .ForMember(dest => dest.DoctorRate, opt => opt.MapFrom(src => src.Doctor.Rate))
                .ForMember(dest => dest.DoctorDiscription, opt => opt.MapFrom(src => src.Doctor.Description));

            CreateMap<LabAppointment, ListAppointmentDto>()
                .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Doctor.FullName))
                .ForMember(dest => dest.DoctorRate, opt => opt.MapFrom(src => src.Doctor.Rate))
                .ForMember(dest => dest.DoctorDiscription, opt => opt.MapFrom(src => src.Doctor.Description));

            CreateMap<ClinicReservation, PatientReservationDto>()
                .ForMember(dest => dest.FromTime, opt => opt.MapFrom(src => src.ClinicAppointmentDate.FromTime))
                .ForMember(dest => dest.ToTime, opt => opt.MapFrom(src => src.ClinicAppointmentDate.ToTime))
                .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.ClinicAppointment.Doctor.FullName))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.ClinicAppointment.Price))
                .ForMember(dest => dest.HospitalName, opt => opt.MapFrom(src => src.ClinicAppointment.ClinicLab.Hospital.Name))
                .ForMember(dest => dest.ClinicName, opt => opt.MapFrom(src => src.ClinicAppointment.ClinicLab.Specialization.Name));

            CreateMap<LabReservation, PatientReservationDto>()
                .ForMember(dest => dest.FromTime, opt => opt.MapFrom(src => src.LabAppointmentDate.FromTime))
                .ForMember(dest => dest.ToTime, opt => opt.MapFrom(src => src.LabAppointmentDate.ToTime))
                .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.LabAppointment.Doctor.FullName))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.LabAppointment.Price))
                .ForMember(dest => dest.HospitalName, opt => opt.MapFrom(src => src.LabAppointment.Lab.Hospital.Name))
                .ForMember(dest => dest.ClinicName, opt => opt.MapFrom(src => src.LabAppointment.Lab.Name));

            CreateMap<XrayReservation, PatientReservationDto>()
                .ForMember(dest => dest.FromTime, opt => opt.MapFrom(src => src.XrayAppointmentDate.FromTime))
                .ForMember(dest => dest.ToTime, opt => opt.MapFrom(src => src.XrayAppointmentDate.ToTime))
                .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.XrayAppointment.Doctor.FullName))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.XrayAppointment.Price))
                .ForMember(dest => dest.HospitalName, opt => opt.MapFrom(src => src.XrayAppointment.Xray.Hospital.Name))
                .ForMember(dest => dest.ClinicName, opt => opt.MapFrom(src => src.XrayAppointment.Xray.XraySpecialization.Name));

            CreateMap<LabReservation, ReservationResponseDto>()
                .ForMember(dest => dest.FromTime, opt => opt.MapFrom(src => src.LabAppointmentDate.FromTime))
                .ForMember(dest => dest.ToTime, opt => opt.MapFrom(src => src.LabAppointmentDate.ToTime))
                .ForMember(dest => dest.PatientPhoneNum, opt => opt.MapFrom(src => src.Patient.PhoneNumber))
                .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient.FullName));

            CreateMap<ClinicReservation, ReservationResponseDto>()
                .ForMember(dest => dest.FromTime, opt => opt.MapFrom(src => src.ClinicAppointmentDate.FromTime))
                .ForMember(dest => dest.ToTime, opt => opt.MapFrom(src => src.ClinicAppointmentDate.ToTime))
                .ForMember(dest => dest.PatientPhoneNum, opt => opt.MapFrom(src => src.Patient.PhoneNumber))
                .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient.FullName));

            CreateMap<XrayReservation, ReservationResponseDto>()
               .ForMember(dest => dest.FromTime, opt => opt.MapFrom(src => src.XrayAppointmentDate.FromTime))
               .ForMember(dest => dest.ToTime, opt => opt.MapFrom(src => src.XrayAppointmentDate.ToTime))
               .ForMember(dest => dest.PatientPhoneNum, opt => opt.MapFrom(src => src.Patient.PhoneNumber))
               .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient.FullName));

            CreateMap<ClinicReservation, AllReservations>()
                .ForMember(dest => dest.RoomId, opt => opt.MapFrom(src => src.ClinicAppointment.ClinicLabId))
                .ForMember(dest => dest.Doctor, opt => opt.MapFrom(src => src.ClinicAppointment.Doctor))
                .ForMember(dest => dest.DoctorId, opt => opt.MapFrom(src => src.ClinicAppointment.DoctorId))
                .ForMember(dest => dest.RoomReservationId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ToTime, opt => opt.MapFrom(src => src.ClinicAppointmentDate.ToTime))
                .ForMember(dest => dest.FromTime, opt => opt.MapFrom(src => src.ClinicAppointmentDate.FromTime))
                .ForMember(dest => dest.RoomAppointmentDateId, opt => opt.MapFrom(src => src.ClinicAppointmentDateId))
                .ForMember(dest => dest.price, opt => opt.MapFrom(src => src.ClinicAppointment.Price))
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<LabReservation, AllReservations>()
                .ForMember(dest => dest.RoomId, opt => opt.MapFrom(src => src.LabAppointment.LabId))
                .ForMember(dest => dest.Doctor, opt => opt.MapFrom(src => src.LabAppointment.Doctor))
                .ForMember(dest => dest.DoctorId, opt => opt.MapFrom(src => src.LabAppointment.DoctorId))
                .ForMember(dest => dest.RoomReservationId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ToTime, opt => opt.MapFrom(src => src.LabAppointmentDate.ToTime))
                .ForMember(dest => dest.FromTime, opt => opt.MapFrom(src => src.LabAppointmentDate.FromTime))
                .ForMember(dest => dest.RoomAppointmentDateId, opt => opt.MapFrom(src => src.LabAppointmentDateId))
                .ForMember(dest => dest.price, opt => opt.MapFrom(src => src.LabAppointment.Price))
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<XrayReservation, AllReservations>()
                .ForMember(dest => dest.RoomId, opt => opt.MapFrom(src => src.XrayAppointment.XrayId))
                .ForMember(dest => dest.Doctor, opt => opt.MapFrom(src => src.XrayAppointment.Doctor))
                .ForMember(dest => dest.DoctorId, opt => opt.MapFrom(src => src.XrayAppointment.DoctorId))
                .ForMember(dest => dest.RoomReservationId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ToTime, opt => opt.MapFrom(src => src.XrayAppointmentDate.ToTime))
                .ForMember(dest => dest.FromTime, opt => opt.MapFrom(src => src.XrayAppointmentDate.FromTime))
                .ForMember(dest => dest.RoomAppointmentDateId, opt => opt.MapFrom(src => src.XrayAppointmentDateId))
                .ForMember(dest => dest.price, opt => opt.MapFrom(src => src.XrayAppointment.Price))
                .ForMember(dest => dest.Id, opt => opt.Ignore()); 

            CreateMap<AllReservations, ReservationResponseDto>()
                .ForMember(dest => dest.MedicalHistoryId, opt => opt.MapFrom(src => src.Patient.MedicalHistoryId))
               .ForMember(dest => dest.PatientPhoneNum, opt => opt.MapFrom(src => src.Patient.PhoneNumber))
               .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient.FullName));

            CreateMap<AllReservations, PatientReservationDto>()
               .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Doctor.FullName));

            CreateMap<Band, AddPrivateBandDto>()
               .ForMember(dest => dest.HospitalName, opt => opt.MapFrom(src => src.Hospital.Name));

            CreateMap<Band, AddPublicBandDto>()
               .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient.FullName))
               .ForMember(dest => dest.PatientNationalId, opt => opt.MapFrom(src => src.Patient.NationalId))
               .ForMember(dest => dest.PatientImagePath, opt => opt.MapFrom(src => src.Patient.UploadedFile.FilePath));

            CreateMap<Band, BandResponseDto>()
               .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient.FullName))
               .ForMember(dest => dest.PatientNationalId, opt => opt.MapFrom(src => src.Patient.NationalId))
               .ForMember(dest => dest.PatientImagePath, opt => opt.MapFrom(src => src.Patient.UploadedFile.FilePath));

            CreateMap<CurrentState, CurrentStateDto>();
            
            CreateMap<AddMedicalHistoryDto, MedicalHistory>();

            CreateMap<MedicalHistory, MedicalHistoryResponseDto>()
                .ForMember(dest => dest.MedicalHistoryId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Patient.FullName))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Patient.PhoneNumber))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Patient.Email))
                .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => src.Patient.UploadedFile.FilePath));

            CreateMap<Patient, PatientResponseDto>()
                .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => src.UploadedFile.FilePath));

            CreateMap<MedicalHistoryFile, FileResponseDto>()
                .ForMember(dest => dest.FilePath, opt => opt.MapFrom(src => src.FilePath))
                .ForMember(dest => dest.FileId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FileType, opt => opt.MapFrom(src => src.Type))
                .ForMember(dest => dest.FileDescription, opt => opt.MapFrom(src => src.Description));

            CreateMap<ClinicLab, ListOfSpecificClinics>()
                .ForMember(dest => dest.ClinicId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ClinicSpecialization, opt => opt.MapFrom(src => src.Specialization.Name))
                .ForMember(dest => dest.ClinicImagePath, opt => opt.MapFrom(src => src.Specialization.UploadedFile.FilePath))
                .ForMember(dest => dest.HospitalId, opt => opt.MapFrom(src => src.HospitalId))
                .ForMember(dest => dest.HospitalImagePath, opt => opt.MapFrom(src => src.Hospital.UploadedFile.FilePath))
                .ForMember(dest => dest.HospitalAddress, opt => opt.MapFrom(src => src.Hospital.Address))
                .ForMember(dest => dest.HospitalDescription, opt => opt.MapFrom(src => src.Hospital.Description))
                .ForMember(dest => dest.HospitalGovernorate, opt => opt.MapFrom(src => src.Hospital.HospitalGovernorate.Governorate.Name))
                .ForMember(dest => dest.HospitalName, opt => opt.MapFrom(src => src.Hospital.Name));


            CreateMap<Xray, ListOfSpecificClinics>()
                .ForMember(dest => dest.ClinicId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ClinicSpecialization, opt => opt.MapFrom(src => src.XraySpecialization.Name))
                .ForMember(dest => dest.ClinicImagePath, opt => opt.MapFrom(src => src.XraySpecialization.UploadedFile.FilePath))
                .ForMember(dest => dest.HospitalId, opt => opt.MapFrom(src => src.HospitalId))
                .ForMember(dest => dest.HospitalImagePath, opt => opt.MapFrom(src => src.Hospital.UploadedFile.FilePath))
                .ForMember(dest => dest.HospitalAddress, opt => opt.MapFrom(src => src.Hospital.Address))
                .ForMember(dest => dest.HospitalDescription, opt => opt.MapFrom(src => src.Hospital.Description))
                .ForMember(dest => dest.HospitalGovernorate, opt => opt.MapFrom(src => src.Hospital.HospitalGovernorate.Governorate.Name))
                .ForMember(dest => dest.HospitalName, opt => opt.MapFrom(src => src.Hospital.Name));


            CreateMap<Lab, ListOfSpecificLabs>()
                .ForMember(dest => dest.LabId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.LabName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.HospitalId, opt => opt.MapFrom(src => src.HospitalId))
                .ForMember(dest => dest.HospitalImagePath, opt => opt.MapFrom(src => src.Hospital.UploadedFile.FilePath))
                .ForMember(dest => dest.HospitalAddress, opt => opt.MapFrom(src => src.Hospital.Address))
                .ForMember(dest => dest.HospitalDescription, opt => opt.MapFrom(src => src.Hospital.Description))
                .ForMember(dest => dest.HospitalGovernorate, opt => opt.MapFrom(src => src.Hospital.HospitalGovernorate.Governorate.Name))
                .ForMember(dest => dest.HospitalName, opt => opt.MapFrom(src => src.Hospital.Name));

        }
    }
}
