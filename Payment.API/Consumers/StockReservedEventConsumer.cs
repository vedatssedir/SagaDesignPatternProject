using MassTransit;
using Shared;

namespace Payment.API.Consumers
{
    public class StockReservedEventConsumer : IConsumer<StockReservedEvent>
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<StockReservedEventConsumer> _logger;
        public StockReservedEventConsumer(IPublishEndpoint publishEndpoint, ILogger<StockReservedEventConsumer> logger)
        {
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }


        public async Task Consume(ConsumeContext<StockReservedEvent> context)
        {
            var balance = 3000m;


            if (balance > context.Message.Payment.TotalPrice)
            {
                _logger.LogInformation($"{context.Message.Payment.TotalPrice} TL was withdrawn from credit card for user id={context.Message.BuyerId}");
                await _publishEndpoint.Publish(new PaymentSuccessedEvent()
                {
                    BuyerId = context.Message.BuyerId,
                    OrderId = context.Message.OrderId
                });
            }
            else
            {
                _logger.LogInformation($"{context.Message.Payment.TotalPrice} TL was not width drawn form credit card for user id ={context.Message.BuyerId}");
                await _publishEndpoint.Publish(new PaymentFailEvent { BuyerId = context.Message.BuyerId, OrderId = context.Message.OrderId, Message = "not enough balance" });
            }
        }
    }
}
