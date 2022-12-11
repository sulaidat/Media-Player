using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Path = System.IO.Path;
using System.Collections.Specialized;
using Newtonsoft.Json;
using System.IO;

namespace Media_Player
{
    /// <summary>
    /// Interaction logic for PlaylistWindow.xaml
    /// </summary>
    public partial class PlaylistWindow : Window
    {
        public PlaylistWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string directory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string filePath = Path.Combine(directory, "playlists.json");
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

        ObservableCollection<Playlist> _playlists { get; set; }

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
    }
}
