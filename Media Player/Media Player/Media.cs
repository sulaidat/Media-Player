using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Media_Player
{
    public class Media : INotifyPropertyChanged
    {
        private string _name;
        private string _path;
        private bool _isPlaying;

        public string Name {
            get { return _name; }
            set
            {
                _name = value;
                EmitPropertyChanged();
            }
        }
        public string Path {
            get { return _path; }   
            set
            {
                _path = value;
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

        public event PropertyChangedEventHandler? PropertyChanged;
        private void EmitPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Media ShallowCopy()
        {
            return (Media)MemberwiseClone();
        }
    }
}
