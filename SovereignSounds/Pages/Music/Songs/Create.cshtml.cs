using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<SelectListItem> AlbumOptions { get; set; } = default!;
        public List<SelectListItem> GenreOptions { get; set; } = default!;

        [BindProperty]
        public Song Song { get; set; } = default!;

        [BindProperty]
        public IFormFile ImageFile { get; set; }

        [BindProperty]
        [MinLength(1, ErrorMessage = "You must select at least one genre")]
        public List<int> SelectedGenreIds { get; set; } = new();

        public async Task OnGet()
        {
            await LoadAlbumOptions();
            await LoadGenreOptions();
        }

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("Song.Picture");
            ModelState.Remove("Song.Genres");

            if (!ModelState.IsValid)
            {
                await LoadAlbumOptions();
                await LoadGenreOptions();

                return Page();
            }

            Song.Duration = TimeSpan.ParseExact(Song.DurationInput, [@"m\:ss", @"mm\:ss"], null);

            Song.Genres = await _context.Genres
                .Where(g => SelectedGenreIds.Contains(g.Id))
                .ToListAsync();

            // add code to get the actual file from the form and save as serialized bitmap
            if (ImageFile != null && ImageFile.Length > 0)
            {
                // Optionally enforce a size or content-type policy here
                using var ms = new MemoryStream();
                await ImageFile.CopyToAsync(ms);
                var bytes = ms.ToArray();
                var base64 = Convert.ToBase64String(bytes);
                // Store as data URI so it can be used directly in <img src="..."> if desired
                Song.Picture = $"data:{ImageFile.ContentType};base64,{base64}";
            }
            else
            {
                // Ensure non-null string for the model property
                Song.Picture ??= string.Empty;
            }
            _context.Songs.Add(Song);
            await _context.SaveChangesAsync();
            return RedirectToPage("./View");
        }

        private async Task LoadAlbumOptions()
        {
            AlbumOptions = await _context.Albums
                .AsNoTracking()
                .Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = $"{a.Artist} - {a.Title}"
                })
                .ToListAsync();
        }

        private async Task LoadGenreOptions()
        {
            GenreOptions = await _context.Genres
                .AsNoTracking()
                .Select(g => new SelectListItem
                {
                    Value = g.Id.ToString(),
                    Text = g.Name
                })
                .ToListAsync();
        }
    }
}
