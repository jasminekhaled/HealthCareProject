using AutoMapper;
using HealthCare.Core.DTOS;
using HealthCare.Core.DTOS.AuthModule.RequestDtos;
using HealthCare.Core.DTOS.ClinicModule.RequestDto;
using HealthCare.Core.DTOS.DoctorModule.RequestDtos;
using HealthCare.Core.DTOS.DoctorModule.ResponseDtos;
using HealthCare.Core.DTOS.HospitalModule.ResponseDto;
using HealthCare.Core.Helpers;
using HealthCare.Core.IRepositories;
using HealthCare.Core.Models;
using HealthCare.Core.Models.AuthModule;
using HealthCare.Core.Models.ClinicModule;
using HealthCare.Core.Models.DoctorModule;
using HealthCare.Core.Models.HospitalModule;
using HealthCare.Core.Models.PatientModule;
using HealthCare.Services.IServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
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
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DoctorServices(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment webHostEnvironment,
            IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        { 
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }



        public async Task<GeneralResponse<string>> AddSpecialization([FromForm]SpecializationRequestDto dto)
        {
            try
            {
                var fakeFileName = Path.GetRandomFileName();
                var uploadedFile = new UploadedFile()
                {
                    FileName = dto.Image.FileName,
                    ContentType = dto.Image.ContentType,
                    StoredFileName = fakeFileName
                };
                var directoryPath = Path.Combine(_webHostEnvironment.ContentRootPath, "Uploads", "SpecializationImages");
                var path = Path.Combine(directoryPath, fakeFileName);
                using FileStream fileStream = new(path, FileMode.Create);
                dto.Image.CopyTo(fileStream);
                uploadedFile.FilePath = path;
                await _unitOfWork.UploadedFileRepository.AddAsync(uploadedFile);
                await _unitOfWork.CompleteAsync();

                var Specialization = new Specialization()
                {
                    Name = dto.Name,
                    UploadedFile = uploadedFile
                };
                await _unitOfWork.SpecializationRepository.AddAsync(Specialization);
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
        public async Task<GeneralResponse<string>> DeleteSpecialization(int SpecializationId)
        {
            try
            {
                var Specialization = await _unitOfWork.SpecializationRepository.GetByIdAsync(SpecializationId);
                if (Specialization == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "Specialization Not Found!"
                    };
                }
                var uploadedFile = await _unitOfWork.UploadedFileRepository.GetByIdAsync(Specialization.UploadedFileId);
                File.Delete(uploadedFile.FilePath);
                _unitOfWork.SpecializationRepository.Remove(Specialization);
                _unitOfWork.UploadedFileRepository.Remove(uploadedFile);
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
                var specializations = await _unitOfWork.SpecializationRepository.GetAllIncludedAsync(a => a.UploadedFile);
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
                HttpContext httpContext = _httpContextAccessor.HttpContext;
                int userId = httpContext.FindFirst();
                var ThisUser = await _unitOfWork.UserRepository.SingleOrDefaultAsync(
                    a => a.Id == userId && a.Role == User.HospitalAdmin);

                if (ThisUser == null)
                {
                    return new GeneralResponse<AddDoctorResponseDto>
                    {
                        IsSuccess = false,
                        Message = "No HospitalAdmin Found!"
                    };
                }
                var ThisHospitalAdmin = await _unitOfWork.HospitalAdminRepository.GetSingleWithIncludesAsync(
                    s => s.UserName == ThisUser.UserName,
                    i => i.AdminOfHospital);

                
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
                        Message = "National Id is already Used," +
                        " may be the doctor you are trying to add is already has an account and you can add him to your hospital directly."
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
                        Message = "This Email is already Used!" +
                        " May be the doctor you are trying to add is already has an account and you can add him to your hospital directly."
                    };
                }

                var specializations = new List<Specialization>();
                foreach (var id in dto.SpecializationId)
                {
                    var specialization = await _unitOfWork.SpecializationRepository.GetSingleWithIncludesAsync(i => i.Id == id,
                        a=>a.UploadedFile);
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
                var hospital = await _unitOfWork.HospitalRepository.GetSingleWithIncludesAsync(
                    s => s.Id == ThisHospitalAdmin.AdminOfHospital.HospitalId,
                    i => i.UploadedFile);

                var doctor = _mapper.Map<Doctor>(dto);
                doctor.FullName = dto.FirstName + " " + dto.LastName;
                doctor.PassWord = HashingService.GetHash(dto.PassWord);
                var data = _mapper.Map<AddDoctorResponseDto>(doctor);
                data.HospitalId = hospital.Id;
                data.HospitalName = hospital.Name;
                data.HospitalImagePath = hospital.UploadedFile.FilePath;
                var s = _mapper.Map<List<SpecializationDto>>(specializations);
                data.Specializations = s;

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
                    await _unitOfWork.CompleteAsync();
                    doctor.UploadedFile = uploadedFile;
                    data.ImagePath = uploadedFile.FilePath;
                }
                else
                {
                    var DefaultFile = new UploadedFile()
                    {
                        FileName = "DefaultImage.png",
                        StoredFileName = "DefaultImage",
                        ContentType = "image/png",
                        FilePath = "G:\\WEB DEVELOPMENT\\HealthCareProject\\HealthCareAPIs\\HealthCare\\Uploads\\DefaultImage"

                    };
                    await _unitOfWork.UploadedFileRepository.AddAsync(DefaultFile);
                    doctor.UploadedFile = DefaultFile;
                    await _unitOfWork.CompleteAsync();
                    data.ImagePath = DefaultFile.FilePath;
                }
                
                await _unitOfWork.DoctorRepository.AddAsync(doctor);
                await _unitOfWork.CompleteAsync();

                var doctorSpecialization = specializations.Select(s => new DoctorSpecialization
                {
                    Specialization = s,
                    Doctor = doctor
                });
                
                var hospitalDoctor = new HospitalDoctor()
                {
                    Hospital = ThisHospitalAdmin.AdminOfHospital.Hospital,
                    Doctor = doctor
                };

                await _unitOfWork.DoctorSpecializationRepository.AddRangeAsync(doctorSpecialization);
                await _unitOfWork.HospitalDoctorRepository.AddAsync(hospitalDoctor);
                await _unitOfWork.CompleteAsync();

                var user = _mapper.Map<User>(doctor);
                user.Role = User.Doctor;
                user.UploadedFile = doctor.UploadedFile;
                var userToken = _mapper.Map<UserTokenDto>(user);
                userToken.Role = User.Doctor;
                var Token = TokenServices.CreateJwtToken(userToken);
                var refreshToken = TokenServices.CreateRefreshToken();

                await _unitOfWork.UserRepository.AddAsync(user);
                await _unitOfWork.CompleteAsync();

                var userRole = new UserRole()
                {
                    User = user,
                    Role = await _unitOfWork.RoleRepository.SingleOrDefaultAsync(s => s.Name == User.Doctor)
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

                data.DoctorId = doctor.Id;
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
        /*
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
                if (upload.StoredFileName != "DefaultImage")
                { File.Delete(upload.FilePath); }

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
        }*/
        
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
                        .WhereIncludeAsync(s => specializationIds.Contains(s.Id), a=>a.UploadedFile);
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
        public async Task<GeneralResponse<DoctorDto>> DoctorDetails()
        {
            try
            {
                HttpContext httpContext = _httpContextAccessor.HttpContext;
                int userId = httpContext.FindFirst();
                var ThisUser = await _unitOfWork.UserRepository.SingleOrDefaultAsync(
                    a => a.Id == userId && a.Role == User.Doctor);

                if (ThisUser == null)
                {
                    return new GeneralResponse<DoctorDto>
                    {
                        IsSuccess = false,
                        Message = "No doctor Found!"
                    };
                }

                var doctor = await _unitOfWork.DoctorRepository.GetSingleWithIncludesAsync(
                    a => a.UserName == ThisUser.UserName,
                    i => i.DoctorSpecialization, s => s.hospitalDoctors, s => s.UploadedFile);

                var data = _mapper.Map<DoctorDto>(doctor);


                   // var user = await _unitOfWork.UserRepository.WhereSelectTheFirstAsync(filter: s => s.UserName == doctor.UserName, select: i => i.UploadedFile);
                    //data.ImagePath = user.FilePath;

                    var specializationIds = doctor.DoctorSpecialization.Select(ds => ds.SpecializationId);
                    var specia = await _unitOfWork.SpecializationRepository
                        .WhereIncludeAsync(s => specializationIds.Contains(s.Id), a=>a.UploadedFile);
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

        public async Task<GeneralResponse<DoctorDto>> EditDoctor([FromForm]EditDoctorDto dto)
        {
            try
            {
                HttpContext httpContext = _httpContextAccessor.HttpContext;
                int userId = httpContext.FindFirst();
                var ThisUser = await _unitOfWork.UserRepository.SingleOrDefaultAsync(a => a.Id == userId && a.Role == User.Doctor);
                if (ThisUser == null)
                {
                    return new GeneralResponse<DoctorDto>
                    {
                        IsSuccess = false,
                        Message = "No doctor Found!"
                    };
                }

                var doctor = await _unitOfWork.DoctorRepository.GetSingleWithIncludesAsync(
                    a => a.UserName == ThisUser.UserName,
                    i => i.DoctorSpecialization, s => s.hospitalDoctors, s => s.UploadedFile);

                if (dto.Image != null)
                {
                    var MaxAllowedPosterSize = _configuration.GetValue<long>("MaxAllowedPosterSize");
                    List<string> AllowedExtenstions = _configuration.GetSection("AllowedExtenstions").Get<List<string>>();

                    if (!AllowedExtenstions.Contains(Path.GetExtension(dto.Image.FileName).ToLower()))
                    {
                        return new GeneralResponse<DoctorDto>
                        {
                            IsSuccess = false,
                            Message = "Only .jpg and .png Images Are Allowed."
                        };
                    }

                    if (dto.Image.Length > MaxAllowedPosterSize)
                    {
                        return new GeneralResponse<DoctorDto>
                        {
                            IsSuccess = false,
                            Message = "Max Allowed Size Is 1MB."
                        };
                    }

                    var fakeFileName = Path.GetRandomFileName();
                    var uploadedFile = await _unitOfWork.UploadedFileRepository.GetByIdAsync(doctor.UploadedFileId);
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

                
                doctor.FullName = dto.FullName ?? doctor.FullName;
                doctor.Description = dto.Description ?? doctor.Description;

                var data = _mapper.Map<DoctorDto>(doctor);

                var specializationIds = doctor.DoctorSpecialization.Select(ds => ds.SpecializationId);
                var specia = await _unitOfWork.SpecializationRepository
                    .WhereIncludeAsync(s => specializationIds.Contains(s.Id), a => a.UploadedFile);

                data.Specializations = _mapper.Map<List<SpecializationDto>>(specia);

                var hospitalIds = doctor.hospitalDoctors.Select(hd => hd.HospitalId);
                var hospitals = await _unitOfWork.HospitalRepository
                    .WhereIncludeAsync(filter: h => hospitalIds.Contains(h.Id), i => i.UploadedFile, i => i.HospitalGovernorate.Governorate);
               
                data.Hospitals = _mapper.Map<List<ListOfHospitalDto>>(hospitals);
                
                _unitOfWork.DoctorRepository.Update(doctor);
                await _unitOfWork.CompleteAsync();
                
                data.ImagePath = doctor.UploadedFile.FilePath;

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
                    Message = "Something went wrong.",
                    Error = ex
                };
            }
        }
        
        public async Task<GeneralResponse<List<DoctorDto>>> GetDoctorByName(string FullName)
        {
            try
            {
                var doctors = await _unitOfWork.DoctorRepository.WhereIncludeAsync(
                     a => a.FullName == FullName, i => i.DoctorSpecialization, i => i.hospitalDoctors, i => i.UploadedFile);
                
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
                    var doctor = doctors.FirstOrDefault(s => s.UserName == d.UserName);

                    var specializationIds = doctor.DoctorSpecialization.Select(ds => ds.SpecializationId);
                    var specia = await _unitOfWork.SpecializationRepository
                        .WhereIncludeAsync(s => specializationIds.Contains(s.Id), i => i.UploadedFile);
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
                HttpContext httpContext = _httpContextAccessor.HttpContext;
                int userId = httpContext.FindFirst();
                var ThisUser = await _unitOfWork.UserRepository.SingleOrDefaultAsync(a => a.Id == userId);
                if (ThisUser == null)
                {
                    return new GeneralResponse<List<DoctorDto>>
                    {
                        IsSuccess = false,
                        Message = "No user Found!"
                    };
                }
                if(ThisUser.Role == User.HospitalAdmin)
                {
                    var hospitalAdmin = await _unitOfWork.HospitalAdminRepository.GetSingleWithIncludesAsync(
                        s => s.UserName == ThisUser.UserName, i => i.AdminOfHospital);
                    if (hospitalAdmin.AdminOfHospital.HospitalId != hospitalId)
                    {
                        return new GeneralResponse<List<DoctorDto>>
                        {
                            IsSuccess = false,
                            Message = "you donot have permission to access this list!!"
                        };
                    }
                }


                var doctorIds = await _unitOfWork.HospitalDoctorRepository.GetSpecificItems(
                    filter: w => w.HospitalId == hospitalId,
                    select: s => s.DoctorId);

                var doctors = await _unitOfWork.DoctorRepository.WhereIncludeAsync(
                    a => doctorIds.Contains(a.Id),
                    i => i.DoctorSpecialization, i => i.UploadedFile, i => i.hospitalDoctors);
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
                    var doctor = doctors.FirstOrDefault(s => s.UserName == d.UserName);

                    var specializationIds = doctor.DoctorSpecialization.Select(ds => ds.SpecializationId);
                    var specia = await _unitOfWork.SpecializationRepository
                        .WhereIncludeAsync(s => specializationIds.Contains(s.Id), i=>i.UploadedFile);
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
       
        public async Task<GeneralResponse<List<ListOfHospitalDto>>> ListOfHospitalsADoctorWorksin()
        {
            try
            {
                HttpContext httpContext = _httpContextAccessor.HttpContext;
                int userId = httpContext.FindFirst();
                var ThisUser = await _unitOfWork.UserRepository.SingleOrDefaultAsync(
                    a => a.Id == userId && a.Role == User.Doctor);
                if (ThisUser == null)
                {
                    return new GeneralResponse<List<ListOfHospitalDto>>
                    {
                        IsSuccess = false,
                        Message = "No doctor Found!"
                    };
                }

                var doctor = await _unitOfWork.DoctorRepository.GetSingleWithIncludesAsync(
                    s => s.UserName == ThisUser.UserName);
                
                var hospitalIds = await _unitOfWork.HospitalDoctorRepository.GetSpecificItems(
                    filter: w => w.DoctorId == doctor.Id,
                    select: s => s.HospitalId);

                var hospitals = await _unitOfWork.HospitalRepository
                    .WhereIncludeAsync(filter: h => hospitalIds.Contains(h.Id),
                    i => i.UploadedFile, i => i.HospitalGovernorate.Governorate);

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
        
        public async Task<GeneralResponse<string>> RateTheDoctor(int doctorId, [FromForm] RateRequestDto dto)
        {
            try
            {
                var doctor = await _unitOfWork.DoctorRepository.GetByIdAsync(doctorId);
                if (doctor == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "No Doctor Found!"
                    };
                }
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
                
                if(dto.Rate <= 0 || dto.Rate > 10)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "Your rating is unacceptable!"
                    };
                }
                var appliedRate = (float)Math.Round(dto.Rate, 1);
                var rate = await _unitOfWork.RateDoctorRepository.SingleOrDefaultAsync(
                    w => w.PatientId == patient.Id && w.DoctorId == doctorId);
                if(rate != null)
                {
                    rate.Rate = appliedRate;
                    _unitOfWork.RateDoctorRepository.Update(rate);
                }
                else
                {
                    var Rating = new RateDoctor()
                    {
                        Doctor = doctor,
                        Patient = patient,
                        Rate = appliedRate
                    };
                    await _unitOfWork.RateDoctorRepository.AddAsync(Rating);
                }
                await _unitOfWork.CompleteAsync();

                var TotalRate = await _unitOfWork.RateDoctorRepository.GetSpecificItems(filter: w => w.DoctorId == doctorId, select: s => s.Rate);
                var Total = TotalRate.Average();
                doctor.Rate = (float)Math.Round(Total, 1);
                _unitOfWork.DoctorRepository.Update(doctor);
                await _unitOfWork.CompleteAsync();
                return new GeneralResponse<string>
                {
                    IsSuccess = true,
                    Message = "Rating Process was completed successfully."   
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

        public async Task<GeneralResponse<string>> AddDoctorToHospital(int doctorId)
        {
            try
            {
                var doctor = await _unitOfWork.DoctorRepository.GetByIdAsync(doctorId);
                if (doctor == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "No Doctor Found!"
                    };
                }
                HttpContext httpContext = _httpContextAccessor.HttpContext;
                int userId = httpContext.FindFirst();
                var ThisUser = await _unitOfWork.UserRepository.SingleOrDefaultAsync(a => a.Id == userId && a.Role == User.HospitalAdmin);
                if (ThisUser == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "No hospitalAdmin Found!"
                    };
                }
                var hospitalAdmin = await _unitOfWork.HospitalAdminRepository.GetSingleWithIncludesAsync(
                    s=>s.UserName == ThisUser.UserName, i=>i.AdminOfHospital);

                if(await _unitOfWork.HospitalDoctorRepository.AnyAsync(
                    s => s.DoctorId == doctorId && s.HospitalId == hospitalAdmin.AdminOfHospital.HospitalId))
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "The doctor is already added to this hospital."
                    };
                }
                var hospitalDoctor = new HospitalDoctor()
                {
                    Hospital = await _unitOfWork.HospitalRepository.GetByIdAsync(hospitalAdmin.AdminOfHospital.HospitalId),
                    Doctor = doctor
                };
                await _unitOfWork.HospitalDoctorRepository.AddAsync(hospitalDoctor);
                await _unitOfWork.CompleteAsync();
                return new GeneralResponse<string>
                {
                    IsSuccess = true,
                    Message = "Doctor is added successfully."
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

        public async Task<GeneralResponse<string>> DeleteDoctorFromHospital(int doctorId)
        {
            try
            {
                var doctor = await _unitOfWork.DoctorRepository.GetByIdAsync(doctorId);
                if (doctor == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "No Doctor Found!"
                    };
                }

                HttpContext httpContext = _httpContextAccessor.HttpContext;
                int userId = httpContext.FindFirst();
                var ThisUser = await _unitOfWork.UserRepository.SingleOrDefaultAsync(a => a.Id == userId && a.Role == User.HospitalAdmin);
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


                
                var hospitalDoctor = await _unitOfWork.HospitalDoctorRepository.SingleOrDefaultAsync(
                    s => s.DoctorId == doctorId && s.HospitalId == hospitalAdmin.AdminOfHospital.HospitalId);
                if(hospitalDoctor == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "The doctor isnot working in this hospital."
                    };
                }
                var clinics = await _unitOfWork.ClinicLabRepository.GetSpecificItems(
                    w => w.HospitalId == hospitalAdmin.AdminOfHospital.HospitalId,
                    a => a.ClinicAppointments.Where(w=>w.DoctorId == doctorId));

                var clinicAppointments = clinics.SelectMany(appointments => appointments).ToList();

                var xrays = await _unitOfWork.XrayRepository.GetSpecificItems(
                    w => w.HospitalId == hospitalAdmin.AdminOfHospital.HospitalId,
                    a => a.XrayAppointments.Where(w => w.DoctorId == doctorId));

                var xrayAppointments = xrays.SelectMany(appointments => appointments).ToList();

                var labs = await _unitOfWork.LabRepository.GetSpecificItems(
                    w => w.HospitalId == hospitalAdmin.AdminOfHospital.HospitalId,
                    a => a.LabAppointments.Where(w => w.DoctorId == doctorId));

                var labAppointments = labs.SelectMany(appointments => appointments).ToList();


                _unitOfWork.ClinicAppointmentRepository.RemoveRange(clinicAppointments);
                _unitOfWork.LabAppointmentRepository.RemoveRange(labAppointments);
                _unitOfWork.XrayAppointmentRepository.RemoveRange(xrayAppointments);
                _unitOfWork.HospitalDoctorRepository.Remove(hospitalDoctor);
                await _unitOfWork.CompleteAsync();

                var CheckHospitalDoctor = await _unitOfWork.HospitalDoctorRepository.SingleOrDefaultAsync(
                    s => s.DoctorId == doctorId);
                if (CheckHospitalDoctor == null)
                {
                    var user = await _unitOfWork.UserRepository.GetSingleWithIncludesAsync(
                        s => s.UserName == doctor.UserName);
                    var upload = await _unitOfWork.UploadedFileRepository.GetByIdAsync(user.UploadedFileId);
                    
                    _unitOfWork.DoctorRepository.Remove(doctor);
                    _unitOfWork.UserRepository.Remove(user);
                    if (upload.StoredFileName != "DefaultImage")
                    { File.Delete(upload.FilePath); }
                    _unitOfWork.UploadedFileRepository.Remove(upload);
                    await _unitOfWork.CompleteAsync();
                }
                return new GeneralResponse<string>
                {
                    IsSuccess = true,
                    Message = "Doctor is removed successfully."
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

        public async Task<GeneralResponse<List<DoctorDto>>> GetDoctorInSpecificHospitalByName(string FullName, int hospitalId)
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

                var doctors = await _unitOfWork.DoctorRepository.WhereIncludeAsync(
                     a => a.FullName == FullName, i => i.DoctorSpecialization, i => i.hospitalDoctors, i => i.UploadedFile);

                if (doctors.Count() == 0)
                {
                    return new GeneralResponse<List<DoctorDto>>
                    {
                        IsSuccess = false,
                        Message = "No Doctors Found!"
                    };
                }

                HttpContext httpContext = _httpContextAccessor.HttpContext;
                int userId = httpContext.FindFirst();
                var ThisUser = await _unitOfWork.UserRepository.SingleOrDefaultAsync(a => a.Id == userId);
                if (ThisUser == null)
                {
                    return new GeneralResponse<List<DoctorDto>>
                    {
                        IsSuccess = false,
                        Message = "No user Found!"
                    };
                }
                if (ThisUser.Role == User.HospitalAdmin)
                {
                    var hospitalAdmin = await _unitOfWork.HospitalAdminRepository.GetSingleWithIncludesAsync(
                        s => s.UserName == ThisUser.UserName, i => i.AdminOfHospital);
                    if (hospitalAdmin.AdminOfHospital.HospitalId != hospitalId)
                    {
                        return new GeneralResponse<List<DoctorDto>>
                        {
                            IsSuccess = false,
                            Message = "you donot have permission to access this list!!"
                        };
                    }
                }
                
                var data = _mapper.Map<List<DoctorDto>>(doctors);

                foreach (var d in data)
                {
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

        public async Task<GeneralResponse<List<DoctorDto>>> ListOfDoctorsNotInThisHospital()
        {
            try
            {
                HttpContext httpContext = _httpContextAccessor.HttpContext;
                int userId = httpContext.FindFirst();
                var ThisUser = await _unitOfWork.UserRepository.SingleOrDefaultAsync(
                    a => a.Id == userId && a.Role == User.HospitalAdmin);
                if (ThisUser == null)
                {
                    return new GeneralResponse<List<DoctorDto>>
                    {
                        IsSuccess = false,
                        Message = "No user Found!"
                    };
                }
                var hospitalAdmin = await _unitOfWork.HospitalAdminRepository.GetSingleWithIncludesAsync(
                       s => s.UserName == ThisUser.UserName, i => i.AdminOfHospital);


                var doctorIds = await _unitOfWork.HospitalDoctorRepository.GetSpecificItems(
                   filter: w => w.HospitalId == hospitalAdmin.AdminOfHospital.HospitalId,
                   select: s => s.DoctorId);

                var doctors = await _unitOfWork.DoctorRepository.WhereIncludeAsync(
                    a => !doctorIds.Contains(a.Id),
                    i => i.DoctorSpecialization, i => i.UploadedFile, i => i.hospitalDoctors);
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
                    var doctor = doctors.FirstOrDefault(s => s.UserName == d.UserName);

                    var specializationIds = doctor.DoctorSpecialization.Select(ds => ds.SpecializationId);
                    var specia = await _unitOfWork.SpecializationRepository
                        .WhereIncludeAsync(s => specializationIds.Contains(s.Id), i => i.UploadedFile);
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



    }
}





