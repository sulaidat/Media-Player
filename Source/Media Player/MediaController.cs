using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Media_Player
{
    public class MediaController : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private void EmitPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private string source;
        public string Source
        {
            get { return source;  }
            set
            {
                source = value;
                EmitPropertyChanged();
            }
        }
    }
}
