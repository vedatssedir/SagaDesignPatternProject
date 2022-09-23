using Shared.Interface;

namespace Shared.Events
{
    public class OrderCreateRequestEvent : IOrderCreatedRequestEvent
    {
        public int OrderId { get; set; }
        public string BuyerId { get; set; }
        public List<OrderItemMessage> OrderItemMessages { get; set; } = new();
        public PaymentMessage Payment { get; set; }
    }
}
