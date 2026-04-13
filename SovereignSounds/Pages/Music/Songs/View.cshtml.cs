using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SovereignSounds.Data;
using SovereignSounds.Models;

namespace SovereignSounds.Pages.Music.Songs;

public class ViewModel : PageModel
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ApplicationDbContext _context;

    public ViewModel(
        UserManager<IdentityUser> userManager,
        ApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public Customer? Customer { get; set; }

    public List<int> OwnedSongIds { get; set; } = [];

    [BindProperty(SupportsGet = true)]
    public string Search { get; set; } = string.Empty;

    [BindProperty(SupportsGet = true)]
    public int PageNumber { get; set; } = 1;

    [BindProperty]
    public ListStyle ListStyle { get; set; }

    private const int PageSize = 24;
    public int TotalItems { get; private set; }
    public int TotalPages { get; private set; }

    public IList<Song> Songs { get; set; } = [];

    public async Task OnGetAsync(ListStyle style)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user is not null)
        {
            Customer = await _context.Customers
            .Include(c => c.IdentityUser)
            .FirstOrDefaultAsync(c => c.IdentityUserId == user.Id);
        }

        ListStyle = style;
        PageNumber = Math.Max(1, PageNumber);

        if (Customer is not null)
        {
            OwnedSongIds = _context.OwnedItems
            .Where(oi => oi.CustomerId == Customer.Id)
            .Select(oi => oi.MusicItemId)
            .ToList();
        }

        var query = _context.Songs
            .AsNoTracking()
            .Include(s => s.Album)
            .OrderBy(s => s.Title)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(Search))
        {
            var term = $"%{Search.Trim()}%";
            query = query.Where(s =>
                EF.Functions.Like(s.Title, term) ||
                EF.Functions.Like(s.Artist, term));
        }

        TotalItems = await query.CountAsync();
        TotalPages = Math.Max(1, (int)Math.Ceiling(TotalItems / (double)PageSize));

        if (PageNumber > TotalPages)
        {
            PageNumber = TotalPages;
        }

        Songs = await query
            .Skip((PageNumber - 1) * PageSize)
            .Take(PageSize)
            .ToListAsync();
    }

    public IActionResult OnPostSetListStyle(ListStyle style)
    {
        return RedirectToPage(new
        {
            style,
            search = Search,
            pageNumber = PageNumber
        });
    }
}