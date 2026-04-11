using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SovereignSounds.Data;
using SovereignSounds.Models;

namespace SovereignSounds.Pages
{
    public class ProfileModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<ProfileModel> _logger;
        private readonly ApplicationDbContext _context;

        public Customer? Customer { get; set; }

        public List<Song> CustomerSongs { get; set; } = [];
        public List<Album> CustomerAlbums { get; set; } = [];

        public ProfileModel(
            UserManager<IdentityUser> userManager,
            ILogger<ProfileModel> logger,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

            Customer = await _context.Customers
                .Include(c => c.IdentityUser)
                .Include(c => c.OrderHistories)
                    .ThenInclude(oh => oh.MusicItem)
                .FirstOrDefaultAsync(c => c.IdentityUserId == user.Id);

            if (Customer == null)
                return NotFound("Customer profile not found.");

            CustomerSongs = Customer.OrderHistories
                .Select(oh => oh.MusicItem)
                .OfType<Song>()
                .DistinctBy(s => s.Id)
                .ToList();

            CustomerAlbums = Customer.OrderHistories
                .Select(oh => oh.MusicItem)
                .OfType<Album>()
                .DistinctBy(a => a.Id)
                .ToList();

            return Page();
        }
    }
}
