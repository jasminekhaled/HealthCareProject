using AutoMapper;
using HealthCare.Core.DTOS;
using HealthCare.Core.DTOS.ClinicModule.RequestDto;
using HealthCare.Core.DTOS.ClinicModule.ResponseDto;
using HealthCare.Core.DTOS.DoctorModule.ResponseDtos;
using HealthCare.Core.IRepositories;
using HealthCare.Core.Models.AuthModule;
using HealthCare.Core.Models.ClinicModule;
using HealthCare.Core.Models.DoctorModule;
using HealthCare.Core.Models.HospitalModule;
using HealthCare.Services.IServices;
using Microsoft.AspNetCore.Hosting;
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

        public ClinicServices(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
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

        public async Task<GeneralResponse<AddClinicResponseDto>> AddClinic(int hospitalAdminId, [FromForm]AddClinicDto dto)
        {
            try
            {
                var hospitalAdmin = await _unitOfWork.HospitalAdminRepository.GetByIdAsync(hospitalAdminId);
                if (hospitalAdmin == null)
                {
                    return new GeneralResponse<AddClinicResponseDto>
                    {
                        IsSuccess = false,
                        Message = "No hospitalAdmin Found!"
                    };
                }
                var specialization = await _unitOfWork.SpecializationRepository.GetSingleWithIncludesAsync(single: s => s.Id == dto.SpecializationId, includes: i => i.UploadedFile);
                if (specialization == null)
                {
                    return new GeneralResponse<AddClinicResponseDto>
                    {
                        IsSuccess = false,
                        Message = "No specialization Found!"
                    };
                }
                var adminOfHospital = await _unitOfWork.AdminOfHospitalRepository.SingleOrDefaultAsync(s => s.HospitalAdminId == hospitalAdmin.Id);
                var hospital = await _unitOfWork.HospitalRepository.GetByIdAsync(adminOfHospital.HospitalId);
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

        public async Task<GeneralResponse<AddClinicResponseDto>> AddXrayLab(int hospitalAdminId, [FromForm]AddClinicDto dto)
        {
            try
            {
                var hospitalAdmin = await _unitOfWork.HospitalAdminRepository.GetByIdAsync(hospitalAdminId);
                if (hospitalAdmin == null)
                {
                    return new GeneralResponse<AddClinicResponseDto>
                    {
                        IsSuccess = false,
                        Message = "No hospitalAdmin Found!"
                    };
                }
                var xraySpecialization = await _unitOfWork.XraySpecializationRepository.GetSingleWithIncludesAsync(single: s => s.Id == dto.SpecializationId, includes: i => i.UploadedFile);
                if (xraySpecialization == null)
                {
                    return new GeneralResponse<AddClinicResponseDto>
                    {
                        IsSuccess = false,
                        Message = "No xraySpecialization Found!"
                    };
                }
                var adminOfHospital = await _unitOfWork.AdminOfHospitalRepository.SingleOrDefaultAsync(s => s.HospitalAdminId == hospitalAdmin.Id);
                var hospital = await _unitOfWork.HospitalRepository.GetByIdAsync(adminOfHospital.HospitalId);
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
                var clinic = await _unitOfWork.ClinicLabRepository.GetByIdAsync(clinicId);
                if (clinic == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "No clinic Found!"
                    };
                }
                var clinicDoctors = await _unitOfWork.ClinicLabDoctorRepository.Where(w => w.ClinicLabId == clinicId);
                _unitOfWork.ClinicLabDoctorRepository.RemoveRange(clinicDoctors);
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
                var xray = await _unitOfWork.XrayRepository.GetByIdAsync(xrayLabId);
                if (xray == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "No xray lab Found!"
                    };
                }
                var xrayDoctors = await _unitOfWork.XrayDoctorRepository.Where(w => w.XrayId == xrayLabId);
                _unitOfWork.XrayDoctorRepository.RemoveRange(xrayDoctors);
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

        public async Task<GeneralResponse<AddLabResponseDto>> AddLab(int hospitalAdminId, [FromForm] AddLabDto dto)
        {
            try
            {
                var hospitalAdmin = await _unitOfWork.HospitalAdminRepository.GetByIdAsync(hospitalAdminId);
                if (hospitalAdmin == null)
                {
                    return new GeneralResponse<AddLabResponseDto>
                    {
                        IsSuccess = false,
                        Message = "No hospitalAdmin Found!"
                    };
                }

                var labSpecializations = new List<LabSpecialization>();
                foreach (var id in dto.SpecializationId)
                {
                    var labSpecialization = await _unitOfWork.LabSpecializationRepository.GetSingleWithIncludesAsync(single: s => s.Id == id, includes: i => i.UploadedFile);
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

                
                var adminOfHospital = await _unitOfWork.AdminOfHospitalRepository.SingleOrDefaultAsync(s => s.HospitalAdminId == hospitalAdmin.Id);
                var hospital = await _unitOfWork.HospitalRepository.GetByIdAsync(adminOfHospital.HospitalId);
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
                data.ImagePath = "G:\\WEB DEVELOPMENT\\HealthCareProject\\HealthCareAPIs\\HealthCare\\Uploads\\LabSpecializationImages\\1z5ysxnp.th1";
                data.Specializations = _mapper.Map<List<SpecializationDto>>(labSpecializations);

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
                var lab = await _unitOfWork.LabRepository.GetByIdAsync(labId);
                if (lab == null)
                {
                    return new GeneralResponse<string>
                    {
                        IsSuccess = false,
                        Message = "No Lab Found!"
                    };
                }
                var labDoctors = await _unitOfWork.LabDoctorRepository.Where(w => w.LabId == labId);
                _unitOfWork.LabDoctorRepository.RemoveRange(labDoctors);
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
                    a.ImagePath = "G:\\WEB DEVELOPMENT\\HealthCareProject\\HealthCareAPIs\\HealthCare\\Uploads\\LabSpecializationImages\\1z5ysxnp.th1";
                    var labSpecializatons = await _unitOfWork.SpecializationsOfLabRepository.GetSpecificItems(filter: s=>s.LabId == a.Id, select: s=>s.LabSpecialization);
                    a.Specializations = _mapper.Map<List<SpecializationDto>>(labSpecializatons);
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
                    a.Specializations = _mapper.Map<List<SpecializationDto>>(labSpecializatons);
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

        public async Task<GeneralResponse<List<AddClinicResponseDto>>> ListOfLabSpecialization()
        {
            try
            {
                var labSpecializations = await _unitOfWork.LabSpecializationRepository.GetAllIncludedAsync(a => a.UploadedFile);
                if (labSpecializations == null)
                {
                    return new GeneralResponse<List<AddClinicResponseDto>>
                    {
                        IsSuccess = false,
                        Message = "No LabSpecializations Found!"
                    };
                }

                var data = _mapper.Map<List<AddClinicResponseDto>>(labSpecializations);

                return new GeneralResponse<List<AddClinicResponseDto>>
                {
                    IsSuccess = true,
                    Message = "The LabSpecializations Listed successfully.",
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
    }
}
