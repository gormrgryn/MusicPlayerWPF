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

        public AllSongsModel model { get; set; }
        
        private ObservableCollection<SongModel> allsongs;

        public ObservableCollection<SongModel> AllSongs
        {
            get
            {
                return allsongs;
            }
            set
            {
                allsongs = value;
                OnPropertyChanged("AllSongs");
            }
        }

        public string Text { get; set; }

        private RelayCommand playSongCommand;
        public RelayCommand PlaySongCommand
        {
            get { return playSongCommand; }
        }

        public AllSongsViewModel()
        {
            model = new AllSongsModel();
            playSongCommand = new RelayCommand(o =>
            {
                AllSongs = null;
                Text = "asdasadsas";
            });
            Text = "blu";
            AllSongs = new ObservableCollection<SongModel>();
            string dirName = Directory.GetDirectories
            (
                Directory.GetDirectories
                (
                    Directory.GetCurrentDirectory()
                )[1]
            )[1];

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
        }
    }
}
