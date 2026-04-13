using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SovereignSounds.Data;
using SovereignSounds.Models;

namespace SovereignSounds.Pages.Music.Albums
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public DetailsModel(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Album Album { get; set; } = default!;
        public bool IsOwned { get; set; }

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

                var user = await _userManager.GetUserAsync(User);
                if (user is not null)
                {
                    var customer = await _context.Customers
                        .FirstOrDefaultAsync(c => c.IdentityUserId == user.Id);

                    if (customer is not null)
                    {
                        IsOwned = await _context.OwnedItems
                            .AnyAsync(oi => oi.CustomerId == customer.Id && oi.MusicItemId == album.Id);
                    }
                }

                return Page();
            }

            return NotFound();
        }
    }
}
