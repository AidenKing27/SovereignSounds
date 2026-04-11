using System.ComponentModel.DataAnnotations;

namespace SovereignSounds.Models;

public class Album : MusicItem
{
    [Required(ErrorMessage = "Please provide at least one genre")]
    [MinLength(1, ErrorMessage = "You must select at least one genre")]
    public List<Genre> Genres { get; set; } = new();

    public List<Song> Songs { get; set; } = new();

    [Display(Name = "Song Count")]
    public int SongCount => Songs.Count;

    public TimeSpan Duration
    {
        get 
        {
            TimeSpan total = TimeSpan.Zero;
            foreach (var song in Songs)
            {
                total += song.Duration;
            }
            return total;
        }
    }
}
