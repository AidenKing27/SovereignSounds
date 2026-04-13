using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SovereignSounds.Data;
using SovereignSounds.Models;

namespace SovereignSounds.Pages.Music.Songs
{
    public class EditModel : PageModel
    {
        private readonly SovereignSounds.Data.ApplicationDbContext _context;

        public EditModel(SovereignSounds.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Song Song { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var song =  await _context.Songs.FirstOrDefaultAsync(m => m.Id == id);
            if (song == null)
            {
                return NotFound();
            }
            Song = song;
            Song.DurationInput = Song.Duration.ToString(@"mm\:ss");
            PopulateAlbumSelectList();
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                PopulateAlbumSelectList();
                return Page();
            }

            if (!TryParseDurationInput(Song.DurationInput, out var duration))
            {
                ModelState.AddModelError("Song.DurationInput", "Song Duration must match: 3:15, 03:15, 12:00, 0:59");
                PopulateAlbumSelectList();
                return Page();
            }

            Song.Duration = duration;

            _context.Attach(Song).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SongExists(Song.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./View");
        }

        private bool SongExists(int id)
        {
            return _context.Songs.Any(e => e.Id == id);
        }

        private void PopulateAlbumSelectList()
        {
            ViewData["AlbumId"] = new SelectList(_context.Albums, "Id", "Artist");
        }

        private static bool TryParseDurationInput(string? durationInput, out TimeSpan duration)
        {
            duration = TimeSpan.Zero;

            if (string.IsNullOrWhiteSpace(durationInput))
            {
                return false;
            }

            return TimeSpan.TryParseExact(durationInput, @"m\:ss", null, out duration)
                || TimeSpan.TryParseExact(durationInput, @"mm\:ss", null, out duration);
        }
    }
}
