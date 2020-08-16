using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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
        readonly IRequestClient<ModuleInfo> _client;
        readonly IPublishEndpoint _publishEndpoint;
        static HttpClient client = new HttpClient();

        public ModuleController(IConfiguration configuration, IBusControl bus, IPublishEndpoint publishEndpoin, IRequestClient<ModuleInfo> client) : base(configuration)
        {
            _configuration = configuration;
            _bus = bus;
            _transID = Guid.NewGuid().ToString();
            _publishEndpoint = publishEndpoin;
            _client = client;
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

        [HttpGet]
        [Route("GetModule")]
        public async Task<ActionResult> GetModule(string moduleID, string subModId, string valuesparam)
        {
            try
            {
                string json = string.Format("[{0}]", valuesparam);
                JArray test = JArray.Parse(json);

                InitializeModulesInfo();

                var moduleInfo = (from module in AllCaches.ModulesInfo
                                  where module.ModuleID == moduleID && module.SubModule == subModId
                                  select module).SingleOrDefault();

                var fielInfo = (from field in AllCaches.ModuleFieldsInfo
                                where field.ModuleID == moduleID && field.FieldGroup == WebCore.CODES.DEFMODFLD.FLDGROUP.PARAMETER
                                select field).ToList();


                List<string> values = new List<string>();
                for (int i = 0; i < test.Count; i++)
                {
                    values.Add(test[i].ToString());
                }
                //values.Add("812807addffceaa08a8fc97dddd63413"); // add keys
                //values.Add("AAPL"); // add symbol
                var result = await CallServiceDirect(moduleInfo, values);

                return Ok(result);

                //var message = new ModuleInfo();
                //message.ModuleID = "03001";
                //message.ModuleName = "Le Minh Tuan";

                //var response = await _client.GetResponse<ModuleInfo>(message);

                //return Ok(response.Message);
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
           
            var response = await _client.GetResponse<ModuleInfo>(message);


            return Ok(response.Message);
        }

        private async Task<IActionResult> CallServiceDirect(ModuleInfo moduleInfo, List<string> values)
        {
            var modEsb = (ModESBInfo)moduleInfo;

            HttpResponseMessage response = await client.GetAsync(ProcessUrl(modEsb.BaseURL, values));
            string result = string.Empty;
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsStringAsync();
            }
            return Ok(result);
        }
        private string ProcessUrl(string baseUrl, List<string> values)
        {
            return String.Format(baseUrl, values.ToArray()); ;
        }

    }
}
