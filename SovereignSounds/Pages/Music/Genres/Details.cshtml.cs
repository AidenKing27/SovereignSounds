using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SovereignSounds.Data;
using SovereignSounds.Models;

namespace SovereignSounds.Pages.Music.Genres
{
    public class DetailsModel : PageModel
    {
        private readonly SovereignSounds.Data.ApplicationDbContext _context;

        public DetailsModel(SovereignSounds.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Genre Genre { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genre = await _context.Genres.FirstOrDefaultAsync(m => m.Id == id);

            if (genre is not null)
            {
                Genre = genre;

                return Page();
            }

            return NotFound();
        }
    }
}
