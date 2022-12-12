using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Path = System.IO.Path;
using System.Collections.Specialized;
using Newtonsoft.Json;
using System.IO;
using System;
using System.ComponentModel;

namespace Media_Player
{
    /// <summary>
    /// Interaction logic for PlaylistWindow.xaml
    /// </summary>
    public partial class PlaylistWindow : Window
    {
        ObservableCollection<Playlist> _playlists { get; set; }
        public EventHandler<string> MediaSelected;
        private static PlaylistWindow _instance;
        (int PlaylistIndex, int MediaIndex) _currentMedia;


        public static PlaylistWindow GetInstance()
        {
            if (_instance == null)
            {
                _instance = new PlaylistWindow();
            }
            return _instance;
        }

        public PlaylistWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadPlaylistFromFile("playlists.json");
            RemoveNonexistingMediaFromPlaylists();
        }

        private void RemoveNonexistingMediaFromPlaylists()
        {
            foreach (var playlist in _playlists)
            {
                for (var i=0; i < playlist.List.Count; i++) 
                {
                    if (!File.Exists(playlist.List[i].Path))
                    {
                        playlist.List.RemoveAt(i);
                    }
                }
            }
        }

        private void LoadPlaylistFromFile(string filename)
        {
            string directory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string filePath = Path.Combine(directory, filename);
            if (File.Exists(filePath))
            {
                _playlists = JsonConvert.DeserializeObject<ObservableCollection<Playlist>>(File.ReadAllText(filePath));
            }
            else
            {
                _playlists = new ObservableCollection<Playlist>();
            }
            _playlists.CollectionChanged += SavePlaylists;
            foreach (var playlist in _playlists)
            {
                playlist.List.CollectionChanged += SavePlaylists;
            }
            listview_playlist.ItemsSource = _playlists;
        }

        private void SavePlaylists(object? sender, NotifyCollectionChangedEventArgs e)
        {
            string json = JsonConvert.SerializeObject(_playlists);
            File.WriteAllText("playlists.json", json);
        }


        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            datagrid_medias.ItemsSource = _playlists[listview_playlist.SelectedIndex].List;
            btn_addMedia.Visibility = Visibility.Visible;
        }

        private void OnClick_NewPlaylist(object sender, RoutedEventArgs e)
        {
            var newPlaylist = new Playlist();
            newPlaylist.Name = "Playlist";
            newPlaylist.List = new ObservableCollection<Media>();
            newPlaylist.IsPlaying = false;
            _playlists.Add(newPlaylist);
        }

        private void OnClick_AddMedia(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Media files (*.mp3;*.mp4)|*.mp3;*.mp4|All files (*.*)|*.*";
            if (dialog.ShowDialog() ?? false)
            {
                var newMedia = new Media();
                newMedia.Path = dialog.FileName;
                newMedia.Name = Path.GetFileName(dialog.FileName);
                newMedia.IsPlaying = false;
                _playlists[listview_playlist.SelectedIndex].List.Add(newMedia);
            }
        }

        private void OnClick_RenamePlaylist(object sender, RoutedEventArgs e)
        {
            var dialog = new PlaylistRenameWindow(_playlists[listview_playlist.SelectedIndex]);
            if (dialog.ShowDialog() ?? false)
            {
                _playlists[listview_playlist.SelectedIndex].Name = dialog.Playlist.Name;
            }
        }

        private void OnClick_RemovePlaylist(object sender, RoutedEventArgs e)
        {
            _playlists.RemoveAt(listview_playlist.SelectedIndex);
        }

        private void OnClick_RemoveMedia(object sender, RoutedEventArgs e)
        {
            _playlists[listview_playlist.SelectedIndex].List.RemoveAt(datagrid_medias.SelectedIndex);
        }

        private void OnMouseDoubleClick_PlayMedia(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _currentMedia.PlaylistIndex = listview_playlist.SelectedIndex;
            _currentMedia.MediaIndex = datagrid_medias.SelectedIndex;
            _playlists[_currentMedia.PlaylistIndex].IsPlaying = true;
            _playlists[_currentMedia.PlaylistIndex].List[_currentMedia.MediaIndex].IsPlaying = true;

            MediaSelected?.Invoke(this, _playlists[_currentMedia.PlaylistIndex].List[_currentMedia.MediaIndex].Path);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            e.Cancel = true;
        }
    }
}
