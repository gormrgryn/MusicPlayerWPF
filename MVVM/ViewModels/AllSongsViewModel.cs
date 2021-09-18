using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using MusicPlayerWPF.Core;
using MusicPlayerWPF.MVVM.Models;

namespace MusicPlayerWPF.MVVM.ViewModels
{
    public class AllSongsViewModel : ObservableObject {

        public AllSongsModel model { get; set; }
        public ObservableCollection<SongModel> AllSongs { get; set; } =
            new ObservableCollection<SongModel>(); //
        public string Text { get; set; }
        public AllSongsViewModel()
        {
            model = new AllSongsModel();
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
                foreach (var i in filesPaths)
                {
                    SongModel song = new SongModel(i);
                    model.AllSongs.AddLast(song);
                    AllSongs.Add(song);
                }
            }
        }
    }
}
