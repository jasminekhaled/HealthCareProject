using AutoMapper;
using HealthCare.Core.DTOS;
using HealthCare.Core.DTOS.AuthModule.ResponseDtos;
using HealthCare.Core.DTOS.PatientModule.RequestDto;
using HealthCare.Core.DTOS.PatientModule.ResponseDto;
using HealthCare.Core.IRepositories;
using HealthCare.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Services.Services
{
    public class PatientServices : IPatientServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PatientServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        
        
        public async Task<GeneralResponse<List<PatientDto>>> ListOfPatients()
        {
            try
            {
                var patients = await _unitOfWork.PatientRepository.Where(w => w.IsEmailConfirmed == true);
                var data = _mapper.Map<List<PatientDto>>(patients);

                return new GeneralResponse<List<PatientDto>>
                {
                    IsSuccess = true,
                    Message = "Patients Listed Successfully",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<List<PatientDto>>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }

        public async Task<GeneralResponse<PatientDto>> GetPatientByNationalId(string nationalId)
        {
            try
            {
                var patient = await _unitOfWork.PatientRepository.SingleOrDefaultAsync(s => s.NationalId == nationalId && s.IsEmailConfirmed == true);
                if(patient == null)
                {
                    return new GeneralResponse<PatientDto>
                    {
                        IsSuccess = false,
                        Message = "No Patient was found!"
                    };
                }
                var data = _mapper.Map<PatientDto>(patient);

                return new GeneralResponse<PatientDto>
                {
                    IsSuccess = true,
                    Message = "The patient was found successfully",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<PatientDto>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }


        public async Task<GeneralResponse<PatientDto>> GetPatientByUserName(string userName)
        {
            try
            {
                var patient = await _unitOfWork.PatientRepository.SingleOrDefaultAsync(s => s.UserName == userName && s.IsEmailConfirmed == true);
                if (patient == null)
                {
                    return new GeneralResponse<PatientDto>
                    {
                        IsSuccess = false,
                        Message = "No Patient was found!"
                    };
                }
                var data = _mapper.Map<PatientDto>(patient);

                return new GeneralResponse<PatientDto>
                {
                    IsSuccess = true,
                    Message = "The patient was found successfully",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<PatientDto>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }

        public async Task<GeneralResponse<string>> DeletePatient(int patientId)
        {
            try
            {
                var patient = await _unitOfWork.PatientRepository.SingleOrDefaultAsync(s => s.Id == patientId && s.IsEmailConfirmed == true);
                if (patient == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "No Patient had been found!"
                    };
                }
                var user = await _unitOfWork.UserRepository.SingleOrDefaultAsync(s => s.UserName == patient.UserName);
                _unitOfWork.UserRepository.Remove(user);
                _unitOfWork.PatientRepository.Remove(patient);
                await _unitOfWork.CompleteAsync();


                return new GeneralResponse<string>
                {
                    IsSuccess = true,
                    Message = "Patient deleted Successfully"
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
        public async Task<GeneralResponse<PatientDto>> PatientDetails(int PatientId)
        {
            try
            {
                var patient = await _unitOfWork.PatientRepository.SingleOrDefaultAsync(s => s.Id == PatientId && s.IsEmailConfirmed == true);
                if (patient == null)
                {
                    return new GeneralResponse<PatientDto>
                    {
                        IsSuccess = false,
                        Message = "No Patient was found!"
                    };
                }
                var data = _mapper.Map<PatientDto>(patient);

                return new GeneralResponse<PatientDto>
                {
                    IsSuccess = true,
                    Message = "The patient details is showed successfully",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<PatientDto>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }
        public async Task<GeneralResponse<PatientDto>> EditPatient(int PatientId, [FromForm]EditPatientDto dto)
        {
            try
            {
                var patient = await _unitOfWork.PatientRepository.SingleOrDefaultAsync(s => s.Id == PatientId && s.IsEmailConfirmed == true);
                if (patient == null)
                {
                    return new GeneralResponse<PatientDto>
                    {
                        IsSuccess = false,
                        Message = "No Patient was found!"
                    };
                }

                if(dto.UserName != null)
                {
                    if(await _unitOfWork.UserRepository.AnyAsync(a => a.UserName == dto.UserName) 
                    || await _unitOfWork.PatientRepository.AnyAsync(n => n.UserName == dto.UserName))
                    {
                        return new GeneralResponse<PatientDto>
                        {
                            IsSuccess = false,
                            Message = "This UserName is already Exists"
                        };
                    }
                }

                
                if (dto.PhoneNumber != null && !dto.PhoneNumber.All(char.IsDigit))
                {
                        return new GeneralResponse<PatientDto>
                        {
                            IsSuccess = false,
                            Message = "Wrong Phone Number"
                        };
                }

                var user = await _unitOfWork.UserRepository.SingleOrDefaultAsync(s => s.UserName == patient.UserName);
                user.UserName = dto.UserName ?? user.UserName;
                patient.UserName = dto.UserName ?? patient.UserName;
                patient.PhoneNumber = dto.PhoneNumber ?? patient.PhoneNumber;
                _unitOfWork.UserRepository.Update(user);
                _unitOfWork.PatientRepository.Update(patient);
                await _unitOfWork.CompleteAsync();
                

                var data = _mapper.Map<PatientDto>(patient);

                return new GeneralResponse<PatientDto>
                {
                    IsSuccess = true,
                    Message = "The patient details is showed successfully",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<PatientDto>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }

        
    }
}
