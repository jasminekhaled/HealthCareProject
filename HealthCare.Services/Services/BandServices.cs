using AutoMapper;
using HealthCare.Core.DTOS;
using HealthCare.Core.DTOS.AppointmentModule.ResponseDto;
using HealthCare.Core.DTOS.BandModule.ResponseDtos;
using HealthCare.Core.Helpers;
using HealthCare.Core.IRepositories;
using HealthCare.Core.Models.AppointmentModule;
using HealthCare.Core.Models.BandModule;
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


        public Task<GeneralResponse<string>> BandActivation(int bandId)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<string>> BandDeactivation(int bandId)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<string>> BandSaved(int doctorId, int bandId)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<string>> BandUnsaved(int doctorId, int bandId)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<BandResponseDto>> ChangePatientOfPrivateBand(int bandId, int patientNatinalId)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<List<BandResponseDto>>> GetHospitalPrivateBands(int hospitalId)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<BandResponseDto>> GetPatientBand(int PatientId)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<List<BandResponseDto>>> GetPublicBands()
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<List<BandResponseDto>>> GetSavedPrivateBands(int doctorId, int hospitalId)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<List<BandResponseDto>>> GetSavedPublicBands(int doctorId)
        {
            throw new NotImplementedException();
        }
        
        public Task<GeneralResponse<CurrentStateDto>> BandCurrentState(int bandId)
        {
            throw new NotImplementedException();
        }
    }
}
