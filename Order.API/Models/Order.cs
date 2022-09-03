namespace Order.API.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string BuyerId { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public Address Address { get; set; }
        public string FailMesssage { get; set; }
        

    }

    public enum OrderStatus
    {
        Suspend,
        Complete,
        Fail
    }
}
