using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SovereignSounds.Models;

public class Song : MusicItem
{
    [Display(Name = "Album")]
    public int? AlbumId { get; set; }
    public Album? Album { get; set; }

    [Required(ErrorMessage = "Please provide at least one genre")]
    [MinLength(1, ErrorMessage = "You must select at least one genre")]
    public List<Genre> Genres { get; set; } = new();

    [NotMapped]
    [Required(ErrorMessage = "Please enter the song duration")]
    [RegularExpression(@"^[0-5]?\d:[0-5]\d$",
        ErrorMessage = "Song Duration must match: 3:15, 03:15, 12:00, 0:59")]
    [Display(Name = "Duration")]
    public string DurationInput { get; set; } = string.Empty;

    public TimeSpan Duration { get; set; }

    [Display(Name = "Duration")]
    public string DurationDisplay => Duration.ToString(@"mm\:ss");
}