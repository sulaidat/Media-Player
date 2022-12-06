using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Media_Player
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string _current_media;
        DispatcherTimer _timer;
        Image _imgPlay;
        Image _imgPause;

        public MainWindow()
        {
            InitializeComponent();

            init();
        }

        void init()
        {
            _imgPlay = new Image();
            _imgPlay.Source = new BitmapImage(new Uri("/imgs/play.png", UriKind.Relative));
            _imgPause = new Image();
            _imgPause.Source = new BitmapImage(new Uri("/imgs/pause.png", UriKind.Relative));
        }

        void OnClick_Open(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Media files (*.mp3;*.mp4)|*.mp3;*.mp4|All files (*.*)|*.*";
            if (dialog.ShowDialog() ?? false)
            {
                _current_media = dialog.FileName;
                this.Title = $"Now playing {_current_media}";
                mediaView.Source = new Uri(_current_media, UriKind.Absolute);

                // wtf??
                mediaView.Play();
                mediaView.Stop();
            }
        }

        private void toggle_play_Checked(object sender, RoutedEventArgs e)
        {
            if (mediaView.Source != null)
            {
                mediaView.Play();
                _timer.Start();
                toggle_play.Content = _imgPause;
            }
        }

        private void toggle_play_Unchecked(object sender, RoutedEventArgs e)
        {
            if (mediaView.Source != null)
            {
                mediaView.Pause();
                _timer.Stop();
                toggle_play.Content = _imgPlay;
            }
        }
        // Play the media.
        void OnClick_PlayMedia(object sender, RoutedEventArgs e)
        {
        }

        // Pause the media.
        void OnClick_PauseMedia(object sender, RoutedEventArgs e)
        {
        }

        // Stop the media.
        void OnClick_StopMedia(object sender, RoutedEventArgs e)
        {
            mediaView.Stop();
        }

        // Change the volume of the media.
        private void ChangeMediaVolume(object sender, RoutedPropertyChangedEventArgs<double> args)
        {
            mediaView.Volume = (double)slider_volume.Value;
        }

        // Change the speed of the media.
        private void ChangeMediaSpeedRatio(object sender, RoutedPropertyChangedEventArgs<double> args)
        {
            mediaView.SpeedRatio = (double)slider_speed.Value;
        }

        // When the media opens, initialize the "Seek To" slider maximum value
        // to the total number of miliseconds in the length of the media clip.
        private void Element_MediaOpened(object sender, EventArgs e)
        {
            slider_seekto.Maximum = mediaView.NaturalDuration.TimeSpan.TotalMilliseconds;
        }

        // When the media playback is finished. Stop() the media to seek to media start.
        private void Element_MediaEnded(object sender, EventArgs e)
        {
            mediaView.Stop();
        }

        // Jump to different parts of the media (seek to).
        private void SeekToMediaPosition(object sender, RoutedPropertyChangedEventArgs<double> args)
        {
            int SliderValue = (int)slider_seekto.Value;

            // Overloaded constructor takes the arguments days, hours, minutes, seconds, milliseconds.
            // Create a TimeSpan with miliseconds equal to the slider value.
            TimeSpan ts = new TimeSpan(0, 0, 0, 0, SliderValue);
            mediaView.Position = ts;
        }

        private void timer_Tick(object? sender, EventArgs e)
        {
            // update slider 
            slider_timeline.Value = mediaView.Position.TotalSeconds;

            // update progress label 
            txt_progress.Text = TimeSpan.FromSeconds(slider_timeline.Value).ToString(@"hh\:mm\:ss");
        }

        private void OnMediaOpened(object sender, RoutedEventArgs e)
        {
            mediaView.Volume = (double)slider_volume.Value;
            mediaView.SpeedRatio = (double)slider_speed.Value;

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += timer_Tick;

            slider_timeline.Minimum = 0;
            slider_timeline.Maximum = mediaView.NaturalDuration.TimeSpan.TotalSeconds;

            txt_progress.Text = TimeSpan.FromSeconds(0).ToString(@"hh\:mm\:ss");
            txt_duration.Text = TimeSpan.FromSeconds(slider_timeline.Maximum).ToString(@"hh\:mm\:ss");
        }

        private void OnMediaEnded(object sender, RoutedEventArgs e)
        {
            return;
        }
    }
}
