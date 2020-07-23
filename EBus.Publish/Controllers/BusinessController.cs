using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WebCore.Entities;

namespace EBus.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessController : ControllerBase
    {
        private IBusControl _bus;

        public BusinessController(IBusControl bus)
        {
            _bus = bus;
        }

        public async Task<IActionResult> ProcessBusiness(ModuleInfo moduleInfo)
        {
            Uri uri = new Uri("rabbitmq://localhost/moduleinfo");
            var endPoint = await _bus.GetSendEndpoint(uri);
            await endPoint.Send(moduleInfo);
            return Ok("success");
        }
    }
}
