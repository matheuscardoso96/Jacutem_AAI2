using JacutemAAI2.WPF.Images;
using JacutemAAI2.WPF.Images.ImagesMapPath;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media.Imaging;

namespace JacutemAAI2.WPF.ViewModel
{
    public class ImageViewModelBase : INotifyPropertyChanged
    {
        private string _statusText;
        private Visibility _statusBarVisibility;
        private bool _isStatusBarAnimationEnabled;
        private ImageMetadata _imageMetaData;
        private FilePaths _filePaths;
        private KeyValuePair<string, string> _selectPaths;
        private bool _isSaveButtonEnabled;
        private bool _isCancelButtonEnabled;
        private bool _isExportButtonEnabled;
        private bool _isImportButtonEnabled;
        private bool _isListEnabled;
        private BitmapImage _loadedImage;
        private BitmapImage _palette;
        private bool _isExecutingBatchOperation;
        public delegate void Load();
        public Load LoadFile;
        public Dictionary<string, DelegateCommand> ScreenCommands { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public FilePaths FilePaths
        {
            get
            {
                return _filePaths;
            }
            set
            {
                _filePaths = value;
                NotifyPropertyChanged("FilePaths");
            }
        }

        public bool IsExecutingBatchOperation
        {
            get
            {
                return _isExecutingBatchOperation;
            }
            set
            {
                _isExecutingBatchOperation = value;
                NotifyPropertyChanged("IsExportButtonEnabled");
            }
        }

        public bool IsSaveButtonEnabled
        {
            get
            {
                return _isSaveButtonEnabled;
            }
            set
            {
                _isSaveButtonEnabled = value;
                NotifyPropertyChanged("IsSaveButtonEnabled");
            }
        }

        public bool IsCancelButtonEnabled
        {
            get
            {
                return _isCancelButtonEnabled;
            }
            set
            {
                _isCancelButtonEnabled = value;
                NotifyPropertyChanged("IsCancelButtonEnabled");
            }
        }

        public bool IsExportButtonEnabled
        {
            get
            {
                return _isExportButtonEnabled;
            }
            set
            {
                _isExportButtonEnabled = value;
                NotifyPropertyChanged("IsExportButtonEnabled");
            }
        }

        public bool IsImportButtonEnabled
        {
            get
            {
                return _isImportButtonEnabled;
            }
            set
            {
                _isImportButtonEnabled = value;
                NotifyPropertyChanged("IsImportButtonEnabled");
            }
        }

        public bool IsListEnabled
        {
            get
            {
                return _isListEnabled;
            }
            set
            {
                _isListEnabled = value;
                NotifyPropertyChanged("IsListEnabled");
            }
        }

        public string StatusText
        {
            get
            {
                return _statusText;
            }
            set
            {
                _statusText = value;
                NotifyPropertyChanged("StatusText");
            }
        }

        public Visibility StatusBarVisibility
        {
            get
            {
                return _statusBarVisibility;
            }
            set
            {
                _statusBarVisibility = value;
                NotifyPropertyChanged("StatusBarVisibility");
            }
        }
        public BitmapImage LoadedImage
        {
            get
            {
                return _loadedImage;
            }
            set
            {
                _loadedImage = value;
                NotifyPropertyChanged("LoadedImage");
            }
        }

        public BitmapImage Palette
        {
            get
            {
                return _palette;
            }
            set
            {
                _palette = value;
                NotifyPropertyChanged("Palette");
            }
        }

        public ImageMetadata ImageMetaData
        {
            get { return _imageMetaData; }
            set
            {
                _imageMetaData = value;
                NotifyPropertyChanged("ImageMetaData");
            }
        }

        public bool IsStatusBarAnimationEnabled
        {
            get
            {
                return _isStatusBarAnimationEnabled;
            }
            set
            {
                _isStatusBarAnimationEnabled = value;
                NotifyPropertyChanged("IsStatusBarAnimationEnabled");
            }
        }

        public KeyValuePair<string, string> SelectedPath
        {
            get { return _selectPaths; }
            set
            {
                _selectPaths = value;
                NotifyPropertyChanged("SelectedPath");
                if (LoadFile != null)
                {
                    LoadFile.Invoke();
                }
                else
                {
                    MessageBox.Show("A ação LoadFile não foi setada.");
                }
                

            }
        }

        public ImageViewModelBase(FilePaths filePaths)
        {
            FilePaths = filePaths;
            IsListEnabled = true;
            StatusBarVisibility = Visibility.Hidden;
           // _loadFile = new Load(load);

        }
      
        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void SaveChanges() { }
        public virtual void CancelChanges() { }

        protected void EnableStatus(string text)
        {
            StatusBarVisibility = Visibility.Visible;
            IsStatusBarAnimationEnabled = true;
            StatusText = text;
        }

        protected void DisableStatus()
        {
            IsStatusBarAnimationEnabled = false;
            StatusBarVisibility = Visibility.Hidden;
            StatusText = null;
        }

        protected void EnableCancelAndSave() 
        {
            IsCancelButtonEnabled = true;
            IsSaveButtonEnabled = true;
        }

        protected void DisableCancelAndSave()
        {
            IsCancelButtonEnabled = false;
            IsSaveButtonEnabled = false;
        }

        protected void DisableViewComponents()
        {
            IsListEnabled = false;
            IsExportButtonEnabled = false;
            IsImportButtonEnabled = false;
        }

        protected void EnableViewComponents()
        {
            IsListEnabled = true;
            IsExportButtonEnabled = true;
            IsImportButtonEnabled = true;
        }

    }
}
