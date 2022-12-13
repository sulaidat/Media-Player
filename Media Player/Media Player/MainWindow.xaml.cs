using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using static Media_Player.PlaylistPage;
using Path = System.IO.Path;

namespace Media_Player
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer _timer;
        Image _imgPlay;
        Image _imgPause;
        Image _imgRepeat;
        Image _imgRepeatOne;
        PlaylistWindow _playlistWindow;
        bool _sliderBeingDragged = false;
        /*
         _loopState: 
            - null: None
            - true: Loop Playlist
            - false: Loop One Media
         */
        bool? _loopState = null;
        bool _shuffleMode = false;
        ShuffleIndices _shuffleIndices;
        MediaSelectedArgs _currentMediaInfo;
        string _currentSingleMedia;


        public MainWindow()
        {
            InitializeComponent();

            initSth();
        }

        void initSth()
        {
            _imgPlay = new Image();
            _imgPlay.Source = new BitmapImage(new Uri("/imgs/play.png", UriKind.Relative));
            _imgPause = new Image();
            _imgPause.Source = new BitmapImage(new Uri("/imgs/pause.png", UriKind.Relative));
            _imgRepeat = new Image();
            _imgRepeat.Source = new BitmapImage(new Uri("/imgs/repeat.png", UriKind.Relative));
            _imgRepeatOne = new Image();
            _imgRepeatOne.Source = new BitmapImage(new Uri("/imgs/repeat-once.png", UriKind.Relative));
        }

        void OnClick_OpenMedia(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Media files (*.mp3;*.mp4)|*.mp3;*.mp4|All files (*.*)|*.*";
            if (dialog.ShowDialog() ?? false)
            {
                PrepareSingleMedia(null, dialog.FileName);
            }
        }

        void OnChecked_PlayMedia(object sender, RoutedEventArgs e)
        {
            PlayMedia();
        }
        void OnUnchecked_PauseMedia(object sender, RoutedEventArgs e)
        {
            PauseMedia();
        }

        // Stop the media.
        void OnClick_StopMedia(object sender, RoutedEventArgs e)
        {
            StopMedia();
        }


        private void PlayMedia()
        {
            if (mediaView.Source != null)
            {
                mediaView.Play();
                _timer.Start();
                toggle_play.Content = _imgPause;
            }
        }
        private void PauseMedia()
        {
            if (mediaView.Source != null)
            {
                mediaView.Pause();
                toggle_play.Content = _imgPlay;
            }
        }
        private void StopMedia()
        {
            mediaView.Stop();
            if (toggle_play.IsChecked ?? false)
            {
                toggle_play.IsChecked = false;
            }
        }

        #region not use yet 
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
        #endregion

        private void timer_Tick(object? sender, EventArgs e)
        {
            // prevent tick when unnecessary
            if (!_sliderBeingDragged && mediaView.Source != null && mediaView.NaturalDuration.HasTimeSpan)
            {
                slider_timeline.Value = mediaView.Position.TotalSeconds;
                slider_timeline.Minimum = 0;
                slider_timeline.Maximum = mediaView.NaturalDuration.TimeSpan.TotalSeconds;
                txt_progress.Text = TimeSpan.FromSeconds(slider_timeline.Value).ToString(@"hh\:mm\:ss");
                txt_duration.Text = TimeSpan.FromSeconds(slider_timeline.Maximum).ToString(@"hh\:mm\:ss");
            }
        }

        private void OnMediaOpened(object sender, RoutedEventArgs e)
        {

        }

        // this behavior happens when media end without Next and Previous button pressing
        // OnMediaEnded depends on LoopOne and Shuffle mode
        private void OnMediaEnded(object sender, RoutedEventArgs e)
        {
            StopMedia();
            if (_currentMediaInfo != null && _loopState != false) // Loop One is not enabled
            {
                if (_shuffleMode)
                {
                    // if shuffle mode is on and shuffleIndices is not end
                    if (!_shuffleIndices.IsEnd())
                    {
                        NextMedia();
                        PlayMedia();
                    }
                }
                else
                {
                    // if there's a next media
                    if (_currentMediaInfo.MediaIndex < _currentMediaInfo.MediaList.Count - 1)
                    {
                        NextMedia();
                        PlayMedia();
                    }
                    // else return to beginning
                    else
                    {
                        NextMedia();
                    }
                }
            }
        }

        private void OnDragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            _sliderBeingDragged = true;
        }

        private void OnDragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            _sliderBeingDragged = false;
        }

        private void OnSliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mediaView.Position = TimeSpan.FromSeconds(slider_timeline.Value);
            txt_progress.Text = TimeSpan.FromSeconds(slider_timeline.Value).ToString(@"hh\:mm\:ss");
        }

        private void OnClick_OpenPlaylist(object sender, RoutedEventArgs e)
        {
            _playlistWindow.Show();
        }

        private void PrepareMedia()
        {
            if (_currentSingleMedia == null && _currentSingleMedia == null) return;
            if (_currentMediaInfo != null)
            {
                var filePath = _currentMediaInfo.MediaList[_currentMediaInfo.MediaIndex].Path;
                this.Title = $"Now playing {filePath}";
                mediaView.Source = new Uri(filePath, UriKind.Absolute);

                // ???????
                mediaView.Play();
                mediaView.Stop();
            }
            else if (_currentSingleMedia != null)
            {
                var filePath = _currentSingleMedia;
                this.Title = $"Now playing {filePath}";
                mediaView.Source = new Uri(filePath, UriKind.Absolute);

                // ???????
                mediaView.Play();
                mediaView.Stop();
            }
        }
        private void PrepareMedia(object? sender, MediaSelectedArgs args)
        {
            _currentMediaInfo = args;
            var filePath = _currentMediaInfo.MediaList[_currentMediaInfo.MediaIndex].Path;
            this.Title = $"Now playing {filePath}";
            mediaView.Source = new Uri(filePath, UriKind.Absolute);

            // ???????
            mediaView.Play();
            mediaView.Stop();

            ForcePlayMedia();
        }
        private void PrepareSingleMedia(object? sender, string filePath)
        {
            _currentSingleMedia = filePath;
            this.Title = $"Now playing {filePath}";
            mediaView.Source = new Uri(filePath, UriKind.Absolute);

            // ???????
            mediaView.Play();
            mediaView.Stop();

            ForcePlayMedia();
        }

        private void ForcePlayMedia()
        {
            if (!toggle_play.IsChecked ?? true)
            {
                toggle_play.IsChecked = true;
            }
            else
            {
                toggle_play.IsChecked = false;
                toggle_play.IsChecked = true;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // set up volume, speed, timeline
            mediaView.Volume = (double)slider_volume.Value;
            mediaView.SpeedRatio = (double)slider_speed.Value;

            // create timer
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(500);
            _timer.Tick += timer_Tick;

            // subscribe _playlistWindow
            _playlistWindow = PlaylistWindow.GetInstance();
            _playlistWindow.MediaSelected += PrepareMedia;

            // load Saved State
            string directory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string filePath = Path.Combine(directory, "current_state.json");
            if (File.Exists(filePath))
            {
                var savedState = JsonConvert.DeserializeObject<SavedState>(File.ReadAllText(filePath));
                _currentMediaInfo = savedState.CurrentMediaInfo;
                _currentSingleMedia = savedState.CurrentSingleMedia;
            }

            PrepareMedia();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            SaveCurrentState();
            _playlistWindow.Close();
        }

        private void SaveCurrentState()
        {
            var savedState = new SavedState();
            savedState.CurrentMediaInfo = _currentMediaInfo;
            savedState.CurrentSingleMedia = _currentSingleMedia;

            var json = JsonConvert.SerializeObject(savedState);
            File.WriteAllText("current_state.json", json);
        }

        public class SavedState
        {
            public MediaSelectedArgs CurrentMediaInfo { get; set; }
            public string CurrentSingleMedia { get; set; }
        }

        private void OnClick_ToggleLoop(object sender, RoutedEventArgs e)
        {
            // none to loop
            if (_loopState == null)
            {
                _loopState = true;
                btn_loop.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#a0a0a4");
                mediaView.MediaEnded += Loop;
            }
            // loop to loop one
            else if (_loopState == true)
            {
                _loopState = false;
                btn_loop.Content = _imgRepeatOne;
                mediaView.MediaEnded -= Loop;
                mediaView.MediaEnded += LoopOne;
            }
            // loop one to none
            else if (_loopState == false)
            {
                _loopState = null;
                btn_loop.Content = _imgRepeat;
                btn_loop.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#dddddd");
                mediaView.MediaEnded -= LoopOne;
            }
        }

        private void Loop(object sender, RoutedEventArgs e)
        {
            NextMedia();
            PlayMedia();
        }
        private void LoopOne(object sender, RoutedEventArgs e)
        {
            PlayMedia();
        }

        // NextMedia and PreviousMedia only depends on shuffle mode
        private void NextMedia()
        {
            if (_currentMediaInfo == null) return;
            if (_shuffleMode == false)
            {
                _currentMediaInfo.MediaIndex = (_currentMediaInfo.MediaIndex + 1 >= _currentMediaInfo.MediaList.Count) ? 0 : _currentMediaInfo.MediaIndex + 1;
            }
            else
            {
                _currentMediaInfo.MediaIndex = _shuffleIndices.Next();
            }
            var filePath = _currentMediaInfo.MediaList[_currentMediaInfo.MediaIndex].Path;
            this.Title = $"Now playing {filePath}";
            mediaView.Source = new Uri(filePath);
        }
        private void PreviousMedia()
        {
            if (_currentMediaInfo == null) return;
            if (_shuffleMode == false)
            {
                _currentMediaInfo.MediaIndex = (_currentMediaInfo.MediaIndex - 1 < 0) ? _currentMediaInfo.MediaList.Count - 1 : _currentMediaInfo.MediaIndex - 1;
            }
            else
            {
                _currentMediaInfo.MediaIndex = _shuffleIndices.Next();
            }
            var filePath = _currentMediaInfo.MediaList[_currentMediaInfo.MediaIndex].Path;
            this.Title = $"Now playing {filePath}";
            mediaView.Source = new Uri(filePath);
        }

        private void OnClick_PlayNextMedia(object sender, RoutedEventArgs e)
        {
            NextMedia();
            PlayMedia();
        }

        private void OnClick_PlayPreviousMedia(object sender, RoutedEventArgs e)
        {
            PreviousMedia();
            PlayMedia();
        }

        private void OnClick_ToggleShuffleMode(object sender, RoutedEventArgs e)
        {
            if (_shuffleMode == false)
            {
                _shuffleMode = true;
                btn_shuffle.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#a0a0a4");
                _shuffleIndices = new ShuffleIndices(_currentMediaInfo.MediaList.Count);
            }
            else
            {
                _shuffleMode = false;
                btn_shuffle.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#dddddd");
            }
        }

        public class ShuffleIndices
        {
            private List<int> _indices;
            private int _currentIndex;

            public ShuffleIndices(int count)
            {
                var random = new Random();
                var candidates = new HashSet<int>();
                while (candidates.Count < count)
                {
                    candidates.Add(random.Next() % count);
                }
                _indices = new List<int>();
                _indices.AddRange(candidates);
                _currentIndex = 0;
            }
            public int Next()
            {
                ++_currentIndex;
                if (_currentIndex >= _indices.Count)
                {
                    _currentIndex = 0;
                }
                return _indices[_currentIndex];
            }
            public int Previous()
            {
                --_currentIndex;
                if (_currentIndex < 0)
                {
                    _currentIndex = 0;
                }
                return _indices[_currentIndex];
            }
            public bool IsEnd()
            {
                return (_currentIndex + 1 == _indices.Count) ? true : false;
            }
        }
    }
}
