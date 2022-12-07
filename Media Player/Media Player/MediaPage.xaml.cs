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
    /// Interaction logic for MediaPage.xaml
    /// </summary>
    public partial class MediaPage : Page
    {
        public MediaPage()
        {
            InitializeComponent();
        }

        public MediaController _mediaManager;

        private void MediaPage_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = this._mediaManager;
        }
    }
}
