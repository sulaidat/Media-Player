using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;
using System.Windows.Automation.Peers;
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
        MediaSelectedArgs _currentPlaylistInfo;
        string _currentSingleMedia;

        public MainWindow()
        {
            InitializeComponent();
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

        public class SavedState:INotifyPropertyChanged
        {
            MediaSelectedArgs _currentMediaInfo;
            public MediaSelectedArgs CurrentMediaInfo
            {
                get
                {
                    return _currentMediaInfo;
                }
                set
                {
                    _currentMediaInfo = value;
                    EmitPropertyChanged();
                }
            }
            public string CurrentSingleMedia { get; set; }

            public event PropertyChangedEventHandler? PropertyChanged;

            private void EmitPropertyChanged([CallerMemberName] String propertyName = "")
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // init Buttons' Contents
            _imgPlay = new Image();
            _imgPlay.Source = new BitmapImage(new Uri("/imgs/play.png", UriKind.Relative));
            _imgPause = new Image();
            _imgPause.Source = new BitmapImage(new Uri("/imgs/pause.png", UriKind.Relative));
            _imgRepeat = new Image();
            _imgRepeat.Source = new BitmapImage(new Uri("/imgs/repeat.png", UriKind.Relative));
            _imgRepeatOne = new Image();
            _imgRepeatOne.Source = new BitmapImage(new Uri("/imgs/repeat-once.png", UriKind.Relative));

            // set up volume, speed, timeline
            mediaView.Volume = (double)slider_volume.Value;
            mediaView.SpeedRatio = (double)slider_speed.Value;

            // create timer
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(10);
            _timer.Tick += TimerTick;

            // fire and forget
            _timer.Start();

            // subscribe _playlistWindow hooks
            _playlistWindow = PlaylistWindow.GetInstance();
            _playlistWindow.MediaSelected += PrepareAndPlayMediaFromPlaylist;

            // subcribe _currentPlaylistInfo hooks
            _currentPlaylistInfo = new MediaSelectedArgs();
            _currentPlaylistInfo.PropertyChanged += SendDataToPlaylistWindow;

            //// load SavedState from file
            LoadSavedStateFromFile("current_state.json");

            // focus on media
            //mediaView.Focus();
        }

        private void SendDataToPlaylistWindow(object? sender, PropertyChangedEventArgs e)
        {
            _playlistWindow.SendData(_currentPlaylistInfo);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            SaveStateToFile("current_state.json");
            _playlistWindow.Close();
        }

        private void TimerTick(object? sender, EventArgs e)
        {
            // prevent tick when unnecessary
            if (!_sliderBeingDragged && mediaView.Source != null && mediaView.NaturalDuration.HasTimeSpan)
            {
                slider_timeline.Value = mediaView.Position.TotalSeconds;
                slider_timeline.Minimum = 0;
                slider_timeline.Maximum = mediaView.NaturalDuration.TimeSpan.TotalSeconds;
                txt_progress.Text = TimeSpan.FromSeconds(slider_timeline.Value).ToString(@"hh\:mm\:ss");
                txt_duration.Text = TimeSpan.FromSeconds(slider_timeline.Maximum).ToString(@"hh\:mm\:ss");

                //if (mediaView.IsFocused == true)
                //{
                //    this.Title = "focused";
                //}
            }
        }

        private void SaveStateToFile(string filename)
        {
            var savedState = new SavedState();
            savedState.CurrentMediaInfo = _currentPlaylistInfo;
            savedState.CurrentSingleMedia = _currentSingleMedia;

            var json = JsonConvert.SerializeObject(savedState);
            File.WriteAllText(filename, json);
        }

        private void LoadSavedStateFromFile(string filename)
        {
            string directory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string filePath = Path.Combine(directory, filename);
            if (File.Exists(filePath))
            {
                var savedState = new SavedState();
                //savedState.PropertyChanged += SendDataToPlaylistWindow;
                savedState = JsonConvert.DeserializeObject<SavedState>(File.ReadAllText(filePath));
                _currentPlaylistInfo = savedState.CurrentMediaInfo;
                _currentSingleMedia = savedState.CurrentSingleMedia;
            }

            if (_currentPlaylistInfo == null && _currentSingleMedia == null) return;

            // Prepare media
            if (_currentPlaylistInfo.Playlist != null)
            {
                var mediaPath = _currentPlaylistInfo.Playlist[_currentPlaylistInfo.MediaIndex].Path;
                this.Title = $"Now playing {mediaPath}";
                mediaView.Source = new Uri(mediaPath, UriKind.Absolute);
                mediaView.Play();
                mediaView.Stop();
            }
            else if (_currentSingleMedia != null)
            {
                var mediaPath = _currentSingleMedia;
                this.Title = $"Now playing {mediaPath}";
                mediaView.Source = new Uri(mediaPath, UriKind.Absolute);
                mediaView.Play();
                mediaView.Stop();
            }
        }


        #region Media Controls
        
        private void PlayMedia()
        {
            if (toggle_play.IsChecked == false)
            {
                toggle_play.IsChecked = true;
            }
        }
        private void ForcePlayMedia()
        {
            if (toggle_play.IsChecked == false)
            {
                toggle_play.IsChecked = true;
            }
            else
            {
                toggle_play.IsChecked = false;
                toggle_play.IsChecked = true;
            }
        }
        private void PauseMedia()
        {
            if (toggle_play.IsChecked == true)
            {
                toggle_play.IsChecked = false;
            }
        }
        // StopMedia switch play button to pause
        private void StopMedia()
        {
            mediaView.Stop();
            PauseMedia();
        }
        private void PrepareAndPlayMediaFromPlaylist(object? sender, MediaSelectedArgs args)
        {
            _currentPlaylistInfo = args;
            var filePath = _currentPlaylistInfo.Playlist[_currentPlaylistInfo.MediaIndex].Path;
            this.Title = $"Now playing {filePath}";

            mediaView.Source = new Uri(filePath, UriKind.Absolute);
            mediaView.Play();
            mediaView.Stop();

            ForcePlayMedia();
        }
        private void PrepareAndPlaySingleMedia(object? sender, string filePath)
        {
            _currentSingleMedia = filePath;
            this.Title = $"Now playing {filePath}";

            mediaView.Source = new Uri(filePath, UriKind.Absolute);
            mediaView.Play();
            mediaView.Stop();

            ForcePlayMedia();
        }

        // NextMedia and PreviousMedia only depends on shuffle mode
        private void NextMedia()
        {
            if (_currentPlaylistInfo == null) return;
            if (_shuffleMode == false)
            {
                _currentPlaylistInfo.MediaIndex = (_currentPlaylistInfo.MediaIndex + 1 >= _currentPlaylistInfo.Playlist.Count) ? 0 : _currentPlaylistInfo.MediaIndex + 1;
            }
            else
            {
                _currentPlaylistInfo.MediaIndex = _shuffleIndices.Next();
            }
            var filePath = _currentPlaylistInfo.Playlist[_currentPlaylistInfo.MediaIndex].Path;
            this.Title = $"Now playing {filePath}";
            mediaView.Source = new Uri(filePath);
        }
        private void PreviousMedia()
        {
            if (_currentPlaylistInfo == null) return;
            if (_shuffleMode == false)
            {
                _currentPlaylistInfo.MediaIndex = (_currentPlaylistInfo.MediaIndex - 1 < 0) ? _currentPlaylistInfo.Playlist.Count - 1 : _currentPlaylistInfo.MediaIndex - 1;
            }
            else
            {
                _currentPlaylistInfo.MediaIndex = _shuffleIndices.Next();
            }
            var filePath = _currentPlaylistInfo.Playlist[_currentPlaylistInfo.MediaIndex].Path;
            this.Title = $"Now playing {filePath}";
            mediaView.Source = new Uri(filePath);
        }

        #endregion

        #region Event Handlers

        void OnClick_OpenMedia(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Media files (*.mp3;*.mp4;*flv;*.mpg)|*.mp3;*.mp4;*flv;*.mpg|All files (*.*)|*.*";
            if (dialog.ShowDialog() ?? false)
            {
                PrepareAndPlaySingleMedia(null, dialog.FileName);
            }
        }
        void OnChecked_PlayMedia(object sender, RoutedEventArgs e)
        {
            if (mediaView.Source != null)
            {
                toggle_play.Content = _imgPause;
                mediaView.Play();
            }
        }
        void OnUnchecked_PauseMedia(object sender, RoutedEventArgs e)
        {
            if (mediaView.Source != null)
            {
                toggle_play.Content = _imgPlay;
                mediaView.Pause();
            }
        }
        // Stop the media.
        void OnClick_StopMedia(object sender, RoutedEventArgs e)
        {
            StopMedia();
        }
        private void OnMediaOpened(object sender, RoutedEventArgs e)
        {

        }
        // this behavior happens when media end without Next and Previous button pressing
        // OnMediaEnded depends on LoopOne and Shuffle mode
        private void OnMediaEnded(object sender, RoutedEventArgs e)
        {
            StopMedia();
            if (_currentPlaylistInfo.Playlist != null && _loopState != false) // Loop One is not enabled
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
                    if (_currentPlaylistInfo.MediaIndex < _currentPlaylistInfo.Playlist.Count - 1)
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
            //popup_preview.IsOpen = true;
        }
        private void OnDragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            _sliderBeingDragged = false;
            //popup_preview.IsOpen = false;
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
            if (_currentPlaylistInfo.Playlist == null) return;
            if (_shuffleMode == false)
            {
                _shuffleMode = true;
                btn_shuffle.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#a0a0a4");
                _shuffleIndices = new ShuffleIndices(_currentPlaylistInfo.Playlist.Count);
            }
            else
            {
                _shuffleMode = false;
                btn_shuffle.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#dddddd");
            }
        }

        #endregion

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


        private void Loop(object sender, RoutedEventArgs e)
        {
            NextMedia();
            PlayMedia();
        }
        private void LoopOne(object sender, RoutedEventArgs e)
        {
            PlayMedia();
        }
        private void OnMouseMove_MakeThumbFollow(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                //var popup = slider_timeline.Template.FindName("InfoPopup", slider_timeline) as CustomPopup;
                //popup.IsOpen = true;

                var slider = (Slider)sender;
                Point position = e.GetPosition(slider);
                double d = 1.0d / slider.ActualWidth * position.X;
                var p = slider.Maximum * d;
                slider.Value = p;
            }
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            //var popup = slider_timeline.Template.FindName("InfoPopup", slider_timeline) as CustomPopup;
            //popup.IsOpen = false;
        }

        private void OnKeyDown_HandleKeyboardInput(object sender, KeyEventArgs e)
        {
            if (mediaView.Source == null) return;
            switch (e.Key)
            {
                case Key.Left:
                    FastForwardMedia(10);
                    break;
                case Key.Right:
                    FastRewindMedia(10);
                    break;
                case Key.Space:
                    if (toggle_play.IsChecked == false)
                    {
                        PlayMedia();
                    } else
                    {
                        PauseMedia();
                    }
                    break;
                case Key.PageDown:
                    PreviousMedia();
                    PlayMedia();
                    break;
                case Key.PageUp:
                    NextMedia();
                    PlayMedia();
                    break;
            }
        }

        private void FastRewindMedia(int seconds)
        {
            mediaView.Position += TimeSpan.FromSeconds(seconds);
            //slider_timeline.Value += seconds;
        }

        private void FastForwardMedia(int seconds)
        {
            mediaView.Position -= TimeSpan.FromSeconds(seconds);
            //slider_timeline.Value -= seconds;
        }

   
    }
}
