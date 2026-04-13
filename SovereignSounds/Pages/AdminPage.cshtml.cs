using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SovereignSounds.Data;
using SovereignSounds.Models;

namespace SovereignSounds.Pages;

[Authorize(Roles = "Admin")]
public class AdminPageModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private const int PageSize = 24;

    public AdminPageModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public IList<Song> Songs { get; private set; } = [];
    public IList<Album> Albums { get; private set; } = [];
    public IList<Genre> Genres { get; private set; } = [];

    [BindProperty(SupportsGet = true)]
    public int SongsPageNumber { get; set; } = 1;

    [BindProperty(SupportsGet = true)]
    public int AlbumsPageNumber { get; set; } = 1;

    [BindProperty(SupportsGet = true)]
    public int GenresPageNumber { get; set; } = 1;

    public int SongsTotalItems { get; private set; }
    public int SongsTotalPages { get; private set; }

    public int AlbumsTotalItems { get; private set; }
    public int AlbumsTotalPages { get; private set; }

    public int GenresTotalItems { get; private set; }
    public int GenresTotalPages { get; private set; }

    public async Task OnGetAsync()
    {
        SongsPageNumber = Math.Max(1, SongsPageNumber);
        AlbumsPageNumber = Math.Max(1, AlbumsPageNumber);
        GenresPageNumber = Math.Max(1, GenresPageNumber);

        var songsQuery = _context.Songs
            .AsNoTracking()
            .Include(s => s.Album)
            .OrderBy(s => s.Title);

        SongsTotalItems = await songsQuery.CountAsync();
        SongsTotalPages = Math.Max(1, (int)Math.Ceiling(SongsTotalItems / (double)PageSize));
        if (SongsPageNumber > SongsTotalPages)
        {
            SongsPageNumber = SongsTotalPages;
        }

        Songs = await songsQuery
            .Skip((SongsPageNumber - 1) * PageSize)
            .Take(PageSize)
            .ToListAsync();

        var albumsQuery = _context.Albums
            .AsNoTracking()
            .Include(a => a.Songs)
            .AsSplitQuery()
            .OrderBy(a => a.Title);

        AlbumsTotalItems = await albumsQuery.CountAsync();
        AlbumsTotalPages = Math.Max(1, (int)Math.Ceiling(AlbumsTotalItems / (double)PageSize));
        if (AlbumsPageNumber > AlbumsTotalPages)
        {
            AlbumsPageNumber = AlbumsTotalPages;
        }

        Albums = await albumsQuery
            .Skip((AlbumsPageNumber - 1) * PageSize)
            .Take(PageSize)
            .ToListAsync();

        var genresQuery = _context.Genres
            .AsNoTracking()
            .OrderBy(g => g.Name);

        GenresTotalItems = await genresQuery.CountAsync();
        GenresTotalPages = Math.Max(1, (int)Math.Ceiling(GenresTotalItems / (double)PageSize));
        if (GenresPageNumber > GenresTotalPages)
        {
            GenresPageNumber = GenresTotalPages;
        }

        Genres = await genresQuery
            .Skip((GenresPageNumber - 1) * PageSize)
            .Take(PageSize)
            .ToListAsync();
    }
}
