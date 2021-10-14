using System;
using MusicPlayerWPF.Core;

namespace MusicPlayerWPF.MVVM.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        public AllSongsViewModel AllSongsVM { get; set; }

        public RelayCommand AllSongsViewCommand { get; set; }

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
            CurrentView = AllSongsVM;

            AllSongsViewCommand = new RelayCommand(o =>
            {
                CurrentView = AllSongsVM;
            });
        }
    }
}
