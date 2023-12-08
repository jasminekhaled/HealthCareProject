using AutoMapper;
using HealthCare.Core.DTOS;
using HealthCare.Core.DTOS.AuthModule.RequestDtos;
using HealthCare.Core.DTOS.DoctorModule.RequestDtos;
using HealthCare.Core.DTOS.DoctorModule.ResponseDtos;
using HealthCare.Core.DTOS.HospitalModule.ResponseDto;
using HealthCare.Core.Helpers;
using HealthCare.Core.IRepositories;
using HealthCare.Core.Models;
using HealthCare.Core.Models.AuthModule;
using HealthCare.Core.Models.DoctorModule;
using HealthCare.Core.Models.HospitalModule;
using HealthCare.Services.IServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace HealthCare.Services.Services
{
    public class DoctorServices : IDoctorServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;

        public DoctorServices(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
        }


        public async Task<GeneralResponse<string>> AddSpecialization(string Name)
        {
            try
            {
                var specialization = new Specialization()
                {
                    Name = Name
                };
                await _unitOfWork.SpecializationRepository.AddAsync(specialization);
                await _unitOfWork.CompleteAsync();

                return new GeneralResponse<string>
                {
                    IsSuccess = true,
                    Message = "New Specialization added successfully."
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
        public async Task<GeneralResponse<string>> DeleteSpecialization(int specializationId)
        {
            try
            {
                var specialization = await _unitOfWork.SpecializationRepository.GetByIdAsync(specializationId);
                if(specialization == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "Specialization Not Found!"
                    };
                }

                _unitOfWork.SpecializationRepository.Remove(specialization);
                await _unitOfWork.CompleteAsync();

                return new GeneralResponse<string>
                {
                    IsSuccess = true,
                    Message = "The Specialization Deleted successfully."
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

        public async Task<GeneralResponse<List<SpecializationDto>>> ListOfSpecialization()
        {
            try
            {
                var specializations = await _unitOfWork.SpecializationRepository.GetAllAsync();
                if (specializations == null)
                {
                    return new GeneralResponse<List<SpecializationDto>>
                    {
                        IsSuccess = false,
                        Message = "No Specializations Found!"
                    };
                }

                var data = _mapper.Map<List<SpecializationDto>>(specializations);

                return new GeneralResponse<List<SpecializationDto>>
                {
                    IsSuccess = true,
                    Message = "The Specializations Listed successfully.",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<List<SpecializationDto>>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }
        public async Task<GeneralResponse<AddDoctorResponseDto>> AddDoctor([FromForm]DoctorRequestDto dto)
        {
            try
            {
                if (!await _unitOfWork.CivilRegestrationRepository.AnyAsync(a => a.NationalId == dto.NationalId))
                {
                    return new GeneralResponse<AddDoctorResponseDto>
                    {
                        IsSuccess = false,
                        Message = "Wrong National Id!"
                    };
                }
                if (await _unitOfWork.DoctorRepository.AnyAsync(a => a.NationalId == dto.NationalId))
                {
                    return new GeneralResponse<AddDoctorResponseDto>
                    {
                        IsSuccess = false,
                        Message = "National Id is already Used!"
                    };
                }
                if (await _unitOfWork.UserRepository.AnyAsync(a => a.UserName == dto.UserName))
                {
                    return new GeneralResponse<AddDoctorResponseDto>
                    {
                        IsSuccess = false,
                        Message = "UserName is already Used!"
                    };
                }
                if (await _unitOfWork.DoctorRepository.AnyAsync(a => a.Email == dto.Email))
                {
                    return new GeneralResponse<AddDoctorResponseDto>
                    {
                        IsSuccess = false,
                        Message = "This Email is already Used!"
                    };
                }

                var specializations = new List<Specialization>();
                foreach (var id in dto.SpecializationId)
                {
                    var specialization = await _unitOfWork.SpecializationRepository.GetByIdAsync(id);
                    if (specialization == null)
                    {
                        return new GeneralResponse<AddDoctorResponseDto>
                        {
                            IsSuccess = false,
                            Message = "specialization not found!"
                        };
                    }
                    specializations.Add(specialization);
                }

                var hospitals = new List<Hospital>();
                foreach (var id in dto.HospitalId)
                {
                    var hospital = await _unitOfWork.HospitalRepository.GetByIdAsync(id);
                    if (hospital == null)
                    {
                        return new GeneralResponse<AddDoctorResponseDto>
                        {
                            IsSuccess = false,
                            Message = "hospital not found!"
                        };
                    }
                    hospitals.Add(hospital);
                }

                var clinics = new List<ClinicLab>();
                if (dto.ClinicLabsId != null)
                {
                    foreach (var id in dto.ClinicLabsId)
                    {
                        var clinic = await _unitOfWork.ClinicLabRepository.GetByIdAsync(id);
                        if (clinic == null)
                        {
                            return new GeneralResponse<AddDoctorResponseDto>
                            {
                                IsSuccess = false,
                                Message = "Clinic or Lab not found!"
                            };
                        }
                        clinics.Add(clinic); 
                    }
                }

                if (dto.Image != null)
                {
                    var MaxAllowedPosterSize = _configuration.GetValue<long>("MaxAllowedPosterSize");
                    List<string> AllowedExtenstions = _configuration.GetSection("AllowedExtenstions").Get<List<string>>();

                    if (!AllowedExtenstions.Contains(Path.GetExtension(dto.Image.FileName).ToLower()))
                    {
                        return new GeneralResponse<AddDoctorResponseDto>
                        {
                            IsSuccess = false,
                            Message = "Only .jpg and .png Images Are Allowed."
                        };
                    }

                    if (dto.Image.Length > MaxAllowedPosterSize)
                    {
                        return new GeneralResponse<AddDoctorResponseDto>
                        {
                            IsSuccess = false,
                            Message = "Max Allowed Size Is 1MB."
                        };
                    }
                }
                var doctor = _mapper.Map<Doctor>(dto);
                doctor.FullName = dto.FirstName + " " + dto.LastName;
                doctor.PassWord = HashingService.GetHash(dto.PassWord);
                await _unitOfWork.DoctorRepository.AddAsync(doctor);
                await _unitOfWork.CompleteAsync();

                var data = _mapper.Map<AddDoctorResponseDto>(doctor);

                var doctorSpecialization = dto.SpecializationId.Select(s => new DoctorSpecialization
                {
                    SpecializationId = s,
                    DoctorId = doctor.Id
                });
                var s = _mapper.Map<List<SpecializationDto>>(specializations);
                data.SpecializationNames = s;

                var hospitalDoctors = dto.HospitalId.Select(s => new HospitalDoctor
                {
                    HospitalId = s,
                    DoctorId = doctor.Id
                });
                var h = _mapper.Map<List<HospitalIdDto>>(hospitals);
                data.HospitalNames = h;

                if(dto.ClinicLabsId != null)
                {
                    var clinicDoctor = dto.ClinicLabsId.Select(s => new ClinicLabDoctor
                    {
                        ClinicLabId = s,
                        DoctorId = doctor.Id
                    });
                    var c = _mapper.Map<List<ClinicIdDto>>(clinics);
                    data.ClinicLabsNames = c;
                    await _unitOfWork.ClinicLabDoctorRepository.AddRangeAsync(clinicDoctor);
                    await _unitOfWork.CompleteAsync();
                }
                

                await _unitOfWork.DoctorSpecializationRepository.AddRangeAsync(doctorSpecialization);
                await _unitOfWork.HospitalDoctorRepository.AddRangeAsync(hospitalDoctors);
                await _unitOfWork.CompleteAsync();

                var user = _mapper.Map<User>(doctor);
                user.RoleId = 4;

                var userToken = _mapper.Map<UserTokenDto>(user);
                userToken.Role = "Doctor";
                var Token = TokenServices.CreateJwtToken(userToken);
                var refreshToken = TokenServices.CreateRefreshToken();

                if (dto.Image == null)
                {
                    var DefaultFile = new UploadedFile()
                    {
                        FileName = "DefaultImage.png",
                        StoredFileName = "DefaultImage",
                        ContentType = "image/png",
                        FilePath = "G:\\WEB DEVELOPMENT\\HealthCareProject\\HealthCareAPIs\\HealthCare\\Uploads\\DefaultImage"

                    };
                    await _unitOfWork.UploadedFileRepository.AddAsync(DefaultFile);
                    user.UploadedFile = DefaultFile;
                    await _unitOfWork.CompleteAsync();
                    data.ImagePath = DefaultFile.FilePath;
                }
                else
                {
                    var fakeFileName = Path.GetRandomFileName();
                    var uploadedFile = new UploadedFile()
                    {
                        FileName = dto.Image.FileName,
                        ContentType = dto.Image.ContentType,
                        StoredFileName = fakeFileName
                    };
                    var directoryPath = Path.Combine(_webHostEnvironment.ContentRootPath, "Uploads", "DoctorImages");
                    var path = Path.Combine(directoryPath, fakeFileName);
                    using FileStream fileStream = new(path, FileMode.Create);
                    dto.Image.CopyTo(fileStream);
                    uploadedFile.FilePath = path;
                    await _unitOfWork.UploadedFileRepository.AddAsync(uploadedFile);
                    user.UploadedFile = uploadedFile;

                    data.ImagePath = path;
                }

                await _unitOfWork.UserRepository.AddAsync(user);
                await _unitOfWork.CompleteAsync();

                var userRole = new UserRole()
                {
                    UserId = user.Id,
                    RoleId = 4
                };

                await _unitOfWork.UserRoleRepository.AddAsync(userRole);
                await _unitOfWork.CompleteAsync();

                var newRefreshToken = new RefreshToken
                {
                    Token = refreshToken.Token,
                    ExpiresOn = refreshToken.ExpiresOn,
                    CreatedOn = refreshToken.CreatedOn,
                    User = user
                };

                await _unitOfWork.RefreshTokenRepository.AddAsync(newRefreshToken);
                await _unitOfWork.CompleteAsync();

                data.RefreshToken = refreshToken.Token;
                data.RefreshTokenExpiration = refreshToken.ExpiresOn;
                data.Token = new JwtSecurityTokenHandler().WriteToken(Token);
                data.ExpiresOn = Token.ValidTo;

                return new GeneralResponse<AddDoctorResponseDto>
                {
                    IsSuccess = true,
                    Message = "Doctor Added Successfully.",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<AddDoctorResponseDto>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }
        
        public Task<GeneralResponse<string>> DeleteDoctor(int doctorId)
        {
            throw new NotImplementedException();
        }
        
        public Task<GeneralResponse<List<DoctorDto>>> ListOfDoctors()
        {
            throw new NotImplementedException();
        }
        public Task<GeneralResponse<DoctorDto>> DoctorDetails(int doctorId)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<EditDoctorResponseDto>> EditDoctor(int doctorId, [FromForm] EditDoctorDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<List<DoctorDto>>> GetDoctorByName(string FullName)
        {
            throw new NotImplementedException();
        }
        public Task<GeneralResponse<List<DoctorDto>>> GetDoctorInSpecificHospitalByName(string FullName)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<List<DoctorDto>>> ListOfDoctorsinHospital(int hospitalId)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<List<ListOfHospitalDto>>> ListOfHospitalsADoctorWorksin(int doctorId)
        {
            throw new NotImplementedException();
        }
        
        public Task<GeneralResponse<string>> RateTheDoctor(int doctorId, int PatientId, [FromForm] RateRequestDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<string>> AddDoctorToClinic(int doctorId, int clinicId)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<string>> AddDoctorToHospital(int doctorId, int hospitalId)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<string>> DeleteDoctorFromClinic(int doctorId, int clinicId)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse<string>> DeleteDoctorFromHospital(int doctorId, int hospitalId)
        {
            throw new NotImplementedException();
        }


        
    }
}




/*try
            {
                if(!await _unitOfWork.CivilRegestrationRepository.AnyAsync(a => a.NationalId == dto.NationalId))
                {
                    return new GeneralResponse<AddDoctorResponseDto>
                    {
                        IsSuccess = false,
                        Message = "Wrong National Id!"
                    };
                }
                if (await _unitOfWork.DoctorRepository.AnyAsync(a => a.NationalId == dto.NationalId))
                {
                    return new GeneralResponse<AddDoctorResponseDto>
                    {
                        IsSuccess = false,
                        Message = "National Id is already Used!"
                    };
                }
                if (await _unitOfWork.UserRepository.AnyAsync(a => a.UserName == dto.UserName))
                {
                    return new GeneralResponse<AddDoctorResponseDto>
                    {
                        IsSuccess = false,
                        Message = "UserName is already Used!"
                    };
                }
                if (await _unitOfWork.DoctorRepository.AnyAsync(a => a.Email == dto.Email))
                {
                    return new GeneralResponse<AddDoctorResponseDto>
                    {
                        IsSuccess = false,
                        Message = "This Email is already Used!"
                    };
                }

                var doctor = _mapper.Map<Doctor>(dto);
                doctor.PassWord = HashingService.GetHash(dto.PassWord);
                doctor.FullName = dto.FirstName + dto.LastName;
                
                var data = _mapper.Map<AddDoctorResponseDto>(doctor);
                var user = _mapper.Map<User>(doctor);
                user.RoleId = 4;


                if (dto.Image == null)
                {
                    var DefaultFile = UploadsService.GetDefaultImage();
                    await _unitOfWork.UploadedFileRepository.AddAsync(DefaultFile);
                    user.UploadedFile = DefaultFile;
                    await _unitOfWork.CompleteAsync();
                    data.ImagePath = DefaultFile.FilePath;
                }
                else
                {
                    var MaxAllowedPosterSize = _configuration.GetValue<long>("MaxAllowedPosterSize");
                    List<string> AllowedExtenstions = _configuration.GetSection("AllowedExtenstions").Get<List<string>>();

                    if (!AllowedExtenstions.Contains(Path.GetExtension(dto.Image.FileName).ToLower()))
                    {
                        return new GeneralResponse<AddDoctorResponseDto>
                        {
                            IsSuccess = false,
                            Message = "Only .jpg and .png Images Are Allowed."
                        };
                    }

                    if (dto.Image.Length > MaxAllowedPosterSize)
                    {
                        return new GeneralResponse<AddDoctorResponseDto>
                        {
                            IsSuccess = false,
                            Message = "Max Allowed Size Is 1MB."
                        };
                    }

                    var fakeFileName = Path.GetRandomFileName();
                    var uploadedFile = new UploadedFile()
                    {
                        FileName = dto.Image.FileName,
                        ContentType = dto.Image.ContentType,
                        StoredFileName = fakeFileName
                    };
                    var directoryPath = Path.Combine(_webHostEnvironment.ContentRootPath, "Uploads", "DoctorImages");
                    var path = Path.Combine(directoryPath, fakeFileName);
                    using FileStream fileStream = new(path, FileMode.Create);
                    dto.Image.CopyTo(fileStream);
                    uploadedFile.FilePath = path;
                    await _unitOfWork.UploadedFileRepository.AddAsync(uploadedFile);
                    user.UploadedFile = uploadedFile;

                    data.ImagePath = path;
                }
                

                foreach (var id in dto.SpecializationId)
                {
                    var specialization = await _unitOfWork.SpecializationRepository.GetByIdAsync(id);
                    if(specialization == null)
                    {
                        return new GeneralResponse<AddDoctorResponseDto>
                        {
                            IsSuccess = false,
                            Message = "specialization not found!"
                        };
                    }
                    var specializations = new List<Specialization>();
                    specializations.Add(specialization);
                    var doctorSpecialization = dto.SpecializationId.Select(s => new DoctorSpecialization
                    {
                        SpecializationId = s,
                        DoctorId = doctor.Id
                    });
                    await _unitOfWork.DoctorSpecializationRepository.AddRangeAsync(doctorSpecialization);
                    var s = _mapper.Map<List<SpecializationDto>>(specializations);
                    data.SpecializationNames = s;
                }

                var hospitals = new List<Hospital>();
                foreach (var id in dto.HospitalId)
                {
                    var hospital = await _unitOfWork.HospitalRepository.GetByIdAsync(id);
                    if (hospital == null)
                    {
                        return new GeneralResponse<AddDoctorResponseDto>
                        {
                            IsSuccess = false,
                            Message = "hospital not found!"
                        };
                    }
                }
                    hospitals.Add(hospital);
                    var hospitalDoctors = dto.HospitalId.Select(s => new HospitalDoctor
                    {
                    HospitalId = s,
                    DoctorId = doctor.Id
                    });
                    await _unitOfWork.HospitalDoctorRepository.AddRangeAsync(hospitalDoctors);
                    var h = _mapper.Map<List<HospitalIdDto>>(hospitals);
                var r = dto.HospitalId.Select(async a => new HospitalIdDto
                {
                    Id = a,
                    var s =  await _unitOfWork.HospitalRepository.GetByIdAsync(a);
                    Name = 
                }
                    data.HospitalNames = h;
                }


                if(dto.ClinicLabsId != null)
                {
                    foreach (var id in dto.ClinicLabsId)
                    {
                        var clinic = await _unitOfWork.ClinicLabRepository.GetByIdAsync(id);
                        if (clinic == null)
                        {
                            return new GeneralResponse<AddDoctorResponseDto>
                            {
                                IsSuccess = false,
                                Message = "hospital not found!"
                            };
                        }
                        var clinics = new List<ClinicLab>();
                        clinics.Add(clinic);
                        var clinicDoctor = dto.ClinicLabsId.Select(s => new ClinicLabDoctor
                        {
                            ClinicLabId = s,
                            DoctorId = doctor.Id
                        });
                        await _unitOfWork.ClinicLabDoctorRepository.AddRangeAsync(clinicDoctor);
                        var c = _mapper.Map<List<ClinicIdDto>>(clinics);
                        data.ClinicLabsNames = c;
                    }
                }
                await _unitOfWork.DoctorRepository.AddAsync(doctor);
                await _unitOfWork.UserRepository.AddAsync(user);
                await _unitOfWork.CompleteAsync();



                var userRole = new UserRole()
                {
                    UserId = user.Id,
                    RoleId = 4
                };
                await _unitOfWork.UserRoleRepository.AddAsync(userRole);
                await _unitOfWork.CompleteAsync();


                var userToken = _mapper.Map<UserTokenDto>(user);
                userToken.Role = "Doctor";
                var Token = TokenServices.CreateJwtToken(userToken);
                var refreshToken = TokenServices.CreateRefreshToken();
                var newRefreshToken = new RefreshToken
                {
                    Token = refreshToken.Token,
                    ExpiresOn = refreshToken.ExpiresOn,
                    CreatedOn = refreshToken.CreatedOn,
                    userId = user.Id
                };

                await _unitOfWork.RefreshTokenRepository.AddAsync(newRefreshToken);
                await _unitOfWork.CompleteAsync();

                data.RefreshToken = refreshToken.Token;
                data.RefreshTokenExpiration = refreshToken.ExpiresOn;
                data.Token = new JwtSecurityTokenHandler().WriteToken(Token);
                data.ExpiresOn = Token.ValidTo;
                
                return new GeneralResponse<AddDoctorResponseDto>
                {
                    IsSuccess = true,
                    Message = "New Specialization added successfully.",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<AddDoctorResponseDto>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }*/
