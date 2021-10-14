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
        private bool _btnExportarEstaAtivo;
        private bool _btnImportarEstaAtivo;
        private bool _btnExportarTodosEstaAtivo = false;
        private bool _btnImportarTodosEstaAtivo = false;
        private bool _listaEstaAtiva = true;
        private bool _animacaoBotaoEstaAtiva = false;
        private bool _animacaoBotaoImportarEstaAtiva = false;
        private Btx _btx;
        private TextureInfo _textureInfo;
        private BitmapImage _imagemCarregada;
        private BitmapImage _palette;
        private InformacoesImagem _informacoesImagem;
        private bool paletaFoiAlterada = false;

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
                    ImagemCarregada = SelectedTextureInfo.TextureImage.ToImageSource();
                    SetPropertiesInformation(SelectedTextureInfo);
                    BtnExportarEstaAtivo = true;
                    BtnImportarEstaAtivo = true;
                }

            }
        }   

        public BitmapImage ImagemCarregada
        {
            get
            {
                return _imagemCarregada;
            }
            set
            {
                _imagemCarregada = value;
                NotifyPropertyChanged("ImagemCarregada");
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

        public bool BtnExportarEstaAtivo
        {
            get
            {
                return _btnExportarEstaAtivo;
            }
            set
            {
                _btnExportarEstaAtivo = value;
                NotifyPropertyChanged("BtnExportarEstaAtivo");
            }
        }
        public bool BtnExportarTodosEstaAtivo
        {
            get
            {
                return _btnExportarTodosEstaAtivo;
            }
            set
            {
                _btnExportarTodosEstaAtivo = value;
                NotifyPropertyChanged("BtnExportarTodosEstaAtivo");
            }
        }

        public bool BtnImportarEstaAtivo
        {
            get
            {
                return _btnImportarEstaAtivo;
            }
            set
            {
                _btnImportarEstaAtivo = value;
                NotifyPropertyChanged("BtnImportarEstaAtivo");
            }
        }

        public bool BtnImportarTodosEstaAtivo
        {
            get
            {
                return _btnImportarTodosEstaAtivo;
            }
            set
            {
                _btnImportarTodosEstaAtivo = value;
                NotifyPropertyChanged("BtnImportarTodosEstaAtivo");
            }
        }

        public bool ListaEstaAtiva
        {
            get
            {
                return _listaEstaAtiva;
            }
            set
            {
                _listaEstaAtiva = value;
                NotifyPropertyChanged("ListaEstaAtiva");
            }
        }

        public bool AnimacaoBotaoEstaAtiva
        {
            get
            {
                return _animacaoBotaoEstaAtiva;
            }
            set
            {
                _animacaoBotaoEstaAtiva = value;
                NotifyPropertyChanged("AnimacaoBotaoEstaAtiva");
            }
        }

        public bool AnimacaoBotaoImportarEstaAtiva
        {
            get
            {
                return _animacaoBotaoImportarEstaAtiva;
            }
            set
            {
                _animacaoBotaoImportarEstaAtiva = value;
                NotifyPropertyChanged("AnimacaoBotaoImportarEstaAtiva");
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
                ImagemCarregada = null;
               // BtnImportarEstaAtivo = true;
                //BtnExportarEstaAtivo = true;
            }
        }
        public InformacoesImagem InformacoesImagem
        {
            get { return _informacoesImagem; }
            set
            {
                _informacoesImagem = value;
                NotifyPropertyChanged("InformacoesImagem");
            }
        }



        public TexturesViewModel()
        {
            MyCommand = new Dictionary<string, DelegateCommand>()
            {
                ["SaveBtxToFile"] = new DelegateCommand(SaveBtxToFile).ObservesCanExecute(() => IsSaveButtonEnabled),//.ObservesCanExecute(() => BtnExportarEstaAtivo),
                ["CancelChanges"] = new DelegateCommand(CancelChanges).ObservesCanExecute(() => IsCancelButtonEnabled),
                ["ExportSelectedTexture"] = new DelegateCommand(ExportSelectedTexture).ObservesCanExecute(() => BtnExportarEstaAtivo),
                ["ImportSelectedTexture"] = new DelegateCommand(ImportSelectedTexture).ObservesCanExecute(() => BtnImportarEstaAtivo),
                ["ExportAllTexturesFromBtx"] = new DelegateCommand(ExportAllTexturesFromBtx).ObservesCanExecute(() => BtnExportarEstaAtivo),
                ["ImportAllTexturesToBtx"] = new DelegateCommand(ImportAllTexturesToBtx).ObservesCanExecute(() => BtnExportarEstaAtivo),
                ["ExportarAllBtx"] = new DelegateCommand(ExportarAllBtx)

            };


            TexturesPaths = new Dictionary<string, string>();
            foreach (var item in _texturesMap.Lista)
            {
                TexturesPaths.Add(item.Key, item.Value);
            }

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
            Btx.SaveToFile();
            MessageBox.Show($"{Path.GetFileName(Btx.BtxPath)} salvo com sucesso.");
            DisableCancelAndSave();
        }

        public async void ExportarAllBtx()
        {
            await Task.Run(() => _texturesMap.Lista.Values.ToList().ForEach(arg => new Btx(arg).ExportAllTextures()));
            MessageBox.Show("Texutras exportadas com sucesso.");

            AnimacaoBotaoEstaAtiva = false;
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
                ImagemCarregada = SelectedTextureInfo.TextureImage.ToImageSource();
               _ = MessageBox.Show($"{Path.GetFileName(SelectedTextureInfo.TextureName)} importado com sucesso.");
                IsCancelButtonEnabled = true;
                IsSaveButtonEnabled = true;
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
                await Task.Run(() => Btx.ImportMultipleTextures(dlg.FileNames));
                ImagemCarregada = SelectedTextureInfo.TextureImage.ToImageSource();
                IsCancelButtonEnabled = true;
                IsSaveButtonEnabled = true;
                _ = MessageBox.Show($"Imagens importadas para {Path.GetFileName(Btx.BtxPath)} com sucesso.");
                
            }

        }

        private void SetPropertiesInformation(TextureInfo selectedTextureInfo)
        {
            InformacoesImagem = new InformacoesImagem
            {
                Altura = selectedTextureInfo.Height,
                Largura = selectedTextureInfo.Width,
                BppString = selectedTextureInfo.Bpp.ToString(),
                QuatidadeCores = selectedTextureInfo.ColorCount
            };
        }

        private void DesativarComponentes()
        {

            BtnExportarEstaAtivo = false;
            BtnExportarTodosEstaAtivo = false;
            ListaEstaAtiva = false;
            BtnImportarEstaAtivo = false;
            BtnImportarTodosEstaAtivo = false;


        }

        private void AtivarComponentes()
        {

            BtnExportarEstaAtivo = true;
            BtnExportarTodosEstaAtivo = true;
            BtnImportarEstaAtivo = true;
            ListaEstaAtiva = true;
            BtnImportarTodosEstaAtivo = true;


        }

        private void DisableCancelAndSave()
        {
            IsCancelButtonEnabled = false;
            IsSaveButtonEnabled = false;
        }

    }

   
}
