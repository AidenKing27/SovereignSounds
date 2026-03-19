namespace SovereignSounds.Models;

public class Album : MusicItem
{
    public int Id { get; set; }

    public List<Song> Songs { get; set; } = new();

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
