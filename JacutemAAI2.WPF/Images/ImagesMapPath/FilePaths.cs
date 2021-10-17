using System.Collections.Generic;
using System.ComponentModel;

namespace JacutemAAI2.WPF.Images.ImagesMapPath
{
    public abstract class FilePaths : INotifyPropertyChanged
    {
        public string _type { get; set; }
        public Dictionary<string, string> _list = new Dictionary<string, string>();
        public event PropertyChangedEventHandler PropertyChanged;
        public string Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
                NotifyPropertyChanged("Type");
            }
        }

        public Dictionary<string, string> List
        {
            get
            {
                return _list;
            }
            set
            {
                _list = value;
                NotifyPropertyChanged("Type");
            }
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
