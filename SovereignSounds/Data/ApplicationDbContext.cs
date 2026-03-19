using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SovereignSounds.Models;

namespace SovereignSounds.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Song> Songs { get; set; }
    public DbSet<Album> Albums { get; set; }
    public DbSet<Genre> Genres { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Album>()
            .HasMany(a => a.Genres)
            .WithMany(g => g.Albums)
            .UsingEntity(j => j.ToTable("GenreAlbum"));

        builder.Entity<Song>()
            .HasMany(s => s.Genres)
            .WithMany(g => g.Songs)
            .UsingEntity(j => j.ToTable("GenreSong"));
    }
}
