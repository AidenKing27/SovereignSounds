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
    public class IndexModel : PageModel
    {
        private readonly SovereignSounds.Data.ApplicationDbContext _context;

        public IndexModel(SovereignSounds.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Song> Song { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Song = await _context.Songs
                .Include(s => s.Album).ToListAsync();
        }
    }
}
