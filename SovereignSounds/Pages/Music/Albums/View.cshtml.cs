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

    private readonly ApplicationDbContext _context;

    public ViewModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public IList<Album> Albums { get; set; } = default!;

    public async Task OnGetAsync(ListStyle style)
    {
        ListStyle = style;
        Albums = await _context.Albums
            .Include(a => a.Songs)
            .Include(a => a.Genres)
            .ToListAsync();
    }

    public IActionResult OnPostSetListStyle(ListStyle style)
    {
        return RedirectToPage(new { style });
    }
}
