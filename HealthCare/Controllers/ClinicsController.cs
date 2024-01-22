using HealthCare.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthCare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClinicsController : ControllerBase
    {
        private readonly IClinicServices _clinicServices;

        public ClinicsController(IClinicServices clinicServices)
        {
            _clinicServices = clinicServices;
        }
    }
}
