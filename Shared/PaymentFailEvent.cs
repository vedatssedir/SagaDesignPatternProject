namespace Shared
{
    public class PaymentFailEvent
    {
        public int OrderId { get; set; }
        public string BuyerId { get; set; }
        public string Message { get; set; }
    }
}
