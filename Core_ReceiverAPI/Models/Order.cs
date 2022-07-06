namespace Core_ReceiverAPI.Models
{
    public class Order
    {
        public string? CustomerName { get; set; }
        public string? OrderedItem { get; set; }
        public int Quantity { get; set; }
        public double AdvanceAmount { get; set; }
    }


    public class Message
    {
        public Order Order { get; set; }
        public double TotalAmount { get; set; }
        public double Advance { get; set; }
    }
}
