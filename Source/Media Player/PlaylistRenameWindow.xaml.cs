using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
    /// Interaction logic for PlaylistRenameWindow.xaml
    /// </summary>
    public partial class PlaylistRenameWindow : Window
    {
        public PlaylistRenameWindow()
        {
            InitializeComponent();
        }
        public PlaylistRenameWindow(Playlist playlist)
        {
            InitializeComponent();
            Playlist = playlist.ShallowCopy();
            old = playlist.ShallowCopy();
            DataContext = this.Playlist;
        }

        public Playlist Playlist { get; set; }
        public Playlist old { get; set; }
        private void Button_Confirm_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            Playlist = old;
            DialogResult = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txt_name.Focus();
            txt_name.SelectAll();
        }
    }
}
