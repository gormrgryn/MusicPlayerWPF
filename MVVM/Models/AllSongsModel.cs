using System;
using System.Collections.Generic;
using System.Linq;

namespace MusicPlayerWPF.MVVM.Models
{
    public class AllSongsModel
    {
        public LinkedList<SongModel> AllSongs { get; set; }
        public SongModel CurrentSong { get; set; }
        public AllSongsModel()
        {
            AllSongs = new LinkedList<SongModel>();
        }
    }
}