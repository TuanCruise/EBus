using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using WebCore.Entities;
using WebCore.Entities.Entities;

namespace Core.Business.Consumer
{
    public class ModuleRequestConsumer : IConsumer<ModuleInfo>
    {
        ILogger<ModuleRequestConsumer> _logger;
        public ModuleRequestConsumer(ILogger<ModuleRequestConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ModuleInfo> context)
        {
            var modresult = new ModuleInfo();
            modresult.ModuleID = "1234";
            modresult.SubModule = "MAD";
            modresult.UIType = "T";
            await context.RespondAsync<ModuleInfo>(modresult);
        }
    }
}
