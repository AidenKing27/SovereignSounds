using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SovereignSounds.Data;
using SovereignSounds.Models;

namespace SovereignSounds.Pages;

[Authorize(Roles = "User,Admin")]
public class CheckoutModel : PageModel
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ApplicationDbContext _context;

    public CheckoutModel(
        UserManager<IdentityUser> userManager,
        ApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public List<MusicItemDto> Cart { get; set; } = [];
    public Customer? Customer { get; set; }
    [BindProperty]
    public OrderConfirmationData Data { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        Cart = [];
        var sessionCart = HttpContext.Session.GetString("Cart");
        if (sessionCart is not null)
            Cart = JsonConvert.DeserializeObject<List<MusicItemDto>>(sessionCart)!;

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

        Customer = await _context.Customers
            .Include(c => c.IdentityUser)
            .Include(c => c.OrderHistories)
                .ThenInclude(oh => oh.Items)
                    .ThenInclude(oi => oi.MusicItem)
            .FirstOrDefaultAsync(c => c.IdentityUserId == user.Id);

        if (Customer == null)
            return NotFound("Customer profile not found.");


        Data.FirstName = Customer.FirstName;
        Data.LastName = Customer.LastName;
        Data.Email = Customer.IdentityUser.Email!;
        Data.Phone = Customer.IdentityUser.PhoneNumber!;
        Data.CreditCardName = $"{Customer.FirstName} {Customer.LastName}".ToUpper();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        Cart = [];
        var sessionCart = HttpContext.Session.GetString("Cart");
        if (sessionCart is not null)
            Cart = JsonConvert.DeserializeObject<List<MusicItemDto>>(sessionCart)!;

        if (Cart.Count == 0)
            return RedirectToPage("/ShoppingCart");

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

        Customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.IdentityUserId == user.Id);

        if (Customer == null)
            return NotFound("Customer profile not found.");

        List<int> cartIds = Cart.Select(c => c.Id).ToList();
        Dictionary<int, MusicItem> musicItems = await _context.MusicItems
            .Where(mi => cartIds.Contains(mi.Id))
            .ToDictionaryAsync(mi => mi.Id);

        if (musicItems.Count == 0)
            return RedirectToPage("/ShoppingCart");

        OrderHistory order = new()
        {
            CustomerId = Customer.Id,
            PurchaseDate = DateTime.Now,
            ShippingAddress = Data.ShippingAddress
        };

        List<int> existingOwnedIds = await _context.OwnedItems
            .Where(oi => oi.CustomerId == Customer.Id)
            .Select(oi => oi.MusicItemId)
            .ToListAsync();

        List<MusicItemDto> albumsBeingPurchased = [];
        foreach (MusicItemDto cartItem in Cart)
        {
            if (!musicItems.TryGetValue(cartItem.Id, out var mi))
                continue;

            OrderItem oi = new()
            {
                MusicItemId = mi.Id,
                PurchasePrice = mi.Price,
                Order = order
            };

            order.Items.Add(oi);

            if (!existingOwnedIds.Contains(cartItem.Id))
            {
                _context.OwnedItems.Add(new OwnedItem
                {
                    CustomerId = Customer.Id,
                    MusicItemId = cartItem.Id,
                    AcquiredDate = DateTime.Now
                });
            }

            if (cartItem.ItemType == "Album")
                albumsBeingPurchased.Add(cartItem);
        }

        foreach (MusicItemDto album in albumsBeingPurchased)
        {
            Album alb = await _context.Albums.FirstOrDefaultAsync(a => a.Id == album.Id);

            List<int> albumSongIds = await _context.Songs
                .Where(s => s.AlbumId == alb.Id)
                .Select(s => s.Id)
                .ToListAsync();

            foreach (int songId in albumSongIds)
            {
                if (!existingOwnedIds.Any(x => x == songId))
                {
                    _context.OwnedItems.Add(new OwnedItem
                    {
                        CustomerId = Customer.Id,
                        MusicItemId = songId,
                        AcquiredDate = DateTime.Now
                    });
                }
            }
        }

        if (order.Items.Count == 0)
            return RedirectToPage("/ShoppingCart");

        order.CartTotal = order.Items.Sum(i => i.PurchasePrice);
        decimal tax = order.CartTotal * 0.05m;
        decimal aidenCut = order.CartTotal * 0.50m;
        order.GrandTotal = order.CartTotal + tax + aidenCut;

        _context.OrderHistories.Add(order);
        await _context.SaveChangesAsync();

        HttpContext.Session.Remove("Cart");
        return RedirectToPage("/Profile");
    }
}
