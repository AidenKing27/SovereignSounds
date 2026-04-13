using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SovereignSounds.Data;
using SovereignSounds.Models;

namespace SovereignSounds.Pages.Music.Albums;

public class ViewModel : PageModel
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ApplicationDbContext _context;
    private const int PageSize = 24;

    public ViewModel(
        UserManager<IdentityUser> userManager,
        ApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public Customer? Customer { get; set; }

    public List<int> OwnedAlbumIds { get; set; } = [];

    [BindProperty(SupportsGet = true)]
    public string Search { get; set; } = string.Empty;

    [BindProperty(SupportsGet = true)]
    public int PageNumber { get; set; } = 1;

    [BindProperty]
    public ListStyle ListStyle { get; set; }

    public int TotalItems { get; private set; }
    public int TotalPages { get; private set; }

    public IList<Album> Albums { get; set; } = [];

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
            OwnedAlbumIds = await _context.OwnedItems
                .Where(oi => oi.CustomerId == Customer.Id)
                .Select(oi => oi.MusicItemId)
                .ToListAsync();
        }

        var query = _context.Albums
            .AsNoTracking()
            .Include(a => a.Songs)
            .AsSplitQuery()
            .OrderBy(a => a.Title)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(Search))
        {
            var term = $"%{Search.Trim()}%";
            query = query.Where(a =>
                EF.Functions.Like(a.Title, term) ||
                EF.Functions.Like(a.Artist, term));
        }

        TotalItems = await query.CountAsync();
        TotalPages = Math.Max(1, (int)Math.Ceiling(TotalItems / (double)PageSize));

        if (PageNumber > TotalPages)
        {
            PageNumber = TotalPages;
        }

        Albums = await query
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