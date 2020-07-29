using MassTransit;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WebCore.Entities;

namespace Core.Business.Consumer
{
    public class ModueleRequestConsumer : IConsumer<ModuleInfo>
    {
        ILogger<ModueleRequestConsumer> _logger;
        public ModueleRequestConsumer(ILogger<ModueleRequestConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ModuleInfo> context)
        {
            _logger.LogInformation("Value: {Value}", context.Message.ModuleID);
        }
    }
}
