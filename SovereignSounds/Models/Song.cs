using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SovereignSounds.Models;

public class Song : MusicItem
{
    public int Id { get; set; }

    public int? AlbumId { get; set; }
    public Album Album { get; set; }

    [NotMapped]
    [Required(ErrorMessage = "Please enter the song duration")]
    [RegularExpression(@"^[0-5]?\d:[0-5]\d$",
        ErrorMessage = "Song Duration must match: 3:15, 03:15, 12:00, 0:59")]
    [Display(Name = "Duration")]
    public string DurationInput { get; set; }
    public TimeSpan Duration { get; set; }
    [Display(Name = "Duration")]
    public string DurationDisplay => Duration.ToString(@"mm\:ss");
}
