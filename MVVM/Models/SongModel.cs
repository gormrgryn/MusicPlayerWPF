using System;
using System.Windows.Media;

namespace MusicPlayerWPF.MVVM.Models
{
    public class SongModel
    {
        public string FilePath { get; set; }
        public string Title { get; set; }
        public string Artists { get; set; }
        public string Album { get; set; }
        public int Id { get; set; }
        public MediaPlayer Player { get; set; }

        public SongModel(string path)
        {
            FilePath = path;
            Player = new MediaPlayer();
            Player.Open(new Uri(path));
            var songFile = TagLib.File.Create(path);
            Title = songFile.Tag.Title;
            Artists = string.Join(",", songFile.Tag.Performers);
            Album = songFile.Tag.Album;
        }
        public void Play()
        {
            Player.Play();
        }
        public void Pause()
        {
            Player.Pause();
        }
        public void Stop()
        {
            Player.Stop();
        }
    }
}
