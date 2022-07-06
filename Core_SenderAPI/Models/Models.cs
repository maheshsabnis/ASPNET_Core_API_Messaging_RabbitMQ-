namespace Core_SenderAPI.Models
{
    /// <summary>
    /// Order Object
    /// </summary>
    public class Order
    {
        public string? CustomerName { get; set; }
        public string? OrderedItem { get; set; }
        public int Quantity { get; set; }
        public double AdvanceAmount { get; set; }
      
    }
    /// <summary>
    /// The Item Information
    /// </summary>
    public class Item
    {
        public string? ItemName { get; set; }
        public double UnitPrice { get; set; }
    }
    /// <summary>
    /// The Response Object to return response to the Client
    /// </summary>
    public class ResponseObject
    {
        public long ApproveId { get; set; }
        public long RejectionId { get; set; }
        public string? Message { get; set; }
    }
    /// <summary>
    /// Message to write into the RabbitMQ
    /// </summary>
    public class Message
    {
        public Order Order { get; set; }
        public double TotalAmount { get; set; }
        public double Advance { get; set; }
    }
    /// <summary>
    /// Items Data
    /// </summary>
    public class ItemDb : List<Item>
    {
        public ItemDb()
        {
            Add(new Item() { ItemName = "Laptop", UnitPrice = 145000 });
            Add(new Item() { ItemName = "RAM", UnitPrice = 14000 });
            Add(new Item() { ItemName = "Router", UnitPrice = 15000 });
            Add(new Item() { ItemName = "SSD", UnitPrice = 18000 });
            Add(new Item() { ItemName = "HDD", UnitPrice = 4000 });
            Add(new Item() { ItemName = "Adapter", UnitPrice = 9000 });
            Add(new Item() { ItemName = "Keyboard", UnitPrice = 1000 });
            Add(new Item() { ItemName = "Mouse", UnitPrice = 200 });
            Add(new Item() { ItemName = "Headphone", UnitPrice = 1600 });
            Add(new Item() { ItemName = "Speaker", UnitPrice = 13000 });
        }
    }
}
