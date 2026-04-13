using System.ComponentModel.DataAnnotations;

namespace SovereignSounds.Models;

public class MusicItemDto
{
    public int Id { get; set; }
    public string ItemType { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Artist { get; set; } = string.Empty;
    public string? Album { get; set; }
    public string ReleaseDate { get; set; } = string.Empty;
    public string Duration { get; set; } = string.Empty;

    [DataType(DataType.Currency)]
    public decimal Price { get; set; }
    public string Picture { get; set; } = string.Empty;
    public List<string> Genres { get; set; } = new();
}
