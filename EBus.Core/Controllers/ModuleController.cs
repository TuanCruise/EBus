using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
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
        readonly IRequestClient<OrderStatusResult> _requestClient;
        readonly IPublishEndpoint _publishEndpoint;
        static HttpClient client = new HttpClient();

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


                var result = await CallServiceDirect(moduleInfo);

                //var message = new ModuleInfo();
                //message.ModuleID = "03001";
                //message.ModuleName = "Le Minh Tuan";

                //var response = await _requestClient.GetResponse<ModuleInfo>(message);

                return Ok(result);
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

        private async Task<IActionResult> CallServiceDirect(ModuleInfo moduleInfo)
        {
            HttpResponseMessage response = await client.GetAsync("https://dog.ceo/api/breeds/image/random?");
            string result = string.Empty;
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsStringAsync();
            }
            return Ok(result);
        }

    }
}
