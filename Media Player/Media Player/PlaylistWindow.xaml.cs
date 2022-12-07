using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

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

            _playlists = new ObservableCollection<Playlist>();
            _playlists.Add(new Playlist() { Name = "Playlist#1", List = new List<Media>() });
            _playlists.Add(new Playlist() { Name = "Playlist#2", List = new List<Media>() });
            _playlists.Add(new Playlist() { Name = "Playlist#3", List = new List<Media>() });
            listbox_playlist.ItemsSource = _playlists;
        }
        ObservableCollection<Playlist> _playlists { get; set; }

        public class Playlist
        {
            public string Name { get; set; }
            public List<Media> List { get; set; }
        }

        public class Media
        {
            public string Name { get; set; }
            public string Path { get; set; }
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            datagrid_medias.ItemsSource = _playlists[listbox_playlist.SelectedIndex].List;
        }

        private void OnClick_NewPlaylist(object sender, RoutedEventArgs e)
        {
            _playlists.Add(new Playlist() { Name = "Playlist", List = new List<Media>() });
        }
    }
}
