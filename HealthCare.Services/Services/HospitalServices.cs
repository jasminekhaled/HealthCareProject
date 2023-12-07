using AutoMapper;
using HealthCare.Core.DTOS;
using HealthCare.Core.DTOS.HospitalModule.RequestDto;
using HealthCare.Core.DTOS.HospitalModule.ResponseDto;
using HealthCare.Core.DTOS.PatientModule.ResponseDto;
using HealthCare.Core.IRepositories;
using HealthCare.Services.IServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using HealthCare.Core.Models;
using HealthCare.Core.Models.HospitalModule;
using Microsoft.Extensions.Configuration;
using System.Xml.Linq;
using System.ComponentModel.DataAnnotations;
using HealthCare.Core.Models.PatientModule;
using HealthCare.Core.Models.AuthModule;
using HealthCare.Core.DTOS.AuthModule.RequestDtos;
using HealthCare.Core.Helpers;
using System.IdentityModel.Tokens.Jwt;
using HealthCare.Core.DTOS.DoctorModule.ResponseDtos;

namespace HealthCare.Services.Services
{
    public class HospitalServices : IHospitalServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;

        public HospitalServices(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
        }


        public async Task<GeneralResponse<List<ListOfHospitalDto>>> ListOfHospitals()
        {
            try
            {
                var hospitals = await _unitOfWork.HospitalRepository.GetAllIncludedAsync(i => i.UploadedFile, i => i.HospitalGovernorate.Governorate);
                if(hospitals == null)
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
        public async Task<GeneralResponse<HospitalDto>> AddHospital([FromForm] HospitalRequestDto dto)
        {
            try
            {
                var governorate = await _unitOfWork.GovernorateRepository.GetByIdAsync(dto.GovernorateId);
                if(governorate == null)
                {
                    return new GeneralResponse<HospitalDto>
                    {
                        IsSuccess = false,
                        Message = "Please choose a Governorate",
                    };
                }

                var MaxAllowedPosterSize = _configuration.GetValue<long>("MaxAllowedPosterSize");
                List<string> AllowedExtenstions = _configuration.GetSection("AllowedExtenstions").Get<List<string>>();

                if (!AllowedExtenstions.Contains(Path.GetExtension(dto.Image.FileName).ToLower()))
                {
                    return new GeneralResponse<HospitalDto>
                    {
                        IsSuccess = false,
                        Message = "Only .jpg and .png Images Are Allowed."
                    };
                }

                if (dto.Image.Length > MaxAllowedPosterSize)
                {
                    return new GeneralResponse<HospitalDto>
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
                var directoryPath = Path.Combine(_webHostEnvironment.ContentRootPath, "Uploads", "HospitalImages");
                var path = Path.Combine(directoryPath, fakeFileName);
                using FileStream fileStream = new(path, FileMode.Create);
                dto.Image.CopyTo(fileStream);
                uploadedFile.FilePath = path;
                await _unitOfWork.UploadedFileRepository.AddAsync(uploadedFile);
                await _unitOfWork.CompleteAsync();

                var hospital = _mapper.Map<Hospital>(dto);
                hospital.UploadedFile = uploadedFile;
                await _unitOfWork.HospitalRepository.AddAsync(hospital);
                await _unitOfWork.CompleteAsync();

                var govern = new HospitalGovernorate()
                {
                    HospitalId = hospital.Id,
                    GovernorateId = dto.GovernorateId
                };
                await _unitOfWork.HospitalGovernorateRepository.AddAsync(govern);
                await _unitOfWork.CompleteAsync();

                var data = _mapper.Map<HospitalDto>(hospital);
                data.ImagePath = path;
                data.Governorate = governorate.Name;

                return new GeneralResponse<HospitalDto>
                {
                    IsSuccess = true,
                    Message = "New Hospital Added Successfully.",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<HospitalDto>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }


        public async Task<GeneralResponse<string>> DeleteHospital(int hospitalId)
        {
            try
            {
                var hospital = await _unitOfWork.HospitalRepository.GetByIdAsync(hospitalId);
                if(hospital == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "No hospital Found ."
                    };
                }
                var upload = await _unitOfWork.UploadedFileRepository.GetByIdAsync(hospital.UploadedFileId);
                File.Delete(upload.FilePath);
                _unitOfWork.UploadedFileRepository.Remove(upload);
                
                _unitOfWork.HospitalRepository.Remove(hospital);
                await _unitOfWork.CompleteAsync();
                

                return new GeneralResponse<string>
                {
                    IsSuccess = true,
                    Message = "Hospital is deleted successfully."
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

        

        public async Task<GeneralResponse<List<ListOfHospitalDto>>> GetHospitalByName(string Name)
        {
            try
            {
                var hospitals = await _unitOfWork.HospitalRepository.GetAllIncludedAsync(i => i.UploadedFile, i => i.HospitalGovernorate.Governorate);
                var hospital = hospitals.Where(a => a.Name == Name).ToList();
                if (hospital.Count == 0)
                {
                    return new GeneralResponse<List<ListOfHospitalDto>>
                    {
                        IsSuccess = false,
                        Message = "No hospital Found."
                    };
                }
                var data = _mapper.Map<List<ListOfHospitalDto>>(hospital);

                return new GeneralResponse<List<ListOfHospitalDto>>
                {
                    IsSuccess = true,
                    Message = "Hospitals with inserted name listed successfully.",
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


        public async Task<GeneralResponse<List<ListOfHospitalDto>>> GetHospitalByGovernorate(int governoratetId)
        {
            try
            {
                if(!await _unitOfWork.GovernorateRepository.AnyAsync(n => n.Id == governoratetId))
                {
                    return new GeneralResponse<List<ListOfHospitalDto>>
                    {
                        IsSuccess = false,
                        Message = "No Governorate Found."
                    };
                }
                var hospitals = await _unitOfWork.HospitalRepository.WhereIncludeAsync(filter: a => a.HospitalGovernorate.GovernorateId == governoratetId, i => i.UploadedFile, i => i.HospitalGovernorate.Governorate);
                
                if (!hospitals.Any())
                {
                    return new GeneralResponse<List<ListOfHospitalDto>>
                    {
                        IsSuccess = false,
                        Message = "No hospital found ."
                    };
                }
                var data = _mapper.Map<List<ListOfHospitalDto>>(hospitals);

                return new GeneralResponse<List<ListOfHospitalDto>>
                {
                    IsSuccess = true,
                    Message = "Hospitals with inserted name listed successfully.",
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
        public async Task<GeneralResponse<HospitalDto>> HospitalDetails(int hospitalId)
        {
            try
            {
                var hospital = await _unitOfWork.HospitalRepository.GetSingleWithIncludesAsync(single: h => h.Id == hospitalId, i => i.UploadedFile, i => i.HospitalGovernorate.Governorate);
                if(hospital == null)
                {
                    return new GeneralResponse<HospitalDto>
                    {
                        IsSuccess = false,
                        Message = "No Hospital found."
                    };
                }

                var data = _mapper.Map<HospitalDto>(hospital);

                return new GeneralResponse<HospitalDto>
                {
                    IsSuccess = true,
                    Message = "Hospital details have been displayed successfully.",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<HospitalDto>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }
        public async Task<GeneralResponse<HospitalDto>> EditHospital(int hospitalId, [FromForm]EditHospitalDto dto)
        {
            try
            {
                var hospital = await _unitOfWork.HospitalRepository.GetSingleWithIncludesAsync(single: h => h.Id == hospitalId, i => i.UploadedFile, i => i.HospitalGovernorate.Governorate);
                if (hospital == null)
                {
                    return new GeneralResponse<HospitalDto>
                    {
                        IsSuccess = false,
                        Message = "No Hospital found."
                    };
                }
                var hospitalGovernorate = await _unitOfWork.HospitalGovernorateRepository.SingleOrDefaultAsync(s => s.HospitalId == hospitalId);
                if(dto.GovernorateId != null && !await _unitOfWork.GovernorateRepository.AnyAsync(a => a.Id == dto.GovernorateId))
                {
                    return new GeneralResponse<HospitalDto>
                    {
                        IsSuccess = false,
                        Message = "No Governorate found."
                    };
                }

                if(dto.Image != null)
                {
                    var MaxAllowedPosterSize = _configuration.GetValue<long>("MaxAllowedPosterSize");
                    List<string> AllowedExtenstions = _configuration.GetSection("AllowedExtenstions").Get<List<string>>();

                    if (!AllowedExtenstions.Contains(Path.GetExtension(dto.Image.FileName).ToLower()))
                    {
                        return new GeneralResponse<HospitalDto>
                        {
                            IsSuccess = false,
                            Message = "Only .jpg and .png Images Are Allowed."
                        };
                    }

                    if (dto.Image.Length > MaxAllowedPosterSize)
                    {
                        return new GeneralResponse<HospitalDto>
                        {
                            IsSuccess = false,
                            Message = "Max Allowed Size Is 1MB."
                        };
                    }

                    var fakeFileName = Path.GetRandomFileName();
                    var uploadedFile = await _unitOfWork.UploadedFileRepository.GetByIdAsync(hospital.UploadedFileId);
                    if (uploadedFile.FilePath != null)
                    {
                        File.Delete(uploadedFile.FilePath);
                    }
                    uploadedFile.FileName = dto.Image.FileName;
                    uploadedFile.ContentType = dto.Image.ContentType;
                    uploadedFile.StoredFileName = fakeFileName;

                    var directoryPath = Path.Combine(_webHostEnvironment.ContentRootPath, "Uploads", "HospitalImages");
                    var path = Path.Combine(directoryPath, fakeFileName);
                    using FileStream fileStream = new(path, FileMode.Create);
                    dto.Image.CopyTo(fileStream);
                    
                    uploadedFile.FilePath = path;
                    _unitOfWork.UploadedFileRepository.Update(uploadedFile);
                    await _unitOfWork.CompleteAsync();
                }

                hospital.Description = dto.Description ?? hospital.Description;
                hospital.Name = dto.Name ?? hospital.Name;
                hospital.Address = dto.Address ?? hospital.Address;
                hospitalGovernorate.GovernorateId = dto.GovernorateId ?? hospitalGovernorate.GovernorateId;
                if(dto.GovernorateId != null)
                {
                    hospitalGovernorate.Governorate = await _unitOfWork.GovernorateRepository.GetByIdAsync(dto.GovernorateId.Value);
                }
                

                _unitOfWork.HospitalGovernorateRepository.Update(hospitalGovernorate);
                _unitOfWork.HospitalRepository.Update(hospital);
                await _unitOfWork.CompleteAsync();

                var data = _mapper.Map<HospitalDto>(hospital);
                

                return new GeneralResponse<HospitalDto>
                {
                    IsSuccess = true,
                    Message = "Hospital has been edited successfully.",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<HospitalDto>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }


        public async Task<GeneralResponse<HospitalAdminDto>> AddHospitalAdmin(int hospitalId, [FromForm]HospitalAdminRequestDto dto)
        {
            try
            {
                var hospital = await _unitOfWork.HospitalRepository.GetByIdAsync(hospitalId);
                if(hospital == null)
                {
                    return new GeneralResponse<HospitalAdminDto>
                    {
                        IsSuccess = false,
                        Message = "No Hospital Found."
                    };
                }
                if(await _unitOfWork.UserRepository.AnyAsync(a => a.UserName == dto.UserName))
                {
                    return new GeneralResponse<HospitalAdminDto>
                    {
                        IsSuccess = false,
                        Message = "This UserName is already used."
                    };
                }
                if (!await _unitOfWork.CivilRegestrationRepository.AnyAsync(a => a.NationalId == dto.NationalId) || await _unitOfWork.HospitalAdminRepository.AnyAsync(a => a.NationalId == dto.NationalId) || await _unitOfWork.HospitalAdminRepository.AnyAsync(a => a.Email == dto.Email))
                {
                    return new GeneralResponse<HospitalAdminDto>
                    {
                        IsSuccess = false,
                        Message = "NationalId Or Email cannot be accepted."
                    };
                }
                
                var admin = _mapper.Map<HospitalAdmin>(dto);
                admin.PassWord = HashingService.GetHash(dto.PassWord);
                var data = _mapper.Map<HospitalAdminDto>(admin);

                if(dto.Image == null)
                {
                    var DefaultFile = new UploadedFile()
                    {
                        FileName = "DefaultImage.png",
                        StoredFileName = "DefaultImage",
                        ContentType = "image/png",
                        FilePath = "G:\\WEB DEVELOPMENT\\HealthCareProject\\HealthCareAPIs\\HealthCare\\Uploads\\DefaultImage"

                    };
                    await _unitOfWork.UploadedFileRepository.AddAsync(DefaultFile);
                    admin.UploadedFile = DefaultFile;
                    await _unitOfWork.CompleteAsync();

                    data.ImagePath = DefaultFile.FilePath;
                }
                else
                {
                    var MaxAllowedPosterSize = _configuration.GetValue<long>("MaxAllowedPosterSize");
                    List<string> AllowedExtenstions = _configuration.GetSection("AllowedExtenstions").Get<List<string>>();

                    if (!AllowedExtenstions.Contains(Path.GetExtension(dto.Image.FileName).ToLower()))
                    {
                        return new GeneralResponse<HospitalAdminDto>
                        {
                            IsSuccess = false,
                            Message = "Only .jpg and .png Images Are Allowed."
                        };
                    }

                    if (dto.Image.Length > MaxAllowedPosterSize)
                    {
                        return new GeneralResponse<HospitalAdminDto>
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
                    var directoryPath = Path.Combine(_webHostEnvironment.ContentRootPath, "Uploads", "HospitalAdminImages");
                    var path = Path.Combine(directoryPath, fakeFileName);
                    using FileStream fileStream = new(path, FileMode.Create);
                    dto.Image.CopyTo(fileStream);
                    uploadedFile.FilePath = path;
                    await _unitOfWork.UploadedFileRepository.AddAsync(uploadedFile);
                    admin.UploadedFile = uploadedFile;
                    await _unitOfWork.CompleteAsync();
                    data.ImagePath = path;

                }
                await _unitOfWork.HospitalAdminRepository.AddAsync(admin);
                await _unitOfWork.CompleteAsync();
                
                var adminOfHospital = new AdminOfHospital()
                {
                    HospitalId = hospitalId,
                    HospitalAdminId = admin.Id
                };
                await _unitOfWork.AdminOfHospitalRepository.AddAsync(adminOfHospital);
                await _unitOfWork.CompleteAsync();

                var user = _mapper.Map<User>(admin);
                user.RoleId = 2;
                await _unitOfWork.UserRepository.AddAsync(user);
                await _unitOfWork.CompleteAsync();

                var userRole = new UserRole()
                {
                    UserId = user.Id,
                    RoleId = 2
                };
                await _unitOfWork.UserRoleRepository.AddAsync(userRole);
                await _unitOfWork.CompleteAsync();


                var userToken = _mapper.Map<UserTokenDto>(user);
                userToken.Role = "HospitalAdmin";
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


                return new GeneralResponse<HospitalAdminDto>
                {
                    IsSuccess = true,
                    Message = "New Admin is added Successfully.",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<HospitalAdminDto>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }

        }

        

        public async Task<GeneralResponse<string>> DeleteHospitalAdmin(int hospitalAdminId)
        {
            try
            {
                var admin = await _unitOfWork.HospitalAdminRepository.GetByIdAsync(hospitalAdminId);
                if(admin == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "No Admin Found!"
                    };
                }
                var user = await _unitOfWork.UserRepository.SingleOrDefaultAsync(s => s.UserName == admin.UserName);
                var refreshToken = await _unitOfWork.RefreshTokenRepository.SingleOrDefaultAsync(s => s.userId == user.Id);
                var uploadedFile = await _unitOfWork.UploadedFileRepository.GetByIdAsync(admin.UploadedFileId);
                if(uploadedFile.FilePath != "G:\\WEB DEVELOPMENT\\HealthCareProject\\HealthCareAPIs\\HealthCare\\Uploads\\DefaultImage")
                {
                    File.Delete(uploadedFile.FilePath);
                }
                
                _unitOfWork.RefreshTokenRepository.Remove(refreshToken);
                _unitOfWork.UserRepository.Remove(user);
                _unitOfWork.HospitalAdminRepository.Remove(admin);
                _unitOfWork.UploadedFileRepository.Remove(uploadedFile);
                await _unitOfWork.CompleteAsync();

                return new GeneralResponse<string>
                {
                    IsSuccess = true,
                    Message = "Admin is deleted successfully"
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

        
        public async Task<GeneralResponse<EditHospitalAdminResponse>> HospitalAdminDetails(int hospitalAdminId)
        {
            try
            {
                var admin = await _unitOfWork.HospitalAdminRepository.GetSingleWithIncludesAsync(single: s => s.Id  == hospitalAdminId, i => i.UploadedFile);
                if (admin == null)
                {
                    return new GeneralResponse<EditHospitalAdminResponse>
                    {
                        IsSuccess = false,
                        Message = "No Admin Found!"
                    };
                }
                var data = _mapper.Map<EditHospitalAdminResponse>(admin);

                return new GeneralResponse<EditHospitalAdminResponse>
                {
                    IsSuccess = true,
                    Message = "Admin Details have been Displayed successfully",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<EditHospitalAdminResponse>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }


        public async Task<GeneralResponse<EditHospitalAdminResponse>> EditHospitalAdmin(int hospitalAdminId, [FromForm]EditHospitalAdminDto dto)
        {
            try
            {
                var admin = await _unitOfWork.HospitalAdminRepository.GetSingleWithIncludesAsync(single: s => s.Id == hospitalAdminId, i => i.UploadedFile);
                var user = await _unitOfWork.UserRepository.SingleOrDefaultAsync(s => s.UserName == admin.UserName);
                if (admin == null)
                {
                    return new GeneralResponse<EditHospitalAdminResponse>
                    {
                        IsSuccess = false,
                        Message = "No Admin Found!"
                    };
                }
                if(await _unitOfWork.UserRepository.AnyAsync(a => a.UserName == dto.UserName))
                {
                    return new GeneralResponse<EditHospitalAdminResponse>
                    {
                        IsSuccess = false,
                        Message = "UserName ia already used."
                    };
                }
                if (dto.NationalId != null && !await _unitOfWork.CivilRegestrationRepository.AnyAsync(a => a.NationalId == dto.NationalId))
                {
                    return new GeneralResponse<EditHospitalAdminResponse>
                    {
                        IsSuccess = false,
                        Message = "NationalId isn't accepted!"
                    };
                }
          
                var check = await _unitOfWork.HospitalAdminRepository.SingleOrDefaultAsync(s => s.NationalId == dto.NationalId);
                if(check != null && admin != check)
                {
                    return new GeneralResponse<EditHospitalAdminResponse>
                    {
                        IsSuccess = false,
                        Message = "NationalId isn't accepted!"
                    };
                }
                var emailUser = await _unitOfWork.HospitalAdminRepository.SingleOrDefaultAsync(s => s.Email == dto.Email);
                if (emailUser != null && admin != emailUser)
                {
                    return new GeneralResponse<EditHospitalAdminResponse>
                    {
                        IsSuccess = false,
                        Message = "Email isn't accepted!"
                    };
                }
                if (dto.Image != null)
                {
                    var MaxAllowedPosterSize = _configuration.GetValue<long>("MaxAllowedPosterSize");
                    List<string> AllowedExtenstions = _configuration.GetSection("AllowedExtenstions").Get<List<string>>();

                    if (!AllowedExtenstions.Contains(Path.GetExtension(dto.Image.FileName).ToLower()))
                    {
                        return new GeneralResponse<EditHospitalAdminResponse>
                        {
                            IsSuccess = false,
                            Message = "Only .jpg and .png Images Are Allowed."
                        };
                    }

                    if (dto.Image.Length > MaxAllowedPosterSize)
                    {
                        return new GeneralResponse<EditHospitalAdminResponse>
                        {
                            IsSuccess = false,
                            Message = "Max Allowed Size Is 1MB."
                        };
                    }

                    var fakeFileName = Path.GetRandomFileName();
                    var uploadedFile = await _unitOfWork.UploadedFileRepository.GetByIdAsync(admin.UploadedFileId);
                    File.Delete(uploadedFile.FilePath);
                    uploadedFile.FileName = dto.Image.FileName;
                    uploadedFile.ContentType = dto.Image.ContentType;
                    uploadedFile.StoredFileName = fakeFileName;

                    var directoryPath = Path.Combine(_webHostEnvironment.ContentRootPath, "Uploads", "HospitalAdminImages");
                    var path = Path.Combine(directoryPath, fakeFileName);
                    using FileStream fileStream = new(path, FileMode.Create);
                    dto.Image.CopyTo(fileStream);

                    uploadedFile.FilePath = path;
                    _unitOfWork.UploadedFileRepository.Update(uploadedFile);
                    await _unitOfWork.CompleteAsync();
                }
                admin.Email = dto.Email ?? admin.Email;
                admin.UserName = dto.UserName ?? admin.UserName;
                admin.NationalId = dto.NationalId ?? admin.NationalId;
                if(dto.PassWord != null)
                {
                    var pw = HashingService.GetHash(dto.PassWord);
                    admin.PassWord = pw;
                    user.PassWord = admin.PassWord;
                }
                user.UserName = admin.Email;
                user.Email = admin.Email;
                user.NationalId = admin.NationalId;
                _unitOfWork.HospitalAdminRepository.Update(admin);
                _unitOfWork.UserRepository.Update(user);
                await _unitOfWork.CompleteAsync();
                
                var data = _mapper.Map<EditHospitalAdminResponse>(admin);

                return new GeneralResponse<EditHospitalAdminResponse>
                {
                    IsSuccess = true,
                    Message = "Admin Details have been Displayed successfully",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<EditHospitalAdminResponse>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }

      

        public async Task<GeneralResponse<List<ListOfHospitalAdminDto>>> ListOfSpecificHospitalAdmins(int HospitalId)
        {
            try
            {
                var hospital = await _unitOfWork.HospitalRepository.GetByIdAsync(HospitalId);
                if(hospital == null)
                {
                    return new GeneralResponse<List<ListOfHospitalAdminDto>>
                    {
                        IsSuccess = false,
                        Message = "No Hospital Found"
                    };
                }
                var adminsOfHospital = await _unitOfWork.AdminOfHospitalRepository.GetSpecificItems(filter: w => w.HospitalId == HospitalId, select: s => s.HospitalAdminId);
                var admins = await _unitOfWork.HospitalAdminRepository.WhereIncludeAsync(filter: i => adminsOfHospital.Contains(i.Id), a => a.UploadedFile );
                var data = _mapper.Map<List<ListOfHospitalAdminDto>>(admins);

                return new GeneralResponse<List<ListOfHospitalAdminDto>>
                {
                    IsSuccess = true,
                    Message = "Admins of Selected Hospital have been Listed Successfuly",
                    Data = data
                };

            }
            catch (Exception ex)
            {
                return new GeneralResponse<List<ListOfHospitalAdminDto>>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }

        public async Task<GeneralResponse<List<GovernorateDto>>> ListOfGovernorates()
        {
            try
            {
                var governorates = await _unitOfWork.GovernorateRepository.GetAllAsync();
                if (governorates == null)
                {
                    return new GeneralResponse<List<GovernorateDto>>
                    {
                        IsSuccess = false,
                        Message = "No Governorates Found!"
                    };
                }

                var data = _mapper.Map<List<GovernorateDto>>(governorates);

                return new GeneralResponse<List<GovernorateDto>>
                {
                    IsSuccess = true,
                    Message = "The Governorates Listed successfully.",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<List<GovernorateDto>>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }

        public async Task<GeneralResponse<string>> AddGovernorate(string name)
        {
            try
            {
                var governorate = new Governorate()
                {
                    Name = name
                };
                await _unitOfWork.GovernorateRepository.AddAsync(governorate);
                await _unitOfWork.CompleteAsync();

                return new GeneralResponse<string>
                {
                    IsSuccess = true,
                    Message = "New Governorate added successfully."
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

        public async Task<GeneralResponse<string>> DeleteGovernorate(int governorateId)
        {
            try
            {
                var governorate = await _unitOfWork.GovernorateRepository.GetByIdAsync(governorateId);
                if (governorate == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "Governorate Not Found!"
                    };
                }

                _unitOfWork.GovernorateRepository.Remove(governorate);
                await _unitOfWork.CompleteAsync();

                return new GeneralResponse<string>
                {
                    IsSuccess = true,
                    Message = "The Governorate Deleted successfully."
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
    }
}
