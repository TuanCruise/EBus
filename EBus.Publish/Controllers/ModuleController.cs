using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebCore.Common;

namespace EBus.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModuleController : BaseController
    {
        private IConfiguration _configuration;
        private IBusControl _bus;
        private string _transID;

        public ModuleController(IConfiguration configuration, IBusControl bus) : base(configuration)
        {
            _configuration = configuration;
            _bus = bus;
            _transID = Guid.NewGuid().ToString();
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


    }
}
