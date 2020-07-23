using Microsoft.AspNetCore.Mvc;

namespace EBus.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VersionController : ControllerBase
    {
        [HttpGet]

        public string Get()
        {
            return "Welcome to Bus.Masstransit";
        }
    }
}
