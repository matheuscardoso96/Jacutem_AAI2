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
        private bool _btnSalvarEstaAtivo = false;
        private bool _btnCancelarEstaAtivo = false;
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

        public bool BtnSalvarEstaAtivo
        {
            get
            {
                return _btnSalvarEstaAtivo;
            }
            set
            {
                _btnSalvarEstaAtivo = value;
                NotifyPropertyChanged("BtnSalvarEstaAtivo");
            }
        }

        public bool BtnCancelarEstaAtivo
        {
            get
            {
                return _btnCancelarEstaAtivo;
            }
            set
            {
                _btnCancelarEstaAtivo = value;
                NotifyPropertyChanged("BtnCancelarEstaAtivo");
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
                    ImagemCarregada = SelectedTextureInfo.Textura.ToImageSource();
                }
                
            }
        }public BitmapImage ImagemCarregada
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
                ["SalvarImagemCarregada"] = new DelegateCommand(SalvarImagemCarregada).ObservesCanExecute(() => BtnSalvarEstaAtivo),//.ObservesCanExecute(() => BtnExportarEstaAtivo),
                ["CancelarSalvamento"] = new DelegateCommand(CancelarSalvamento).ObservesCanExecute(() => BtnCancelarEstaAtivo),
                ["ImportarSelecionado"] = new DelegateCommand(ImportarSelecionado).ObservesCanExecute(() => BtnImportarEstaAtivo),
                ["ExportarImagemCarregada"] = new DelegateCommand(ExportarImagemCarregada).ObservesCanExecute(() => BtnExportarEstaAtivo)

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
            //ImagemCarregada = NgcrCarregado.Imagens[0].ToImageSource();
            //InformacoesImagem = new InformacoesImagem
            //{
            //    Altura = NgcrCarregado.Imagens[0].Height,
            //    Largura = NgcrCarregado.Imagens[0].Width,
            //    BppString = NgcrCarregado.Char.IntensidadeDeBits == 3 ? "4" : "8",
            //    QuatidadeCores = NgcrCarregado.ArquivoNclr.Pltt.Paleta.Length / 2
            //};

            //Palette = PaletteVisualGenerator.CreateImage(NgcrCarregado.ArquivoNclr.Colors);


        }

        public async void ExportarImagemCarregada()
        {
            //string args = _texturePath.Value;
            //NgcrCarregado.ExportarImagem.Invoke(_texturePath.Value.Split(',').ToList().Last());
            //ImagemCarregada = NgcrCarregado.Imagens[0].ToImageSource();           

        }

        public async void SalvarImagemCarregada() 
        {
            //await Task.Run(() => GerenciadorConversaoImagens.SalvarNcgr(NgcrCarregado, paletaFoiAlterada));
            //BtnCancelarEstaAtivo = false;
            //BtnSalvarEstaAtivo = false;
            //BtnImportarEstaAtivo = true;
            //BtnExportarEstaAtivo = true;
            //MessageBox.Show("Imagem salva na pasta imagens.");
        }

        public void CancelarSalvamento()
        {
            BtnCancelarEstaAtivo = false;
            BtnSalvarEstaAtivo = false;
            LoadBtx();
        }

       

        public async void ExportarTodos()
        {
            AnimacaoBotaoEstaAtiva = true;
            DesativarComponentes();
           // await Task.Run(() => AaiBin.ExportarTodos(Binarios));
            VerificarListas();
            AtivarComponentes();
            MessageBox.Show(AaiBin.Mensagem);

            AnimacaoBotaoEstaAtiva = false;
        }

        public async void ImportarSelecionado()
        {
        
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.DefaultExt = ".png";
            dlg.Filter = "Imagens (.png)|*.png";

           
            Nullable<bool> result = dlg.ShowDialog();

     
            if (result == true)
            {
      
                //NgcrCarregado.ImportarNgcr.Invoke(dlg.FileName);
                //ImagemCarregada = NgcrCarregado.Imagens[0].ToImageSource();
                //BtnImportarEstaAtivo = false;
                //BtnExportarEstaAtivo = false;
                //BtnCancelarEstaAtivo = true;
                //BtnSalvarEstaAtivo = true;
               
            }
           
    

        }

        public async void ImportarTodos()
        {
            AnimacaoBotaoImportarEstaAtiva = true;
            DesativarComponentes();
           // await Task.Run(() => AaiBin.ImportarTodos(ListasDeImportacao));
            AtivarComponentes();
            AnimacaoBotaoImportarEstaAtiva = false;
            MessageBox.Show(AaiBin.Mensagem);

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

        public void VerificarListas()
        {
            //ListasDeImportacao = AaiBin.ObtenhaListasDEImportaco(Properties.Settings.Default.PastaInfoBinarios);
            //Binarios = AaiBin.ObtenhaBinariosDaRom(Properties.Settings.Default.DiretorioRomDesmonstada);


            //if (Binarios.Count > 0)
            //{
            //    BtnExportarTodosEstaAtivo = true;
            //}

            //if (ListasDeImportacao.Count > 0)
            //{
            //    BtnImportarTodosEstaAtivo = true;
            //}
        }
    }

   
}
