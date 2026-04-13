using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using SovereignSounds.Data;
using SovereignSounds.Models;

namespace SovereignSounds.Pages.Music.Songs
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

        public Song Song { get; set; } = default!;
        public bool IsOwned { get; set; }

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

                var user = await _userManager.GetUserAsync(User);
                if (user is not null)
                {
                    var customer = await _context.Customers
                        .FirstOrDefaultAsync(c => c.IdentityUserId == user.Id);

                    if (customer is not null)
                    {
                        IsOwned = await _context.OwnedItems
                            .AnyAsync(oi => oi.CustomerId == customer.Id && oi.MusicItemId == song.Id);
                    }
                }

                return Page();
            }

            return NotFound();
        }
    }
}
