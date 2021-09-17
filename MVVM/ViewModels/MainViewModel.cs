using System;
using MusicPlayerWPF.Core;

namespace MusicPlayerWPF.MVVM.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        public AllSongsViewModel AllSongsVM { get; set; }

        public AlbumsViewModel AlbumsVM { get; set; }

        public RelayCommand AllSongsViewCommand { get; set; }

        public RelayCommand AlbumsViewCommand { get; set; }

        private object _currentView;

        public object CurrentView
        { 
            get { return _currentView; }
            set {
                _currentView = value;
                OnPropertyChanged();
            }
        }
        public MainViewModel()
        {
            AllSongsVM = new AllSongsViewModel();
            AlbumsVM = new AlbumsViewModel();
            CurrentView = AllSongsVM;

            AllSongsViewCommand = new RelayCommand(o =>
            {
                CurrentView = AllSongsVM;
            });
            AlbumsViewCommand = new RelayCommand(o =>
            {
                CurrentView = AlbumsVM;
            });
        }
    }
}
