using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared;
using Stock.API.Models;

namespace Stock.API.Consumers
{
    public class OrderCreatedEventConsumer : IConsumer<OrderCreatedEvent>
    {
        private readonly MainDbContext _mainContext;
        private readonly ISendEndpointProvider _sendEndpointProvider;
        private readonly IPublishEndpoint _publishEndpoint;
        public OrderCreatedEventConsumer(MainDbContext mainContext, ISendEndpointProvider sendEndpointProvider, IPublishEndpoint publishEndpoint)
        {
            _mainContext = mainContext;
            _sendEndpointProvider = sendEndpointProvider;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            var stockResult = new List<bool>();
            foreach (var item in context.Message.orderItems)
            {
                stockResult.Add(await _mainContext.StockItems.AnyAsync(x => x.ProductId == item.ProductId && x.Count > item.Count));
            }

            if (stockResult.All(x => x.Equals(true)))
            {
                foreach (var item in context.Message.orderItems)
                {
                    var stock = await _mainContext.StockItems.FirstOrDefaultAsync(x => x.ProductId == item.ProductId);
                    if (stock != null)
                    {
                        stock.Count = -item.Count;
                    }

                    await _mainContext.SaveChangesAsync();
                    var sendEndpoints = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{RabbitMqSettingsConst.StockReservedEventQueueName}"));
                    var stockReservedEvent = new StockReservedEvent
                    {
                        Payment = context.Message.Payment,
                        BuyerId = context.Message.BuyerId,
                        OrderId = context.Message.OrderId,
                        OrderItems = context.Message.orderItems
                    };
                    await sendEndpoints.Send(stockReservedEvent);

                }
            }
            else
            {
                await _publishEndpoint.Publish(new StockNotReservedEvent()
                {
                    OrderId = context.Message.OrderId,
                    Message = "Not enough stock"
                });
            }
        }
    }
}
