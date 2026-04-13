using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SovereignSounds.Data;
using SovereignSounds.Models;

namespace SovereignSounds.Pages;

[Authorize(Roles = "User,Admin")]
public class ProfileModel : PageModel
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ApplicationDbContext _context;

    public ProfileModel(
        UserManager<IdentityUser> userManager,
        ApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public Customer? Customer { get; set; }
    public List<Song> CustomerSongs { get; set; } = [];
    public List<Album> CustomerAlbums { get; set; } = [];

    [BindProperty(SupportsGet = true)]
    public LibrarySection View { get; set; } = LibrarySection.Songs;

    public async Task<IActionResult> OnGet()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

        Customer = await _context.Customers
            .Include(c => c.IdentityUser)
            .FirstOrDefaultAsync(c => c.IdentityUserId == user.Id);

        if (Customer == null)
            return NotFound("Customer profile not found.");

        List<MusicItem> ownedMusicItems = await _context.OwnedItems
            .Where(oi => oi.CustomerId == Customer.Id)
            .Include(oi => oi.MusicItem)
            .Select(oi => oi.MusicItem)
            .ToListAsync();

        var ownedAlbumIds = ownedMusicItems
            .OfType<Album>()
            .Select(a => a.Id)
            .Distinct()
            .ToList();

        CustomerSongs = ownedMusicItems
            .OfType<Song>()
            .DistinctBy(s => s.Id)
            .ToList();

        CustomerAlbums = await _context.Albums
            .Where(a => ownedAlbumIds.Contains(a.Id))
            .Include(a => a.Songs)
            .AsSplitQuery()
            .ToListAsync();

        return Page();
    }

    public IActionResult OnPostSetLibraryView(LibrarySection view)
    {
        return RedirectToPage(new { view });
    }
}
