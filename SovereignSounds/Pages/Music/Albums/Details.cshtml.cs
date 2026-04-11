using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SovereignSounds.Data;
using SovereignSounds.Models;

namespace SovereignSounds.Pages.Music.Albums
{
    public class DetailsModel : PageModel
    {
        private readonly SovereignSounds.Data.ApplicationDbContext _context;

        public DetailsModel(SovereignSounds.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Album Album { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var album = await _context.Albums
                .Include(a => a.Genres)
                .Include(a => a.Songs)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (album is not null)
            {
                Album = album;

                return Page();
            }

            return NotFound();
        }
    }
}
