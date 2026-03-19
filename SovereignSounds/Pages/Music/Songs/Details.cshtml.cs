using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SovereignSounds.Data;
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

            var song = await _context.Songs.FirstOrDefaultAsync(m => m.Id == id);

            if (song is not null)
            {
                Song = song;

                return Page();
            }

            return NotFound();
        }
    }
}
