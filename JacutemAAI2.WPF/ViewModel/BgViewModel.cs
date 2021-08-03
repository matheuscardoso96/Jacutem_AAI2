﻿using Jacutem_AAI2.Imagens;
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

namespace JacutemAAI2.WPF.ViewModel
{
    public class BgViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private Dictionary<string, string> _bGTileSemMapsDi;
        private BGTileSemMap _bGTileSemMap = new BGTileSemMap();
        private BGTileComMap _bGTileComMap = new BGTileComMap();
        KeyValuePair<string, string> _chaveSelecionada;
        public Dictionary<string, DelegateCommand> MyCommand { get; set; }
        private bool _btnExportarEstaAtivo;
        private bool _btnImportarEstaAtivo;
        private bool _btnExportarTodosEstaAtivo = false;
        private bool _btnImportarTodosEstaAtivo = false;
        private bool _listaEstaAtiva = true;
        private bool _animacaoBotaoEstaAtiva = false;
        private bool _animacaoBotaoImportarEstaAtiva = false;
        private Ncgr _ngcrCarregado;
        private BitmapImage _imagemCarregada;
        private InformacoesImagem _informacoesImagem;


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

        public Dictionary<string, string> BGTileSemMapsDi
        {
            get
            {
                return _bGTileSemMapsDi;
            }
            set
            {
                _bGTileSemMapsDi = value;
                NotifyPropertyChanged("BGTileSemMapsDi");

            }
        }




        public KeyValuePair<string,string> CaminhoImagem
        {
            get { return _chaveSelecionada; }
            set
            {
                _chaveSelecionada = value;             
                NotifyPropertyChanged("ChaveSelecionada");
                CarregarNcgr();
            }
        }

        public Ncgr NgcrCarregado
        {
            get { return _ngcrCarregado; }
            set
            {
                _ngcrCarregado = value;
                NotifyPropertyChanged("NgcrCarregado");
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



        public BgViewModel()
        {
            MyCommand = new Dictionary<string, DelegateCommand>()
            {
                ["SalvarImagemCarregada"] = new DelegateCommand(SalvarImagemCarregada)//.ObservesCanExecute(() => BtnExportarEstaAtivo),
              //  ["ExportarTodos"] = new DelegateCommand(ExportarTodos).ObservesCanExecute(() => BtnExportarTodosEstaAtivo),
              //  ["ImportarSelecionado"] = new DelegateCommand(ImportarSelecionado).ObservesCanExecute(() => BtnImportarEstaAtivo),
               // ["ImportarTodos"] = new DelegateCommand(ImportarTodos).ObservesCanExecute(() => BtnImportarTodosEstaAtivo)

            };


            BGTileSemMapsDi = new Dictionary<string, string>();
            foreach (var item in _bGTileComMap.Lista)
            {
                BGTileSemMapsDi.Add(item.Key, item.Value);
            }

            foreach (var item in _bGTileSemMap.Lista)
            {
                BGTileSemMapsDi.Add(item.Key,item.Value);
            }



        }

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



        public async void CarregarNcgr()
        {
            string args = CaminhoImagem.Value;
            NgcrCarregado = await Task.Run(() =>GerenciadorConversaoImagens.CarregarNcgr(args));
            ImagemCarregada = BitmapToImageSource(NgcrCarregado.Imagens[0]);
            InformacoesImagem = new InformacoesImagem
            {
                Altura = NgcrCarregado.Imagens[0].Height,
                Largura = NgcrCarregado.Imagens[0].Width,
                BppString = NgcrCarregado.Char.IntensidadeDeBits == 3 ? "4" : "8",
                QuatidadeCores = NgcrCarregado.ArquivoNclr.Pltt.Paleta.Length / 2
            };




        }

        public void SalvarImagemCarregada() 
        {
            NgcrCarregado.Exportar();
            MessageBox.Show("Imagem salva na pasta imagens.");
        }

        BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
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
            AnimacaoBotaoImportarEstaAtiva = true;
            DesativarComponentes();
          //  await Task.Run(() => AaiBin.Importar(_listaImportcaoSelecionada.Diretorio));
            AtivarComponentes();
            AnimacaoBotaoImportarEstaAtiva = false;
            MessageBox.Show(AaiBin.Mensagem);

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
