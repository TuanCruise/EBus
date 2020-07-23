using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using WebCore.Entities.Entities;

namespace EBus.Publish.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class OrderController : ControllerBase
    {
        readonly ILogger<OrderController> _logger;
        readonly IRequestClient<SubmitOrder> _submitOrderRequestClient;
        readonly ISendEndpointProvider _sendEndpointProvider;
        readonly IRequestClient<CheckOrder> _checkOrderClient;
        readonly IPublishEndpoint _publishEndpoint;

        private IBusControl _bus;

        public OrderController(IBusControl bus, ILogger<OrderController> logger, IRequestClient<SubmitOrder> submitOrderRequestClient,
            ISendEndpointProvider sendEndpointProvider, IRequestClient<CheckOrder> checkOrderClient, IPublishEndpoint publishEndpoint)
        {
            _bus = bus;
            _logger = logger;
            _submitOrderRequestClient = submitOrderRequestClient;
            _sendEndpointProvider = sendEndpointProvider;
            _checkOrderClient = checkOrderClient;
            _publishEndpoint = publishEndpoint;
        }
        [HttpGet]
        public string Get()
        {
            return "Welcome to Bus.Masstransit";
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(Orders order)
        {
            Uri uri = new Uri("rabbitmq://localhost/order_queue");
            var endPoint = await _bus.GetSendEndpoint(uri);
            await endPoint.Send(order);
            return Ok("success");



            // Test new code
            //var (accepted, rejected) = await _submitOrderRequestClient.GetResponse<OrderSubmissionAccepted, OrderSubmissionRejected>(new
            //{
            //    OrderId = order.Id,
            //    InVar.Timestamp,
            //    order.ProductName,
            //    order.Qtty,
            //});

            //if (accepted.IsCompletedSuccessfully)
            //{
            //    var response = await accepted;

            //    return Accepted(response);
            //}

            //if (accepted.IsCompleted)
            //{
            //    await accepted;

            //    return Problem("Order was not accepted");
            //}
            //else
            //{
            //    var response = await rejected;

            //    return BadRequest(response.Message);
            //}
        }

        [HttpPut]
        public async Task<IActionResult> Put(Guid id, string customerNumber)
        {
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("rabbitmq://localhost/order_queue"));

            await endpoint.Send<SubmitOrder>(new
            {
                OrderId = id,
                InVar.Timestamp,
                CustomerNumber = customerNumber
            });

            return Accepted();
        }
    }
}
