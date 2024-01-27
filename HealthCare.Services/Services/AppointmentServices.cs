using AutoMapper;
using HealthCare.Core.DTOS;
using HealthCare.Core.DTOS.AppointmentModule.RequestDto;
using HealthCare.Core.DTOS.AppointmentModule.ResponseDto;
using HealthCare.Core.DTOS.AuthModule.ResponseDtos;
using HealthCare.Core.DTOS.ClinicModule.ResponseDto;
using HealthCare.Core.IRepositories;
using HealthCare.Core.Models.AppointmentModule;
using HealthCare.Core.Models.AuthModule;
using HealthCare.Core.Models.ClinicModule;
using HealthCare.Core.Models.HospitalModule;
using HealthCare.Services.IServices;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Services.Services
{
    public class AppointmentServices : IAppointmentServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AppointmentServices(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<GeneralResponse<AppointmentResponseDto>> AddClinicAppointment(int clinicId, AddAppointmentRequestDto dto)
        {
            try
            {
                var clinic = await _unitOfWork.ClinicLabRepository.GetByIdAsync(clinicId);
                if (clinic == null)
                {
                    return new GeneralResponse<AppointmentResponseDto>
                    {
                        IsSuccess = false,
                        Message = "No Clinic Found!"
                    };
                }
                var doctor = await _unitOfWork.DoctorRepository.SingleOrDefaultAsync(s => s.Id == dto.DoctorId);
                if (doctor == null)
                {
                    return new GeneralResponse<AppointmentResponseDto>
                    {
                        IsSuccess = false,
                        Message = "No doctor Found!"
                    };
                }
                var day = await _unitOfWork.DayRepository.SingleOrDefaultAsync(s => s.Id == dto.DayId);
                if (day == null)
                {
                    return new GeneralResponse<AppointmentResponseDto>
                    {
                        IsSuccess = false,
                        Message = "No day Found!"
                    };
                }
                if (!dto.Price.All(char.IsDigit))
                {
                    return new GeneralResponse<AppointmentResponseDto>
                    {
                        IsSuccess = false,
                        Message = "Wrong Price"
                    };
                }
                var clinicAppointment = new ClinicAppointment()
                {
                    Price = dto.Price,
                    Doctor = doctor,
                    ClinicLab = clinic,
                };
                var clinicAppointmentDate = new ClinicAppointmentDate()
                {
                    Day = day,
                    FromTime = dto.FromTime,
                    ToTime = dto.ToTime,
                    ClinicAppointment = clinicAppointment
                };
                await _unitOfWork.ClinicAppointmentRepository.AddAsync(clinicAppointment);
                await _unitOfWork.ClinicAppointmentDateRepository.AddAsync(clinicAppointmentDate);
                await _unitOfWork.CompleteAsync();

                var data = _mapper.Map<AppointmentResponseDto>(clinicAppointment);
                var user = await _unitOfWork.UserRepository.GetSingleWithIncludesAsync(single: s => s.UserName == doctor.UserName, i => i.UploadedFile);
                data.DoctorImagePath = user.UploadedFile.FilePath;
                data.AppointmentDates = _mapper.Map<List<AppointmentDateResponseDto>>(clinicAppointmentDate);

                return new GeneralResponse<AppointmentResponseDto>
                {
                    IsSuccess = true,
                    Message = "New ClinicAppointment is added sucessfully.",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<AppointmentResponseDto>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }

        public Task<GeneralResponse<AppointmentResponseDto>> AddClinicAppointmentDatesToAnAppointment(int clinicAppointment, AddAppointmentDateRequestDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<AppointmentResponseDto>> AddLabAppointment(int labId, AddAppointmentRequestDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<AppointmentResponseDto>> AddLabAppointmentDatesToAnAppointment(int labAppointment, AddAppointmentDateRequestDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<AppointmentResponseDto>> AddXrayAppointment(int xrayId, AddAppointmentRequestDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<AppointmentResponseDto>> AddXrayAppointmentDatesToAnAppointment(int xrayAppointment, AddAppointmentDateRequestDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<PatientReservationDto>> BookAnAppointmentOfClinic(int patientId, int clinicAppointmentDateId)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<PatientReservationDto>> BookAnAppointmentOfLab(int patientId, int labAppointmentDateId)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<PatientReservationDto>> BookAnAppointmentOfXray(int patientId, int xrayAppointmentDateId)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<string>> CancelClinicReservation(int patientId, int clinicReservationId)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<string>> CancelLabReservation(int patientId, int labReservationId)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<string>> CancelXrayReservation(int patientId, int xrayReservationId)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<string>> DeleteClinicAppointment(int clinicAppointmentId)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<string>> DeleteClinicAppointmentDateOfAnAppointment(int clinicAppointmentDateId)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<string>> DeleteLabAppointment(int labAppointmentId)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<string>> DeleteLabAppointmentDateOfAnAppointment(int labAppointmentDateId)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<string>> DeleteXrayAppointment(int xrayAppointmentId)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<string>> DeleteXrayAppointmentDateOfAnAppointment(int xrayAppointmentDateId)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<string>> DoneReservation(int reservationId)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<List<AppointmentResponseDto>>> ListOfAppointmentOfClinic(int clinicId)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<List<AppointmentResponseDto>>> ListOfAppointmentOfLab(int labId)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<List<AppointmentResponseDto>>> ListOfAppointmentOfXray(int xrayId)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<List<ReservationResponseDto>>> ListOfReservationsOfDoctor(int hospitalId, int doctorId)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<List<PatientReservationDto>>> ListOfReservationsOfPatient(int patientId)
        {
            throw new NotImplementedException();
        }
    }
}
