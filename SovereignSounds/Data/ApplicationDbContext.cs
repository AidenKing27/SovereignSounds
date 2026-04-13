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

    public DbSet<MusicItem> MusicItems { get; set; }
    public DbSet<Song> Songs { get; set; }
    public DbSet<Album> Albums { get; set; }
    public DbSet<Genre> Genres { get; set; }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<OrderHistory> OrderHistories { get; set; }
    public DbSet<OwnedItem> OwnedItems { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Map Song/Album inheritance using TPH and a discriminator column named MusicItemType
        builder.Entity<MusicItem>()
            .HasDiscriminator<string>("MusicItemType")
            .HasValue<Song>("Song")
            .HasValue<Album>("Album");

        // Configure currency precision
        builder.Entity<MusicItem>()
            .Property(mi => mi.Price)
            .HasPrecision(18, 2);

        // Speed up indexing
        builder.Entity<MusicItem>()
            .HasIndex(mi => new { mi.Title, mi.Artist });

        // Album <-> Genre many-to-many mapping using join table GenreAlbum
        builder.Entity<Album>()
            .HasMany(a => a.Genres)
            .WithMany(g => g.Albums)
            .UsingEntity(j => j.ToTable("GenreAlbum"));

        // Song <-> Genre many-to-many mapping using join table GenreSong
        builder.Entity<Song>()
            .HasMany(s => s.Genres)
            .WithMany(g => g.Songs)
            .UsingEntity(j => j.ToTable("GenreSong"));

        // Customer -> IdentityUser one-to-one mapping (one app profile per identity account)
        builder.Entity<Customer>()
            .HasOne(c => c.IdentityUser)
            .WithOne()
            .HasForeignKey<Customer>(c => c.IdentityUserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Enforce uniqueness of IdentityUserId in Customers at the database level
        builder.Entity<Customer>()
            .HasIndex(c => c.IdentityUserId)
            .IsUnique();

        // OrderHistory -> Customer many-to-one mapping with restricted deletes to preserve purchase records
        builder.Entity<OrderHistory>()
            .HasOne(oh => oh.Customer)
            .WithMany(c => c.OrderHistories)
            .HasForeignKey(oh => oh.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        // OrderHistory -> OrderItem one-to-many mapping
        builder.Entity<OrderHistory>()
            .HasMany(oh => oh.Items)
            .WithOne(oi => oi.Order)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        // OrderItem -> MusicItem many-to-one mapping with restricted deletes to preserve purchase records
        builder.Entity<OrderItem>()
            .HasOne(oi => oi.MusicItem)
            .WithMany(mi => mi.AllOrders)
            .HasForeignKey(oi => oi.MusicItemId)
            .OnDelete(DeleteBehavior.Restrict);

        // OwnedItem -> Customer many-to-one mapping
        builder.Entity<OwnedItem>()
            .HasOne(oi => oi.Customer)
            .WithMany(c => c.OwnedItems)
            .HasForeignKey(oi => oi.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        // OwnedItem -> MusicItem many-to-one mapping
        builder.Entity<OwnedItem>()
            .HasOne(oi => oi.MusicItem)
            .WithMany()
            .HasForeignKey(oi => oi.MusicItemId)
            .OnDelete(DeleteBehavior.Restrict);

        // Prevent duplicate ownership rows per customer/music item
        builder.Entity<OwnedItem>()
            .HasIndex(oi => new { oi.CustomerId, oi.MusicItemId })
            .IsUnique();

        // Configure currency precision
        builder.Entity<OrderItem>()
            .Property(oi => oi.PurchasePrice)
            .HasPrecision(18, 2);

        // Configure currency precision
        builder.Entity<OrderHistory>()
            .Property(oh => oh.CartTotal)
            .HasPrecision(18, 2);

        builder.Entity<OrderHistory>()
            .Property(oh => oh.GrandTotal)
            .HasPrecision(18, 2);

    }
}