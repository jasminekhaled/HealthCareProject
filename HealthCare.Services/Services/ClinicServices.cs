using AutoMapper;
using HealthCare.Core.DTOS;
using HealthCare.Core.DTOS.ClinicModule.RequestDto;
using HealthCare.Core.DTOS.ClinicModule.ResponseDto;
using HealthCare.Core.DTOS.DoctorModule.ResponseDtos;
using HealthCare.Core.Helpers;
using HealthCare.Core.IRepositories;
using HealthCare.Core.Models.AppointmentModule;
using HealthCare.Core.Models.AuthModule;
using HealthCare.Core.Models.ClinicModule;
using HealthCare.Core.Models.DoctorModule;
using HealthCare.Core.Models.HospitalModule;
using HealthCare.Services.IServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Services.Services
{
    public class ClinicServices : IClinicServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClinicServices(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment webHostEnvironment,
             IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<GeneralResponse<string>> AddXraySpecialization([FromForm]SpecializationRequestDto dto)
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
                var directoryPath = Path.Combine(_webHostEnvironment.ContentRootPath, "Uploads", "XraySpecializationImages");
                var path = Path.Combine(directoryPath, fakeFileName);
                using FileStream fileStream = new(path, FileMode.Create);
                dto.Image.CopyTo(fileStream);
                uploadedFile.FilePath = path;
                await _unitOfWork.UploadedFileRepository.AddAsync(uploadedFile);
                await _unitOfWork.CompleteAsync();

                var xraySpecialization = new XraySpecialization()
                {
                    Name = dto.Name,
                    UploadedFile = uploadedFile
                };
                await _unitOfWork.XraySpecializationRepository.AddAsync(xraySpecialization);
                await _unitOfWork.CompleteAsync();

                return new GeneralResponse<string>
                {
                    IsSuccess = true,
                    Message = "New X-raySpecialization added successfully."
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

        public async Task<GeneralResponse<string>> DeleteXraySpecialization(int xraySpecializationId)
        {
            try
            {
                var xraySpecialization = await _unitOfWork.XraySpecializationRepository.GetByIdAsync(xraySpecializationId);
                if (xraySpecialization == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "X-raySpecialization Not Found!"
                    };
                }
                var uploadedFile = await _unitOfWork.UploadedFileRepository.GetByIdAsync(xraySpecialization.UploadedFileId);
                File.Delete(uploadedFile.FilePath);
                _unitOfWork.UploadedFileRepository.Remove(uploadedFile);
                _unitOfWork.XraySpecializationRepository.Remove(xraySpecialization);
                await _unitOfWork.CompleteAsync();

                return new GeneralResponse<string>
                {
                    IsSuccess = true,
                    Message = "The X-raySpecialization Deleted successfully."
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

        public async Task<GeneralResponse<string>> AddLabSpecialization([FromForm] LabSpecializationRequestDto dto)
        {
            try
            {
                var uploadedFile = new UploadedFile()
                {
                    FileName = "chemical - analysis(1).png",
                    ContentType = "image/png",
                    StoredFileName = "1z5ysxnp.th1",
                    FilePath = "G:\\WEB DEVELOPMENT\\HealthCareProject\\HealthCareAPIs\\HealthCare\\Uploads\\LabSpecializationImages\\1z5ysxnp.th1"
                };
                await _unitOfWork.UploadedFileRepository.AddAsync(uploadedFile);
                await _unitOfWork.CompleteAsync();

                var labSpecialization = new LabSpecialization()
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    UploadedFile = uploadedFile
                };
                await _unitOfWork.LabSpecializationRepository.AddAsync(labSpecialization);
                await _unitOfWork.CompleteAsync();

                return new GeneralResponse<string>
                {
                    IsSuccess = true,
                    Message = "New LabSpecialization added successfully."
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

        public async Task<GeneralResponse<string>> DeleteLabSpecialization(int labSpecializationId)
        {
            try
            {
                var labSpecialization = await _unitOfWork.LabSpecializationRepository.GetByIdAsync(labSpecializationId);
                if (labSpecialization == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "LabSpecialization Not Found!"
                    };
                }
                var uploadedFile = await _unitOfWork.UploadedFileRepository.GetByIdAsync(labSpecialization.UploadedFileId);
                File.Delete(uploadedFile.FilePath);
                _unitOfWork.UploadedFileRepository.Remove(uploadedFile);
                _unitOfWork.LabSpecializationRepository.Remove(labSpecialization);
                await _unitOfWork.CompleteAsync();

                return new GeneralResponse<string>
                {
                    IsSuccess = true,
                    Message = "The LabSpecialization Deleted successfully."
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

        public async Task<GeneralResponse<AddClinicResponseDto>> AddClinic([FromForm]AddClinicDto dto)
        { 
            try
            {
                HttpContext httpContext = _httpContextAccessor.HttpContext;
                int userId = httpContext.FindFirst();
                var ThisUser = await _unitOfWork.UserRepository.SingleOrDefaultAsync(
                    a => a.Id == userId && a.Role == User.HospitalAdmin);

                if (ThisUser == null)
                {
                    return new GeneralResponse<AddClinicResponseDto>
                    {
                        IsSuccess = false,
                        Message = "No HospitalAdmin Found!"
                    };
                }

                var hospitalAdmin = await _unitOfWork.HospitalAdminRepository.GetSingleWithIncludesAsync(
                    s => s.UserName == ThisUser.UserName , i => i.AdminOfHospital);

                var specialization = await _unitOfWork.SpecializationRepository.GetSingleWithIncludesAsync(
                    single: s => s.Id == dto.SpecializationId, includes: i => i.UploadedFile);
                if (specialization == null)
                {
                    return new GeneralResponse<AddClinicResponseDto>
                    {
                        IsSuccess = false,
                        Message = "No specialization Found!"
                    };
                }
                var hospital = await _unitOfWork.HospitalRepository.GetByIdAsync(hospitalAdmin.AdminOfHospital.HospitalId);
                var clinic = new ClinicLab()
                {
                    Specialization = specialization,
                    Hospital = hospital
                };
                await _unitOfWork.ClinicLabRepository.AddAsync(clinic);
                await _unitOfWork.CompleteAsync();
                var data = _mapper.Map<AddClinicResponseDto>(specialization);
                data.Id = clinic.Id;
                return new GeneralResponse<AddClinicResponseDto>
                {
                    IsSuccess = true,
                    Message = "New Clinic is Added successfully.",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<AddClinicResponseDto>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }

        public async Task<GeneralResponse<AddClinicResponseDto>> AddXrayLab([FromForm]AddClinicDto dto)
        {
            try
            {
                HttpContext httpContext = _httpContextAccessor.HttpContext;
                int userId = httpContext.FindFirst();
                var ThisUser = await _unitOfWork.UserRepository.SingleOrDefaultAsync(
                    a => a.Id == userId && a.Role == User.HospitalAdmin);

                if (ThisUser == null)
                {
                    return new GeneralResponse<AddClinicResponseDto>
                    {
                        IsSuccess = false,
                        Message = "No HospitalAdmin Found!"
                    };
                }

                var hospitalAdmin = await _unitOfWork.HospitalAdminRepository.GetSingleWithIncludesAsync(
                    s => s.UserName == ThisUser.UserName, i => i.AdminOfHospital);

                var xraySpecialization = await _unitOfWork.XraySpecializationRepository.GetSingleWithIncludesAsync(
                    single: s => s.Id == dto.SpecializationId, includes: i => i.UploadedFile);
                if (xraySpecialization == null)
                {
                    return new GeneralResponse<AddClinicResponseDto>
                    {
                        IsSuccess = false,
                        Message = "No xraySpecialization Found!"
                    };
                }
                
                var hospital = await _unitOfWork.HospitalRepository.GetByIdAsync(hospitalAdmin.AdminOfHospital.HospitalId);
                var xray = new Xray()
                {
                    XraySpecialization = xraySpecialization,
                    Hospital = hospital
                };
                await _unitOfWork.XrayRepository.AddAsync(xray);
                await _unitOfWork.CompleteAsync();
                var data = _mapper.Map<AddClinicResponseDto>(xraySpecialization);
                data.Id = xray.Id;
                return new GeneralResponse<AddClinicResponseDto>
                {
                    IsSuccess = true,
                    Message = "New Xray Lab is Added successfully.",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<AddClinicResponseDto>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }

        public async Task<GeneralResponse<string>> DeleteClinic(int clinicId)
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
                        Message = "No HospitalAdmin Found!"
                    };
                }
                var hospitalAdmin = await _unitOfWork.HospitalAdminRepository.GetSingleWithIncludesAsync(
                    s => s.UserName == ThisUser.UserName, i => i.AdminOfHospital);

                var clinic = await _unitOfWork.ClinicLabRepository.GetSingleWithIncludesAsync(
                    s => s.Id == clinicId && s.HospitalId == hospitalAdmin.AdminOfHospital.HospitalId);
                if (clinic == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "No clinic Found by this Id in your hospital!"
                    };
                }
                var clinicAppointments = await _unitOfWork.ClinicAppointmentRepository.Where(
                    w => w.ClinicLabId == clinicId);
                var allReservations = await _unitOfWork.AllReservationsRepository.Where(
                    w => w.Type == AllReservations.Clinic && w.RoomId == clinicId);
                _unitOfWork.AllReservationsRepository.RemoveRange(allReservations);
                _unitOfWork.ClinicAppointmentRepository.RemoveRange(clinicAppointments);
                _unitOfWork.ClinicLabRepository.Remove(clinic);
                await _unitOfWork.CompleteAsync();
                return new GeneralResponse<string>
                {
                    IsSuccess = true,
                    Message = "The Clinic is Deleted successfully."
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

        public async Task<GeneralResponse<string>> DeleteXrayLab(int xrayLabId)
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
                        Message = "No HospitalAdmin Found!"
                    };
                }
                var hospitalAdmin = await _unitOfWork.HospitalAdminRepository.GetSingleWithIncludesAsync(
                    s => s.UserName == ThisUser.UserName, i => i.AdminOfHospital);


                var xray = await _unitOfWork.XrayRepository.GetSingleWithIncludesAsync(
                    s => s.Id == xrayLabId && s.HospitalId == hospitalAdmin.AdminOfHospital.HospitalId);
                if (xray == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "No xray Found!"
                    };
                }

                var xrayAppointments = await _unitOfWork.XrayAppointmentRepository.Where(
                    w => w.XrayId == xrayLabId);
                var allReservations = await _unitOfWork.AllReservationsRepository.Where(
                    w => w.Type == AllReservations.Xray && w.RoomId == xrayLabId);
                _unitOfWork.AllReservationsRepository.RemoveRange(allReservations);
                _unitOfWork.XrayAppointmentRepository.RemoveRange(xrayAppointments);
                _unitOfWork.XrayRepository.Remove(xray);
                await _unitOfWork.CompleteAsync();

                return new GeneralResponse<string>
                {
                    IsSuccess = true,
                    Message = "The Xray Lab is Deleted successfully."
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
        //not necessary
        public async Task<GeneralResponse<List<AddClinicResponseDto>>> ListOfClinicsADoctorWorksin(int doctorId, int hospitalId)
        {
            try
            {
                var doctor = await _unitOfWork.DoctorRepository.GetByIdAsync(doctorId);
                if (doctor == null)
                {
                    return new GeneralResponse<List<AddClinicResponseDto>>
                    {
                        IsSuccess = false,
                        Message = "No doctor Found!"
                    };
                }
                var hospital = await _unitOfWork.HospitalRepository.GetByIdAsync(hospitalId);
                if (hospital == null)
                {
                    return new GeneralResponse<List<AddClinicResponseDto>>
                    {
                        IsSuccess = false,
                        Message = "No hospital Found!"
                    };
                }
                if(!await _unitOfWork.HospitalDoctorRepository.AnyAsync(a => a.HospitalId==hospitalId && a.DoctorId == doctorId))
                {
                    return new GeneralResponse<List<AddClinicResponseDto>>
                    {
                        IsSuccess = false,
                        Message = "The doctor canot be Found in this hospital!"
                    };
                }
                var clinics = await _unitOfWork.ClinicLabDoctorRepository.GetSpecificItems(filter: w => w.DoctorId == doctorId, select: s => s.ClinicLabId);
                var hosClinics = await _unitOfWork.ClinicLabRepository.GetSpecificItems(filter: s => s.HospitalId == hospitalId, select: s => s.Id);
                List<int> commonValues = clinics.Intersect(hosClinics).ToList();
                var doctorClinic = await _unitOfWork.ClinicLabRepository.WhereIncludeAsync(filter:w => commonValues.Contains(w.Id),  i => i.Specialization.UploadedFile, i => i.Specialization);

                var data = _mapper.Map<List<AddClinicResponseDto>>(doctorClinic);
                
                return new GeneralResponse<List<AddClinicResponseDto>>
                {
                    IsSuccess = true,
                    Message = "The clinics are listed successfully.",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<List<AddClinicResponseDto>>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }
        //not necessary
        public async Task<GeneralResponse<List<AddClinicResponseDto>>> ListOfXrayLabsADoctorWorksin(int doctorId, int hospitalId)
        {
            try
            {
                var doctor = await _unitOfWork.DoctorRepository.GetByIdAsync(doctorId);
                if (doctor == null)
                {
                    return new GeneralResponse<List<AddClinicResponseDto>>
                    {
                        IsSuccess = false,
                        Message = "No doctor Found!"
                    };
                }
                var hospital = await _unitOfWork.HospitalRepository.GetByIdAsync(hospitalId);
                if (hospital == null)
                {
                    return new GeneralResponse<List<AddClinicResponseDto>>
                    {
                        IsSuccess = false,
                        Message = "No hospital Found!"
                    };
                }
                if (!await _unitOfWork.HospitalDoctorRepository.AnyAsync(a => a.HospitalId == hospitalId && a.DoctorId == doctorId))
                {
                    return new GeneralResponse<List<AddClinicResponseDto>>
                    {
                        IsSuccess = false,
                        Message = "The doctor canot be Found in this hospital!"
                    };
                }
                var xrays = await _unitOfWork.XrayDoctorRepository.GetSpecificItems(filter: w => w.DoctorId == doctorId, select: s => s.XrayId);
                var hosXrays = await _unitOfWork.XrayRepository.GetSpecificItems(filter: s => s.HospitalId == hospitalId, select: s => s.Id);
                List<int> commonValues = xrays.Intersect(hosXrays).ToList();
                var doctorXray = await _unitOfWork.XrayRepository.WhereIncludeAsync(filter: w => commonValues.Contains(w.Id), i => i.XraySpecialization.UploadedFile, i => i.XraySpecialization);

                var data = _mapper.Map<List<AddClinicResponseDto>>(doctorXray);

                return new GeneralResponse<List<AddClinicResponseDto>>
                {
                    IsSuccess = true,
                    Message = "The Xray labs are listed successfully.",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<List<AddClinicResponseDto>>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }

        public async Task<GeneralResponse<List<AddClinicResponseDto>>> ListOfClinicsInHospital(int hospitalId)
        {
            try
            {
                var hospital = await _unitOfWork.HospitalRepository.GetByIdAsync(hospitalId);
                if (hospital == null)
                {
                    return new GeneralResponse<List<AddClinicResponseDto>>
                    {
                        IsSuccess = false,
                        Message = "No hospital Found!"
                    };
                }
                var clinics = await _unitOfWork.ClinicLabRepository.WhereIncludeAsync(filter:s => s.HospitalId == hospitalId, i => i.Specialization.UploadedFile, i => i.Specialization);
                var data = _mapper.Map<List<AddClinicResponseDto>>(clinics);

                return new GeneralResponse<List<AddClinicResponseDto>>
                {
                    IsSuccess = true,
                    Message = "The clinics are listed successfully.",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<List<AddClinicResponseDto>>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }
        public async Task<GeneralResponse<List<AddClinicResponseDto>>> ListOfXrayLabsInHospital(int hospitalId)
        {
            try
            {
                var hospital = await _unitOfWork.HospitalRepository.GetByIdAsync(hospitalId);
                if (hospital == null)
                {
                    return new GeneralResponse<List<AddClinicResponseDto>>
                    {
                        IsSuccess = false,
                        Message = "No hospital Found!"
                    };
                }
                var xrays = await _unitOfWork.XrayRepository.WhereIncludeAsync(filter: s => s.HospitalId == hospitalId, i => i.XraySpecialization.UploadedFile, i => i.XraySpecialization);
                var data = _mapper.Map<List<AddClinicResponseDto>>(xrays);

                return new GeneralResponse<List<AddClinicResponseDto>>
                {
                    IsSuccess = true,
                    Message = "The xray labs are listed successfully.",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<List<AddClinicResponseDto>>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }
  
        public async Task<GeneralResponse<AddLabResponseDto>> AddLab([FromForm] AddLabDto dto)
        {
            try
            {
                HttpContext httpContext = _httpContextAccessor.HttpContext;
                int userId = httpContext.FindFirst();
                var ThisUser = await _unitOfWork.UserRepository.SingleOrDefaultAsync(
                    a => a.Id == userId && a.Role == User.HospitalAdmin);

                if (ThisUser == null)
                {
                    return new GeneralResponse<AddLabResponseDto>
                    {
                        IsSuccess = false,
                        Message = "No HospitalAdmin Found!"
                    };
                }

                var hospitalAdmin = await _unitOfWork.HospitalAdminRepository.GetSingleWithIncludesAsync(
                    s => s.UserName == ThisUser.UserName, i => i.AdminOfHospital);

                var labSpecializations = new List<LabSpecialization>();
                foreach (var id in dto.SpecializationId)
                {
                    var labSpecialization = await _unitOfWork.LabSpecializationRepository.GetSingleWithIncludesAsync(
                        single: s => s.Id == id, includes: i => i.UploadedFile);
                    if (labSpecialization == null)
                    {
                        return new GeneralResponse<AddLabResponseDto>
                        {
                            IsSuccess = false,
                            Message = "Labspecialization not found!"
                        };
                    }
                    labSpecializations.Add(labSpecialization);
                }

                
                var hospital = await _unitOfWork.HospitalRepository.GetByIdAsync(hospitalAdmin.AdminOfHospital.HospitalId);
                var lab = new Lab()
                {
                    Name = "معمل تحاليل",
                    Hospital = hospital
                };
                var specializationOfLab = labSpecializations.Select(s => new SpecializationsOfLab
                {
                    Lab = lab,
                    LabSpecialization = s
                });
                
                await _unitOfWork.LabRepository.AddAsync(lab);
                await _unitOfWork.SpecializationsOfLabRepository.AddRangeAsync(specializationOfLab);
                await _unitOfWork.CompleteAsync();

                var data = _mapper.Map<AddLabResponseDto>(lab);
                data.ImagePath = UploadedFile.LabSpecializationImagePath;
                data.LabSpecializations = _mapper.Map<List<LabSpecializationDto>>(labSpecializations);

                return new GeneralResponse<AddLabResponseDto>
                {
                    IsSuccess = true,
                    Message = "New Lab is Added successfully.",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<AddLabResponseDto>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }

        public async Task<GeneralResponse<string>> DeleteLab(int labId) 
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
                        Message = "No HospitalAdmin Found!"
                    };
                }
                var hospitalAdmin = await _unitOfWork.HospitalAdminRepository.GetSingleWithIncludesAsync(
                    s => s.UserName == ThisUser.UserName, i => i.AdminOfHospital);


                var lab = await _unitOfWork.LabRepository.GetSingleWithIncludesAsync(
                    s => s.Id == labId && s.HospitalId == hospitalAdmin.AdminOfHospital.HospitalId);
                if (lab == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "No Lab Found by this id in your hospital!"
                    };
                }

                var labAppointments = await _unitOfWork.LabAppointmentRepository.Where(
                    w => w.LabId == labId);
                var allReservations = await _unitOfWork.AllReservationsRepository.Where(
                    w => w.Type == AllReservations.Lab && w.RoomId == labId);
                _unitOfWork.AllReservationsRepository.RemoveRange(allReservations);
                _unitOfWork.LabAppointmentRepository.RemoveRange(labAppointments);
                _unitOfWork.LabRepository.Remove(lab);
                await _unitOfWork.CompleteAsync();

                return new GeneralResponse<string>
                {
                    IsSuccess = true,
                    Message = "The lab is Deleted successfully."
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
      
        public async Task<GeneralResponse<List<AddLabResponseDto>>> ListOfLabsInHospital(int hospitalId)
        {
            try
            {
                var hospital = await _unitOfWork.HospitalRepository.GetByIdAsync(hospitalId);
                if (hospital == null)
                {
                    return new GeneralResponse<List<AddLabResponseDto>>
                    {
                        IsSuccess = false,
                        Message = "No hospital Found!"
                    };
                }
                var labs = await _unitOfWork.LabRepository.WhereIncludeAsync(filter: s => s.HospitalId == hospitalId);
                var data = _mapper.Map<List<AddLabResponseDto>>(labs);
                
                foreach(var a in data)
                {
                    a.ImagePath = Lab.LabImagePath;
                    var labSpecializatons = await _unitOfWork.SpecializationsOfLabRepository.GetSpecificItems(filter: s=>s.LabId == a.Id, select: s=>s.LabSpecialization);
                    a.LabSpecializations = _mapper.Map<List<LabSpecializationDto>>(labSpecializatons);
                }
                return new GeneralResponse<List<AddLabResponseDto>>
                {
                    IsSuccess = true,
                    Message = "The labs are listed successfully.",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<List<AddLabResponseDto>>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }
        //not necessary
        public async Task<GeneralResponse<List<AddLabResponseDto>>> ListOfLabsADoctorWorksin(int doctorId, int hospitalId)
        {
            try
            {
                var doctor = await _unitOfWork.DoctorRepository.GetByIdAsync(doctorId);
                if (doctor == null)
                {
                    return new GeneralResponse<List<AddLabResponseDto>>
                    {
                        IsSuccess = false,
                        Message = "No doctor Found!"
                    };
                }
                var hospital = await _unitOfWork.HospitalRepository.GetByIdAsync(hospitalId);
                if (hospital == null)
                {
                    return new GeneralResponse<List<AddLabResponseDto>>
                    {
                        IsSuccess = false,
                        Message = "No hospital Found!"
                    };
                }
                if (!await _unitOfWork.HospitalDoctorRepository.AnyAsync(a => a.HospitalId == hospitalId && a.DoctorId == doctorId))
                {
                    return new GeneralResponse<List<AddLabResponseDto>>
                    {
                        IsSuccess = false,
                        Message = "The doctor canot be Found in this hospital!"
                    };
                }
                var labs = await _unitOfWork.LabDoctorRepository.GetSpecificItems(filter: w => w.DoctorId == doctorId, select: s => s.LabId);
                var hosLabs = await _unitOfWork.LabRepository.GetSpecificItems(filter: s => s.HospitalId == hospitalId, select: s => s.Id);
                List<int> commonValues = labs.Intersect(hosLabs).ToList();
                var doctorLab = await _unitOfWork.LabRepository.WhereIncludeAsync(filter: w => commonValues.Contains(w.Id));

                var data = _mapper.Map<List<AddLabResponseDto>>(doctorLab);
                foreach (var a in data)
                {
                    a.ImagePath = "G:\\WEB DEVELOPMENT\\HealthCareProject\\HealthCareAPIs\\HealthCare\\Uploads\\LabSpecializationImages\\1z5ysxnp.th1";
                    var labSpecializatons = await _unitOfWork.SpecializationsOfLabRepository.GetSpecificItems(filter: s => s.LabId == a.Id, select: s => s.LabSpecialization);
                    a.LabSpecializations = _mapper.Map<List<LabSpecializationDto>>(labSpecializatons);
                }
                return new GeneralResponse<List<AddLabResponseDto>>
                {
                    IsSuccess = true,
                    Message = "The labs are listed successfully.",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<List<AddLabResponseDto>>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }

        public async Task<GeneralResponse<List<AddClinicResponseDto>>> ListOfXraySpecialization()
        {
            try
            {
                var xraySpecializations = await _unitOfWork.XraySpecializationRepository.GetAllIncludedAsync(a => a.UploadedFile);
                if (xraySpecializations == null)
                {
                    return new GeneralResponse<List<AddClinicResponseDto>>
                    {
                        IsSuccess = false,
                        Message = "No X-raySpecializations Found!"
                    };
                }

                var data = _mapper.Map<List<AddClinicResponseDto>>(xraySpecializations);

                return new GeneralResponse<List<AddClinicResponseDto>>
                {
                    IsSuccess = true,
                    Message = "The X-raySpecializations Listed successfully.",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<List<AddClinicResponseDto>>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }
       
        public async Task<GeneralResponse<List<ListOfLabSpecializationDto>>> ListOfLabSpecialization()
        {
            try
            {
                var labSpecializations = await _unitOfWork.LabSpecializationRepository.GetAllIncludedAsync(a => a.UploadedFile);
                if (labSpecializations == null)
                {
                    return new GeneralResponse<List<ListOfLabSpecializationDto>>
                    {
                        IsSuccess = false,
                        Message = "No LabSpecializations Found!"
                    };
                }

                var data = _mapper.Map<List<ListOfLabSpecializationDto>>(labSpecializations);

                return new GeneralResponse<List<ListOfLabSpecializationDto>>
                {
                    IsSuccess = true,
                    Message = "The LabSpecializations Listed successfully.",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<List<ListOfLabSpecializationDto>>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }

        public async Task<GeneralResponse<List<ListOfSpecificClinics>>> ListOfClinicsOfSpecificSpecialization(int specializationId)
        {
            try
            {
                var specalize = await _unitOfWork.SpecializationRepository.GetByIdAsync(specializationId);
                if(specalize == null)
                {
                    return new GeneralResponse<List<ListOfSpecificClinics>>
                    {
                        IsSuccess = false,
                        Message = "No specialization found."
                    };
                }
                var clinics = await _unitOfWork.ClinicLabRepository.WhereIncludeAsync(
                    s => s.SpecializationId == specializationId,
                    a => a.Hospital, a => a.Hospital.HospitalGovernorate.Governorate,
                    a => a.Hospital.UploadedFile, a => a.Specialization.UploadedFile);

                var data = _mapper.Map<List<ListOfSpecificClinics>>(clinics);

                return new GeneralResponse<List<ListOfSpecificClinics>>
                {
                    IsSuccess = true,
                    Message = "The Specific Clinics are Listed successfully.",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<List<ListOfSpecificClinics>>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }

        public async Task<GeneralResponse<List<ListOfSpecificClinics>>> ListOfXraysOfSpecificSpecialization(int xraySpecializationId)
        {
            try
            {
                var specalize = await _unitOfWork.XraySpecializationRepository.GetByIdAsync(xraySpecializationId);
                if (specalize == null)
                {
                    return new GeneralResponse<List<ListOfSpecificClinics>>
                    {
                        IsSuccess = false,
                        Message = "No XraySpecialization found."
                    };
                }
                var xrays = await _unitOfWork.XrayRepository.WhereIncludeAsync(
                    s => s.XraySpecializationId == xraySpecializationId,
                    a => a.Hospital, a => a.Hospital.HospitalGovernorate.Governorate,
                    a => a.Hospital.UploadedFile, a => a.XraySpecialization.UploadedFile);

                var data = _mapper.Map<List<ListOfSpecificClinics>>(xrays);

                return new GeneralResponse<List<ListOfSpecificClinics>>
                {
                    IsSuccess = true,
                    Message = "The Specific XrayLabs are Listed successfully.",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<List<ListOfSpecificClinics>>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }

        public async Task<GeneralResponse<List<ListOfSpecificLabs>>> ListOfLabsOfSpecificSpecialization(int labSpecializationId)
        {
            try
            {
                var specalize = await _unitOfWork.LabSpecializationRepository.GetByIdAsync(labSpecializationId);
                if (specalize == null)
                {
                    return new GeneralResponse<List<ListOfSpecificLabs>>
                    {
                        IsSuccess = false,
                        Message = "No LabSpecialization found."
                    };
                }
                var labIds = await _unitOfWork.SpecializationsOfLabRepository.GetSpecificItems(
                    s => s.LabSpecializationId == labSpecializationId, a => a.LabId);
                var labs = await _unitOfWork.LabRepository.WhereIncludeAsync(w => labIds.Contains(w.Id), 
                    a => a.Hospital, a => a.Hospital.HospitalGovernorate.Governorate,
                    a => a.Hospital.UploadedFile);

                var data = _mapper.Map<List<ListOfSpecificLabs>>(labs);
                foreach (var a in data)
                {
                    a.LabImagePath = Lab.LabImagePath;
                    var labSpecializatons = await _unitOfWork.SpecializationsOfLabRepository.GetSpecificItems(filter: s => s.LabId == a.LabId, select: s => s.LabSpecialization.Name);
                    a.LabSpecializationNames = labSpecializatons.ToList();
                }

                return new GeneralResponse<List<ListOfSpecificLabs>>
                {
                    IsSuccess = true,
                    Message = "The Specific TestLabs are Listed successfully.",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<List<ListOfSpecificLabs>>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }

        public async Task<GeneralResponse<List<ListOfSpecificClinics>>> FilterClinicsBySpecializeAndGovernorate(int SpecializationId, int GovernorateId)
        {
            try
            {
                var specalize = await _unitOfWork.SpecializationRepository.GetByIdAsync(SpecializationId);
                if (specalize == null)
                {
                    return new GeneralResponse<List<ListOfSpecificClinics>>
                    {
                        IsSuccess = false,
                        Message = "No specialization found."
                    };
                }
                var governorate = await _unitOfWork.GovernorateRepository.GetByIdAsync(GovernorateId);
                if (governorate == null)
                {
                    return new GeneralResponse<List<ListOfSpecificClinics>>
                    {
                        IsSuccess = false,
                        Message = "No governorate found."
                    };
                }
                var clinics = await _unitOfWork.ClinicLabRepository.WhereIncludeAsync(
                    s => s.SpecializationId == SpecializationId &&
                    s.Hospital.HospitalGovernorate.GovernorateId == GovernorateId,
                    a => a.Hospital, a => a.Hospital.HospitalGovernorate.Governorate,
                    a => a.Hospital.UploadedFile, a => a.Specialization.UploadedFile);

                var data = _mapper.Map<List<ListOfSpecificClinics>>(clinics);

                return new GeneralResponse<List<ListOfSpecificClinics>>
                {
                    IsSuccess = true,
                    Message = "The Specific Clinics are Listed successfully.",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<List<ListOfSpecificClinics>>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }

        public async Task<GeneralResponse<List<ListOfSpecificClinics>>> FilterXraysBySpecializeAndGovernorate(int xraySpecializationId, int GovernorateId)
        {
            try
            {
                var specalize = await _unitOfWork.XraySpecializationRepository.GetByIdAsync(xraySpecializationId);
                if (specalize == null)
                {
                    return new GeneralResponse<List<ListOfSpecificClinics>>
                    {
                        IsSuccess = false,
                        Message = "No XraySpecialization found."
                    };
                }
                var governorate = await _unitOfWork.GovernorateRepository.GetByIdAsync(GovernorateId);
                if (governorate == null)
                {
                    return new GeneralResponse<List<ListOfSpecificClinics>>
                    {
                        IsSuccess = false,
                        Message = "No governorate found."
                    };
                }

                var xrays = await _unitOfWork.XrayRepository.WhereIncludeAsync(
                    s => s.XraySpecializationId == xraySpecializationId &&
                    s.Hospital.HospitalGovernorate.GovernorateId == GovernorateId,
                    a => a.Hospital, a => a.Hospital.HospitalGovernorate.Governorate,
                    a => a.Hospital.UploadedFile, a => a.XraySpecialization.UploadedFile);

                var data = _mapper.Map<List<ListOfSpecificClinics>>(xrays);

                return new GeneralResponse<List<ListOfSpecificClinics>>
                {
                    IsSuccess = true,
                    Message = "The Specific XrayLabs are Listed successfully.",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<List<ListOfSpecificClinics>>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }

        public async Task<GeneralResponse<List<ListOfSpecificLabs>>> FilterLabsBySpecializeAndGovernorate(int labSpecializationId, int GovernorateId)
        {
            try
            {
                var specalize = await _unitOfWork.LabSpecializationRepository.GetByIdAsync(labSpecializationId);
                if (specalize == null)
                {
                    return new GeneralResponse<List<ListOfSpecificLabs>>
                    {
                        IsSuccess = false,
                        Message = "No LabSpecialization found."
                    };
                }
                var governorate = await _unitOfWork.GovernorateRepository.GetByIdAsync(GovernorateId);
                if (governorate == null)
                {
                    return new GeneralResponse<List<ListOfSpecificLabs>>
                    {
                        IsSuccess = false,
                        Message = "No governorate found."
                    };
                }

                var labIds = await _unitOfWork.SpecializationsOfLabRepository.GetSpecificItems(
                    s => s.LabSpecializationId == labSpecializationId, a => a.LabId);
                var labs = await _unitOfWork.LabRepository.WhereIncludeAsync(w => labIds.Contains(w.Id) && 
                    w.Hospital.HospitalGovernorate.GovernorateId == GovernorateId,
                    a => a.Hospital, a => a.Hospital.HospitalGovernorate.Governorate,
                    a => a.Hospital.UploadedFile);

                var data = _mapper.Map<List<ListOfSpecificLabs>>(labs);
                foreach (var a in data)
                {
                    a.LabImagePath = Lab.LabImagePath;
                    var labSpecializatons = await _unitOfWork.SpecializationsOfLabRepository.GetSpecificItems(filter: s => s.LabId == a.LabId, select: s => s.LabSpecialization.Name);
                    a.LabSpecializationNames = labSpecializatons.ToList();
                }

                return new GeneralResponse<List<ListOfSpecificLabs>>
                {
                    IsSuccess = true,
                    Message = "The Specific TestLabs are Listed successfully.",
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<List<ListOfSpecificLabs>>
                {
                    IsSuccess = false,
                    Message = "Something went wrong",
                    Error = ex
                };
            }
        }
    }
}
