using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SovereignSounds.Data;
using SovereignSounds.Models;

namespace SovereignSounds.Pages.Music.Albums;

public class ViewModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public ViewModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty(SupportsGet = true)]
    public string Search { get; set; } = string.Empty;

    [BindProperty]
    public ListStyle ListStyle { get; set; }

    public IList<Album> Albums { get; set; } = default!;

    public async Task OnGetAsync(ListStyle style)
    {
        ListStyle = style;

        var query = _context.Albums
            .Include(a => a.Songs)
            .Include(a => a.Genres)
            .OrderBy(a => a.Title)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(Search))
        {
            query = query.Where(a =>
                a.Title.Contains(Search) ||
                a.Artist.Contains(Search));
        }

        Albums = await query.ToListAsync();
    }

    public IActionResult OnPostSetListStyle(ListStyle style)
    {
        return RedirectToPage(new { style });
    }
}
