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

namespace SovereignSounds.Pages.Music.Albums
{
    public class CreateModel : PageModel
    {
        private readonly SovereignSounds.Data.ApplicationDbContext _context;

        public CreateModel(SovereignSounds.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public List<SelectListItem> GenreOptions { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            await LoadGenreOptions();
            return Page();
        }

        [BindProperty]
        public Album Album { get; set; } = default!;

        [BindProperty]
        public IFormFile ImageFile { get; set; }

        [BindProperty]
        [MinLength(1, ErrorMessage = "You must select at least one genre")]
        public List<int> SelectedGenreIds { get; set; } = new();

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("Album.Picture");
            ModelState.Remove("Album.Genres");

            if (!ModelState.IsValid)
            {
                await LoadGenreOptions();
                return Page();
            }

            Album.Genres = await _context.Genres
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
                Album.Picture = $"data:{ImageFile.ContentType};base64,{base64}";
            }
            else
            {
                // Ensure non-null string for the model property
                Album.Picture ??= string.Empty;
            }
            _context.Albums.Add(Album);
            await _context.SaveChangesAsync();
            return RedirectToPage("./View");
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
