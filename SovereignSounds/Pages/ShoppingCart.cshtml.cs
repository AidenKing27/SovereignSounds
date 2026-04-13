using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SovereignSounds.Data;
using SovereignSounds.Models;

namespace SovereignSounds.Pages;

[Authorize(Roles = "User,Admin")]
public class ShoppingCartModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public ShoppingCartModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<MusicItemDto> Cart { get; set; } = [];

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        Cart = [];
        var sessionCart = HttpContext.Session.GetString("Cart");
        if (sessionCart is not null)
            Cart = JsonConvert.DeserializeObject<List<MusicItemDto>>(sessionCart)!;

        if (id is null)
            return Page();

        MusicItemDto? musicItem = null;

        Song? song = await _context.Songs
            .Include(s => s.Album)
            .Include(s => s.Genres)
            .FirstOrDefaultAsync(s => s.Id == id.Value);

        if (song is not null)
        {
            musicItem = new()
            {
                Id = song.Id,
                ItemType = song.GetType().Name,
                Title = song.Title,
                Artist = song.Artist,
                Album = song.Album!.Title,
                ReleaseDate = song.ReleaseDateDisplay,
                Duration = song.DurationDisplay,
                Price = song.Price,
                Picture = song.Picture,
                Genres = song.Genres.Select(g => g.Name).ToList()
            };
        }
        else
        {
            Album? album = await _context.Albums
                .Include(a => a.Genres)
                .FirstOrDefaultAsync(a => a.Id == id.Value);

            musicItem = new()
            {
                Id = album.Id,
                ItemType = album.GetType().Name,
                Title = album.Title,
                Artist = album.Artist,
                ReleaseDate = album.ReleaseDateDisplay,
                Duration = album.DurationDisplay,
                Price = album.Price,
                Picture = album.Picture,
                Genres = album.Genres.Select(g => g.Name).ToList()
            };
        }

        if (!Cart.Any(c => c.Id == id.Value))
        {
            Cart.Add(musicItem);
            HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(Cart));
        }

        return Page();
    }

    public IActionResult OnPostRemoveFromCart(int? id)
    {
        Cart = [];
        var sessionCart = HttpContext.Session.GetString("Cart");
        if (sessionCart is not null)
            Cart = JsonConvert.DeserializeObject<List<MusicItemDto>>(sessionCart)!;

        MusicItemDto? musicItem = Cart.FirstOrDefault(mi => mi.Id == id);

        if (musicItem is not null)
        {
            Cart.Remove(musicItem);
            HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(Cart));
        }

        return RedirectToPage();
    }
}
