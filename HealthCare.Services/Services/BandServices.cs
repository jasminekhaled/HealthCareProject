using AutoMapper;
using HealthCare.Core.DTOS;
using HealthCare.Core.DTOS.AppointmentModule.ResponseDto;
using HealthCare.Core.DTOS.AuthModule.ResponseDtos;
using HealthCare.Core.DTOS.BandModule.RequestDtos;
using HealthCare.Core.DTOS.BandModule.ResponseDtos;
using HealthCare.Core.Helpers;
using HealthCare.Core.IRepositories;
using HealthCare.Core.Models.AppointmentModule;
using HealthCare.Core.Models.AuthModule;
using HealthCare.Core.Models.BandModule;
using HealthCare.Core.Models.DoctorModule;
using HealthCare.Core.Models.HospitalModule;
using HealthCare.Core.Models.PatientModule;
using HealthCare.Services.IServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Services.Services
{
    public class BandServices : IBandServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BandServices(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GeneralResponse<AddPrivateBandDto>> AddPrivateBand()
        {
            try
            {
                HttpContext httpContext = _httpContextAccessor.HttpContext;
                int userId = httpContext.FindFirst(); 
                var user = await _unitOfWork.UserRepository.SingleOrDefaultAsync(a => a.Id == userId && a.Role == User.HospitalAdmin);
                if (user == null)
                {
                    return new GeneralResponse<AddPrivateBandDto>
                    {
                        IsSuccess = false,
                        Message = "No user Found!"
                    };
                }
                var hospitalAdmin = await _unitOfWork.HospitalAdminRepository.GetSingleWithIncludesAsync(
                    a => a.UserName == user.UserName, i => i.AdminOfHospital.Hospital);
                var band = new Band()
                {
                    UniqueId = BandIdService.GenerateUniqueString(),
                    Type = Band.Private,
                    Hospital = hospitalAdmin.AdminOfHospital.Hospital,
                    IsActive = true
                };
                var currentState = new CurrentState()
                {
                    Band = band
                };
                await _unitOfWork.BandRepository.AddAsync(band);
                await _unitOfWork.CurrentStateRepository.AddAsync(currentState);
                await _unitOfWork.CompleteAsync();
                var data = _mapper.Map<AddPrivateBandDto>(band);
                return new GeneralResponse<AddPrivateBandDto>
                {
                    IsSuccess = true,
                    Message = "New Private Band is added sucessfully.",
                    Data = data
                }; 
            }
            catch (Exception ex)
            {
                return new GeneralResponse<AddPrivateBandDto>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }

        public async Task<GeneralResponse<AddPublicBandDto>> AddPublicBand(string patientNationalId)
        {
            try
            {
                HttpContext httpContext = _httpContextAccessor.HttpContext;
                int userId = httpContext.FindFirst();
                var user = await _unitOfWork.UserRepository.SingleOrDefaultAsync(a => a.Id == userId && a.Role == User.HospitalAdmin);
                if (user == null)
                {
                    return new GeneralResponse<AddPublicBandDto>
                    {
                        IsSuccess = false,
                        Message = "No user Found!"
                    };
                }
                var hospitalAdmin = await _unitOfWork.HospitalAdminRepository.GetSingleWithIncludesAsync(
                    a => a.UserName == user.UserName, i => i.AdminOfHospital.Hospital);
                var patient = await _unitOfWork.PatientRepository.GetSingleWithIncludesAsync
                    (a => a.NationalId == patientNationalId, i=>i.UploadedFile);
                if (patient == null)
                {
                    return new GeneralResponse<AddPublicBandDto>
                    {
                        IsSuccess = false,
                        Message = "No patient found with this NationalId, please make sure that this user have an account on our webApplication with the same NationalId!"
                    };
                }
                if(await _unitOfWork.BandRepository.AnyAsync(a=>a.PatientId==patient.Id && a.Type == Band.Public))
                {
                    return new GeneralResponse<AddPublicBandDto>
                    {
                        IsSuccess = false,
                        Message = "This User already have a public Band."
                    };
                }
                var band = new Band()
                {
                    UniqueId = BandIdService.GenerateUniqueString(),
                    Type = Band.Public,
                    Hospital = hospitalAdmin.AdminOfHospital.Hospital,
                    IsActive = true,
                    Patient = patient,
                };
                var currentState = new CurrentState()
                {
                    Band = band
                };
                await _unitOfWork.BandRepository.AddAsync(band);
                await _unitOfWork.CurrentStateRepository.AddAsync(currentState);
                await _unitOfWork.CompleteAsync();
                var data = _mapper.Map<AddPublicBandDto>(band);
                return new GeneralResponse<AddPublicBandDto>
                {
                    IsSuccess = true,
                    Message = "New Private Band is added sucessfully.",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<AddPublicBandDto>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }
        public async Task<GeneralResponse<string>> DeletePrivateBand(int bandId)
        {
            try
            {
                HttpContext httpContext = _httpContextAccessor.HttpContext;
                int userId = httpContext.FindFirst();
                var user = await _unitOfWork.UserRepository.SingleOrDefaultAsync(a => a.Id == userId && a.Role == User.HospitalAdmin);
                if (user == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "No user Found!"
                    };
                }
                var hospitalAdmin = await _unitOfWork.HospitalAdminRepository.GetSingleWithIncludesAsync(
                    a => a.UserName == user.UserName, i => i.AdminOfHospital.Hospital);

                var band = await _unitOfWork.BandRepository
                    .GetSingleWithIncludesAsync(a => a.Id == bandId &&
                    a.Type==Band.Private &&
                    a.HospitalId == hospitalAdmin.AdminOfHospital.HospitalId,
                    i => i.CurrentState);
                if (band == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "No Private band belong to your hospital Found with this Id!"
                    };
                }
                _unitOfWork.BandRepository.Remove(band);
                _unitOfWork.CurrentStateRepository.Remove(band.CurrentState);
                await _unitOfWork.CompleteAsync();
                return new GeneralResponse<string>
                {
                    IsSuccess = true,
                    Message = "The Band is deleted sucessfully."
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

        public async Task<GeneralResponse<string>> DeletePublicBand(int bandId)
        {
            try
            {
                HttpContext httpContext = _httpContextAccessor.HttpContext;
                int userId = httpContext.FindFirst();
                var user = await _unitOfWork.UserRepository.SingleOrDefaultAsync(a => a.Id == userId && a.Role == User.HospitalAdmin);
                if (user == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "No HospitalAdmin Found!"
                    };
                }
                var hospitalAdmin = await _unitOfWork.HospitalAdminRepository.GetSingleWithIncludesAsync(
                    a => a.UserName == user.UserName, s => s.AdminOfHospital);
                
                var band = await _unitOfWork.BandRepository
                    .GetSingleWithIncludesAsync(a => a.Id == bandId &&
                    a.Type == Band.Public &&
                    a.HospitalId == hospitalAdmin.AdminOfHospital.HospitalId,
                    i => i.CurrentState);
                if (band == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "No Band belongs to you had been Found with this Id!"
                    };
                }
                _unitOfWork.CurrentStateRepository.Remove(band.CurrentState);
                _unitOfWork.BandRepository.Remove(band);
                await _unitOfWork.CompleteAsync();
                return new GeneralResponse<string>
                {
                    IsSuccess = true,
                    Message = "The Band is deleted sucessfully."
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


        public async Task<GeneralResponse<string>> BandAlarm(string uniqueId, bool bandAlarm)
        {
            try
            {
                var band = await _unitOfWork.BandRepository.GetSingleWithIncludesAsync(
                    u => u.UniqueId == uniqueId , a=>a.Patient);
                if (band == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "No band Found!"
                    };
                }
                if (bandAlarm == false)
                { 
                    band.IsActive = false; 
                }
                else 
                {
                    band.IsActive = true; 
                }

                if (band.IsActive == false)
                {
                    var email = band.Patient.Email;
                    string room;
                    if (band.RoomNum != null)
                    {
                        room = band.RoomNum.ToString();
                    }
                    else
                    {
                        room = "N/A";
                    }
                    
                    string patientData = "<p style=\"font-size: 16px;\">Patient  ' " + band.Patient.FullName + "'  is in Danger</p><br>" +
                     "<p style=\"font-size: 16px;\">Band Id: " + band.UniqueId + "</p><br>" +
                     "<p style=\"font-size: 16px;\">Room Num: " + room + "</p>";

                    if (!await MailServices.SendEmailAsync(email, "Band Alarm", patientData))
                    {
                        return new GeneralResponse<string>()
                        {
                            IsSuccess = false,
                            Message = "Sending the Mail is Failed"
                        };
                    }
                }
                _unitOfWork.BandRepository.Update(band);
                await _unitOfWork.CompleteAsync();
                return new GeneralResponse<string>
                {
                    IsSuccess = true,
                    Data = band.IsActive.ToString()
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

        

        public async Task<GeneralResponse<string>> BandSaved(int bandId)
        {
            try
            {
                HttpContext httpContext = _httpContextAccessor.HttpContext;
                int userId = httpContext.FindFirst();
                var user = await _unitOfWork.UserRepository.SingleOrDefaultAsync(a => a.Id == userId && a.Role == User.Doctor);
                if (user == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "No doctor Found!"
                    };
                }
                var doctor = await _unitOfWork.DoctorRepository.GetSingleWithIncludesAsync(
                    a => a.UserName == user.UserName);
                var doctorHospitals = await _unitOfWork.HospitalDoctorRepository.GetSpecificItems(a => a.DoctorId == doctor.Id, s => s.HospitalId);
                var hospitalIds = doctorHospitals.ToList();

                var band = await _unitOfWork.BandRepository.SingleOrDefaultAsync(a => a.Id == bandId);
                if (band == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "No band exist with this id!"
                    };
                }
                if (band.Type == Band.Private && !hospitalIds.Contains(band.HospitalId))
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "No Private band in the hospitals that you are working in had been found with this id!"
                    };
                }
                
                var savedBand = await _unitOfWork.SavedBandRepository.GetSingleWithIncludesAsync(s => s.BandId == bandId && s.DoctorId == doctor.Id);
                string data ;
                if(savedBand == null)
                {
                    var saveBand = new SavedBand()
                    {
                        Doctor = doctor,
                        Band = band
                    };
                    await _unitOfWork.SavedBandRepository.AddAsync(saveBand);
                    await _unitOfWork.CompleteAsync();
                    data = "Band Saved";
                }
                else
                {
                    _unitOfWork.SavedBandRepository.Remove(savedBand);
                    await _unitOfWork.CompleteAsync();
                    data = "Band UnSaved";
                }

                return new GeneralResponse<string>
                {
                    IsSuccess = true,
                    Data = data
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

        public async Task<GeneralResponse<BandResponseDto>> ChangePatientOfPrivateBand(int bandId, [FromForm]ChangeBandPatientDto dto)
        {
            try
            {
                HttpContext httpContext = _httpContextAccessor.HttpContext;
                int userId = httpContext.FindFirst();
                var user = await _unitOfWork.UserRepository.SingleOrDefaultAsync(a => a.Id == userId && a.Role == User.HospitalAdmin);
                if (user == null)
                {
                    return new GeneralResponse<BandResponseDto>
                    {
                        IsSuccess = false,
                        Message = "No HospitalAdmin Found!"
                    };
                }
                var hospitalAdmin = await _unitOfWork.HospitalAdminRepository.GetSingleWithIncludesAsync(
                    a => a.UserName == user.UserName, i => i.AdminOfHospital.Hospital);

                var band = await _unitOfWork.BandRepository.SingleOrDefaultAsync(a => a.Id == bandId && a.Type == Band.Private && a.HospitalId == hospitalAdmin.AdminOfHospital.HospitalId);
                if (band == null)
                {
                    return new GeneralResponse<BandResponseDto>
                    {
                        IsSuccess = false,
                        Message = "No Private band belong to your hospital Found!"
                    };
                }
                var patient = await _unitOfWork.PatientRepository.GetSingleWithIncludesAsync(s => s.NationalId == dto.PatientNationalId, i => i.UploadedFile);
                if (patient == null)
                {
                    return new GeneralResponse<BandResponseDto>
                    {
                        IsSuccess = false,
                        Message = "No patient Found with this nationalId!"
                    };
                }
                if(await _unitOfWork.BandRepository.AnyAsync(
                    a=>a.Type == Band.Private &&
                    a.PatientId == patient.Id &&
                    a.HospitalId == band.HospitalId)) 
                {
                    return new GeneralResponse<BandResponseDto>
                    {
                        IsSuccess = false,
                        Message = "This patient is already have another private band from this hospital!"
                    };
                }
                band.Patient = patient;
                band.RoomNum = dto.RoomNum ?? null;
                _unitOfWork.BandRepository.Update(band);
                await _unitOfWork.CompleteAsync();
                var data = _mapper.Map<BandResponseDto>(band);
                return new GeneralResponse<BandResponseDto>
                {
                    IsSuccess = true,
                    Message= "The patient is changed successfully",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<BandResponseDto>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }

        public async Task<GeneralResponse<List<BandResponseDto>>> GetHospitalPrivateBands(int hospitalId)
        {
            try
            {
                HttpContext httpContext = _httpContextAccessor.HttpContext;
                int userId = httpContext.FindFirst();
                var user = await _unitOfWork.UserRepository.SingleOrDefaultAsync(a => a.Id == userId);
                if (user == null)
                {
                    return new GeneralResponse<List<BandResponseDto>>
                    {
                        IsSuccess = false,
                        Message = "No user Found!"
                    };
                }
                
                var hospital = await _unitOfWork.HospitalRepository.GetByIdAsync(hospitalId);
                if(hospital == null)
                {
                    return new GeneralResponse<List<BandResponseDto>>
                    {
                        IsSuccess = false,
                        Message = "No hospital Found!"
                    };
                }
                if (user.Role == User.HospitalAdmin)
                {
                    var hospitalAdmin = await _unitOfWork.HospitalAdminRepository.SingleOrDefaultAsync(s => s.UserName == user.UserName);
                    var CheckHospitalAdmin = await _unitOfWork.AdminOfHospitalRepository.SingleOrDefaultAsync(
                        s => s.HospitalAdminId == hospitalAdmin.Id && s.HospitalId == hospitalId);
                    if (CheckHospitalAdmin == null)
                    {
                        return new GeneralResponse<List<BandResponseDto>>
                        {
                            IsSuccess = false,
                            Message = "You donot have the permission to see this data!"
                        };
                    }
                }
                if(user.Role == User.Doctor)
                {
                    var doctor = await _unitOfWork.DoctorRepository.GetSingleWithIncludesAsync(
                    a => a.UserName == user.UserName);
                    if (!await _unitOfWork.HospitalDoctorRepository.AnyAsync
                    (a => a.HospitalId == hospitalId && a.DoctorId == doctor.Id))
                    {
                        return new GeneralResponse<List<BandResponseDto>>
                        {
                            IsSuccess = false,
                            Message = "You donot have the permission to see this data!"
                        };
                    }
                }
                if (user.Role == User.Patient)
                {
                    return new GeneralResponse<List<BandResponseDto>>
                    {
                        IsSuccess = false,
                        Message = "You donot have the permissions to access this band informations!"
                    };
                }

                var bands = await _unitOfWork.BandRepository.WhereIncludeAsync(
                    w => w.HospitalId == hospitalId && w.Type == Band.Private,
                    i => i.Patient.UploadedFile);
                var data = _mapper.Map<List<BandResponseDto>>(bands);

                return new GeneralResponse<List<BandResponseDto>>
                {
                    IsSuccess = true,
                    Message = "Private Bands Listed successfully",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<List<BandResponseDto>>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }

        public async Task<GeneralResponse<BandResponseDto>> GetPatientBand()
        {
            try
            {
                HttpContext httpContext = _httpContextAccessor.HttpContext;
                int userId = httpContext.FindFirst();
                var user = await _unitOfWork.UserRepository.SingleOrDefaultAsync(a => a.Id == userId && a.Role == User.Patient);
                if (user == null)
                {
                    return new GeneralResponse<BandResponseDto>
                    {
                        IsSuccess = false,
                        Message = "No Patient Found!"
                    };
                }
                var patient = await _unitOfWork.PatientRepository.GetSingleWithIncludesAsync(
                    a => a.UserName == user.UserName);

                var band = await _unitOfWork.BandRepository.GetSingleWithIncludesAsync(
                    a => a.PatientId == patient.Id && a.Type == Band.Public,
                    i => i.Patient.UploadedFile);
                if (band == null)
                {
                    return new GeneralResponse<BandResponseDto>
                    {
                        IsSuccess = false,
                        Message = "No band belongs to you had been Found!"
                    };
                }
                var data = _mapper.Map<BandResponseDto>(band);
                return new GeneralResponse<BandResponseDto>
                {
                    IsSuccess = true,
                    Message = "Band has been found successfully",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<BandResponseDto>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }

        public async Task<GeneralResponse<List<BandResponseDto>>> GetPublicBands()
        {
            try
            {
                var bands = await _unitOfWork.BandRepository.WhereIncludeAsync(
                    w => w.Type == Band.Public,
                    i => i.Patient,
                    i => i.Patient.UploadedFile);
                var data = _mapper.Map<List<BandResponseDto>>(bands);

                return new GeneralResponse<List<BandResponseDto>>
                {
                    IsSuccess = true,
                    Message = "Public Bands Listed successfully",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<List<BandResponseDto>>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }

        public async Task<GeneralResponse<List<BandResponseDto>>> GetSavedPrivateBands(int hospitalId)
        {
            try
            {
                HttpContext httpContext = _httpContextAccessor.HttpContext;
                int userId = httpContext.FindFirst();
                var user = await _unitOfWork.UserRepository.SingleOrDefaultAsync(a => a.Id == userId && a.Role == User.Doctor);
                if (user == null)
                {
                    return new GeneralResponse<List<BandResponseDto>>
                    {
                        IsSuccess = false,
                        Message = "No doctor Found!"
                    };
                }
                var hospital = await _unitOfWork.HospitalRepository.GetByIdAsync(hospitalId);
                if (hospital == null)
                {
                    return new GeneralResponse<List<BandResponseDto>>
                    {
                        IsSuccess = false,
                        Message = "No hospital Found!"
                    };
                }
                var doctor = await _unitOfWork.DoctorRepository.GetSingleWithIncludesAsync(
                    a => a.UserName == user.UserName);
                if (!await _unitOfWork.HospitalDoctorRepository.AnyAsync
                    (a => a.HospitalId == hospitalId && a.DoctorId == doctor.Id))
                {
                    return new GeneralResponse<List<BandResponseDto>>
                    {
                        IsSuccess = false,
                        Message = "This doctor isnot working in this hospital!"
                    };
                }
                var savedBands = await _unitOfWork.SavedBandRepository.WhereIncludeAsync(
                    w => w.DoctorId == doctor.Id &&
                    w.Band.Type == Band.Private &&
                    w.Band.HospitalId == hospitalId,
                    a => a.Band.Patient.UploadedFile);

                var bands = savedBands.Select(s => s.Band);
                var data = _mapper.Map<List<BandResponseDto>>(bands);

                return new GeneralResponse<List<BandResponseDto>>
                {
                    IsSuccess = true,
                    Message = "Saved Private Bands Listed successfully",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<List<BandResponseDto>>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }

        public async Task<GeneralResponse<List<BandResponseDto>>> GetSavedPublicBands()
        {
            try
            {
                HttpContext httpContext = _httpContextAccessor.HttpContext;
                int userId = httpContext.FindFirst();
                var user = await _unitOfWork.UserRepository.SingleOrDefaultAsync(a => a.Id == userId && a.Role == User.Doctor);
                if (user == null)
                {
                    return new GeneralResponse<List<BandResponseDto>>
                    {
                        IsSuccess = false,
                        Message = "No doctor Found!"
                    };
                }
                var doctor = await _unitOfWork.DoctorRepository.GetSingleWithIncludesAsync(
                    a => a.UserName == user.UserName);
                
                var savedBands = await _unitOfWork.SavedBandRepository.WhereIncludeAsync(
                    w => w.DoctorId == doctor.Id &&
                    w.Band.Type == Band.Public,
                    a => a.Band.Patient.UploadedFile);

                var bands = savedBands.Select(s => s.Band);
                var data = _mapper.Map<List<BandResponseDto>>(bands);

                return new GeneralResponse<List<BandResponseDto>>
                {
                    IsSuccess = true,
                    Message = "Saved Private Bands Listed successfully",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<List<BandResponseDto>>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }

        }
        
        
        
        public async Task<GeneralResponse<BandResponseDto>> GetPublicBandByUniqueId(string uniqueId)
        {
            try
            {
                var band = await _unitOfWork.BandRepository.GetSingleWithIncludesAsync
                    (s => s.UniqueId == uniqueId && s.Type == Band.Public, a => a.Patient.UploadedFile);
                if(band == null)
                {
                    return new GeneralResponse<BandResponseDto>
                    {
                        IsSuccess = false,
                        Message = "No Public Band exist with this Id!"
                    };
                }
                var data = _mapper.Map<BandResponseDto>(band);
                return new GeneralResponse<BandResponseDto>
                {
                    IsSuccess = true,
                    Message = "Band has been found successfully",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<BandResponseDto>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }

        public async Task<GeneralResponse<BandResponseDto>> GetPrivateBandByUniqueId(string uniqueId)
        {
            try
            {
                HttpContext httpContext = _httpContextAccessor.HttpContext;
                int userId = httpContext.FindFirst();
                var user = await _unitOfWork.UserRepository.SingleOrDefaultAsync(a => a.Id == userId);
                if (user == null)
                {
                    return new GeneralResponse<BandResponseDto>
                    {
                        IsSuccess = false,
                        Message = "No user Found!"
                    };
                }

                var band = await _unitOfWork.BandRepository.GetSingleWithIncludesAsync
                    (s => s.UniqueId == uniqueId && s.Type == Band.Private, a => a.Patient.UploadedFile);
                if (band == null)
                {
                    return new GeneralResponse<BandResponseDto>
                    {
                        IsSuccess = false,
                        Message = "No Private Band exist with this Id!"
                    };
                }

                if (user.Role == User.Doctor)
                {
                    var doctor = await _unitOfWork.DoctorRepository.SingleOrDefaultAsync(s => s.UserName == user.UserName);
                    var doctorHospitals = await _unitOfWork.HospitalDoctorRepository
                        .GetSpecificItems(a => a.DoctorId == doctor.Id, s => s.HospitalId);
                    var hospitalIds = doctorHospitals.ToList();
                    if (!hospitalIds.Contains(band.HospitalId))
                    {
                        return new GeneralResponse<BandResponseDto>
                        {
                            IsSuccess = false,
                            Message = "You donot have the permission to access this band informations!"
                        };
                    }
                }
                if(user.Role == User.HospitalAdmin)
                {
                    var hospitalAdmin = await _unitOfWork.HospitalAdminRepository
                        .GetSingleWithIncludesAsync(s => s.UserName == user.UserName, a => a.AdminOfHospital);
                    if (band.HospitalId != hospitalAdmin.AdminOfHospital.HospitalId)
                    {
                        return new GeneralResponse<BandResponseDto>
                        {
                            IsSuccess = false,
                            Message = "You donot have the permissions to access this band informations!"
                        };
                    }
                }
                if (user.Role == User.Patient)
                {
                    return new GeneralResponse<BandResponseDto>
                    {
                        IsSuccess = false,
                        Message = "You donot have the permissions to access this band informations!"
                    };
                }

                var data = _mapper.Map<BandResponseDto>(band);
                return new GeneralResponse<BandResponseDto>
                {
                    IsSuccess = true,
                    Message = "Band has been found successfully",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<BandResponseDto>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }

        public async Task<GeneralResponse<string>> ChangePatientFromBand(string uniqueId, ChangeBandPatientDto dto)
        {
            try
            {
                
                var band = await _unitOfWork.BandRepository.SingleOrDefaultAsync(a => a.UniqueId == uniqueId && a.Type == Band.Private);
                if (band == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "No Private band Found!"
                    };
                }
                var patient = await _unitOfWork.PatientRepository.GetSingleWithIncludesAsync(s => s.NationalId == dto.PatientNationalId);
                if (patient == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "No patient Found with this nationalId!"
                    };
                }
                if (await _unitOfWork.BandRepository.AnyAsync(
                    a => a.Type == Band.Private &&
                    a.PatientId == patient.Id &&
                    a.HospitalId == band.HospitalId))
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "This patient is already have another private band from this hospital!"
                    };
                }
                band.Patient = patient;
                band.RoomNum = dto.RoomNum ?? null;
                _unitOfWork.BandRepository.Update(band);
                await _unitOfWork.CompleteAsync();
                var user = await _unitOfWork.CivilRegestrationRepository.SingleOrDefaultAsync(s => s.NationalId == dto.PatientNationalId);
                var data = user.Name;
                return new GeneralResponse<string>
                {
                    IsSuccess = true,
                    Message = "The patient is changed successfully",
                    Data = data
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

        public async Task<GeneralResponse<string>> FlagStatus(string uniqueId)
        {
            try
            {

                var band = await _unitOfWork.BandRepository.SingleOrDefaultAsync(a => a.UniqueId == uniqueId);
                if (band == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "No band Found!"
                    };
                }

                return new GeneralResponse<string>
                {
                    IsSuccess = true,
                    Data = band.Flag.ToString()
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

        public async Task<GeneralResponse<string>> ChangeFlag(string uniqueId)
        {
            try
            {

                var band = await _unitOfWork.BandRepository.SingleOrDefaultAsync(a => a.UniqueId == uniqueId);
                if (band == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "No band Found!"
                    };
                }
                band.Flag = true;
                _unitOfWork.BandRepository.Update(band);
                await _unitOfWork.CompleteAsync();
                return new GeneralResponse<string>
                {
                    IsSuccess = true,
                    Message = "Flag is true now"
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

        public async Task<GeneralResponse<CurrentStateDto>> BandCurrentState(string uniqueId, BandStateDto dto)
        {
            try
            {

                var band = await _unitOfWork.BandRepository.SingleOrDefaultAsync(s => s.UniqueId == uniqueId && s.Flag == true);
                if (band == null)
                {
                    return new GeneralResponse<CurrentStateDto>
                    {
                        IsSuccess = false,
                        Message = "No Band Found with this Id"
                    };
                }
                var currentState = await _unitOfWork.CurrentStateRepository.GetByIdAsync(band.CurrentStateId);
                currentState.Temperature = dto.Temperature;
                currentState.BloodPressure = dto.BloodPressure;
                currentState.BloodSugar = dto.BloodSugar;
                currentState.HeartRate = dto.HeartRate;
                currentState.Oxygen = dto.Oxygen;
                _unitOfWork.CurrentStateRepository.Update(currentState);
                await _unitOfWork.CompleteAsync();

                band.Flag = false;
                _unitOfWork.BandRepository.Update(band);
                await _unitOfWork.CompleteAsync();

                var data = _mapper.Map<CurrentStateDto>(dto);
                data.Id = currentState.Id;

                return new GeneralResponse<CurrentStateDto>
                {
                    IsSuccess = true,
                    Message = "Current State has been send successfully",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<CurrentStateDto>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }

        public async Task<GeneralResponse<CurrentStateDto>> GetBandCurrentState(string uniqueId)
        {
            try
            {

                var band = await _unitOfWork.BandRepository.SingleOrDefaultAsync(s => s.UniqueId == uniqueId);
                if (band == null)
                {
                    return new GeneralResponse<CurrentStateDto>
                    {
                        IsSuccess = false,
                        Message = "No Band Found with this Id"
                    };
                }
                var currentState = await _unitOfWork.CurrentStateRepository.GetByIdAsync(band.CurrentStateId);

                var data = _mapper.Map<CurrentStateDto>(currentState);

                return new GeneralResponse<CurrentStateDto>
                {
                    IsSuccess = true,
                    Message = "Current State has been displayed successfully",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<CurrentStateDto>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }


    }

}
