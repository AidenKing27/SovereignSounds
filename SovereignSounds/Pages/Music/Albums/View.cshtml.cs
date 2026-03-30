using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SovereignSounds.Data;
using SovereignSounds.Models;

namespace SovereignSounds.Pages.Music.Albums;

public class ViewModel : PageModel
{
    [BindProperty]
    public ListStyle ListStyle { get; set; }

    private readonly SovereignSounds.Data.ApplicationDbContext _context;

    public ViewModel(SovereignSounds.Data.ApplicationDbContext context)
    {
        _context = context;
    }

    public IList<Album> Album { get; set; } = default!;

    public async Task OnGetAsync(ListStyle style)
    {
        ListStyle = style;
        Album = await _context.Albums
            .Include(a => a.Songs)
            .ToListAsync();
    }

    public IActionResult OnPostSetListStyle(ListStyle style)
    {
        return RedirectToPage(new { style });
    }
}
