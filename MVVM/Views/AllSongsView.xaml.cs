using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using MusicPlayerWPF.MVVM.Models;
using MusicPlayerWPF.MVVM.ViewModels;

namespace MusicPlayerWPF.MVVM.Views
{
    public partial class AllSongsView : UserControl
    {
        public AllSongsView()
        {
            InitializeComponent();
        }
        private void Slider_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            var vm = this.DataContext as AllSongsViewModel;
            vm.model.CurrentSong.Player.Position = TimeSpan.FromSeconds(SongSlider.Value);
            vm.CurrentSong = vm.model.CurrentSong;
        }
    }
}
