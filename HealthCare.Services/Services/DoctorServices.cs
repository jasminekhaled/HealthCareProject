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
        
        public async Task<GeneralResponse<string>> DeleteDoctor(int doctorId)
        {
            try
            {
                var doctor = await _unitOfWork.DoctorRepository.GetByIdAsync(doctorId);
                if (doctor == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "No Doctor Found ."
                    };
                }
                var user = await _unitOfWork.UserRepository.SingleOrDefaultAsync(s => s.UserName == doctor.UserName);
                var upload = await _unitOfWork.UploadedFileRepository.GetByIdAsync(user.UploadedFileId);
                if(upload.StoredFileName != "DefaultImage") File.Delete(upload.FilePath);

                _unitOfWork.UserRepository.Remove(user);
                _unitOfWork.UploadedFileRepository.Remove(upload);
                _unitOfWork.DoctorRepository.Remove(doctor);
                await _unitOfWork.CompleteAsync();


                return new GeneralResponse<string>
                {
                    IsSuccess = true,
                    Message = "Doctor is deleted successfully."
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
        
        public async Task<GeneralResponse<List<DoctorDto>>> ListOfDoctors()
        {
            try
            {
                var doctors = await _unitOfWork.DoctorRepository.GetAllIncludedAsync(i => i.DoctorSpecialization, s => s.hospitalDoctors);
                if (doctors.Count() == 0)
                {
                    return new GeneralResponse<List<DoctorDto>>
                    {
                        IsSuccess = false,
                        Message = "No Doctors Found!"
                    };

                }
                var data = _mapper.Map<List<DoctorDto>>(doctors);

                foreach (var d in data)
                {
                    var user = await _unitOfWork.UserRepository.WhereSelectTheFirstAsync(filter: s => s.UserName == d.UserName, select: i => i.UploadedFile);
                    d.ImagePath = user.FilePath;

                    var doctor = doctors.FirstOrDefault(s => s.UserName == d.UserName);

                    var specializationIds = doctor.DoctorSpecialization.Select(ds => ds.SpecializationId);
                    var specia = await _unitOfWork.SpecializationRepository
                        .Where(s => specializationIds.Contains(s.Id));
                    d.Specializations = _mapper.Map<List<SpecializationDto>>(specia);

                    var hospitalIds = doctor.hospitalDoctors.Select(hd => hd.HospitalId);
                    var hospitals = await _unitOfWork.HospitalRepository
                        .WhereIncludeAsync(filter: h => hospitalIds.Contains(h.Id), i => i.UploadedFile, i => i.HospitalGovernorate.Governorate);
                    d.Hospitals = _mapper.Map<List<ListOfHospitalDto>>(hospitals);
                }

                return new GeneralResponse<List<DoctorDto>>
                {
                    IsSuccess = true,
                    Message = "Doctors Listed Successfully.",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<List<DoctorDto>>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }
        public async Task<GeneralResponse<DoctorDto>> DoctorDetails(int doctorId)
        {
            try
            {
                var doctor = await _unitOfWork.DoctorRepository.GetSingleWithIncludesAsync(single: a => a.Id == doctorId, i => i.DoctorSpecialization, s => s.hospitalDoctors);
                if (doctor == null)
                {
                    return new GeneralResponse<DoctorDto>
                    {
                        IsSuccess = false,
                        Message = "No Doctor Found!"
                    };

                }
                var data = _mapper.Map<DoctorDto>(doctor);


                    var user = await _unitOfWork.UserRepository.WhereSelectTheFirstAsync(filter: s => s.UserName == doctor.UserName, select: i => i.UploadedFile);
                    data.ImagePath = user.FilePath;

                    var specializationIds = doctor.DoctorSpecialization.Select(ds => ds.SpecializationId);
                    var specia = await _unitOfWork.SpecializationRepository
                        .Where(s => specializationIds.Contains(s.Id));
                    data.Specializations = _mapper.Map<List<SpecializationDto>>(specia);

                    var hospitalIds = doctor.hospitalDoctors.Select(hd => hd.HospitalId);
                    var hospitals = await _unitOfWork.HospitalRepository
                        .WhereIncludeAsync(filter: h => hospitalIds.Contains(h.Id), i => i.UploadedFile, i => i.HospitalGovernorate.Governorate );
                    data.Hospitals = _mapper.Map<List<ListOfHospitalDto>>(hospitals);
                

                return new GeneralResponse<DoctorDto>
                {
                    IsSuccess = true,
                    Message = "Doctor Details have been displayed Successfully.",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<DoctorDto>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }

        public async Task<GeneralResponse<EditDoctorResponseDto>> EditDoctor(int doctorId, [FromForm] EditDoctorDto dto)
        {
            try
            {
                var doctor = await _unitOfWork.DoctorRepository.GetSingleWithIncludesAsync(single: a => a.Id == doctorId, i => i.DoctorSpecialization, s => s.hospitalDoctors);
                if (doctor == null)
                {
                    return new GeneralResponse<EditDoctorResponseDto>
                    {
                        IsSuccess = false,
                        Message = "No Doctor Found!"
                    };

                }
                var user = await _unitOfWork.UserRepository.GetSingleWithIncludesAsync(single: s => s.UserName == doctor.UserName, includes: i => i.UploadedFile);

                if(dto.UserName != null)
                {
                    var person = await _unitOfWork.UserRepository.SingleOrDefaultAsync(a => a.UserName == dto.UserName);
                    if(person != null && person != user)
                    return new GeneralResponse<EditDoctorResponseDto>
                    {
                        IsSuccess = false,
                        Message = "UserName ia already used."
                    };
                }
                if (dto.NationalId != null && !await _unitOfWork.CivilRegestrationRepository.AnyAsync(a => a.NationalId == dto.NationalId))
                {
                    return new GeneralResponse<EditDoctorResponseDto>
                    {
                        IsSuccess = false,
                        Message = "NationalId isn't accepted!"
                    };
                }
          
                var check = await _unitOfWork.DoctorRepository.SingleOrDefaultAsync(s => s.NationalId == dto.NationalId);
                if(check != null && doctor != check)
                {
                    return new GeneralResponse<EditDoctorResponseDto>
                    {
                        IsSuccess = false,
                        Message = "NationalId isn't accepted!"
                    };
                }
                var emailCheck = await _unitOfWork.DoctorRepository.SingleOrDefaultAsync(s => s.Email == dto.Email);
                if (emailCheck != null && doctor != emailCheck)
                {
                    return new GeneralResponse<EditDoctorResponseDto>
                    {
                        IsSuccess = false,
                        Message = "Email isn't accepted!"
                    };
                }

                var specializations = new List<Specialization>();
                if (dto.SpecializationId != null)
                {
                    foreach (var id in dto.SpecializationId)
                    {
                        var specialization = await _unitOfWork.SpecializationRepository.GetByIdAsync(id);
                        if (specialization == null)
                        {
                            return new GeneralResponse<EditDoctorResponseDto>
                            {
                                IsSuccess = false,
                                Message = "specialization not found!"
                            };
                        }
                        specializations.Add(specialization);
                    }
                }

                if (dto.Image != null)
                {
                    var MaxAllowedPosterSize = _configuration.GetValue<long>("MaxAllowedPosterSize");
                    List<string> AllowedExtenstions = _configuration.GetSection("AllowedExtenstions").Get<List<string>>();

                    if (!AllowedExtenstions.Contains(Path.GetExtension(dto.Image.FileName).ToLower()))
                    {
                        return new GeneralResponse<EditDoctorResponseDto>
                        {
                            IsSuccess = false,
                            Message = "Only .jpg and .png Images Are Allowed."
                        };
                    }

                    if (dto.Image.Length > MaxAllowedPosterSize)
                    {
                        return new GeneralResponse<EditDoctorResponseDto>
                        {
                            IsSuccess = false,
                            Message = "Max Allowed Size Is 1MB."
                        };
                    }

                    var fakeFileName = Path.GetRandomFileName();
                    var uploadedFile = await _unitOfWork.UploadedFileRepository.GetByIdAsync(user.UploadedFileId);
                    File.Delete(uploadedFile.FilePath);
                    uploadedFile.FileName = dto.Image.FileName;
                    uploadedFile.ContentType = dto.Image.ContentType;
                    uploadedFile.StoredFileName = fakeFileName;

                    var directoryPath = Path.Combine(_webHostEnvironment.ContentRootPath, "Uploads", "DoctorImages");
                    var path = Path.Combine(directoryPath, fakeFileName);
                    using FileStream fileStream = new(path, FileMode.Create);
                    dto.Image.CopyTo(fileStream);

                    uploadedFile.FilePath = path;
                    _unitOfWork.UploadedFileRepository.Update(uploadedFile);
                    
                }

                

                if (dto.PassWord != null)
                {
                    var pw = HashingService.GetHash(dto.PassWord);
                    doctor.PassWord = pw;
                    user.PassWord = pw;
                }
                doctor.Email = dto.Email ?? doctor.Email;
                doctor.UserName = dto.UserName ?? doctor.UserName;
                doctor.NationalId = dto.NationalId ?? doctor.NationalId;
                doctor.FullName = dto.FullName ?? doctor.FullName;
                doctor.Description = dto.Description ?? doctor.Description;

                var data = _mapper.Map<EditDoctorResponseDto>(doctor);
                if (dto.SpecializationId != null)
                {
                    var oldDS = await _unitOfWork.DoctorSpecializationRepository.WhereIncludeAsync(s => s.DoctorId == doctor.Id);
                    _unitOfWork.DoctorSpecializationRepository.RemoveRange(oldDS);
                    await _unitOfWork.CompleteAsync();

                    var doctorSpecialization = dto.SpecializationId.Select(s => new DoctorSpecialization
                    {
                        SpecializationId = s,
                        DoctorId = doctorId
                    }).ToList();
                    await _unitOfWork.DoctorSpecializationRepository.AddRangeAsync(doctorSpecialization);
                    //await _unitOfWork.CompleteAsync();
                    var s = _mapper.Map<List<SpecializationDto>>(specializations);
                    data.specializations = s;
                }

                user.UserName = doctor.UserName;
                user.Email = doctor.Email;
                user.NationalId = doctor.NationalId;
                
                _unitOfWork.DoctorRepository.Update(doctor);
                _unitOfWork.UserRepository.Update(user);
                await _unitOfWork.CompleteAsync();
                
                data.ImagePath = user.UploadedFile.FilePath;
                if(dto.SpecializationId == null)
                {
                    var specializationIds = doctor.DoctorSpecialization.Select(ds => ds.SpecializationId);
                    var specia = await _unitOfWork.SpecializationRepository
                        .Where(s => specializationIds.Contains(s.Id));
                    data.specializations = _mapper.Map<List<SpecializationDto>>(specia);
                }
                


                return new GeneralResponse<EditDoctorResponseDto>
                {
                    IsSuccess = true,
                    Message = "Doctor Details have been displayed Successfully.",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<EditDoctorResponseDto>
                {
                    IsSuccess = false,
                    Message = "Something went wrong.",
                    Error = ex
                };
            }
        }

        public async Task<GeneralResponse<List<DoctorDto>>> GetDoctorByName(string FullName)
        {
            try
            {
                var doctors = await _unitOfWork.DoctorRepository.WhereIncludeAsync(filter: a => a.FullName == FullName, i => i.DoctorSpecialization, s => s.hospitalDoctors);
                if (doctors.Count() == 0)
                {
                    return new GeneralResponse<List<DoctorDto>>
                    {
                        IsSuccess = false,
                        Message = "No Doctors Found!"
                    };

                }
                var data = _mapper.Map<List<DoctorDto>>(doctors);

                foreach (var d in data)
                {
                    var user = await _unitOfWork.UserRepository.WhereSelectTheFirstAsync(filter: s => s.UserName == d.UserName, select: i => i.UploadedFile);
                    d.ImagePath = user.FilePath;

                    var doctor = doctors.FirstOrDefault(s => s.UserName == d.UserName);

                    var specializationIds = doctor.DoctorSpecialization.Select(ds => ds.SpecializationId);
                    var specia = await _unitOfWork.SpecializationRepository
                        .Where(s => specializationIds.Contains(s.Id));
                    d.Specializations = _mapper.Map<List<SpecializationDto>>(specia);

                    var hospitalIds = doctor.hospitalDoctors.Select(hd => hd.HospitalId);
                    var hospitals = await _unitOfWork.HospitalRepository
                        .WhereIncludeAsync(filter: h => hospitalIds.Contains(h.Id), i => i.UploadedFile, i => i.HospitalGovernorate.Governorate);
                    d.Hospitals = _mapper.Map<List<ListOfHospitalDto>>(hospitals);
                }

                return new GeneralResponse<List<DoctorDto>>
                {
                    IsSuccess = true,
                    Message = "Doctors Listed Successfully.",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<List<DoctorDto>>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }
        public Task<GeneralResponse<List<DoctorDto>>> GetDoctorInSpecificHospitalByName(string FullName)
        {
            throw new NotImplementedException();
        }

        public async Task<GeneralResponse<List<DoctorDto>>> ListOfDoctorsinHospital(int hospitalId)
        {
            try
            {
                var hospital = await _unitOfWork.HospitalRepository.GetByIdAsync(hospitalId);
                if (hospital == null)
                {
                    return new GeneralResponse<List<DoctorDto>>
                    {
                        IsSuccess = false,
                        Message = "No hospital Found!"
                    };
                }

                var doctorIds = await _unitOfWork.HospitalDoctorRepository.GetSpecificItems(filter: w => w.HospitalId == hospitalId, select: s => s.DoctorId);
                var doctors = await _unitOfWork.DoctorRepository.WhereIncludeAsync(filter: a => doctorIds.Contains(a.Id), i => i.DoctorSpecialization, s => s.hospitalDoctors);
                if (doctors.Count() == 0)
                {
                    return new GeneralResponse<List<DoctorDto>>
                    {
                        IsSuccess = false,
                        Message = "No Doctors Found!"
                    };

                }
                var data = _mapper.Map<List<DoctorDto>>(doctors);

                foreach (var d in data)
                {
                    var user = await _unitOfWork.UserRepository.WhereSelectTheFirstAsync(filter: s => s.UserName == d.UserName, select: i => i.UploadedFile);
                    d.ImagePath = user.FilePath;

                    var doctor = doctors.FirstOrDefault(s => s.UserName == d.UserName);

                    var specializationIds = doctor.DoctorSpecialization.Select(ds => ds.SpecializationId);
                    var specia = await _unitOfWork.SpecializationRepository
                        .Where(s => specializationIds.Contains(s.Id));
                    d.Specializations = _mapper.Map<List<SpecializationDto>>(specia);

                    var hospitalIds = doctor.hospitalDoctors.Select(hd => hd.HospitalId);
                    var hospitals = await _unitOfWork.HospitalRepository
                        .WhereIncludeAsync(filter: h => hospitalIds.Contains(h.Id), i => i.UploadedFile, i => i.HospitalGovernorate.Governorate);
                    d.Hospitals = _mapper.Map<List<ListOfHospitalDto>>(hospitals);
                }

                return new GeneralResponse<List<DoctorDto>>
                {
                    IsSuccess = true,
                    Message = "Doctors Listed Successfully.",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<List<DoctorDto>>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }

        }

        public async Task<GeneralResponse<List<ListOfHospitalDto>>> ListOfHospitalsADoctorWorksin(int doctorId)
        {
            try
            {
                var doctor = await _unitOfWork.DoctorRepository.GetByIdAsync(doctorId);
                if (doctor == null)
                {
                    return new GeneralResponse<List<ListOfHospitalDto>>
                    {
                        IsSuccess = false,
                        Message = "No Doctor Found!"
                    };

                }
                var hospitalIds = await _unitOfWork.HospitalDoctorRepository.GetSpecificItems(filter: w => w.DoctorId == doctorId, select: s => s.HospitalId);
                var hospitals = await _unitOfWork.HospitalRepository.WhereIncludeAsync(filter: w =>  hospitalIds.Contains(w.Id), i => i.UploadedFile, i => i.HospitalGovernorate.Governorate);
                if (hospitals.Count() == 0)
                {
                    return new GeneralResponse<List<ListOfHospitalDto>>
                    {
                        IsSuccess = false,
                        Message = "No Hospitals Found!!"
                    };

                }
                var data = _mapper.Map<List<ListOfHospitalDto>>(hospitals);

                return new GeneralResponse<List<ListOfHospitalDto>>
                {
                    IsSuccess = true,
                    Message = "Hospitals Listed Successfully.",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<List<ListOfHospitalDto>>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
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





