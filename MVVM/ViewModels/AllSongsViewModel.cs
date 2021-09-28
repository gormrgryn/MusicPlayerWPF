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
        public AllSongsModel model { get; set; }
        
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

        private RelayCommand playSongCommand;
        public RelayCommand PlaySongCommand
        {
            get
            {
                return playSongCommand ?? (playSongCommand = new RelayCommand(o =>
                {
                    SongModel currentSong = model.CurrentSong;
                    SongModel newCurrentSong = o as SongModel;

                    if (currentSong != default(SongModel)) // != null
                    {
                        if (currentSong.Id == newCurrentSong.Id) // same
                        {
                            if (currentSong.IsCurrent) // isplaying
                            {
                                currentSong.Pause();
                                currentSong.IsCurrent = false;
                            }
                            else
                            {
                                currentSong.Play();
                                currentSong.IsCurrent = true;
                            }
                        } else // non same
                        {
                            currentSong.IsCurrent = false;
                            currentSong.Stop();
                            model.CurrentSong = newCurrentSong;
                            newCurrentSong.IsCurrent = true;
                            newCurrentSong.Play();
                        }
                        
                    } else // == null
                    {    
                        model.CurrentSong = newCurrentSong;
                        newCurrentSong.IsCurrent = true;
                        newCurrentSong.Play();
                    }
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
                    song.Id = i;
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
        }
    }
}
