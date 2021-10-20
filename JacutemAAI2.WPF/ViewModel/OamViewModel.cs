using LibDeImagensGbaDs.Sprites;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JacutemAAI2.WPF.ViewModel
{
    public class OamViewModel : INotifyPropertyChanged
    {
        private uint _y;
        private uint _x;
        private uint _width;
        private uint _height;
        private uint _tileId;
        private uint _priority;
        private uint _objSize;
        private uint _paletteId;
        private bool _rotateOrScaling;
        private bool _mozaic;
        private bool _horizontalFlip;
        private bool _verticalFlip;
        private Dictionary<string, int> _oamResolutions;
        private Oam _oam;
        private KeyValuePair<string, int> _settedResolution;
        private int _comboResIndex;

        public event PropertyChangedEventHandler PropertyChanged;

        public Dictionary<string, int> OamResolutions
        {
            get
            {
                return _oamResolutions;
            }
            set 
            {
                _oamResolutions = value;
                NotifyPropertyChanged(nameof(OamResolutions));
            }
        }

        public KeyValuePair<string, int> SettedResolution
        {
            get
            {
                return _settedResolution;
            }
            set
            {
                _settedResolution = value;
                NotifyPropertyChanged(nameof(SettedResolution));
                if (Oam != null)
                {
                    int x = Convert.ToInt32(SettedResolution.Key.Replace(" ", "").Split('x')[0]);
                    int y = Convert.ToInt32(SettedResolution.Key.Replace(" ", "").Split('x')[1]);
                    Oam.SetOamResolution(x, y);
                }
                

            }
        }

        public uint Y
        {
            get { return _y; }
            set
            {
                _y = value;
                NotifyPropertyChanged(nameof(Y));
            }
        }
        public uint X
        {
            get
            {
                return _x;
            }
            set
            {
                _x = value;
                NotifyPropertyChanged(nameof(X));
            }
        }
        public uint Width
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
                NotifyPropertyChanged(nameof(Width));
            }
        }
        public uint Height
        {
            get
            {
                return _height;
            }
            set
            {
                _height = value;
                NotifyPropertyChanged(nameof(Height));
            }
        }
        public uint OBJSize
        {
            get
            {
                return _objSize;
            }
            set
            {
                _objSize = value;
                NotifyPropertyChanged(nameof(OBJSize));
            }
        }
        public uint TileId
        {
            get
            {
                return _tileId;
            }
            set
            {
                _tileId = value;
                NotifyPropertyChanged(nameof(TileId));
            }
        }
        public uint Priority
        {
            get
            {
                return _priority;
            }
            set
            {
                _priority = value;
                NotifyPropertyChanged(nameof(Priority));
            }
        }
        public uint PaletteId
        {
            get
            {
                return _paletteId;
            }
            set
            {
                _paletteId = value;
                NotifyPropertyChanged(nameof(PaletteId));
            }
        }
        public bool RotateOrScaling
        {
            get
            {
                return _rotateOrScaling;
            }
            set
            {
                _rotateOrScaling = value;
                NotifyPropertyChanged(nameof(RotateOrScaling));
            }
        }
        public bool Mozaic
        {
            get
            {
                return _mozaic;
            }
            set
            {
                _mozaic = value;
                NotifyPropertyChanged(nameof(Mozaic));
            }
        }
        public bool HorizontalFlip
        {
            get
            {
                return _horizontalFlip;
            }
            set
            {
                _horizontalFlip = value;
                NotifyPropertyChanged(nameof(HorizontalFlip));
            }
        }
        public bool VerticalFlip
        {
            get
            {
                return _verticalFlip;
            }
            set
            {
                _verticalFlip = value;
                NotifyPropertyChanged(nameof(VerticalFlip));
            }
        }

        public Oam Oam
        {
            get
            {
                return _oam;
            }
            set
            {
                _oam = value;
                NotifyPropertyChanged(nameof(Oam));
            }
        }
        
        public int ComboResIndex
        {
            get
            {
                return _comboResIndex;
            }
            set
            {
                _comboResIndex = value;
                NotifyPropertyChanged(nameof(ComboResIndex));
            }
        }

        public OamViewModel(Oam oam)
        {
            Oam = oam;
            OamResolutions = new Dictionary<string, int>()
            {
                ["8 x 8"] = 0,
                ["16 x 8"] = 1,
                ["32 x 8"] = 2,
                ["8 x 16"] = 3,
                ["16 x 16"] = 4,
                ["32 x 16"] = 5,
                ["8 x 32"] = 6,
                ["16 x 32"] = 7,
                ["32 x 32"] = 8,
                ["64 x 32"] = 9,
                ["32 x 64"] = 10,
                ["64 x 64"] = 11
            };

            X = oam.X;
            Y = oam.Y;
            Width = oam.Width;
            Height = oam.Height;
            OBJSize = oam.OBJSize;
            TileId = oam.TileId;
            Priority = oam.Priority;
            PaletteId = oam.PaletteId;
            RotateOrScaling = oam.RotateOrScaling;
            Mozaic = oam.Mozaic;
            HorizontalFlip = oam.HorizontalFlip;
            VerticalFlip = oam.VerticalFlip;
           

        }

        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
