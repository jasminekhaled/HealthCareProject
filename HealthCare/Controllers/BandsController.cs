using HealthCare.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthCare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BandsController : ControllerBase
    {
        private readonly IBandServices _bandServices;

        public BandsController(IBandServices bandServices)
        {
            _bandServices = bandServices;
        }
    }
}
