using MassTransit;
using System;
using System.Threading.Tasks;
using WebCore.Entities;
using WebCore.Entities.Entities;

namespace Core.Business.Consumer
{
    public class ModuleRequestConsumer : IConsumer<ModuleInfo>
    {
        readonly IOrderRepository _orderRepository;

        public ModuleRequestConsumer(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task Consume(ConsumeContext<ModuleInfo> context)
        {
            //var order = await _orderRepository.Get(context.Message.OrderId);
            //if (order == null)
            //    throw new InvalidOperationException("Order not found");
            //v

            await context.RespondAsync<OrderStatusResult>(new
            {
                OrderId = "123",
                Timestamp = DateTime.Now,
                StatusCode = "abc",
                StatusText = "con ga con"
            });
        }
    }
}
