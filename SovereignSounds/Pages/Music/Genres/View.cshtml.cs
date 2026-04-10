using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SovereignSounds.Data;
using SovereignSounds.Models;

namespace SovereignSounds.Pages.Music.Genres
{
    [Authorize(Roles = "Admin")]
    public class ViewModel : PageModel
    {
        private readonly SovereignSounds.Data.ApplicationDbContext _context;

        public ViewModel(SovereignSounds.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Genre> Genres { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Genres = await _context.Genres.ToListAsync();
        }
    }
}
