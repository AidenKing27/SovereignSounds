namespace SovereignSounds.Models;

public class OrderItem
{
    public int Id { get; set; }
    public int MusicItemId { get; set; }
    public MusicItem MusicItem { get; set; }
    public decimal PurchasePrice { get; set; }
    public int OrderId { get; set; }
    public OrderHistory Order { get; set; }
}
