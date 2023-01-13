using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Media_Player
{
    public class Playlist : INotifyPropertyChanged
    {
        private string _name;
        private ObservableCollection<Media> _list;
        private bool _isPlaying;
        private SolidColorBrush _background;

        public string Name { 
            get { return _name; }
            set
            {
                _name = value;
                EmitPropertyChanged();
            }
        }
        public ObservableCollection<Media> List
        {
            get { return _list; }   
            set
            {
                _list = value;
                EmitPropertyChanged();
            }
        }
        public bool IsPlaying
        {
            get { return _isPlaying; }
            set
            {
                _isPlaying = value;
                EmitPropertyChanged();
            }
        }
        public SolidColorBrush Background
        {
            get { return _background; }
            set
            {
                _background = value;
                EmitPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void EmitPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Playlist ShallowCopy()
        {
            return (Playlist)MemberwiseClone();
        }
    }
}
