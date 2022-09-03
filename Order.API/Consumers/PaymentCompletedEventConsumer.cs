using MassTransit;
using Order.API.Models;
using Shared;

namespace Order.API.Consumers
{
    public class PaymentCompletedEventConsumer :IConsumer<PaymentSuccessedEvent>
    {
        private readonly MainContext _context;
        private readonly ILogger<PaymentCompletedEventConsumer> _logger;

        public PaymentCompletedEventConsumer(MainContext context, ILogger<PaymentCompletedEventConsumer> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<PaymentSuccessedEvent> context)
        {
            var order = await _context.Orders.FindAsync(context.Message.OrderId);
            if (order is not null)
            {
                order.OrderStatus = OrderStatus.Complete;
                await _context.SaveChangesAsync();
            }
            else
            {
                _logger.LogInformation("Order not found");
            }
        }
    }
}
