using SovereignSounds.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System;

namespace SovereignSounds.Data;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using var context = new ApplicationDbContext(
            serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());
            
        var env = serviceProvider.GetRequiredService<IWebHostEnvironment>();

        // Helper function to get or create genres so we don't accidentally duplicate genres
        var pop = GetOrCreateGenre(context, "Pop");
        var rock = GetOrCreateGenre(context, "Rock");
        var rnb = GetOrCreateGenre(context, "R&B");
        var funk = GetOrCreateGenre(context, "Funk");
        var progRock = GetOrCreateGenre(context, "Progressive Rock");
        context.SaveChanges();

        string thrillerPicture = GetImageAsDataUri(env.WebRootPath, "thriller.png");
        string operaPicture = GetImageAsDataUri(env.WebRootPath, "opera.png");

        // 1. Create Thriller Album if it doesn't exist
        if (!context.Albums.Any(a => a.Title == "Thriller"))
        {
            var thrillerAlbum = new Album
            {
                Title = "Thriller",
                Artist = "Michael Jackson",
                ReleaseDate = new DateOnly(1982, 11, 29),
                Price = 12.99m,
                Picture = thrillerPicture,
                Genres = new List<Genre> { pop, rnb, funk }
            };

            var thrillerSongs = new List<Song>
            {
                CreateSong(thrillerAlbum, "Wanna Be Startin' Somethin'", "6:03", 1.99m, thrillerPicture),
                CreateSong(thrillerAlbum, "Baby Be Mine", "4:20", 1.99m, thrillerPicture),
                CreateSong(thrillerAlbum, "The Girl Is Mine", "3:42", 1.99m, thrillerPicture),
                CreateSong(thrillerAlbum, "Thriller", "5:57", 1.99m, thrillerPicture),
                CreateSong(thrillerAlbum, "Beat It", "4:18", 1.99m, thrillerPicture),
                CreateSong(thrillerAlbum, "Billie Jean", "4:54", 1.99m, thrillerPicture),
                CreateSong(thrillerAlbum, "Human Nature", "4:06", 1.99m, thrillerPicture),
                CreateSong(thrillerAlbum, "P.Y.T. (Pretty Young Thing)", "3:59", 1.99m, thrillerPicture),
                CreateSong(thrillerAlbum, "The Lady in My Life", "5:00", 1.99m, thrillerPicture)
            };
            thrillerAlbum.Songs = thrillerSongs;
            context.Albums.Add(thrillerAlbum);
        }

        // 2. Create A Night At The Opera Album if it doesn't exist
        if (!context.Albums.Any(a => a.Title == "A Night at the Opera"))
        {
            var nightAtTheOpera = new Album
            {
                Title = "A Night at the Opera",
                Artist = "Queen",
                ReleaseDate = new DateOnly(1975, 11, 21),
                Price = 14.99m,
                Picture = operaPicture,
                Genres = new List<Genre> { rock, progRock }
            };

            var queenSongs = new List<Song>
            {
                CreateSong(nightAtTheOpera, "Death on Two Legs (Dedicated to...)", "3:43", 1.99m, operaPicture),
                CreateSong(nightAtTheOpera, "Lazing on a Sunday Afternoon", "1:07", 1.99m, operaPicture),
                CreateSong(nightAtTheOpera, "I'm in Love with My Car", "3:05", 1.99m, operaPicture),
                CreateSong(nightAtTheOpera, "You're My Best Friend", "2:52", 1.99m, operaPicture),
                CreateSong(nightAtTheOpera, "'39", "3:31", 1.99m, operaPicture),
                CreateSong(nightAtTheOpera, "Sweet Lady", "4:03", 1.99m, operaPicture),
                CreateSong(nightAtTheOpera, "Seaside Rendezvous", "2:15", 1.99m, operaPicture),
                CreateSong(nightAtTheOpera, "The Prophet's Song", "8:21", 1.99m, operaPicture),
                CreateSong(nightAtTheOpera, "Love of My Life", "3:39", 1.99m, operaPicture),
                CreateSong(nightAtTheOpera, "Good Company", "3:23", 1.99m, operaPicture),
                CreateSong(nightAtTheOpera, "Bohemian Rhapsody", "5:55", 1.99m, operaPicture),
                CreateSong(nightAtTheOpera, "God Save the Queen", "1:18", 1.99m, operaPicture)
            };
            nightAtTheOpera.Songs = queenSongs;
            context.Albums.Add(nightAtTheOpera);
        }

        // Save any newly added albums
        context.SaveChanges();
    }

    private static string GetImageAsDataUri(string webRootPath, string fileName)
    {
        var filePath = Path.Combine(webRootPath, "images", fileName);
        if (!File.Exists(filePath))
        {
            return string.Empty;
        }

        var bytes = File.ReadAllBytes(filePath);
        var base64 = Convert.ToBase64String(bytes);
        
        var extension = Path.GetExtension(fileName).TrimStart('.').ToLower();
        var contentType = extension switch
        {
            "jpg" or "jpeg" => "image/jpeg",
            "png" => "image/png",
            "gif" => "image/gif",
            "svg" => "image/svg+xml",
            _ => "application/octet-stream"
        };

        return $"data:{contentType};base64,{base64}";
    }

    private static Genre GetOrCreateGenre(ApplicationDbContext context, string name)
    {
        var genre = context.Genres.FirstOrDefault(g => g.Name == name);
        if (genre == null)
        {
            genre = new Genre { Name = name };
            context.Genres.Add(genre);
        }
        return genre;
    }

    private static Song CreateSong(Album album, string title, string durationInput, decimal price, string pictureDataUri)
    {
        // Parse "m:ss" into TimeSpan
        var timeSpan = TimeSpan.ParseExact(durationInput, @"m\:ss", null);

        return new Song
        {
            Title = title,
            Artist = album.Artist,
            ReleaseDate = album.ReleaseDate,
            Genres = album.Genres.ToList(), // Copy album's genres
            Price = price,
            DurationInput = durationInput,
            Duration = timeSpan,
            Picture = pictureDataUri,
            Album = album // Explicitly establish the relationship
        };
    }
}