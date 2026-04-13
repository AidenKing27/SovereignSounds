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
        var electronic = GetOrCreateGenre(context, "Electronic");
        var alternative = GetOrCreateGenre(context, "Alternative");
        context.SaveChanges();

        string thrillerPicture = GetImageAsDataUri(env.WebRootPath, "thriller.png");
        string operaPicture = GetImageAsDataUri(env.WebRootPath, "opera.png");
        string hannahPicture = GetImageAsDataUri(env.WebRootPath, "hannah.png");
        string vertigoPicture = GetImageAsDataUri(env.WebRootPath, "vertigo.png");
        string noFuturePicture = GetImageAsDataUri(env.WebRootPath, "nofuture.png");
        string icymiPicture = GetImageAsDataUri(env.WebRootPath, "icymi.png");
        string darkPicture = GetImageAsDataUri(env.WebRootPath, "dark.png");
        string artOfChangePicture = GetImageAsDataUri(env.WebRootPath, "artofchange.png");

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

        // 3. Create Hannah Montana: The Hits Album if it doesn't exist
        if (!context.Albums.Any(a => a.Title == "Hannah Montana: The Hits"))
        {
            var hannahMontanaHits = new Album
            {
                Title = "Hannah Montana: The Hits",
                Artist = "Hannah Montana",
                ReleaseDate = new DateOnly(2009, 3, 17),
                Price = 13.99m,
                Picture = hannahPicture,
                Genres = new List<Genre> { pop }
            };

            var hannahSongs = new List<Song>
            {
                CreateSong(hannahMontanaHits, "The Best of Both Worlds", "2:54", 1.99m, hannahPicture),
                CreateSong(hannahMontanaHits, "If We Were a Movie", "3:03", 1.99m, hannahPicture),
                CreateSong(hannahMontanaHits, "Who Said", "3:15", 1.99m, hannahPicture),
                CreateSong(hannahMontanaHits, "Rock Star", "3:16", 1.99m, hannahPicture),
                CreateSong(hannahMontanaHits, "Nobody's Perfect", "3:20", 1.99m, hannahPicture),
                CreateSong(hannahMontanaHits, "One in a Million", "3:33", 1.99m, hannahPicture),
                CreateSong(hannahMontanaHits, "We Got the Party", "3:54", 1.99m, hannahPicture),
                CreateSong(hannahMontanaHits, "He Could Be the One", "3:02", 1.99m, hannahPicture),
                CreateSong(hannahMontanaHits, "Let's Get Crazy", "3:10", 1.99m, hannahPicture),
                CreateSong(hannahMontanaHits, "The Climb", "3:55", 1.99m, hannahPicture)
            };
            hannahMontanaHits.Songs = hannahSongs;
            context.Albums.Add(hannahMontanaHits);
        }

        // 4. Create EDEN - vertigo Album if it doesn't exist
        // Source: https://en.wikipedia.org/wiki/Vertigo_(Eden_album)
        if (!context.Albums.Any(a => a.Title == "vertigo" && a.Artist == "EDEN"))
        {
            var vertigoAlbum = new Album
            {
                Title = "vertigo",
                Artist = "EDEN",
                ReleaseDate = new DateOnly(2018, 1, 19),
                Price = 11.49m,
                Picture = vertigoPicture,
                Genres = new List<Genre> { electronic, alternative }
            };

            var vertigoSongs = new List<Song>
            {
                CreateSong(vertigoAlbum, "wrong", "1:04", 1.09m, vertigoPicture),
                CreateSong(vertigoAlbum, "take care", "3:15", 1.39m, vertigoPicture),
                CreateSong(vertigoAlbum, "start//end", "5:34", 1.89m, vertigoPicture),
                CreateSong(vertigoAlbum, "wings", "2:57", 1.29m, vertigoPicture),
                CreateSong(vertigoAlbum, "icarus", "6:45", 1.99m, vertigoPicture),
                CreateSong(vertigoAlbum, "lost//found", "3:23", 1.39m, vertigoPicture),
                CreateSong(vertigoAlbum, "crash", "3:52", 1.69m, vertigoPicture),
                CreateSong(vertigoAlbum, "gold", "3:16", 1.49m, vertigoPicture),
                CreateSong(vertigoAlbum, "forever//over", "5:42", 1.89m, vertigoPicture),
                CreateSong(vertigoAlbum, "float", "3:18", 1.39m, vertigoPicture),
                CreateSong(vertigoAlbum, "wonder", "4:23", 1.59m, vertigoPicture),
                CreateSong(vertigoAlbum, "love; not wrong (brave)", "3:37", 1.49m, vertigoPicture),
                CreateSong(vertigoAlbum, "falling in reverse", "4:59", 1.79m, vertigoPicture),
            };
            vertigoAlbum.Songs = vertigoSongs;
            context.Albums.Add(vertigoAlbum);
        }

        // 5. Create EDEN - no future Album if it doesn't exist
        // Source: https://en.wikipedia.org/wiki/No_Future_(album)
        if (!context.Albums.Any(a => a.Title == "no future" && a.Artist == "EDEN"))
        {
            var noFutureAlbum = new Album
            {
                Title = "no future",
                Artist = "EDEN",
                ReleaseDate = new DateOnly(2020, 2, 14),
                Price = 12.79m,
                Picture = noFuturePicture,
                Genres = new List<Genre> { electronic, alternative }
            };

            var noFutureSongs = new List<Song>
            {
                CreateSong(noFutureAlbum, "good morning", "3:29", 1.29m, noFuturePicture),
                CreateSong(noFutureAlbum, "in", "0:31", 0.69m, noFuturePicture),
                CreateSong(noFutureAlbum, "hertz", "3:18", 1.49m, noFuturePicture),
                CreateSong(noFutureAlbum, "static", "0:48", 0.69m, noFuturePicture),
                CreateSong(noFutureAlbum, "projector", "3:42", 1.59m, noFuturePicture),
                CreateSong(noFutureAlbum, "love, death, distraction", "4:17", 1.69m, noFuturePicture),
                CreateSong(noFutureAlbum, "how to sleep", "2:37", 1.29m, noFuturePicture),
                CreateSong(noFutureAlbum, "calm down", "2:35", 1.29m, noFuturePicture),
                CreateSong(noFutureAlbum, "just saying", "4:06", 1.59m, noFuturePicture),
                CreateSong(noFutureAlbum, "fomo", "3:09", 1.39m, noFuturePicture),
                CreateSong(noFutureAlbum, "so far so good", "3:25", 1.49m, noFuturePicture),
                CreateSong(noFutureAlbum, "isohel", "4:30", 1.69m, noFuturePicture),
                CreateSong(noFutureAlbum, "????", "2:59", 1.29m, noFuturePicture),
                CreateSong(noFutureAlbum, "tides", "1:43", 0.99m, noFuturePicture),
                CreateSong(noFutureAlbum, "rushing", "5:07", 1.79m, noFuturePicture),
                CreateSong(noFutureAlbum, "$treams", "3:20", 1.39m, noFuturePicture),
                CreateSong(noFutureAlbum, "2020", "3:31", 1.39m, noFuturePicture),
                CreateSong(noFutureAlbum, "out", "0:42", 0.69m, noFuturePicture),
                CreateSong(noFutureAlbum, "untitled", "3:36", 1.49m, noFuturePicture)
            };
            noFutureAlbum.Songs = noFutureSongs;
            context.Albums.Add(noFutureAlbum);
        }

        // 6. Create EDEN - ICYMI Album if it doesn't exist
        // Source: Amazon Music / Last.fm (11 tracks, 38:48)
        if (!context.Albums.Any(a => a.Title == "ICYMI" && a.Artist == "EDEN"))
        {
            var icymiAlbum = new Album
            {
                Title = "ICYMI",
                Artist = "EDEN",
                ReleaseDate = new DateOnly(2022, 9, 9),
                Price = 13.49m,
                Picture = icymiPicture,
                Genres = new List<Genre> { electronic, alternative }
            };

            var icymiSongs = new List<Song>
            {
                CreateSong(icymiAlbum, "A Call", "2:07", 1.09m, icymiPicture),
                CreateSong(icymiAlbum, "Balling", "3:48", 1.69m, icymiPicture),
                CreateSong(icymiAlbum, "Sci-Fi", "2:57", 1.39m, icymiPicture),
                CreateSong(icymiAlbum, "Modern Warfare", "3:22", 1.49m, icymiPicture),
                CreateSong(icymiAlbum, "Waiting Room", "2:14", 1.19m, icymiPicture),
                CreateSong(icymiAlbum, "Closer 2", "3:18", 1.49m, icymiPicture),
                CreateSong(icymiAlbum, "PS1", "4:03", 1.69m, icymiPicture),
                CreateSong(icymiAlbum, "Call Me Back", "6:21", 1.99m, icymiPicture),
                CreateSong(icymiAlbum, "Duvidha", "1:42", 0.99m, icymiPicture),
                CreateSong(icymiAlbum, "Elsewhere", "6:04", 1.99m, icymiPicture),
                CreateSong(icymiAlbum, "Reaching 2", "2:59", 1.29m, icymiPicture),
            };
            icymiAlbum.Songs = icymiSongs;
            context.Albums.Add(icymiAlbum);
        }

        // 7. Create EDEN - Dark Album if it doesn't exist
        // Source: RateYourMusic / Drowned World Records (13 tracks, released 2025-08-22)
        if (!context.Albums.Any(a => a.Title == "Dark" && a.Artist == "EDEN"))
        {
            var darkAlbum = new Album
            {
                Title = "Dark",
                Artist = "EDEN",
                ReleaseDate = new DateOnly(2025, 8, 22),
                Price = 13.99m,
                Picture = darkPicture,
                Genres = new List<Genre> { electronic, alternative }
            };

            var darkSongs = new List<Song>
            {
                CreateSong(darkAlbum, "Still", "3:16", 1.29m, darkPicture),
                CreateSong(darkAlbum, "Zzz", "3:00", 1.29m, darkPicture),
                CreateSong(darkAlbum, "TEAM", "2:47", 1.19m, darkPicture),
                CreateSong(darkAlbum, "Paris, Teargas", "2:51", 1.19m, darkPicture),
                CreateSong(darkAlbum, "Ghost in the Shell", "4:13", 1.69m, darkPicture),
                CreateSong(darkAlbum, "True", "2:42", 1.19m, darkPicture),
                CreateSong(darkAlbum, "5ever", "3:18", 1.39m, darkPicture),
                CreateSong(darkAlbum, "Light Sleeper", "4:05", 1.69m, darkPicture),
                CreateSong(darkAlbum, "(Cold Water freestyle)", "2:22", 1.09m, darkPicture),
                CreateSong(darkAlbum, "gggiiiiirrrrlllll", "1:49", 0.99m, darkPicture),
                CreateSong(darkAlbum, "At Once", "2:56", 1.19m, darkPicture),
                CreateSong(darkAlbum, "Pocket (montreal)", "1:58", 0.99m, darkPicture),
                CreateSong(darkAlbum, "Quantuuuum", "4:40", 1.79m, darkPicture),
            };
            darkAlbum.Songs = darkSongs;
            context.Albums.Add(darkAlbum);
        }

        // 8. Create DROELOE - The Art of Change Album if it doesn't exist
        // Source: https://en.wikipedia.org/wiki/The_Art_of_Change
        if (!context.Albums.Any(a => a.Title == "The Art of Change" && a.Artist == "DROELOE"))
        {
            var artOfChangeAlbum = new Album
            {
                Title = "The Art of Change",
                Artist = "DROELOE",
                ReleaseDate = new DateOnly(2023, 9, 15),
                Price = 13.99m,
                Picture = artOfChangePicture,
                Genres = new List<Genre> { electronic, alternative }
            };

            var artOfChangeSongs = new List<Song>
            {
                CreateSong(artOfChangeAlbum, "Spark", "1:12", 0.79m, artOfChangePicture),
                CreateSong(artOfChangeAlbum, "Foolish Fish", "2:41", 1.29m, artOfChangePicture),
                CreateSong(artOfChangeAlbum, "Strange Wave", "3:54", 1.59m, artOfChangePicture),
                CreateSong(artOfChangeAlbum, "Landscape (with Banji)", "2:41", 1.29m, artOfChangePicture),
                CreateSong(artOfChangeAlbum, "Mundane Av.", "3:38", 1.49m, artOfChangePicture),
                CreateSong(artOfChangeAlbum, "Kicking Gates", "2:04", 1.09m, artOfChangePicture),
                CreateSong(artOfChangeAlbum, "A Current, A Void", "3:42", 1.49m, artOfChangePicture),
                CreateSong(artOfChangeAlbum, "Feeble Games", "3:02", 1.39m, artOfChangePicture),
                CreateSong(artOfChangeAlbum, "Lion Heart (ft. Lisa van Nes)", "2:44", 1.29m, artOfChangePicture),
                CreateSong(artOfChangeAlbum, "Hermit (ft. Emilia Ali)", "3:58", 1.59m, artOfChangePicture),
                CreateSong(artOfChangeAlbum, "The Wheel", "1:12", 0.79m, artOfChangePicture),
                CreateSong(artOfChangeAlbum, "Decision", "4:23", 1.69m, artOfChangePicture),
                CreateSong(artOfChangeAlbum, "Downside Up (ft. Transviolet)", "3:36", 1.49m, artOfChangePicture),
                CreateSong(artOfChangeAlbum, "Terminal Velocity", "3:42", 1.49m, artOfChangePicture),
                CreateSong(artOfChangeAlbum, "Counting Ten", "5:09", 1.89m, artOfChangePicture),
            };
            artOfChangeAlbum.Songs = artOfChangeSongs;
            context.Albums.Add(artOfChangeAlbum);
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