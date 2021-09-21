using System.Media;

namespace MusicPlayerWPF.MVVM.Models
{
    public class SongModel
    {
        public string FilePath { get; set; }
        public string Title { get; set; }
        public string Artists { get; set; }
        public string Album { get; set; }
        public int Id { get; set; }

        public SoundPlayer Player { get; set; } //
        public SongModel(string path)
        {
            FilePath = path;
            Player = new SoundPlayer(path);

            var songFile = TagLib.File.Create(path);
            Title = songFile.Tag.Title;
            Artists = string.Join(",", songFile.Tag.Performers);
            Album = songFile.Tag.Album;
        }
    }
}
