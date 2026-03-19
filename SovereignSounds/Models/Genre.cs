using System.ComponentModel.DataAnnotations;

namespace SovereignSounds.Models;

public class Genre
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Please enter the genre name")]
    [StringLength(50, ErrorMessage = "Genre Name cannot exceed 50 characters")]
    public string Name { get; set; }

    public List<Song> Songs { get; set; } = new();
    public List<Album> Albums { get; set; } = new();
}
