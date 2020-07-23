using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebCore.Common;
using WebCore.Entities;
using WebCore.Entities.Entities;

namespace EBus.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModuleController : BaseController
    {
        private IConfiguration _configuration;
        private IBusControl _bus;
        private string _transID;
        readonly IRequestClient<SubmitOrder> _requestClient;
        readonly IPublishEndpoint _publishEndpoint;

        public ModuleController(IConfiguration configuration, IBusControl bus, IPublishEndpoint publishEndpoin) : base(configuration)
        {
            _configuration = configuration;
            _bus = bus;
            _transID = Guid.NewGuid().ToString();
            _publishEndpoint = publishEndpoin;
        }
        [HttpGet]
        [Route("GetAllModule")]
        public async Task<ActionResult> GetAllModule()
        {
            try
            {
                InitializeModulesInfo();
                var data = JsonConvert.SerializeObject(AllCaches.ModulesInfo);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [HttpGet("{moduleID}")]
        [Route("GetModule")]
        public async Task<ActionResult> GetModule(string moduleID)
        {
            try
            {
                InitializeModulesInfo();

                var moduleInfo = (from module in AllCaches.ModulesInfo
                                  where module.ModuleID == moduleID && module.SubModule == "MMN"
                                  select module).SingleOrDefault();

                Uri uri = new Uri("rabbitmq://localhost/moduleinfo");
                var endPoint = await _bus.GetSendEndpoint(uri);
                await endPoint.Send(moduleInfo);

                return Ok("success");
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        private void InitializeModulesInfo()
        {
            var m_Client = new CoreController(_configuration);

            AllCaches.ModulesInfo = m_Client.BuildModulesInfo();
            AllCaches.ModuleFieldsInfo = m_Client.BuildModuleFieldsInfo();
        }


        [HttpPost]
        public async Task<IActionResult> Post(string id, string customerNumber)
        {
            var message = new ModuleInfo();
            message.ModuleID = "03001";
            message.ModuleName = "Le Minh Tuan";

            await _publishEndpoint.Publish<ModuleInfo>(message);

            return Ok();
        }

    }
}
