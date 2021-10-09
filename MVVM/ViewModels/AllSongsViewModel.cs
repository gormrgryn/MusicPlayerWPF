using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Threading.Tasks;
using MusicPlayerWPF.Core;
using MusicPlayerWPF.Core.AsyncCommands;
using MusicPlayerWPF.MVVM.Models;
using Genius;

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

        // private RelayCommand playCommand;
        // public RelayCommand PlayCommand
        // {
        //     get
        //     {
        //         return playCommand ?? (playCommand = new RelayCommand(o =>
        //         {
        //             SongModel newCurrentSong = o as SongModel;

        //             if (CurrentSong != default(SongModel))
        //             {
        //                 if (CurrentSong.Id == newCurrentSong.Id)
        //                 {
        //                     if (CurrentSong.IsCurrent)
        //                     {
        //                         CurrentSong.Pause();
        //                         CurrentSong.IsCurrent = false;
        //                     }
        //                     else
        //                     {
        //                         CurrentSong.Play();
        //                         CurrentSong.IsCurrent = true;
        //                     }
        //                 } else
        //                 {
        //                     SwapSong(ref newCurrentSong);
        //                 }
                        
        //             } else
        //             {
        //                 SongModel songToPlay = newCurrentSong == default(SongModel)
        //                     ? model.AllSongs.First.Value
        //                     : newCurrentSong;
        //                 SwapSong(ref songToPlay);
        //             }

        //             if(CurrentSong != default(SongModel) && CurrentSong.CoverArtUrl == null)
        //             {
        //                 await GetSongCoverArt(CurrentSong);
        //             }
        //         }));
        //     }
        // }

        // private RelayCommand skipPreviousCommand;
        // public RelayCommand SkipPreviousCommand
        // {
        //     get
        //     {
        //         return skipPreviousCommand ?? (skipPreviousCommand = new RelayCommand(async o =>
        //         {
        //             SongModel newCurrentSong = model.AllSongs.Find(CurrentSong)?.Previous?.Value;
        //             if (newCurrentSong != default(SongModel)) SwapSong(ref newCurrentSong);

        //             if(CurrentSong.CoverArtUrl == null)
        //             {
        //                 await GetSongCoverArt(CurrentSong);
        //             }
        //         }));
        //     }
        // }

        // private RelayCommand skipNextCommand;
        // public RelayCommand SkipNextCommand
        // {
        //     get
        //     {
        //         return skipNextCommand ?? (skipNextCommand = new RelayCommand(async o =>
        //         {
        //             SongModel newCurrentSong = model.AllSongs.Find(CurrentSong)?.Next?.Value;
        //             if (newCurrentSong != default(SongModel)) SwapSong(ref newCurrentSong);

        //             if(CurrentSong.CoverArtUrl == null)
        //             {
        //                 await GetSongCoverArt(CurrentSong);
        //             }
        //         }));
        //     }
        // }

        public ICommand PlayCommand { get; private set; }

        private GeniusClient geniusClient;

        public AllSongsViewModel()
        {
            geniusClient = new GeniusClient("rHeNwXXfSXsxQoQOsE6B89cQz-s4czDv37Sh-w2CgDgS6OmgyXw_QNYsgYkll2V2");
            PlayCommand = new AsyncRelayCommand(PlayAction, ex => CurrentSong.CoverArtUrl = ex.Message);

            model = new AllSongsModel();
            AllSongs = new ObservableCollection<SongModel>();
            string dirName = Directory.GetDirectories
            (
                Directory.GetDirectories
                (
                    Directory.GetCurrentDirectory()
                )[1]
            )[3];

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

            PlayIconUri = Directory.GetFiles(Directory.GetDirectories(Directory.GetDirectories(Directory.GetCurrentDirectory())[1])[2])[1];
            PauseIconUri = Directory.GetFiles(Directory.GetDirectories(Directory.GetDirectories(Directory.GetCurrentDirectory())[1])[2])[0];
            SkipNextIcon = Directory.GetFiles(Directory.GetDirectories(Directory.GetDirectories(Directory.GetCurrentDirectory())[1])[2])[2];
            SkipPreviousIcon = Directory.GetFiles(Directory.GetDirectories(Directory.GetDirectories(Directory.GetCurrentDirectory())[1])[2])[3];
        }

        private async Task PlayAction(object o)
        {
            SongModel newCurrentSong = o as SongModel;

            if (CurrentSong != default(SongModel))
            {
                if (CurrentSong.Id == newCurrentSong.Id)
                {
                    if (CurrentSong.IsCurrent)
                    {
                        CurrentSong.Pause();
                        CurrentSong.IsCurrent = false;
                    }
                    else
                    {
                        CurrentSong.Play();
                        CurrentSong.IsCurrent = true;
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

            if (string.IsNullOrEmpty(CurrentSong.CoverArtUrl))
            {
                var searchResponse = await geniusClient.SearchClient.Search($"{CurrentSong.Artists} {CurrentSong.Title}");
                CurrentSong.CoverArtUrl = searchResponse.Response.Hits.First().Result.SongArtImageThumbnailUrl;
            }
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
