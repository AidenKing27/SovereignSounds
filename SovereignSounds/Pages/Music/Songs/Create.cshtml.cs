using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SovereignSounds.Data;
using SovereignSounds.Models;

namespace SovereignSounds.Pages.Music.Songs
{
    public class CreateModel : PageModel
    {
        private readonly SovereignSounds.Data.ApplicationDbContext _context;

        public CreateModel(SovereignSounds.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["AlbumId"] = new SelectList(_context.Albums, "Id", "Artist");
            return Page();
        }

        [BindProperty]
        public Song Song { get; set; } = default!;

        [BindProperty]
        public IFormFile ImageFile { get; set; }

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("Movie.Picture");
            if (!ModelState.IsValid)
            {
                return Page();
            }
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
            return RedirectToPage("./Index");
        }
    }
}
