namespace SovereignSounds.Models;

public class OwnedItem
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
    public int MusicItemId { get; set; }
    public MusicItem MusicItem { get; set; } = null!;
    public DateTime AcquiredDate { get; set; }
}
