using System.ComponentModel.DataAnnotations;

namespace SovereignSounds.Models;

public abstract class MusicItem
{
    [Required(ErrorMessage = "Please enter the title")]
    [StringLength(50, ErrorMessage = "Title cannot exceed 50 characters")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Please enter the artist")]
    [StringLength(30, ErrorMessage = "Artist cannot exceed 30 characters")]
    public string Artist { get; set; }

    [Required(ErrorMessage = "Please provide at least one genre")]
    [MinLength(1, ErrorMessage = "You must select at least one genre")]
    public List<Genre> Genres { get; set; } = new();

    [Required(ErrorMessage = "Please enter the release date")]
    [Display(Name = "Release Date")]
    [DataType(DataType.Date)]
    public DateOnly ReleaseDate { get; set; }
    [Display(Name = "Release Date")]
    public string ReleaseDateDisplay => ReleaseDate.ToString("MMM dd, yyyy");

    [Required(ErrorMessage = "Please enter the price")]
    [DataType(DataType.Currency)]
    [RegularExpression(@"^\d+(\.\d{1,2})?$",
        ErrorMessage = "Price must match: 1.99, 0.82, 16, 10.5, 12.34")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Please select a cover art")]
    [Display(Name = "Cover Art")]
    public string Picture { get; set; }
}
