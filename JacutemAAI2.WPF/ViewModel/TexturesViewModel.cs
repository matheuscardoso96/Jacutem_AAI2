using FormatosNitro.Imagens;
using JacutemAAI2.WPF.Ferramentas.Internas;
using JacutemAAI2.WPF.Imagens.MapaDeArquivos;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using JacutemAAI2.WPF.Imagens;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace JacutemAAI2.WPF.ViewModel
{
    public class TexturesViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private Dictionary<string, string> _texturesPath;
        private TexturesMap _texturesMap = new TexturesMap();
        KeyValuePair<string, string> _texturePath;
        public Dictionary<string, DelegateCommand> MyCommand { get; set; }
        private bool _isSaveButtonEnabled = false;
        private bool _isCancelButtonEnabled = false;
        private bool _isExportButtonEnabled;
        private bool _isImportButtonEnabled;
        private bool _isListEnabled = true;
        private bool _isStatusBarAnimationEnabled = false;
        private Visibility _statusBarVisibility;
        private Btx _btx;
        private TextureInfo _textureInfo;
        private BitmapImage _loadedImage;
        private BitmapImage _palette;
        private ImageMetadata _imageMetaData;
        private bool _isExecutingBatchOperation = false;
        private string _statusText;

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



        public TextureInfo SelectedTextureInfo
        {
            get
            {
                return _textureInfo;
            }
            set
            {
                _textureInfo = value;
                NotifyPropertyChanged("SelectedTextureInfo");
                if (SelectedTextureInfo != null)
                {
                    LoadedImage = SelectedTextureInfo.TextureImage.ToImageSource();
                    SetImageMetada(SelectedTextureInfo);
                    IsExportButtonEnabled = true;
                    IsImportButtonEnabled = true;
                    Palette = PaletteVisualGenerator.CreateImage(Btx.PaletteInfos[SelectedTextureInfo.PaletteIndex].Palette);
                }

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

        public Dictionary<string, string> TexturesPaths
        {
            get
            {
                return _texturesPath;
            }
            set
            {
                _texturesPath = value;
                NotifyPropertyChanged("TexturesPaths");

            }
        }

        public KeyValuePair<string,string> TexturePath
        {
            get { return _texturePath; }
            set
            {
                _texturePath = value;             
                NotifyPropertyChanged("TexturePath");
                LoadBtx();
            }
        }

        public Btx Btx
        {
            get { return _btx; }
            set
            {
                _btx = value;
                NotifyPropertyChanged("Btx");
                LoadedImage = null;
                DisableCancelAndSave();
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



        public TexturesViewModel()
        {
            MyCommand = new Dictionary<string, DelegateCommand>()
            {
                ["SaveBtxToFile"] = new DelegateCommand(SaveBtxToFile).ObservesCanExecute(() => IsSaveButtonEnabled),//.ObservesCanExecute(() => BtnExportarEstaAtivo),
                ["CancelChanges"] = new DelegateCommand(CancelChanges).ObservesCanExecute(() => IsCancelButtonEnabled),
                ["ExportSelectedTexture"] = new DelegateCommand(ExportSelectedTexture).ObservesCanExecute(() => IsExportButtonEnabled),
                ["ImportSelectedTexture"] = new DelegateCommand(ImportSelectedTexture).ObservesCanExecute(() => IsImportButtonEnabled),
                ["ExportAllTexturesFromBtx"] = new DelegateCommand(ExportAllTexturesFromBtx).ObservesCanExecute(() => IsExportButtonEnabled),
                ["ImportAllTexturesToBtx"] = new DelegateCommand(ImportAllTexturesToBtx).ObservesCanExecute(() => IsImportButtonEnabled),
                ["ExportarAllBtx"] = new DelegateCommand(ExportarAllBtx),
                ["ImportAllBtx"] = new DelegateCommand(ImportAllBtx)

            };


            TexturesPaths = new Dictionary<string, string>();
            foreach (var item in _texturesMap.Lista)
            {
                TexturesPaths.Add(item.Key, item.Value);
            }

            IsListEnabled = true;
            StatusBarVisibility = Visibility.Hidden;
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async void LoadBtx()
        {
            string args = _texturePath.Value;
            Btx = await Task.Run(() =>GerenciadorConversaoImagens.LoadBtx(args));
        }

        public async void ExportSelectedTexture()
        {
            await Task.Run(() => Btx.ExportOneTexture(SelectedTextureInfo));
            MessageBox.Show($"{Path.GetFileName(Btx.BtxPath)} {SelectedTextureInfo.TextureName} exportado com sucesso.");     

        }

        public async void ExportAllTexturesFromBtx()
        {
            await Task.Run(() => Btx.ExportAllTextures());
            MessageBox.Show($"{Path.GetFileName(Btx.BtxPath)} exportado com sucesso.");
        }

        public void SaveBtxToFile()
        {
            if (!_isExecutingBatchOperation)
            {
                Btx.SaveToFile();
                _ = MessageBox.Show($"{Path.GetFileName(Btx.BtxPath)} salvo com sucesso.");
                DisableCancelAndSave();
            }
            
        }

        public async void ExportarAllBtx()
        {
            await Task.Run(() => _texturesMap.Lista.Values.ToList().ForEach(arg => new Btx(arg).ExportAllTextures()));
            _ = MessageBox.Show("Texutras exportadas com sucesso.");
        }

        public void CancelChanges()
        {
            DisableCancelAndSave();
            LoadBtx();
        }

        public async void ImportSelectedTexture()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.DefaultExt = ".png";
            dlg.Filter = "Imagens (.png)|*.png";
            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                await Task.Run(() => Btx.ImportOneTexture(dlg.FileName));
                LoadedImage = SelectedTextureInfo.TextureImage.ToImageSource();
                EnableStatus("Importando texturas...");
                if (Btx.Errors.Count == 0)
                {
                    _ = MessageBox.Show($"{Path.GetFileName(SelectedTextureInfo.TextureName)} importado com sucesso.");
                    IsCancelButtonEnabled = true;
                    IsSaveButtonEnabled = true;
                }
                else
                {
                    _ = MessageBox.Show($"{string.Join("\r\n", Btx.Errors)}");
                    Btx.Errors = new List<string>();
                }
                
                DisableStatus();
            }

        }

        public async void ImportAllTexturesToBtx()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.DefaultExt = ".png";
            dlg.Filter = "Imagens (.png)|*.png";
            dlg.Multiselect = true;
            bool? result = dlg.ShowDialog();
            if (result == true)
            {
                EnableStatus("Importando texturas...");
                await Task.Run(() => Btx.ImportMultipleTextures(dlg.FileNames));
                LoadedImage = SelectedTextureInfo.TextureImage.ToImageSource();
                if (Btx.Errors.Count == 0)
                {
                    IsCancelButtonEnabled = true;
                    IsSaveButtonEnabled = true;
                    _ = MessageBox.Show($"Imagens importadas para {Path.GetFileName(Btx.BtxPath)} com sucesso.");
                }
                else
                {
                    _ = MessageBox.Show($"{string.Join("\r\n", Btx.Errors)}");
                    Btx.Errors = new List<string>();
                }
                DisableStatus();
            }

        }

        public async void ImportAllBtx()
        {
            _isExecutingBatchOperation = true;
            CommonOpenFileDialog commonOpenFileDialog = new CommonOpenFileDialog();
            commonOpenFileDialog.IsFolderPicker = true;
            var importErros = new List<string>();
            if (commonOpenFileDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                EnableStatus("Importando texturas...");
                DisableViewComponents();
                string[] folders = Directory.GetDirectories(commonOpenFileDialog.FileName);
                foreach (var finalDir in folders)
                {
                    string arg;
                    _texturesMap.Lista.TryGetValue($"{finalDir.Split('\\').Last()}.btx", out arg);
                    if (arg != null)
                    {
                        Btx tmp = new Btx(arg);
                        await Task.Run(() => tmp.ImportMultipleTextures(Directory.GetFiles(finalDir)));
                        if (tmp.Errors.Count == 0)
                        {
                            tmp.SaveToFile();
                        }
                        else
                        {
                            importErros.AddRange(tmp.Errors);
                        }

                        tmp.TextureInfos.ForEach(x => x.TextureImage.Dispose());

                    }
                }
                EnableViewComponents();
                DisableStatus();
                if (importErros.Count == 0)
                {
                    _ = MessageBox.Show($"Imagens importadas com sucesso.");
                }
                else
                {
                    _ = MessageBox.Show($"{string.Join("\r\n", importErros)}");
                }
                

            }
            _isExecutingBatchOperation = false;

        }

        private void SetImageMetada(TextureInfo selectedTextureInfo)
        {
            ImageMetaData = new ImageMetadata(selectedTextureInfo.Width, selectedTextureInfo.Height, selectedTextureInfo.Bpp.ToString(), selectedTextureInfo.ColorCount);

        }

        private void DisableViewComponents()
        {
            IsListEnabled = false;
            IsExportButtonEnabled = false;
            IsImportButtonEnabled = false;
        }

        private void EnableViewComponents()
        {
           IsListEnabled = true;
           IsExportButtonEnabled = true;
           IsImportButtonEnabled = true;
        }

        private void DisableCancelAndSave()
        {
            IsCancelButtonEnabled = false;
            IsSaveButtonEnabled = false;
        }

        private void EnableStatus(string text)
        {
            StatusBarVisibility = Visibility.Visible;
            IsStatusBarAnimationEnabled = true;
            StatusText = text;
        }

        private void DisableStatus()
        {
            IsStatusBarAnimationEnabled = false;
            StatusBarVisibility = Visibility.Hidden;
            StatusText = null;
        }
    }

   
}
