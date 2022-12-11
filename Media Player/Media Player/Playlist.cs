using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Media_Player
{
    public class Playlist : INotifyPropertyChanged
    {
        private string _name;
        private ObservableCollection<Media> _list;

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
