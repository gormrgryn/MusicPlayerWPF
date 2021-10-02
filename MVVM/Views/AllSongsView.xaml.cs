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

            SongSlider.ApplyTemplate();
            Thumb thumb = (SongSlider.Template.FindName("PART_Track", SongSlider) as Track).Thumb;
            thumb.MouseEnter += new MouseEventHandler(thumb_MouseEnter);
        }
        
        private void Slider_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            var vm = this.DataContext as AllSongsViewModel;
            vm.model.CurrentSong.Player.Position = TimeSpan.FromSeconds(SongSlider.Value);
            vm.CurrentSong = vm.model.CurrentSong;
        }

        private void thumb_MouseEnter(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && e.MouseDevice.Captured == null)
            {
                MouseButtonEventArgs args =
                    new MouseButtonEventArgs(e.MouseDevice, e.Timestamp, MouseButton.Left);
                args.RoutedEvent = MouseLeftButtonDownEvent;
                (sender as Thumb).RaiseEvent(args);
            }
        }
    }
}
