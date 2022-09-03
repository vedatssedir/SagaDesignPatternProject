using MassTransit;
using Order.API.Models;
using Shared;

namespace Order.API.Consumers
{
    public class PaymentFailedConsumer :IConsumer<PaymentFailEvent>
    {
        private readonly MainContext _mainContext;
        private readonly ILogger<PaymentFailedConsumer> _logger;

        public PaymentFailedConsumer(MainContext mainContext, ILogger<PaymentFailedConsumer> logger)
        {
            _mainContext = mainContext;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<PaymentFailEvent> context)
        {
            var order = await _mainContext.Orders.FindAsync(context.Message.OrderId);
            if (order is not null)
            {
                order.OrderStatus = OrderStatus.Fail;
                order.FailMesssage = context.Message.Message;
                await _mainContext.SaveChangesAsync();
            }
            else
            {
                _logger.LogInformation("Order not found");
            }
        }
    }
}
