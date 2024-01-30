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
using HealthCare.Core.Models.PatientModule;
using HealthCare.Services.IServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Bcpg.Attr.ImageAttrib;

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

        public async Task<GeneralResponse<AppointmentResponseDto>> AddClinicAppointment(int clinicId, [FromForm]AddAppointmentRequestDto dto)
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
                if (await _unitOfWork.ClinicAppointmentRepository.AnyAsync(a => a.ClinicLabId == clinic.Id && a.DoctorId == doctor.Id))
                {
                    return new GeneralResponse<AppointmentResponseDto>
                    {
                        IsSuccess = false,
                        Message = "This doctor is already had an appointment in this clinic!"
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
                string[] format = { "h:mm tt" };
                if (!DateTime.TryParseExact(dto.FromTime, format, null, System.Globalization.DateTimeStyles.None, out DateTime parsedTime) ||
                    !DateTime.TryParseExact(dto.FromTime, format, null, System.Globalization.DateTimeStyles.None, out DateTime ParsedTime))
                {
                    return new GeneralResponse<AppointmentResponseDto>
                    {
                        IsSuccess = false,
                        Message = "invalid Time Format."
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
                data.AppointmentDates = _mapper.Map<AppointmentDateResponseDto>(clinicAppointmentDate);

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

        public async Task<GeneralResponse<AppointmentResponseDto>> AddLabAppointment(int labId, AddAppointmentRequestDto dto)
        {
            try
            {
                var lab = await _unitOfWork.LabRepository.GetByIdAsync(labId);
                if (lab == null)
                {
                    return new GeneralResponse<AppointmentResponseDto>
                    {
                        IsSuccess = false,
                        Message = "No Lab Found!"
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
                if (await _unitOfWork.LabAppointmentRepository.AnyAsync(a => a.LabId == lab.Id && a.DoctorId == doctor.Id))
                {
                    return new GeneralResponse<AppointmentResponseDto>
                    {
                        IsSuccess = false,
                        Message = "This doctor is already had an appointment in this Lab!"
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
                string[] format = { "h:mm tt" };
                if (!DateTime.TryParseExact(dto.FromTime, format, null, System.Globalization.DateTimeStyles.None, out DateTime parsedTime) ||
                    !DateTime.TryParseExact(dto.FromTime, format, null, System.Globalization.DateTimeStyles.None, out DateTime ParsedTime))
                {
                    return new GeneralResponse<AppointmentResponseDto>
                    {
                        IsSuccess = false,
                        Message = "invalid Time Format."
                    };
                }
                var labAppointment = new LabAppointment()
                {
                    Price = dto.Price,
                    Doctor = doctor,
                    Lab = lab,
                };
                var labAppointmentDate = new LabAppointmentDate()
                {
                    Day = day,
                    FromTime = dto.FromTime,
                    ToTime = dto.ToTime,
                    LabAppointment = labAppointment
                };
                await _unitOfWork.LabAppointmentRepository.AddAsync(labAppointment);
                await _unitOfWork.LabAppointmentDateRepository.AddAsync(labAppointmentDate);
                await _unitOfWork.CompleteAsync();

                var data = _mapper.Map<AppointmentResponseDto>(labAppointment);
                var user = await _unitOfWork.UserRepository.GetSingleWithIncludesAsync(single: s => s.UserName == doctor.UserName, i => i.UploadedFile);
                data.DoctorImagePath = user.UploadedFile.FilePath;
                data.AppointmentDates = _mapper.Map<AppointmentDateResponseDto>(labAppointmentDate);

                return new GeneralResponse<AppointmentResponseDto>
                {
                    IsSuccess = true,
                    Message = "New LabAppointment is added sucessfully.",
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

        public async Task<GeneralResponse<AppointmentResponseDto>> AddXrayAppointment(int xrayId, AddAppointmentRequestDto dto)
        {
            try
            {
                var xray = await _unitOfWork.XrayRepository.GetByIdAsync(xrayId);
                if (xray == null)
                {
                    return new GeneralResponse<AppointmentResponseDto>
                    {
                        IsSuccess = false,
                        Message = "No Xray Found!"
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
                if (await _unitOfWork.XrayAppointmentRepository.AnyAsync(a => a.XrayId == xray.Id && a.DoctorId == doctor.Id))
                {
                    return new GeneralResponse<AppointmentResponseDto>
                    {
                        IsSuccess = false,
                        Message = "This doctor is already had an appointment in this Xray!"
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
                string[] format = { "h:mm tt" };
                if (!DateTime.TryParseExact(dto.FromTime, format, null, System.Globalization.DateTimeStyles.None, out DateTime parsedTime) ||
                    !DateTime.TryParseExact(dto.FromTime, format, null, System.Globalization.DateTimeStyles.None, out DateTime ParsedTime))
                {
                    return new GeneralResponse<AppointmentResponseDto>
                    {
                        IsSuccess = false,
                        Message = "invalid Time Format."
                    };
                }
                var xrayAppointment = new XrayAppointment()
                {
                    Price = dto.Price,
                    Doctor = doctor,
                    Xray = xray,
                };
                var xrayAppointmentDate = new XrayAppointmentDate()
                {
                    Day = day,
                    FromTime = dto.FromTime,
                    ToTime = dto.ToTime,
                    XrayAppointment = xrayAppointment
                };
                await _unitOfWork.XrayAppointmentRepository.AddAsync(xrayAppointment);
                await _unitOfWork.XrayAppointmentDateRepository.AddAsync(xrayAppointmentDate);
                await _unitOfWork.CompleteAsync();

                var data = _mapper.Map<AppointmentResponseDto>(xrayAppointment);
                var user = await _unitOfWork.UserRepository.GetSingleWithIncludesAsync(single: s => s.UserName == doctor.UserName, i => i.UploadedFile);
                data.DoctorImagePath = user.UploadedFile.FilePath;
                data.AppointmentDates = _mapper.Map<AppointmentDateResponseDto>(xrayAppointmentDate);

                return new GeneralResponse<AppointmentResponseDto>
                {
                    IsSuccess = true,
                    Message = "New LabAppointment is added sucessfully.",
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

        public async Task<GeneralResponse<ListAppointmentDto>> AddClinicAppointmentDatesToAnAppointment(int clinicAppointmentId, AddAppointmentDateRequestDto dto)
        {
            try
            {
                var clinicAppointment = await _unitOfWork.ClinicAppointmentRepository.GetSingleWithIncludesAsync(s => s.Id ==clinicAppointmentId, i=>i.Doctor,i=>i.ClinicAppointmentDates);
                if (clinicAppointment == null)
                {
                    return new GeneralResponse<ListAppointmentDto>
                    {
                        IsSuccess = false,
                        Message = "No ClinicAppointment Found!"
                    };
                }
                var day = await _unitOfWork.DayRepository.SingleOrDefaultAsync(s => s.Id == dto.DayId);
                if (day == null)
                {
                    return new GeneralResponse<ListAppointmentDto>
                    {
                        IsSuccess = false,
                        Message = "No day Found!"
                    };
                }
                var otherDay = await _unitOfWork.ClinicAppointmentDateRepository
                    .SingleOrDefaultAsync(s => s.ClinicAppointment == clinicAppointment && s.DayId == dto.DayId);
                if (otherDay != null)
                {
                    return new GeneralResponse<ListAppointmentDto>
                    {
                        IsSuccess = false,
                        Message = "This Doctor already have another appointment in this clinic in the same day, please choose another day!"
                    };
                }
                string[] format = { "h:mm tt" };
                if (!DateTime.TryParseExact(dto.FromTime, format, null, System.Globalization.DateTimeStyles.None, out DateTime parsedTime) ||
                    !DateTime.TryParseExact(dto.FromTime, format, null, System.Globalization.DateTimeStyles.None, out DateTime ParsedTime))
                {
                    return new GeneralResponse<ListAppointmentDto>
                    {
                        IsSuccess = false,
                        Message = "invalid Time Format."
                    };
                }
                
                var clinicAppointmentDate = new ClinicAppointmentDate()
                {
                    Day = day,
                    FromTime = dto.FromTime,
                    ToTime = dto.ToTime,
                    ClinicAppointment = clinicAppointment
                };
               
                await _unitOfWork.ClinicAppointmentDateRepository.AddAsync(clinicAppointmentDate);
                clinicAppointment.ClinicAppointmentDates.Add(clinicAppointmentDate);
                _unitOfWork.ClinicAppointmentRepository.Update(clinicAppointment);
                await _unitOfWork.CompleteAsync();

                var data = _mapper.Map<ListAppointmentDto>(clinicAppointment);
                var doctor = await _unitOfWork.DoctorRepository.GetByIdAsync(clinicAppointment.DoctorId);
                var user = await _unitOfWork.UserRepository.GetSingleWithIncludesAsync(single: s => s.UserName == doctor.UserName, i => i.UploadedFile);
                data.DoctorImagePath = user.UploadedFile.FilePath;
                var dates = await _unitOfWork.ClinicAppointmentDateRepository.WhereIncludeAsync(w => w.ClinicAppointmentId == clinicAppointment.Id, i => i.Day);
                var s = dates.OrderBy(S => S.DayId);
                data.AppointmentDates = _mapper.Map<List<AppointmentDateResponseDto>>(s);

                return new GeneralResponse<ListAppointmentDto>
                {
                    IsSuccess = true,
                    Message = "New Date of a ClinicAppointment is added sucessfully.",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<ListAppointmentDto>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }


        public async Task<GeneralResponse<ListAppointmentDto>> AddLabAppointmentDatesToAnAppointment(int labAppointmentId, AddAppointmentDateRequestDto dto)
        {
            try
            {
                var labAppointment = await _unitOfWork.LabAppointmentRepository.GetSingleWithIncludesAsync(s => s.Id == labAppointmentId, i => i.Doctor, i => i.LabAppointmentDates);
                if (labAppointment == null)
                {
                    return new GeneralResponse<ListAppointmentDto>
                    {
                        IsSuccess = false,
                        Message = "No LabAppointment Found!"
                    };
                }
                var day = await _unitOfWork.DayRepository.SingleOrDefaultAsync(s => s.Id == dto.DayId);
                if (day == null)
                {
                    return new GeneralResponse<ListAppointmentDto>
                    {
                        IsSuccess = false,
                        Message = "No day Found!"
                    };
                }
                var otherDay = await _unitOfWork.LabAppointmentDateRepository
                    .SingleOrDefaultAsync(s => s.LabAppointment == labAppointment && s.DayId == dto.DayId);
                if (otherDay != null)
                {
                    return new GeneralResponse<ListAppointmentDto>
                    {
                        IsSuccess = false,
                        Message = "This Doctor already have another appointment in this Lab in the same day, please choose another day!"
                    };
                }
                string[] format = { "h:mm tt" };
                if (!DateTime.TryParseExact(dto.FromTime, format, null, System.Globalization.DateTimeStyles.None, out DateTime parsedTime) ||
                    !DateTime.TryParseExact(dto.FromTime, format, null, System.Globalization.DateTimeStyles.None, out DateTime ParsedTime))
                {
                    return new GeneralResponse<ListAppointmentDto>
                    {
                        IsSuccess = false,
                        Message = "invalid Time Format."
                    };
                }

                var labAppointmentDate = new LabAppointmentDate()
                {
                    Day = day,
                    FromTime = dto.FromTime,
                    ToTime = dto.ToTime,
                    LabAppointment = labAppointment
                };

                await _unitOfWork.LabAppointmentDateRepository.AddAsync(labAppointmentDate);
                labAppointment.LabAppointmentDates.Add(labAppointmentDate);
                _unitOfWork.LabAppointmentRepository.Update(labAppointment);
                await _unitOfWork.CompleteAsync();

                var data = _mapper.Map<ListAppointmentDto>(labAppointment);
                var doctor = await _unitOfWork.DoctorRepository.GetByIdAsync(labAppointment.DoctorId);
                var user = await _unitOfWork.UserRepository.GetSingleWithIncludesAsync(single: s => s.UserName == doctor.UserName, i => i.UploadedFile);
                data.DoctorImagePath = user.UploadedFile.FilePath;
                var dates = await _unitOfWork.LabAppointmentDateRepository.WhereIncludeAsync(w => w.LabAppointmentId == labAppointment.Id, i => i.Day);
                var s = dates.OrderBy(S => S.DayId);
                data.AppointmentDates = _mapper.Map<List<AppointmentDateResponseDto>>(s);

                return new GeneralResponse<ListAppointmentDto>
                {
                    IsSuccess = true,
                    Message = "New Date of a LabAppointment is added sucessfully.",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<ListAppointmentDto>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }

        public async Task<GeneralResponse<ListAppointmentDto>> AddXrayAppointmentDatesToAnAppointment(int xrayAppointmentId, AddAppointmentDateRequestDto dto)
        {
            try
            {
                var xrayAppointment = await _unitOfWork.XrayAppointmentRepository.GetSingleWithIncludesAsync(s => s.Id == xrayAppointmentId, i => i.Doctor, i => i.XrayAppointmentDates);
                if (xrayAppointment == null)
                {
                    return new GeneralResponse<ListAppointmentDto>
                    {
                        IsSuccess = false,
                        Message = "No XrayAppointment Found!"
                    };
                }
                var day = await _unitOfWork.DayRepository.SingleOrDefaultAsync(s => s.Id == dto.DayId);
                if (day == null)
                {
                    return new GeneralResponse<ListAppointmentDto>
                    {
                        IsSuccess = false,
                        Message = "No day Found!"
                    };
                }
                var otherDay = await _unitOfWork.XrayAppointmentDateRepository
                    .SingleOrDefaultAsync(s => s.XrayAppointment == xrayAppointment && s.DayId == dto.DayId);
                if (otherDay != null)
                {
                    return new GeneralResponse<ListAppointmentDto>
                    {
                        IsSuccess = false,
                        Message = "This Doctor already have another appointment in this xray in the same day, please choose another day!"
                    };
                }
                string[] format = { "h:mm tt" };
                if (!DateTime.TryParseExact(dto.FromTime, format, null, System.Globalization.DateTimeStyles.None, out DateTime parsedTime) ||
                    !DateTime.TryParseExact(dto.FromTime, format, null, System.Globalization.DateTimeStyles.None, out DateTime ParsedTime))
                {
                    return new GeneralResponse<ListAppointmentDto>
                    {
                        IsSuccess = false,
                        Message = "invalid Time Format."
                    };
                }

                var xrayAppointmentDate = new XrayAppointmentDate()
                {
                    Day = day,
                    FromTime = dto.FromTime,
                    ToTime = dto.ToTime,
                    XrayAppointment = xrayAppointment
                };

                await _unitOfWork.XrayAppointmentDateRepository.AddAsync(xrayAppointmentDate);
                xrayAppointment.XrayAppointmentDates.Add(xrayAppointmentDate);
                _unitOfWork.XrayAppointmentRepository.Update(xrayAppointment);
                await _unitOfWork.CompleteAsync();

                var data = _mapper.Map<ListAppointmentDto>(xrayAppointment);
                var doctor = await _unitOfWork.DoctorRepository.GetByIdAsync(xrayAppointment.DoctorId);
                var user = await _unitOfWork.UserRepository.GetSingleWithIncludesAsync(single: s => s.UserName == doctor.UserName, i => i.UploadedFile);
                data.DoctorImagePath = user.UploadedFile.FilePath;
                var dates = await _unitOfWork.XrayAppointmentDateRepository.WhereIncludeAsync(w => w.XrayAppointmentId == xrayAppointment.Id, i => i.Day);
                var s = dates.OrderBy(S => S.DayId);
                data.AppointmentDates = _mapper.Map<List<AppointmentDateResponseDto>>(s);

                return new GeneralResponse<ListAppointmentDto>
                {
                    IsSuccess = true,
                    Message = "New Date of a XrayAppointment is added sucessfully.",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<ListAppointmentDto>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }

        
        public async Task<GeneralResponse<PatientReservationDto>> BookAnAppointmentOfClinic(int patientId, int clinicAppointmentDateId, string date)
        {
            try 
            {
                var patient = await _unitOfWork.PatientRepository.GetByIdAsync(patientId);
                if (patient == null)
                {
                    return new GeneralResponse<PatientReservationDto>
                    {
                        IsSuccess = false,
                        Message = "No Patient Found!"
                    };
                }
                var clinicAppointmentDate = await _unitOfWork.ClinicAppointmentDateRepository.GetSingleWithIncludesAsync(s=> s.Id == clinicAppointmentDateId,
                    i=>i.ClinicAppointment, i=>i.Day, i=>i.ClinicAppointment.Doctor,
                    i=>i.ClinicAppointment.ClinicLab.Specialization, i=>i.ClinicAppointment.ClinicLab.Hospital);
                if (clinicAppointmentDate == null)
                {
                    return new GeneralResponse<PatientReservationDto>
                    {
                        IsSuccess = false,
                        Message = "No clinicAppointmentDate Found!"
                    };
                }
                if(await _unitOfWork.ClinicReservationRepository.AnyAsync(a=>a.PatientId==patientId && a.ClinicAppointmentDateId == clinicAppointmentDateId))
                {
                    return new GeneralResponse<PatientReservationDto>
                    {
                        IsSuccess = false,
                        Message = "You have made a prior reservation at this clinic with this doctor." +
                        " If you want to change the appointment time, please cancel the advance reservation"
                    };
                }
                if(!DateTime.TryParse(date, out DateTime parsedDate))
                {
                    return new GeneralResponse<PatientReservationDto>
                    {
                        IsSuccess = false,
                        Message = "Invalid Date Formate!"
                    };
                }
                var selectedDate = DateTime.Parse(date);
                if (selectedDate.DayOfWeek.ToString()!=clinicAppointmentDate.Day.DayName)
                {
                    return new GeneralResponse<PatientReservationDto>
                    {
                        IsSuccess = false,
                        Message = $"The selected date doesnot match the day of the choosen appointment: {clinicAppointmentDate.Day.DayName}"
                    };
                }
                if(DateTime.Now.AddMonths(1) < selectedDate)
                {
                    return new GeneralResponse<PatientReservationDto>
                    {
                        IsSuccess = false,
                        Message = "the selected date isn't accepted because it's after more than Month"
                    };
                }
                var reservation = new ClinicReservation()
                {
                    Patient = patient,
                    Date = date,
                    ClinicAppointmentDate = clinicAppointmentDate,
                    ClinicAppointment = clinicAppointmentDate.ClinicAppointment
                };
                await _unitOfWork.ClinicReservationRepository.AddAsync(reservation);
                await _unitOfWork.CompleteAsync();
                var data = _mapper.Map<PatientReservationDto>(reservation);
                var user = await _unitOfWork.UserRepository.GetSingleWithIncludesAsync(
                    s => s.UserName == clinicAppointmentDate.ClinicAppointment.Doctor.UserName, a => a.UploadedFile);
                data.DoctorImagePath = user.UploadedFile.FilePath;
                return new GeneralResponse<PatientReservationDto>
                {
                    IsSuccess = true,
                    Message = "The Reservation is Completed sucessfully.",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<PatientReservationDto>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }

        public async Task<GeneralResponse<PatientReservationDto>> BookAnAppointmentOfLab(int patientId, int labAppointmentDateId, string date)
        {
            try
            {
                var patient = await _unitOfWork.PatientRepository.GetByIdAsync(patientId);
                if (patient == null)
                {
                    return new GeneralResponse<PatientReservationDto>
                    {
                        IsSuccess = false,
                        Message = "No Patient Found!"
                    };
                }
                var labAppointmentDate = await _unitOfWork.LabAppointmentDateRepository.GetSingleWithIncludesAsync(s => s.Id == labAppointmentDateId,
                    i => i.LabAppointment, i => i.Day, i => i.LabAppointment.Doctor,
                    i => i.LabAppointment.Lab, i => i.LabAppointment.Lab.Hospital);
                if (labAppointmentDate == null)
                {
                    return new GeneralResponse<PatientReservationDto>
                    {
                        IsSuccess = false,
                        Message = "No labAppointmentDate Found!"
                    };
                }
                if (await _unitOfWork.LabReservationRepository.AnyAsync(a => a.PatientId == patientId && a.LabAppointmentDateId == labAppointmentDateId))
                {
                    return new GeneralResponse<PatientReservationDto>
                    {
                        IsSuccess = false,
                        Message = "You have made a prior reservation at this lab with this doctor." +
                        " If you want to change the appointment time, please cancel the advance reservation"
                    };
                }
                if (!DateTime.TryParse(date, out DateTime parsedDate))
                {
                    return new GeneralResponse<PatientReservationDto>
                    {
                        IsSuccess = false,
                        Message = "Invalid Date Formate!"
                    };
                }
                var selectedDate = DateTime.Parse(date);
                if (selectedDate.DayOfWeek.ToString() != labAppointmentDate.Day.DayName)
                {
                    return new GeneralResponse<PatientReservationDto>
                    {
                        IsSuccess = false,
                        Message = $"The selected date doesnot match the day of the choosen appointment: {labAppointmentDate.Day.DayName}"
                    };
                }
                if (DateTime.Now.AddMonths(1) < selectedDate)
                {
                    return new GeneralResponse<PatientReservationDto>
                    {
                        IsSuccess = false,
                        Message = "the selected date isn't accepted because it's after more than Month"
                    };
                }
                var reservation = new LabReservation()
                {
                    Patient = patient,
                    Date = date,
                    LabAppointmentDate = labAppointmentDate,
                    LabAppointment = labAppointmentDate.LabAppointment
                };
                await _unitOfWork.LabReservationRepository.AddAsync(reservation);
                await _unitOfWork.CompleteAsync();
                var data = _mapper.Map<PatientReservationDto>(reservation);
                var user = await _unitOfWork.UserRepository.GetSingleWithIncludesAsync(
                    s => s.UserName == labAppointmentDate.LabAppointment.Doctor.UserName, a => a.UploadedFile);
                data.DoctorImagePath = user.UploadedFile.FilePath;
                return new GeneralResponse<PatientReservationDto>
                {
                    IsSuccess = true,
                    Message = "The Reservation is Completed sucessfully.",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<PatientReservationDto>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }

        public async Task<GeneralResponse<PatientReservationDto>> BookAnAppointmentOfXray(int patientId, int xrayAppointmentDateId, string date)
        {
            try
            {
                var patient = await _unitOfWork.PatientRepository.GetByIdAsync(patientId);
                if (patient == null)
                {
                    return new GeneralResponse<PatientReservationDto>
                    {
                        IsSuccess = false,
                        Message = "No Patient Found!"
                    };
                }
                var xrayAppointmentDate = await _unitOfWork.XrayAppointmentDateRepository.GetSingleWithIncludesAsync(s => s.Id == xrayAppointmentDateId,
                    i => i.XrayAppointment, i => i.Day, i => i.XrayAppointment.Doctor,
                    i => i.XrayAppointment.Xray.XraySpecialization, i => i.XrayAppointment.Xray.Hospital);
                if (xrayAppointmentDate == null)
                {
                    return new GeneralResponse<PatientReservationDto>
                    {
                        IsSuccess = false,
                        Message = "No xrayAppointmentDate Found!"
                    };
                }
                if (await _unitOfWork.XrayReservationRepository.AnyAsync(a => a.PatientId == patientId && a.XrayAppointmentDateId == xrayAppointmentDateId))
                {
                    return new GeneralResponse<PatientReservationDto>
                    {
                        IsSuccess = false,
                        Message = "You have made a prior reservation at this Xray with this doctor." +
                        " If you want to change the appointment time, please cancel the advance reservation"
                    };
                }
                if (!DateTime.TryParse(date, out DateTime parsedDate))
                {
                    return new GeneralResponse<PatientReservationDto>
                    {
                        IsSuccess = false,
                        Message = "Invalid Date Formate!"
                    };
                }
                var selectedDate = DateTime.Parse(date);
                if (selectedDate.DayOfWeek.ToString() != xrayAppointmentDate.Day.DayName)
                {
                    return new GeneralResponse<PatientReservationDto>
                    {
                        IsSuccess = false,
                        Message = $"The selected date doesnot match the day of the choosen appointment: {xrayAppointmentDate.Day.DayName}"
                    };
                }
                if (DateTime.Now.AddMonths(1) < selectedDate)
                {
                    return new GeneralResponse<PatientReservationDto>
                    {
                        IsSuccess = false,
                        Message = "the selected date isn't accepted because it's after more than Month"
                    };
                }
                var reservation = new XrayReservation()
                {
                    Patient = patient,
                    Date = date,
                    XrayAppointmentDate = xrayAppointmentDate,
                    XrayAppointment = xrayAppointmentDate.XrayAppointment
                };
                await _unitOfWork.XrayReservationRepository.AddAsync(reservation);
                await _unitOfWork.CompleteAsync();
                var data = _mapper.Map<PatientReservationDto>(reservation);
                var user = await _unitOfWork.UserRepository.GetSingleWithIncludesAsync(
                    s => s.UserName == xrayAppointmentDate.XrayAppointment.Doctor.UserName, a => a.UploadedFile);
                data.DoctorImagePath = user.UploadedFile.FilePath;
                return new GeneralResponse<PatientReservationDto>
                {
                    IsSuccess = true,
                    Message = "The Reservation is Completed sucessfully.",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<PatientReservationDto>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }

        }

        public async Task<GeneralResponse<string>> CancelClinicReservation(int patientId, int clinicReservationId)
        {
            try
            {
                var patient = await _unitOfWork.PatientRepository.GetByIdAsync(patientId);
                if (patient == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "No Patient Found!"
                    };
                }
                var clinicReservation = await _unitOfWork.ClinicReservationRepository.GetByIdAsync(clinicReservationId);
                if (clinicReservation == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "No clinicReservation Found!"
                    };
                }
                _unitOfWork.ClinicReservationRepository.Remove(clinicReservation);
                await _unitOfWork.CompleteAsync();
                
                return new GeneralResponse<string>
                {
                    IsSuccess = true,
                    Message = "The Reservation is Canceled sucessfully."
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<string>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }

        public async Task<GeneralResponse<string>> CancelLabReservation(int patientId, int labReservationId)
        {
            try
            {
                var patient = await _unitOfWork.PatientRepository.GetByIdAsync(patientId);
                if (patient == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "No Patient Found!"
                    };
                }
                var labReservation = await _unitOfWork.LabReservationRepository.GetByIdAsync(labReservationId);
                if (labReservation == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "No labReservation Found!"
                    };
                }
                _unitOfWork.LabReservationRepository.Remove(labReservation);
                await _unitOfWork.CompleteAsync();

                return new GeneralResponse<string>
                {
                    IsSuccess = true,
                    Message = "The Reservation is Canceled sucessfully."
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<string>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }

        public async Task<GeneralResponse<string>> CancelXrayReservation(int patientId, int xrayReservationId)
        {
            try
            {
                var patient = await _unitOfWork.PatientRepository.GetByIdAsync(patientId);
                if (patient == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "No Patient Found!"
                    };
                }
                var xrayReservation = await _unitOfWork.XrayReservationRepository.GetByIdAsync(xrayReservationId);
                if (xrayReservation == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "No xrayReservation Found!"
                    };
                }
                _unitOfWork.XrayReservationRepository.Remove(xrayReservation);
                await _unitOfWork.CompleteAsync();

                return new GeneralResponse<string>
                {
                    IsSuccess = true,
                    Message = "The Reservation is Canceled sucessfully."
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<string>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }


        public async Task<GeneralResponse<string>> DeleteClinicAppointment(int clinicAppointmentId)
        {
            try
            {
                var clinicAppointment = await _unitOfWork.ClinicAppointmentRepository.GetSingleWithIncludesAsync(s=>s.Id == clinicAppointmentId, a=>a.ClinicReservations);
                if (clinicAppointment == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "No clinicAppointment Found!"
                    };
                }
                _unitOfWork.ClinicReservationRepository.RemoveRange(clinicAppointment.ClinicReservations);
                _unitOfWork.ClinicAppointmentRepository.Remove(clinicAppointment);
                await _unitOfWork.CompleteAsync();

                return new GeneralResponse<string>
                {
                    IsSuccess = true,
                    Message = "The ClinicAppointment is deleted sucessfully."
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<string>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }
        
        public async Task<GeneralResponse<string>> DeleteLabAppointment(int labAppointmentId)
        {
            try
            {
                var labAppointment = await _unitOfWork.LabAppointmentRepository.GetSingleWithIncludesAsync(s => s.Id == labAppointmentId, a => a.LabReservations);
                if (labAppointment == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "No labAppointment Found!"
                    };
                }
                _unitOfWork.LabReservationRepository.RemoveRange(labAppointment.LabReservations);
                _unitOfWork.LabAppointmentRepository.Remove(labAppointment);
                await _unitOfWork.CompleteAsync();

                return new GeneralResponse<string>
                {
                    IsSuccess = true,
                    Message = "The LabAppointment is deleted sucessfully."
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<string>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }
        public async Task<GeneralResponse<string>> DeleteXrayAppointment(int xrayAppointmentId)
        {
            try
            {
                var xrayAppointment = await _unitOfWork.XrayAppointmentRepository.GetSingleWithIncludesAsync(s => s.Id == xrayAppointmentId, a => a.XrayReservations);
                if (xrayAppointment == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "No xxrayAppointment Found!"
                    };
                }
                _unitOfWork.XrayReservationRepository.RemoveRange(xrayAppointment.XrayReservations);
                _unitOfWork.XrayAppointmentRepository.Remove(xrayAppointment);
                await _unitOfWork.CompleteAsync();

                return new GeneralResponse<string>
                {
                    IsSuccess = true,
                    Message = "The XrayAppointment is deleted sucessfully."
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<string>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }

        public Task<GeneralResponse<string>> DeleteClinicAppointmentDateOfAnAppointment(int clinicAppointmentDateId)
        {
            throw new NotImplementedException();
        }

        

        public Task<GeneralResponse<string>> DeleteLabAppointmentDateOfAnAppointment(int labAppointmentDateId)
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
