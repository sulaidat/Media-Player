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

namespace Media_Player
{
    /// <summary>
    /// Interaction logic for Playlist.xaml
    /// </summary>
    public partial class PlaylistPage : Page
    {

        List<Playlist> _playlists { get; set; }

        public PlaylistPage()
        {
            InitializeComponent();

            _playlists = new List<Playlist>();
            _playlists.Add(new Playlist() { Name="Playlist#1"});
            _playlists.Add(new Playlist() { Name="Playlist#2"});
            _playlists.Add(new Playlist() { Name="Playlist#3"});
            listbox_playlist.ItemsSource = _playlists;
        }

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
    }
}
