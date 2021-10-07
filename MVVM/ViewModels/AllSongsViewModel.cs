using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MusicPlayerWPF.Core;
using MusicPlayerWPF.MVVM.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MusicPlayerWPF.MVVM.ViewModels
{
    public class AllSongsViewModel : ObservableObject {
        
        public string PlayIconUri { get; set; }
        public string PauseIconUri { get; set; }
        public string SkipNextIcon { get; set; }
        public string SkipPreviousIcon { get; set; }

        public AllSongsModel model { get; set; }

        private SongModel currentSong;
        public SongModel CurrentSong
        {
            get
            {
                return currentSong ?? default(SongModel);
            }
            set
            {
                currentSong = value;
                currentSong.Player.MediaEnded -= new EventHandler(NextSong);
                currentSong.Player.MediaEnded += new EventHandler(NextSong);
                OnPropertyChanged("CurrentSong");
            }
        }

        private ObservableCollection<SongModel> allsongs;
        public ObservableCollection<SongModel> AllSongs
        {
            get { return allsongs; }
            set
            {
                allsongs = value;
                OnPropertyChanged("AllSongs");
            }
        }

        private RelayCommand playCommand;
        public RelayCommand PlayCommand
        {
            get
            {
                return playCommand ?? (playCommand = new RelayCommand(o =>
                {
                    SongModel modelCurrentSong = model.CurrentSong;
                    SongModel newCurrentSong = o as SongModel;

                    if (modelCurrentSong != default(SongModel))
                    {
                        if (modelCurrentSong.Id == newCurrentSong.Id)
                        {
                            if (modelCurrentSong.IsCurrent)
                            {
                                modelCurrentSong.Pause();
                                modelCurrentSong.IsCurrent = false;
                            }
                            else
                            {
                                modelCurrentSong.Play();
                                modelCurrentSong.IsCurrent = true;
                            }
                        } else
                        {
                            SwapSong(ref newCurrentSong);
                        }
                        
                    } else
                    {
                        SongModel songToPlay = newCurrentSong == default(SongModel)
                            ? model.AllSongs.First.Value
                            : newCurrentSong;
                        SwapSong(ref songToPlay);
                    }
                }));
            }
        }

        private RelayCommand skipPreviousCommand;
        public RelayCommand SkipPreviousCommand
        {
            get
            {
                return skipPreviousCommand ?? (skipPreviousCommand = new RelayCommand(o =>
                {
                    SongModel newCurrentSong = model.AllSongs.Find(CurrentSong)?.Previous?.Value;
                    if (newCurrentSong != default(SongModel)) SwapSong(ref newCurrentSong);
                }));
            }
        }
        private RelayCommand skipNextCommand;
        public RelayCommand SkipNextCommand
        {
            get
            {
                return skipNextCommand ?? (skipNextCommand = new RelayCommand(o =>
                {
                    SongModel newCurrentSong = model.AllSongs.Find(CurrentSong)?.Next?.Value;
                    if (newCurrentSong != default(SongModel)) SwapSong(ref newCurrentSong);
                }));
            }
        }

        public AllSongsViewModel()
        {
            model = new AllSongsModel();
            AllSongs = new ObservableCollection<SongModel>();
            string dirName = Directory.GetDirectories
            (
                Directory.GetDirectories
                (
                    Directory.GetCurrentDirectory()
                )[1]
            )[2];

            if(Directory.Exists(dirName))
            {
                string[] filesPaths = Directory.GetFiles(dirName);
                for (var i = 0; i < filesPaths.Length; i++)
                {
                    SongModel song = new SongModel(filesPaths[i]);
                    song.Id = i + 1;
                    model.AllSongs.AddLast(song);
                    AllSongs.Add(song);
                }
            }

            PlayIconUri = Directory.GetFiles(Directory.GetDirectories
            (
                Directory.GetDirectories
                (
                    Directory.GetCurrentDirectory()
                )[1]
            )[1])[1];
            PauseIconUri = Directory.GetFiles(Directory.GetDirectories
            (
                Directory.GetDirectories
                (
                    Directory.GetCurrentDirectory()
                )[1]
            )[1])[0];
            SkipNextIcon = Directory.GetFiles(Directory.GetDirectories
            (
                Directory.GetDirectories
                (
                    Directory.GetCurrentDirectory()
                )[1]
            )[1])[2];
            SkipPreviousIcon = Directory.GetFiles(Directory.GetDirectories
            (
                Directory.GetDirectories
                (
                    Directory.GetCurrentDirectory()
                )[1]
            )[1])[3];
        }

        private void NextSong(object sender, EventArgs e)
        {
            SongModel newCurrentSong = model.AllSongs.Find(CurrentSong).Next.Value;
            SwapSong(ref newCurrentSong);
        }
        private void SwapSong(ref SongModel newCurrentSong)
        {
            if (CurrentSong != default(SongModel))
            {
                CurrentSong.IsCurrent = false;
                CurrentSong.Stop();
            }
            newCurrentSong.IsCurrent = true;
            CurrentSong = newCurrentSong;
            model.CurrentSong = CurrentSong;
            CurrentSong.Play();
        }
    }
}
