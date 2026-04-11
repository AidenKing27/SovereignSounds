using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SovereignSounds.Models;

namespace SovereignSounds.Pages.Music.Songs
{
    public class DetailsModel : PageModel
    {
        private readonly SovereignSounds.Data.ApplicationDbContext _context;

        public DetailsModel(SovereignSounds.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Song Song { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var song = await _context.Songs
                .Include(s => s.Album)
                .Include(s => s.Genres)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (song is not null)
            {
                Song = song;

                return Page();
            }

            return NotFound();
        }
    }
}
