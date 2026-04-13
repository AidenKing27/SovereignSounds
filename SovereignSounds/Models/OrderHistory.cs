namespace SovereignSounds.Models;

public class OrderHistory
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public Customer Customer { get; set; }
    public List<OrderItem> Items { get; set; } = [];
    public DateTime PurchaseDate { get; set; }
    public string ShippingAddress { get; set; } = string.Empty;
    public decimal CartTotal { get; set; }
    public decimal GrandTotal { get; set; }
}
