namespace SovereignSounds.Models;

public class OrderHistory
{
    public int Id { get; set; }

    public int CustomerId { get; set; }
    public Customer Customer { get; set; }
    public int MusicItemId { get; set; }
    public MusicItem MusicItem { get; set; }
    public decimal PurchasePrice { get; set; }
    public DateTime PurchaseDate { get; set; }

}
