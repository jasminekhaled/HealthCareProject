using AutoMapper;
using HealthCare.Core.DTOS;
using HealthCare.Core.DTOS.AppointmentModule.RequestDto;
using HealthCare.Core.DTOS.AppointmentModule.ResponseDto;
using HealthCare.Core.DTOS.AuthModule.ResponseDtos;
using HealthCare.Core.DTOS.ClinicModule.ResponseDto;
using HealthCare.Core.Helpers;
using HealthCare.Core.IRepositories;
using HealthCare.Core.Models.AppointmentModule;
using HealthCare.Core.Models.AuthModule;
using HealthCare.Core.Models.ClinicModule;
using HealthCare.Core.Models.DoctorModule;
using HealthCare.Core.Models.HospitalModule;
using HealthCare.Core.Models.PatientModule;
using HealthCare.Services.IServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.AspNetCore.Server.IIS.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
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
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AppointmentServices(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment webHostEnvironment,
            IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GeneralResponse<AppointmentResponseDto>> AddClinicAppointment(int clinicId, [FromForm]AddAppointmentRequestDto dto)
        {
            try
            {
                HttpContext httpContext = _httpContextAccessor.HttpContext;
                int userId = httpContext.FindFirst();
                var ThisUser = await _unitOfWork.UserRepository.SingleOrDefaultAsync(
                    a => a.Id == userId && a.Role == User.HospitalAdmin);

                if (ThisUser == null)
                {
                    return new GeneralResponse<AppointmentResponseDto>
                    {
                        IsSuccess = false,
                        Message = "No HospitalAdmin Found!"
                    };
                }
                var hospitalAdmin = await _unitOfWork.HospitalAdminRepository.GetSingleWithIncludesAsync(
                    s => s.UserName == ThisUser.UserName, i => i.AdminOfHospital);

                var clinic = await _unitOfWork.ClinicLabRepository.GetByIdAsync(clinicId);
                if (clinic == null || clinic.HospitalId != hospitalAdmin.AdminOfHospital.HospitalId)
                {
                    return new GeneralResponse<AppointmentResponseDto>
                    {
                        IsSuccess = false,
                        Message = "No Clinic Found by this id in your hospital!"
                    };
                }
                var doctor = await _unitOfWork.DoctorRepository.SingleOrDefaultAsync(s => s.Id == dto.DoctorId);
                if (doctor == null ||  !await _unitOfWork.HospitalDoctorRepository.AnyAsync(w=>w.HospitalId == clinic.HospitalId && w.DoctorId == dto.DoctorId))
                {
                    return new GeneralResponse<AppointmentResponseDto>
                    {
                        IsSuccess = false,
                        Message = "No doctor Found in this hospital!"
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
                string[] format = { "hh:mm tt" };
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
                HttpContext httpContext = _httpContextAccessor.HttpContext;
                int userId = httpContext.FindFirst();
                var ThisUser = await _unitOfWork.UserRepository.SingleOrDefaultAsync(
                    a => a.Id == userId && a.Role == User.HospitalAdmin);

                if (ThisUser == null)
                {
                    return new GeneralResponse<AppointmentResponseDto>
                    {
                        IsSuccess = false,
                        Message = "No HospitalAdmin Found!"
                    };
                }
                var hospitalAdmin = await _unitOfWork.HospitalAdminRepository.GetSingleWithIncludesAsync(
                    s => s.UserName == ThisUser.UserName, i => i.AdminOfHospital);


                var lab = await _unitOfWork.LabRepository.GetByIdAsync(labId);
                if (lab == null || lab.HospitalId != hospitalAdmin.AdminOfHospital.HospitalId)
                {
                    return new GeneralResponse<AppointmentResponseDto>
                    {
                        IsSuccess = false,
                        Message = "No Lab Found in your hospital with this id!"
                    };
                }
                var doctor = await _unitOfWork.DoctorRepository.SingleOrDefaultAsync(s => s.Id == dto.DoctorId);
                if (doctor == null || !await _unitOfWork.HospitalDoctorRepository.AnyAsync(w => w.HospitalId == lab.HospitalId && w.DoctorId == dto.DoctorId))
                {
                    return new GeneralResponse<AppointmentResponseDto>
                    {
                        IsSuccess = false,
                        Message = "No doctor Found in this hospital!"
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
                HttpContext httpContext = _httpContextAccessor.HttpContext;
                int userId = httpContext.FindFirst();
                var ThisUser = await _unitOfWork.UserRepository.SingleOrDefaultAsync(
                    a => a.Id == userId && a.Role == User.HospitalAdmin);

                if (ThisUser == null)
                {
                    return new GeneralResponse<AppointmentResponseDto>
                    {
                        IsSuccess = false,
                        Message = "No HospitalAdmin Found!"
                    };
                }
                var hospitalAdmin = await _unitOfWork.HospitalAdminRepository.GetSingleWithIncludesAsync(
                    s => s.UserName == ThisUser.UserName, i => i.AdminOfHospital);


                var xray = await _unitOfWork.XrayRepository.GetByIdAsync(xrayId);
                if (xray == null || xray.HospitalId != hospitalAdmin.AdminOfHospital.HospitalId)
                {
                    return new GeneralResponse<AppointmentResponseDto>
                    {
                        IsSuccess = false,
                        Message = "No Xray Found!"
                    };
                }
                var doctor = await _unitOfWork.DoctorRepository.SingleOrDefaultAsync(s => s.Id == dto.DoctorId);
                if (doctor == null || !await _unitOfWork.HospitalDoctorRepository.AnyAsync(w => w.HospitalId == xray.HospitalId && w.DoctorId == dto.DoctorId))
                {
                    return new GeneralResponse<AppointmentResponseDto>
                    {
                        IsSuccess = false,
                        Message = "No doctor Found in this hospital!"
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
                HttpContext httpContext = _httpContextAccessor.HttpContext;
                int userId = httpContext.FindFirst();
                var ThisUser = await _unitOfWork.UserRepository.SingleOrDefaultAsync(
                    a => a.Id == userId && a.Role == User.HospitalAdmin);

                if (ThisUser == null)
                {
                    return new GeneralResponse<ListAppointmentDto>
                    {
                        IsSuccess = false,
                        Message = "No HospitalAdmin Found!"
                    };
                }
                var hospitalAdmin = await _unitOfWork.HospitalAdminRepository.GetSingleWithIncludesAsync(
                    s => s.UserName == ThisUser.UserName, i => i.AdminOfHospital);


                var clinicAppointment = await _unitOfWork.ClinicAppointmentRepository.GetSingleWithIncludesAsync(
                    s => s.Id ==clinicAppointmentId && s.ClinicLab.HospitalId == hospitalAdmin.AdminOfHospital.HospitalId
                    , i=>i.Doctor,i=>i.ClinicAppointmentDates);
                if (clinicAppointment == null )
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
                HttpContext httpContext = _httpContextAccessor.HttpContext;
                int userId = httpContext.FindFirst();
                var ThisUser = await _unitOfWork.UserRepository.SingleOrDefaultAsync(
                    a => a.Id == userId && a.Role == User.HospitalAdmin);

                if (ThisUser == null)
                {
                    return new GeneralResponse<ListAppointmentDto>
                    {
                        IsSuccess = false,
                        Message = "No HospitalAdmin Found!"
                    };
                }
                var hospitalAdmin = await _unitOfWork.HospitalAdminRepository.GetSingleWithIncludesAsync(
                    s => s.UserName == ThisUser.UserName, i => i.AdminOfHospital);


                var labAppointment = await _unitOfWork.LabAppointmentRepository.GetSingleWithIncludesAsync(
                    s => s.Id == labAppointmentId && s.Lab.HospitalId == hospitalAdmin.AdminOfHospital.HospitalId,
                    i => i.Doctor, i => i.LabAppointmentDates);
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
                HttpContext httpContext = _httpContextAccessor.HttpContext;
                int userId = httpContext.FindFirst();
                var ThisUser = await _unitOfWork.UserRepository.SingleOrDefaultAsync(
                    a => a.Id == userId && a.Role == User.HospitalAdmin);

                if (ThisUser == null)
                {
                    return new GeneralResponse<ListAppointmentDto>
                    {
                        IsSuccess = false,
                        Message = "No HospitalAdmin Found!"
                    };
                }
                var hospitalAdmin = await _unitOfWork.HospitalAdminRepository.GetSingleWithIncludesAsync(
                    s => s.UserName == ThisUser.UserName, i => i.AdminOfHospital);


                var xrayAppointment = await _unitOfWork.XrayAppointmentRepository.GetSingleWithIncludesAsync(
                    s => s.Id == xrayAppointmentId && s.Xray.HospitalId == hospitalAdmin.AdminOfHospital.HospitalId,
                    i => i.Doctor, i => i.XrayAppointmentDates);
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

        
        public async Task<GeneralResponse<PatientReservationDto>> BookAnAppointmentOfClinic(int clinicAppointmentDateId, string date)
        {
            try 
            {
                HttpContext httpContext = _httpContextAccessor.HttpContext;
                int userId = httpContext.FindFirst();
                var ThisUser = await _unitOfWork.UserRepository.SingleOrDefaultAsync(
                    a => a.Id == userId && a.Role == User.Patient);

                if (ThisUser == null)
                {
                    return new GeneralResponse<PatientReservationDto>
                    {
                        IsSuccess = false,
                        Message = "No Patient Found!"
                    };
                }
                var patient = await _unitOfWork.PatientRepository.SingleOrDefaultAsync(s => s.UserName == ThisUser.UserName);

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
                if(await _unitOfWork.ClinicReservationRepository.AnyAsync(a=>a.PatientId==patient.Id && a.ClinicAppointmentDateId == clinicAppointmentDateId))
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
                var all = _mapper.Map<AllReservations>(reservation);
                all.Type = AllReservations.Clinic;
                await _unitOfWork.AllReservationsRepository.AddAsync(all);
                await _unitOfWork.CompleteAsync();
                var data = _mapper.Map<PatientReservationDto>(reservation);
                var user = await _unitOfWork.UserRepository.GetSingleWithIncludesAsync(
                    s => s.UserName == clinicAppointmentDate.ClinicAppointment.Doctor.UserName, a => a.UploadedFile);
                data.DoctorImagePath = user.UploadedFile.FilePath;
                data.Type = AllReservations.Clinic;
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

        public async Task<GeneralResponse<PatientReservationDto>> BookAnAppointmentOfLab(int labAppointmentDateId, string date)
        {
            try
            {
                HttpContext httpContext = _httpContextAccessor.HttpContext;
                int userId = httpContext.FindFirst();
                var ThisUser = await _unitOfWork.UserRepository.SingleOrDefaultAsync(
                    a => a.Id == userId && a.Role == User.Patient);

                if (ThisUser == null)
                {
                    return new GeneralResponse<PatientReservationDto>
                    {
                        IsSuccess = false,
                        Message = "No Patient Found!"
                    };
                }
                var patient = await _unitOfWork.PatientRepository.SingleOrDefaultAsync(s => s.UserName == ThisUser.UserName);

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
                if (await _unitOfWork.LabReservationRepository.AnyAsync(a => a.PatientId == patient.Id && a.LabAppointmentDateId == labAppointmentDateId))
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
                var all = _mapper.Map<AllReservations>(reservation);
                all.Type = AllReservations.Lab;

                await _unitOfWork.AllReservationsRepository.AddAsync(all);
                await _unitOfWork.CompleteAsync();
                var data = _mapper.Map<PatientReservationDto>(reservation);
                var user = await _unitOfWork.UserRepository.GetSingleWithIncludesAsync(
                    s => s.UserName == labAppointmentDate.LabAppointment.Doctor.UserName, a => a.UploadedFile);
                data.DoctorImagePath = user.UploadedFile.FilePath;
                data.Type = AllReservations.Lab;
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

        public async Task<GeneralResponse<PatientReservationDto>> BookAnAppointmentOfXray(int xrayAppointmentDateId, string date)
        {
            try
            {
                HttpContext httpContext = _httpContextAccessor.HttpContext;
                int userId = httpContext.FindFirst();
                var ThisUser = await _unitOfWork.UserRepository.SingleOrDefaultAsync(
                    a => a.Id == userId && a.Role == User.Patient);

                if (ThisUser == null)
                {
                    return new GeneralResponse<PatientReservationDto>
                    {
                        IsSuccess = false,
                        Message = "No Patient Found!"
                    };
                }
                var patient = await _unitOfWork.PatientRepository.SingleOrDefaultAsync(s => s.UserName == ThisUser.UserName);

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
                if (await _unitOfWork.XrayReservationRepository.AnyAsync(a => a.PatientId == patient.Id && a.XrayAppointmentDateId == xrayAppointmentDateId))
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
                var all = _mapper.Map<AllReservations>(reservation);
                await _unitOfWork.XrayReservationRepository.AddAsync(reservation);
                await _unitOfWork.CompleteAsync();
                all.Type = AllReservations.Xray;

                await _unitOfWork.AllReservationsRepository.AddAsync(all);
                await _unitOfWork.CompleteAsync();
                var data = _mapper.Map<PatientReservationDto>(reservation);
                var user = await _unitOfWork.UserRepository.GetSingleWithIncludesAsync(
                    s => s.UserName == xrayAppointmentDate.XrayAppointment.Doctor.UserName, a => a.UploadedFile);
                data.DoctorImagePath = user.UploadedFile.FilePath;
                data.Type = AllReservations.Xray;

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

        public async Task<GeneralResponse<string>> CancelClinicReservation(int clinicReservationId)
        {
            try
            {
                HttpContext httpContext = _httpContextAccessor.HttpContext;
                int userId = httpContext.FindFirst();
                var ThisUser = await _unitOfWork.UserRepository.SingleOrDefaultAsync(
                    a => a.Id == userId && a.Role == User.Patient);

                if (ThisUser == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "No Patient Found!"
                    };
                }
                var patient = await _unitOfWork.PatientRepository.SingleOrDefaultAsync(s => s.UserName == ThisUser.UserName);

                var clinicReservation = await _unitOfWork.ClinicReservationRepository.GetByIdAsync(clinicReservationId);
                if (clinicReservation == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "No clinicReservation Found!"
                    };
                }
               var all = await _unitOfWork.AllReservationsRepository.SingleOrDefaultAsync
                    (s => s.RoomReservationId == clinicReservationId && s.Type == AllReservations.Clinic);
                _unitOfWork.AllReservationsRepository.Remove(all);
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

        public async Task<GeneralResponse<string>> CancelLabReservation(int labReservationId)
        {
            try
            {
                HttpContext httpContext = _httpContextAccessor.HttpContext;
                int userId = httpContext.FindFirst();
                var ThisUser = await _unitOfWork.UserRepository.SingleOrDefaultAsync(
                    a => a.Id == userId && a.Role == User.Patient);

                if (ThisUser == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "No Patient Found!"
                    };
                }
                var patient = await _unitOfWork.PatientRepository.SingleOrDefaultAsync(s => s.UserName == ThisUser.UserName);

                var labReservation = await _unitOfWork.LabReservationRepository.GetByIdAsync(labReservationId);
                if (labReservation == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "No labReservation Found!"
                    };
                }
                var all = await _unitOfWork.AllReservationsRepository.SingleOrDefaultAsync
                    (s => s.RoomReservationId == labReservationId && s.Type == AllReservations.Lab);
                _unitOfWork.AllReservationsRepository.Remove(all);
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

        public async Task<GeneralResponse<string>> CancelXrayReservation(int xrayReservationId)
        {
            try
            {
                HttpContext httpContext = _httpContextAccessor.HttpContext;
                int userId = httpContext.FindFirst();
                var ThisUser = await _unitOfWork.UserRepository.SingleOrDefaultAsync(
                    a => a.Id == userId && a.Role == User.Patient);

                if (ThisUser == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "No Patient Found!"
                    };
                }
                var patient = await _unitOfWork.PatientRepository.SingleOrDefaultAsync(s => s.UserName == ThisUser.UserName);

                var xrayReservation = await _unitOfWork.XrayReservationRepository.GetByIdAsync(xrayReservationId);
                if (xrayReservation == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "No xrayReservation Found!"
                    };
                }
                var all = await _unitOfWork.AllReservationsRepository.SingleOrDefaultAsync
                    (s => s.RoomReservationId == xrayReservationId && s.Type == AllReservations.Xray);
                _unitOfWork.AllReservationsRepository.Remove(all);
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
                HttpContext httpContext = _httpContextAccessor.HttpContext;
                int userId = httpContext.FindFirst();
                var ThisUser = await _unitOfWork.UserRepository.SingleOrDefaultAsync(
                    a => a.Id == userId && a.Role == User.HospitalAdmin);

                if (ThisUser == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "No hospitalAdmin Found!"
                    };
                }
                var hospitalAdmin = await _unitOfWork.HospitalAdminRepository.GetSingleWithIncludesAsync(
                    s => s.UserName == ThisUser.UserName, i => i.AdminOfHospital);


                var clinicAppointment = await _unitOfWork.ClinicAppointmentRepository.GetSingleWithIncludesAsync(
                    s=>s.Id == clinicAppointmentId && s.ClinicLab.HospitalId == hospitalAdmin.AdminOfHospital.HospitalId, 
                    a=>a.ClinicReservations);
                if (clinicAppointment == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "No clinicAppointment Found!"
                    };
                }
                var clinicDates = await _unitOfWork.ClinicAppointmentDateRepository
                    .GetSpecificItems(w => w.ClinicAppointmentId == clinicAppointmentId, a => a.Id);

                var all = await _unitOfWork.AllReservationsRepository
                    .Where(w => clinicDates.Contains(w.RoomAppointmentDateId) && w.Type == AllReservations.Clinic);

                _unitOfWork.AllReservationsRepository.RemoveRange(all);
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
                HttpContext httpContext = _httpContextAccessor.HttpContext;
                int userId = httpContext.FindFirst();
                var ThisUser = await _unitOfWork.UserRepository.SingleOrDefaultAsync(
                    a => a.Id == userId && a.Role == User.HospitalAdmin);

                if (ThisUser == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "No hospitalAdmin Found!"
                    };
                }
                var hospitalAdmin = await _unitOfWork.HospitalAdminRepository.GetSingleWithIncludesAsync(
                    s => s.UserName == ThisUser.UserName, i => i.AdminOfHospital);


                var labAppointment = await _unitOfWork.LabAppointmentRepository.GetSingleWithIncludesAsync(
                    s => s.Id == labAppointmentId && s.Lab.HospitalId == hospitalAdmin.AdminOfHospital.HospitalId, 
                    a => a.LabReservations);
                if (labAppointment == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "No labAppointment Found!"
                    };
                }
                var labDates = await _unitOfWork.LabAppointmentDateRepository
                    .GetSpecificItems(w => w.LabAppointmentId == labAppointmentId, a => a.Id);

                var all = await _unitOfWork.AllReservationsRepository
                    .Where(w => labDates.Contains(w.RoomAppointmentDateId) && w.Type == AllReservations.Lab);

                _unitOfWork.AllReservationsRepository.RemoveRange(all);
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
                HttpContext httpContext = _httpContextAccessor.HttpContext;
                int userId = httpContext.FindFirst();
                var ThisUser = await _unitOfWork.UserRepository.SingleOrDefaultAsync(
                    a => a.Id == userId && a.Role == User.HospitalAdmin);

                if (ThisUser == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "No hospitalAdmin Found!"
                    };
                }
                var hospitalAdmin = await _unitOfWork.HospitalAdminRepository.GetSingleWithIncludesAsync(
                    s => s.UserName == ThisUser.UserName, i => i.AdminOfHospital);


                var xrayAppointment = await _unitOfWork.XrayAppointmentRepository.GetSingleWithIncludesAsync(
                    s => s.Id == xrayAppointmentId && s.Xray.HospitalId == hospitalAdmin.AdminOfHospital.HospitalId, 
                    a => a.XrayReservations);
                if (xrayAppointment == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "No xxrayAppointment Found!"
                    };
                }
                var xrayDates = await _unitOfWork.XrayAppointmentDateRepository
                    .GetSpecificItems(w => w.XrayAppointmentId == xrayAppointmentId, a => a.Id);

                var all = await _unitOfWork.AllReservationsRepository
                    .Where(w => xrayDates.Contains(w.RoomAppointmentDateId) && w.Type == AllReservations.Xray);

                _unitOfWork.AllReservationsRepository.RemoveRange(all);
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

        public async Task<GeneralResponse<string>> DeleteClinicAppointmentDateOfAnAppointment(int clinicAppointmentDateId)
        {
            try
            {
                HttpContext httpContext = _httpContextAccessor.HttpContext;
                int userId = httpContext.FindFirst();
                var ThisUser = await _unitOfWork.UserRepository.SingleOrDefaultAsync(
                    a => a.Id == userId && a.Role == User.HospitalAdmin);

                if (ThisUser == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "No hospitalAdmin Found!"
                    };
                }
                var hospitalAdmin = await _unitOfWork.HospitalAdminRepository.GetSingleWithIncludesAsync(
                    s => s.UserName == ThisUser.UserName, i => i.AdminOfHospital);


                var clinicAppointmentDate = await _unitOfWork.ClinicAppointmentDateRepository.GetSingleWithIncludesAsync(
                    s => s.Id == clinicAppointmentDateId &&
                    s.ClinicAppointment.ClinicLab.HospitalId == hospitalAdmin.AdminOfHospital.HospitalId, 
                    a => a.ClinicReservations);
                if (clinicAppointmentDate == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "No clinicAppointmentDate Found!"
                    };
                }
                var all = await _unitOfWork.AllReservationsRepository
                    .Where(w => w.RoomAppointmentDateId==clinicAppointmentDateId && w.Type == AllReservations.Clinic);

                _unitOfWork.AllReservationsRepository.RemoveRange(all);
                _unitOfWork.ClinicReservationRepository.RemoveRange(clinicAppointmentDate.ClinicReservations);
                _unitOfWork.ClinicAppointmentDateRepository.Remove(clinicAppointmentDate);
                await _unitOfWork.CompleteAsync();

                return new GeneralResponse<string>
                {
                    IsSuccess = true,
                    Message = "The ClinicAppointmentDate is deleted sucessfully."
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

        public async Task<GeneralResponse<string>> DeleteLabAppointmentDateOfAnAppointment(int labAppointmentDateId)
        {
            try
            {
                HttpContext httpContext = _httpContextAccessor.HttpContext;
                int userId = httpContext.FindFirst();
                var ThisUser = await _unitOfWork.UserRepository.SingleOrDefaultAsync(
                    a => a.Id == userId && a.Role == User.HospitalAdmin);

                if (ThisUser == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "No hospitalAdmin Found!"
                    };
                }
                var hospitalAdmin = await _unitOfWork.HospitalAdminRepository.GetSingleWithIncludesAsync(
                    s => s.UserName == ThisUser.UserName, i => i.AdminOfHospital);


                var labAppointmentDate = await _unitOfWork.LabAppointmentDateRepository.GetSingleWithIncludesAsync(
                    s => s.Id == labAppointmentDateId &&
                    s.LabAppointment.Lab.HospitalId == hospitalAdmin.AdminOfHospital.HospitalId, 
                    a => a.LabReservations);
                if (labAppointmentDate == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "No labAppointmentDate Found!"
                    };
                }
                var all = await _unitOfWork.AllReservationsRepository
                    .Where(w => w.RoomAppointmentDateId == labAppointmentDateId && w.Type == AllReservations.Lab);

                _unitOfWork.AllReservationsRepository.RemoveRange(all);
                _unitOfWork.LabReservationRepository.RemoveRange(labAppointmentDate.LabReservations);
                _unitOfWork.LabAppointmentDateRepository.Remove(labAppointmentDate);
                await _unitOfWork.CompleteAsync();

                return new GeneralResponse<string>
                {
                    IsSuccess = true,
                    Message = "The LabAppointmentDate is deleted sucessfully."
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

        public async Task<GeneralResponse<string>> DeleteXrayAppointmentDateOfAnAppointment(int xrayAppointmentDateId)
        {
            try
            {
                HttpContext httpContext = _httpContextAccessor.HttpContext;
                int userId = httpContext.FindFirst();
                var ThisUser = await _unitOfWork.UserRepository.SingleOrDefaultAsync(
                    a => a.Id == userId && a.Role == User.HospitalAdmin);

                if (ThisUser == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "No hospitalAdmin Found!"
                    };
                }
                var hospitalAdmin = await _unitOfWork.HospitalAdminRepository.GetSingleWithIncludesAsync(
                    s => s.UserName == ThisUser.UserName, i => i.AdminOfHospital);


                var xrayAppointmentDate = await _unitOfWork.XrayAppointmentDateRepository.GetSingleWithIncludesAsync(
                    s => s.Id == xrayAppointmentDateId &&
                    s.XrayAppointment.Xray.HospitalId == hospitalAdmin.AdminOfHospital.HospitalId,
                    a => a.XrayReservations);
                if (xrayAppointmentDate == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "No xrayAppointmentDate Found!"
                    };
                }
                var all = await _unitOfWork.AllReservationsRepository
                    .Where(w => w.RoomAppointmentDateId == xrayAppointmentDateId && w.Type == AllReservations.Xray);

                _unitOfWork.AllReservationsRepository.RemoveRange(all);
                _unitOfWork.XrayReservationRepository.RemoveRange(xrayAppointmentDate.XrayReservations);
                _unitOfWork.XrayAppointmentDateRepository.Remove(xrayAppointmentDate);
                await _unitOfWork.CompleteAsync();

                return new GeneralResponse<string>
                {
                    IsSuccess = true,
                    Message = "The XrayAppointmentDate is deleted sucessfully."
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

        
        public async Task<GeneralResponse<List<ListAppointmentDto>>> ListOfAppointmentOfClinic(int clinicId)
        {
            try
            {
                var clinic = await _unitOfWork.ClinicLabRepository.GetSingleWithIncludesAsync(s => s.Id == clinicId, a => a.ClinicAppointments);
                if (clinic == null)
                {
                    return new GeneralResponse<List<ListAppointmentDto>>
                    {
                        IsSuccess = false,
                        Message = "No clinic Found!"
                    };
                }
                var appointments = await _unitOfWork.ClinicAppointmentRepository.WhereIncludeAsync(w => w.ClinicLabId == clinicId,
                     a => a.ClinicAppointmentDates, a=>a.Doctor);
                var data = _mapper.Map<List<ListAppointmentDto>>(appointments);
                

                 foreach(var a in data)
                {
                    var appointmentDate = await _unitOfWork.ClinicAppointmentDateRepository.WhereIncludeAsync
                        (w => w.ClinicAppointmentId == a.Id, a => a.Day);
                    var ordered = appointmentDate.OrderBy(a => a.DayId);
                    a.AppointmentDates = _mapper.Map<List<AppointmentDateResponseDto>>(ordered);
                    var doctor = await _unitOfWork.ClinicAppointmentRepository.GetSingleWithIncludesAsync(s => s.Id == a.Id, w => w.Doctor);
                    var user = await _unitOfWork.UserRepository.GetSingleWithIncludesAsync(s => s.UserName == doctor.Doctor.UserName, a => a.UploadedFile);
                    a.DoctorImagePath = user.UploadedFile.FilePath;
                }

                return new GeneralResponse<List<ListAppointmentDto>>
                {
                    IsSuccess = true,
                    Message = "The ClinicAppointmentDate is deleted sucessfully.",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<List<ListAppointmentDto>>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }

        public async Task<GeneralResponse<List<ListAppointmentDto>>> ListOfAppointmentOfLab(int labId)
        {
            try
            {
                var lab = await _unitOfWork.LabRepository.GetSingleWithIncludesAsync(s => s.Id == labId, a => a.LabAppointments);
                if (lab == null)
                {
                    return new GeneralResponse<List<ListAppointmentDto>>
                    {
                        IsSuccess = false,
                        Message = "No lab Found!"
                    };
                }
                var appointments = await _unitOfWork.LabAppointmentRepository.WhereIncludeAsync(w => w.LabId == labId,
                     a => a.LabAppointmentDates, a => a.Doctor);
                var data = _mapper.Map<List<ListAppointmentDto>>(appointments);


                foreach (var a in data)
                {
                    var appointmentDate = await _unitOfWork.LabAppointmentDateRepository.WhereIncludeAsync
                        (w => w.LabAppointmentId == a.Id, a => a.Day);
                    var ordered = appointmentDate.OrderBy(a => a.DayId);
                    a.AppointmentDates = _mapper.Map<List<AppointmentDateResponseDto>>(ordered);
                    var doctor = await _unitOfWork.LabAppointmentRepository.GetSingleWithIncludesAsync(s => s.Id == a.Id, w => w.Doctor);
                    var user = await _unitOfWork.UserRepository.GetSingleWithIncludesAsync(s => s.UserName == doctor.Doctor.UserName, a => a.UploadedFile);
                    a.DoctorImagePath = user.UploadedFile.FilePath;
                }

                return new GeneralResponse<List<ListAppointmentDto>>
                {
                    IsSuccess = true,
                    Message = "The LabAppointmentDate is deleted sucessfully.",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<List<ListAppointmentDto>>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }

        public async Task<GeneralResponse<List<ListAppointmentDto>>> ListOfAppointmentOfXray(int xrayId)
        {
            try
            {
                var xray = await _unitOfWork.XrayRepository.GetSingleWithIncludesAsync(s => s.Id == xrayId, a => a.XrayAppointments);
                if (xray == null)
                {
                    return new GeneralResponse<List<ListAppointmentDto>>
                    {
                        IsSuccess = false,
                        Message = "No xray Found!"
                    };
                }
                var appointments = await _unitOfWork.XrayAppointmentRepository.WhereIncludeAsync(w => w.XrayId == xrayId,
                     a => a.XrayAppointmentDates, a => a.Doctor);
                var data = _mapper.Map<List<ListAppointmentDto>>(appointments);


                foreach (var a in data)
                {
                    var appointmentDate = await _unitOfWork.XrayAppointmentDateRepository.WhereIncludeAsync
                        (w => w.XrayAppointmentId == a.Id, a => a.Day);
                    var ordered = appointmentDate.OrderBy(a => a.DayId);
                    a.AppointmentDates = _mapper.Map<List<AppointmentDateResponseDto>>(ordered);
                    var doctor = await _unitOfWork.XrayAppointmentRepository.GetSingleWithIncludesAsync(s => s.Id == a.Id, w => w.Doctor);
                    var user = await _unitOfWork.UserRepository.GetSingleWithIncludesAsync(s => s.UserName == doctor.Doctor.UserName, a => a.UploadedFile);
                    a.DoctorImagePath = user.UploadedFile.FilePath;
                }

                return new GeneralResponse<List<ListAppointmentDto>>
                {
                    IsSuccess = true,
                    Message = "The XrayAppointmentDate is deleted sucessfully.",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<List<ListAppointmentDto>>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }

        public async Task<GeneralResponse<List<ReservationResponseDto>>> ListOfReservationsOfDoctor(int doctorId, int hospitalId)
        {
            try
            {
                var doctor = await _unitOfWork.DoctorRepository.SingleOrDefaultAsync(s => s.Id == doctorId);
                if (doctor == null)
                {
                    return new GeneralResponse<List<ReservationResponseDto>>
                    {
                        IsSuccess = false,
                        Message = "No doctor Found!"
                    };
                }

                var hospital = await _unitOfWork.HospitalRepository.GetSingleWithIncludesAsync(s => s.Id == hospitalId);
                if (hospital == null)
                {
                    return new GeneralResponse<List<ReservationResponseDto>>
                    {
                        IsSuccess = false,
                        Message = "No Hospital Found!"
                    };
                }

                if (!await _unitOfWork.HospitalDoctorRepository.AnyAsync(
                    a=>a.HospitalId==hospitalId && a.DoctorId==doctor.Id))
                {
                    return new GeneralResponse<List<ReservationResponseDto>>
                    {
                        IsSuccess = false,
                        Message = "No doctor Found!"
                    };
                }

                var all = await _unitOfWork.AllReservationsRepository.WhereIncludeAsync(
                    w => w.DoctorId == doctor.Id, a=>a.Patient);
             
                var available = all.Where(a => DateTime.Parse(a.Date) >= DateTime.Now).ToList();
               
                var data = _mapper.Map<List<ReservationResponseDto>>(available.OrderBy(available => DateTime.Parse(available.Date)));


                var unAvailable = all.Where(a => DateTime.Parse(a.Date) < DateTime.Now).ToList();

                var clinicReservationIds = unAvailable.Where(w => w.Type == AllReservations.Clinic)
                    .Select(s=>s.RoomReservationId);
                var clinicReservations = await _unitOfWork.ClinicReservationRepository.Where(
                    w => clinicReservationIds.Contains(w.Id));

                _unitOfWork.ClinicReservationRepository.RemoveRange(clinicReservations);

                var labReservationIds = unAvailable.Where(w => w.Type == AllReservations.Lab)
                    .Select(s => s.RoomReservationId);
                var labReservations = await _unitOfWork.LabReservationRepository.Where(
                    w => labReservationIds.Contains(w.Id));
                _unitOfWork.LabReservationRepository.RemoveRange(labReservations);

                var xrayReservationIds = unAvailable.Where(w => w.Type == AllReservations.Xray)
                    .Select(s => s.RoomReservationId);
                var xrayReservations = await _unitOfWork.XrayReservationRepository.Where(
                    w => xrayReservationIds.Contains(w.Id));
                _unitOfWork.XrayReservationRepository.RemoveRange(xrayReservations);

                _unitOfWork.AllReservationsRepository.RemoveRange(unAvailable);
                await _unitOfWork.CompleteAsync();
                
                

                return new GeneralResponse<List<ReservationResponseDto>>
                {
                    IsSuccess = true,
                    Message = "The Appointment is Listed sucessfully.",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<List<ReservationResponseDto>>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }

        public async Task<GeneralResponse<List<PatientReservationDto>>> ListOfReservationsOfPatient()
        {
            try
            {
                HttpContext httpContext = _httpContextAccessor.HttpContext;
                int userId = httpContext.FindFirst();
                var ThisUser = await _unitOfWork.UserRepository.SingleOrDefaultAsync(
                    a => a.Id == userId && a.Role == User.Patient);

                if (ThisUser == null)
                {
                    return new GeneralResponse<List<PatientReservationDto>>
                    {
                        IsSuccess = false,
                        Message = "No patient Found!"
                    };
                }
                var patient = await _unitOfWork.PatientRepository.SingleOrDefaultAsync(s => s.UserName == ThisUser.UserName);


                var all = await _unitOfWork.AllReservationsRepository.WhereIncludeAsync(
                    w => w.PatientId == patient.Id, a => a.Doctor);

                var available = all.Where(a => DateTime.Parse(a.Date) >= DateTime.Now).ToList();

                var data = _mapper.Map<List<PatientReservationDto>>(available.OrderBy(available => DateTime.Parse(available.Date)));
                foreach(var a in data)
                {
                    var reservation = await _unitOfWork.AllReservationsRepository.GetSingleWithIncludesAsync(
                        s => s.Id == a.Id, w => w.Doctor, w => w.Doctor.UploadedFile);
                    
                    a.DoctorImagePath = reservation.Doctor.UploadedFile.FilePath;
                    if(a.Type == AllReservations.Clinic)
                    {
                        var name = await _unitOfWork.ClinicLabRepository.GetSingleWithIncludesAsync(s => s.Id == a.RoomId, w => w.Specialization, w => w.Hospital);
                        a.ClinicName = name.Specialization.Name;
                        a.HospitalName = name.Hospital.Name;
                    }
                    if (a.Type == AllReservations.Lab)
                    {
                        var name = await _unitOfWork.LabRepository.GetSingleWithIncludesAsync(s => s.Id == a.RoomId, w => w.Hospital);
                        a.ClinicName = name.Name;
                        a.HospitalName = name.Hospital.Name;
                    }
                    if (a.Type == AllReservations.Xray)
                    {
                        var name = await _unitOfWork.XrayRepository.GetSingleWithIncludesAsync(s => s.Id == a.RoomId, w => w.XraySpecialization, w => w.Hospital);
                        a.ClinicName = name.XraySpecialization.Name;
                        a.HospitalName = name.Hospital.Name;
                    }
                }

                var unAvailable = all
                        .Where(a => DateTime.Parse(a.Date) < DateTime.Now).ToList();

                var clinicReservationIds = unAvailable.Where(w => w.Type == AllReservations.Clinic)
                    .Select(s => s.RoomReservationId);
                var clinicReservations = await _unitOfWork.ClinicReservationRepository.Where(
                    w => clinicReservationIds.Contains(w.Id));
                _unitOfWork.ClinicReservationRepository.RemoveRange(clinicReservations);

                var labReservationIds = unAvailable.Where(w => w.Type == AllReservations.Lab)
                    .Select(s => s.RoomReservationId);
                var labReservations = await _unitOfWork.LabReservationRepository.Where(w => labReservationIds.Contains(w.Id));
                _unitOfWork.LabReservationRepository.RemoveRange(labReservations);

                var xrayReservationIds = unAvailable.Where(w => w.Type == AllReservations.Xray)
                    .Select(s => s.RoomReservationId);
                var xrayReservations = await _unitOfWork.XrayReservationRepository.Where(
                    w => xrayReservationIds.Contains(w.Id));
                _unitOfWork.XrayReservationRepository.RemoveRange(xrayReservations);

                _unitOfWork.AllReservationsRepository.RemoveRange(unAvailable);
                await _unitOfWork.CompleteAsync();

                
                return new GeneralResponse<List<PatientReservationDto>>
                {
                    IsSuccess = true,
                    Message = "The Appointment is Listed sucessfully.",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<List<PatientReservationDto>>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }
        
        public async Task<GeneralResponse<string>> DoneReservation(int reservationId)
        {
            try
            {
                HttpContext httpContext = _httpContextAccessor.HttpContext;
                int userId = httpContext.FindFirst();
                var ThisUser = await _unitOfWork.UserRepository.SingleOrDefaultAsync(
                    a => a.Id == userId);

                if (ThisUser.Role!= User.Doctor && ThisUser.Role!= User.HospitalAdmin)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "No user Found!"
                    };
                }
                AllReservations reservation;
                if(ThisUser.Role == User.Doctor)
                {
                    var doctor = await _unitOfWork.DoctorRepository.SingleOrDefaultAsync(s => s.UserName == ThisUser.UserName);

                     reservation = await _unitOfWork.AllReservationsRepository.GetSingleWithIncludesAsync(
                        s => s.Id == reservationId && s.DoctorId == doctor.Id);
                    if (reservation == null)
                    {
                        return new GeneralResponse<string>
                        {
                            IsSuccess = false,
                            Message = "No reservation Found!"
                        };
                    }
                }
                else
                {
                    reservation = await _unitOfWork.AllReservationsRepository.GetSingleWithIncludesAsync(
                       s => s.Id == reservationId);
                    if (reservation == null)
                    {
                        return new GeneralResponse<string>
                        {
                            IsSuccess = false,
                            Message = "No reservation Found!"
                        };
                    }

                }

                if (reservation.Type == AllReservations.Clinic)
                {
                    var clinicReservation = await _unitOfWork.ClinicReservationRepository.GetByIdAsync(reservation.RoomReservationId);
                    _unitOfWork.ClinicReservationRepository.Remove(clinicReservation);
                    await _unitOfWork.CompleteAsync();
                }
                if (reservation.Type == AllReservations.Lab)
                {
                    var labReservation = await _unitOfWork.LabReservationRepository.GetByIdAsync(reservation.RoomReservationId);
                    _unitOfWork.LabReservationRepository.Remove(labReservation);
                    await _unitOfWork.CompleteAsync();
                }
                if (reservation.Type == AllReservations.Xray)
                {
                    var xrayReservation = await _unitOfWork.XrayReservationRepository.GetByIdAsync(reservation.RoomReservationId);
                    _unitOfWork.XrayReservationRepository.Remove(xrayReservation);
                    await _unitOfWork.CompleteAsync();
                }
                _unitOfWork.AllReservationsRepository.Remove(reservation);
                await _unitOfWork.CompleteAsync();

                return new GeneralResponse<string>
                {
                    IsSuccess = true,
                    Message = "This Reservation is Done ."
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


        public async Task<GeneralResponse<string>> CancelReservation(int reservationId)
        {
            try
            {
                HttpContext httpContext = _httpContextAccessor.HttpContext;
                int userId = httpContext.FindFirst();
                var ThisUser = await _unitOfWork.UserRepository.SingleOrDefaultAsync(
                    a => a.Id == userId && a.Role == User.Patient);

                if (ThisUser == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "No patient Found!"
                    };
                }
                var patient = await _unitOfWork.PatientRepository.SingleOrDefaultAsync(s => s.UserName == ThisUser.UserName);

                var reservation = await _unitOfWork.AllReservationsRepository.GetSingleWithIncludesAsync(
                    s => s.Id == reservationId && s.PatientId == patient.Id);
                if (reservation == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "No reservation Found!"
                    };
                }
                if (reservation.Type == AllReservations.Clinic)
                {
                    var clinicReservation = await _unitOfWork.ClinicReservationRepository.GetByIdAsync(reservation.RoomReservationId);
                    _unitOfWork.ClinicReservationRepository.Remove(clinicReservation);
                    await _unitOfWork.CompleteAsync();
                }
                if (reservation.Type == AllReservations.Lab)
                {
                    var labReservation = await _unitOfWork.LabReservationRepository.GetByIdAsync(reservation.RoomReservationId);
                    _unitOfWork.LabReservationRepository.Remove(labReservation);
                    await _unitOfWork.CompleteAsync();
                }
                if (reservation.Type == AllReservations.Xray)
                {
                    var xrayReservation = await _unitOfWork.XrayReservationRepository.GetByIdAsync(reservation.RoomReservationId);
                    _unitOfWork.XrayReservationRepository.Remove(xrayReservation);
                    await _unitOfWork.CompleteAsync();
                }
                _unitOfWork.AllReservationsRepository.Remove(reservation);
                await _unitOfWork.CompleteAsync();

                return new GeneralResponse<string>
                {
                    IsSuccess = true,
                    Message = "This Reservation is canceled ."
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

        public async Task<GeneralResponse<List<DayDto>>> ListOfDays() 
        {
            try
            {
                var Days = await _unitOfWork.DayRepository.GetAllAsync();
                var data = _mapper.Map<List<DayDto>>(Days);

                return new GeneralResponse<List<DayDto>>
                {
                    IsSuccess = true,
                    Message = "This Reservation is canceled .",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<List<DayDto>>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }


    }
}
