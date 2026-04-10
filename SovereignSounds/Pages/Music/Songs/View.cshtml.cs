using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SovereignSounds.Data;
using SovereignSounds.Models;

namespace SovereignSounds.Pages.Music.Songs;

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

    public IList<Song> Songs { get; set; } = default!;

    public async Task OnGetAsync(ListStyle style)
    {
        ListStyle = style;

        var query = _context.Songs
            .Include(s => s.Album)
            .Include(s => s.Genres)
            .OrderBy(s => s.Title)
            .AsQueryable();

        if (!string.IsNullOrEmpty(Search))
        {
            query = query.Where(s => 
            s.Title.Contains(Search) ||
            s.Artist.Contains(Search));
        }

        Songs = await query.ToListAsync();
    }

    public IActionResult OnPostSetListStyle(ListStyle style)
    {
        return RedirectToPage(new { style });
    }
}