using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using WebCore.Entities;
using WebCore.Entities.Entities;

namespace Core.Business.Consumer
{
    public class ModuleConsumer : IConsumer<ModuleInfo>
    {
        ILogger<ModuleConsumer> _logger;

        public ModuleConsumer(ILogger<ModuleConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ModuleInfo> context)
        {            
            _logger.LogInformation("Value: {Value}", context.Message.ModuleID);
           
        }
    }
}
