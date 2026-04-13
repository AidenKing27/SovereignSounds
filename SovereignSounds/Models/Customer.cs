using Microsoft.AspNetCore.Identity;

namespace SovereignSounds.Models;

public class Customer
{
    public int Id { get; set; }
    public string IdentityUserId { get; set; } = string.Empty;
    public IdentityUser IdentityUser { get; set; } = null!;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public List<OrderHistory> OrderHistories { get; set; } = new();
    public List<OwnedItem> OwnedItems { get; set; } = new();
}