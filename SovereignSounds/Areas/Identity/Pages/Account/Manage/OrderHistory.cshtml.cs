using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SovereignSounds.Data;
using SovereignSounds.Models;

namespace SovereignSounds.Areas.Identity.Pages.Account.Manage;

public class OrderHistoryModel : PageModel
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ApplicationDbContext _context;

    public OrderHistoryModel(UserManager<IdentityUser> userManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public List<OrderHistory> Orders { get; set; } = [];

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

        var customer = await _context.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.IdentityUserId == user.Id);

        if (customer == null)
            return NotFound("Customer profile not found.");

        Orders = await _context.OrderHistories
            .AsNoTracking()
            .Where(o => o.CustomerId == customer.Id)
            .Include(o => o.Items)
                .ThenInclude(i => i.MusicItem)
            .OrderByDescending(o => o.PurchaseDate)
            .ToListAsync();

        return Page();
    }
}
