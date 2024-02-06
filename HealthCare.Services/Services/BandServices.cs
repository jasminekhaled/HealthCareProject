using AutoMapper;
using HealthCare.Core.DTOS;
using HealthCare.Core.DTOS.AppointmentModule.ResponseDto;
using HealthCare.Core.DTOS.BandModule.RequestDtos;
using HealthCare.Core.DTOS.BandModule.ResponseDtos;
using HealthCare.Core.Helpers;
using HealthCare.Core.IRepositories;
using HealthCare.Core.Models.AppointmentModule;
using HealthCare.Core.Models.BandModule;
using HealthCare.Core.Models.DoctorModule;
using HealthCare.Core.Models.HospitalModule;
using HealthCare.Core.Models.PatientModule;
using HealthCare.Services.IServices;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Services.Services
{
    public class BandServices : IBandServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BandServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GeneralResponse<AddPrivateBandDto>> AddPrivateBand(string token)
        {
            try
            {
                var userId = TokenServices.ExtractUserIdFromToken(token).Data.userId;
                var user = await _unitOfWork.UserRepository.SingleOrDefaultAsync(a => a.Id == userId && a.RoleId == 2);
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
                    IsActive = false
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

        public async Task<GeneralResponse<AddPublicBandDto>> AddPublicBand(string token, string patientNationalId)
        {
            try
            {
                var userId = TokenServices.ExtractUserIdFromToken(token).Data.userId;
                var user = await _unitOfWork.UserRepository.SingleOrDefaultAsync(a => a.Id == userId && a.RoleId == 2);
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
                    IsActive = false,
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
        public async Task<GeneralResponse<string>> DeletePrivateBand(string token, int bandId)
        {
            try
            {
                var userId = TokenServices.ExtractUserIdFromToken(token).Data.userId;
                var user = await _unitOfWork.UserRepository.SingleOrDefaultAsync(a => a.Id == userId && a.RoleId == 2);
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

                var band = await _unitOfWork.BandRepository.SingleOrDefaultAsync(a => a.Id == bandId && a.Type==Band.Private && a.HospitalId == hospitalAdmin.AdminOfHospital.HospitalId);
                if (band == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "No Private band belong to your hospital Found!"
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

        public async Task<GeneralResponse<string>> DeletePublicBand(string token, int bandId)
        {
            try
            {
                var userId = TokenServices.ExtractUserIdFromToken(token).Data.userId;
                var user = await _unitOfWork.UserRepository.SingleOrDefaultAsync(a => a.Id == userId && a.RoleId == 3);
                if (user == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "No user Found!"
                    };
                }
                var patient = await _unitOfWork.PatientRepository.GetSingleWithIncludesAsync(
                    a => a.UserName == user.UserName);

                var band = await _unitOfWork.BandRepository.SingleOrDefaultAsync(a => a.Id == bandId && a.Type == Band.Public && a.Patient == patient);
                if (band == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "No Band belongs to you had been Found!"
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


        public async Task<GeneralResponse<string>> PrivateBandActivation(string token, int bandId)
        {
            try
            {
                var userId = TokenServices.ExtractUserIdFromToken(token).Data.userId;
                var user = await _unitOfWork.UserRepository.SingleOrDefaultAsync(a => a.Id == userId && a.RoleId == 2);
                if (user == null)
                {
                    return new GeneralResponse<string>
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
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "No Private band belong to your hospital Found!"
                    };
                }
                if (band.IsActive) { band.IsActive = false; }
                else { band.IsActive = true; }

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

        public async Task<GeneralResponse<string>> PublicBandActivation(string token, int bandId)
        {
            try
            {
                var userId = TokenServices.ExtractUserIdFromToken(token).Data.userId;
                var user = await _unitOfWork.UserRepository.SingleOrDefaultAsync(a => a.Id == userId && a.RoleId == 3);
                if (user == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "No Patient Found!"
                    };
                }
                var patient = await _unitOfWork.PatientRepository.GetSingleWithIncludesAsync(
                    a => a.UserName == user.UserName);

                var band = await _unitOfWork.BandRepository.SingleOrDefaultAsync(a => a.Id == bandId && a.Type == Band.Public && a.Patient == patient);
                if (band == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "No band belongs to you had been Found!"
                    };
                }
                if (band.IsActive) { band.IsActive = false; }
                else { band.IsActive = true; }

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

        public async Task<GeneralResponse<string>> BandSaved(string token, int bandId)
        {
            try
            {
                var userId = TokenServices.ExtractUserIdFromToken(token).Data.userId;
                var user = await _unitOfWork.UserRepository.SingleOrDefaultAsync(a => a.Id == userId && a.RoleId == 4);
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

                var band = await _unitOfWork.BandRepository.SingleOrDefaultAsync(a => a.Id == bandId);
                if (band == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "No Band Found!"
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

        public async Task<GeneralResponse<BandResponseDto>> ChangePatientOfPrivateBand(string token, int bandId, ChangeBandPatientDto dto)
        {
            try
            {
                var userId = TokenServices.ExtractUserIdFromToken(token).Data.userId;
                var user = await _unitOfWork.UserRepository.SingleOrDefaultAsync(a => a.Id == userId && a.RoleId == 2);
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

        public async Task<GeneralResponse<List<BandResponseDto>>> GetHospitalPrivateBands(string token, int hospitalId)
        {
            try
            {
                var userId = TokenServices.ExtractUserIdFromToken(token).Data.userId;
                var user = await _unitOfWork.UserRepository.SingleOrDefaultAsync(a => a.Id == userId && a.RoleId == 4);
                if (user == null)
                {
                    return new GeneralResponse<List<BandResponseDto>>
                    {
                        IsSuccess = false,
                        Message = "No doctor Found!"
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
                var doctor = await _unitOfWork.DoctorRepository.GetSingleWithIncludesAsync(
                    a => a.UserName == user.UserName);
                if(!await _unitOfWork.HospitalDoctorRepository.AnyAsync
                    (a=>a.HospitalId==hospitalId && a.DoctorId == doctor.Id))
                {
                    return new GeneralResponse<List<BandResponseDto>>
                    {
                        IsSuccess = false,
                        Message = "This doctor isnot working in this hospital!"
                    };
                }
                var bands = await _unitOfWork.BandRepository.WhereIncludeAsync(
                    w => w.HospitalId == hospitalId && w.Type == Band.Private,
                    i => i.Patient);
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

        public async Task<GeneralResponse<BandResponseDto>> GetPatientBand(string token)
        {
            try
            {
                var userId = TokenServices.ExtractUserIdFromToken(token).Data.userId;
                var user = await _unitOfWork.UserRepository.SingleOrDefaultAsync(a => a.Id == userId && a.RoleId == 3);
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

                var band = await _unitOfWork.BandRepository.SingleOrDefaultAsync(
                    a => a.PatientId == patient.Id && a.Type == Band.Public);
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
                    i => i.Patient);
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

        public async Task<GeneralResponse<List<BandResponseDto>>> GetSavedPrivateBands(string token, int hospitalId)
        {
            try
            {
                var userId = TokenServices.ExtractUserIdFromToken(token).Data.userId;
                var user = await _unitOfWork.UserRepository.SingleOrDefaultAsync(a => a.Id == userId && a.RoleId == 4);
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
                    a => a.Band.Patient);

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

        public async Task<GeneralResponse<List<BandResponseDto>>> GetSavedPublicBands(string token)
        {
            try
            {
                var userId = TokenServices.ExtractUserIdFromToken(token).Data.userId;
                var user = await _unitOfWork.UserRepository.SingleOrDefaultAsync(a => a.Id == userId && a.RoleId == 4);
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
                    a => a.Band.Patient);

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
        
        public async Task<GeneralResponse<CurrentStateDto>> BandCurrentState(string token, int bandId)
        {
            try
            {
                var userId = TokenServices.ExtractUserIdFromToken(token).Data.userId;
                var user = await _unitOfWork.UserRepository.SingleOrDefaultAsync(a => a.Id == userId);
                if (user == null)
                {
                    return new GeneralResponse<CurrentStateDto>
                    {
                        IsSuccess = false,
                        Message = "No user Found!"
                    };
                }
                CurrentStateDto data = null;
                if(user.RoleId == 3)
                {
                    var patient = await _unitOfWork.PatientRepository.SingleOrDefaultAsync(s => s.UserName == user.UserName);
                    var band = await _unitOfWork.BandRepository.GetSingleWithIncludesAsync(
                        s => s.Id == bandId && s.Type == Band.Public && s.PatientId == patient.Id);
                    if(band == null)
                    {
                        return new GeneralResponse<CurrentStateDto>
                        {
                            IsSuccess = false,
                            Message = "No Puplic Band belong to you had been Found!"
                        };
                    }
                    data = _mapper.Map<CurrentStateDto>(band.CurrentState);
                }

                if(user.RoleId == 4)
                {
                    var doctor = await _unitOfWork.DoctorRepository.SingleOrDefaultAsync(s => s.UserName == user.UserName);
                    var doctorHospitals = await _unitOfWork.HospitalDoctorRepository.GetSpecificItems(a => a.DoctorId == doctor.Id, s=>s.HospitalId);
                    var hospitalIds = doctorHospitals.ToList();
                    var band = await _unitOfWork.BandRepository.SingleOrDefaultAsync(a => a.Id == bandId);
                    if(band==null)
                    {
                        return new GeneralResponse<CurrentStateDto>
                        {
                            IsSuccess = false,
                            Message = "No band Found!"
                        };
                    }
                    if(band.Type == Band.Private && !hospitalIds.Contains(band.HospitalId))
                    {
                        return new GeneralResponse<CurrentStateDto>
                        {
                            IsSuccess = false,
                            Message = "You donot have the permissions to access this band informations!"
                        };
                    }
                    data = _mapper.Map<CurrentStateDto>(band.CurrentState);
                }

                if(user.RoleId == 2)
                {
                    var hospitalAdmin = await _unitOfWork.HospitalAdminRepository.GetSingleWithIncludesAsync(s => s.UserName == user.UserName, a=>a.AdminOfHospital);
                    var band = await _unitOfWork.BandRepository.SingleOrDefaultAsync(a => a.Id == bandId);
                    if (band == null)
                    {
                        return new GeneralResponse<CurrentStateDto>
                        {
                            IsSuccess = false,
                            Message = "No band Found!"
                        };
                    }
                    if (band.Type == Band.Private && band.HospitalId != hospitalAdmin.AdminOfHospital.HospitalId)
                    {
                        return new GeneralResponse<CurrentStateDto>
                        {
                            IsSuccess = false,
                            Message = "You donot have the permissions to access this band informations!"
                        };
                    }
                    data = _mapper.Map<CurrentStateDto>(band.CurrentState);
                }
               

                return new GeneralResponse<CurrentStateDto>
                {
                    IsSuccess = true,
                    Message = "Saved Private Bands Listed successfully",
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

        public async Task<GeneralResponse<BandResponseDto>> GetPublicBandByUniqueId(string uniqueId)
        {
            try
            {
                var band = await _unitOfWork.BandRepository.GetSingleWithIncludesAsync
                    (s => s.UniqueId == uniqueId && s.Type == Band.Public, a => a.Patient);
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

        public async Task<GeneralResponse<BandResponseDto>> GetPrivateBandByUniqueId(string token, string uniqueId)
        {
            try
            {
                var userId = TokenServices.ExtractUserIdFromToken(token).Data.userId;
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
                    (s => s.UniqueId == uniqueId && s.Type == Band.Private, a => a.Patient);
                if (band == null)
                {
                    return new GeneralResponse<BandResponseDto>
                    {
                        IsSuccess = false,
                        Message = "No Private Band exist with this Id!"
                    };
                }

                if (user.RoleId == 4)
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
                if(user.RoleId == 2)
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
    }
}
