using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SovereignSounds.Data;
using SovereignSounds.Models;

namespace SovereignSounds.Pages;

public class ShoppingCartModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public ShoppingCartModel(ApplicationDbContext context)
    {
        _context = context;
    }

    List<MusicItem> Cart { get; set; } = default!;

    //public async Task<IActionResult> OnGetAsync(int? id)
    //{
    //    var SessCart = HttpContext.Session.GetString("Cart");
    //    if (SessCart != null)
    //        Cart = JsonConvert.DeserializeObject<List<MusicItem>>(SessCart);
    //    else
    //        Cart = new List<MusicItem>();

    //    var song = await _context.Songs.FirstOrDefaultAsync(s => s.Id == id);
    //    if (song != null)
    //    {
    //        bool found = false;
    //        for (int i = 0; i < Cart.Count; i++)
    //        {
    //            if (Cart[i].Id == id)
    //            {
    //                Cart[i].Quantity++;
    //                found = true;
    //                break;
    //            }
    //        }
    //        if (!found)
    //        {
    //            song.Quantity = 1;
    //            Cart.Add(movie);
    //        }
    //        HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(Cart));
    //    }
    //    return Page();
    //}
}
