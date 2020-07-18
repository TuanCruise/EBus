using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace EBus.Publish.Controllers
{
    public class BaseController : ControllerBase
    {
        protected IConfiguration Configuration;
        public string ConnectionString { get { return Configuration["ConfigApp:DbContext"]; } }

        public BaseController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

    }
}
