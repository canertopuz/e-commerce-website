namespace UyumsoftProject.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string UserId { get; set; }
        public int AddressId { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public Address Address { get; set; }
    }
}
