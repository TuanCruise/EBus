using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using WebCore.Common;

namespace EBus.Publish.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModuleController : BaseController
    {
        private IConfiguration _configuration;

        public ModuleController(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;

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

        private void InitializeModulesInfo()
        {
            var m_Client = new CoreController(_configuration);

            AllCaches.ModulesInfo = m_Client.BuildModulesInfo();
            AllCaches.ModuleFieldsInfo = m_Client.BuildModuleFieldsInfo();
        }


    }
}
