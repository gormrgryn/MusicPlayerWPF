using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using MusicPlayerWPF.Core;

namespace MusicPlayerWPF.MVVM.Models
{
    public class SongModel : ObservableObject
    {
        public string FilePath { get; set; }
        public string Title { get; set; }
        public string Artists { get; set; }
        public string Album { get; set; }

        public int NaturalDuration { get; set; }
        public string Duration { get; set; }
        private int naturalPosition;
        public int NaturalPosition
        {
            get { return naturalPosition; }
            set
            {
                naturalPosition = value;
                OnPropertyChanged("NaturalPosition");
            }
        }
        public int Id { get; set; }
        private bool isCurrent;
        public bool IsCurrent
        {
            get
            {
                return isCurrent;
            }
            set
            {
                isCurrent = value;
                OnPropertyChanged("IsCurrent");
            }
        }
        public MediaPlayer Player { get; set; }
        private DispatcherTimer dispatcherTimer;

        public SongModel(string path)
        {
            FilePath = path;
            Player = new MediaPlayer();
            Player.Open(new Uri(path));

            var songFile = TagLib.File.Create(path);
            Title = songFile.Tag.Title;
            Artists = string.Join(",", songFile.Tag.Performers);
            Album = songFile.Tag.Album;
            var naturalDuration = songFile.Properties.Duration;
            Duration = naturalDuration.Seconds.ToString().Length == 1
                ? naturalDuration.Minutes.ToString() + ":0" + naturalDuration.Seconds.ToString()
                : naturalDuration.Minutes.ToString() + ":" + naturalDuration.Seconds.ToString();
            NaturalDuration = (naturalDuration.Minutes * 60 + naturalDuration.Seconds);

            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0,0,1);
        }
        public void Play()
        {
            Player.Play();
            dispatcherTimer.Start();
        }
        public void Pause()
        {
            Player.Pause();
            dispatcherTimer.Stop();
        }
        public void Stop()
        {
            Player.Stop();
            dispatcherTimer.Stop();
            NaturalPosition = 0;
        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            ++NaturalPosition;
        }
    }
}
