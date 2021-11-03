using JacutemAAI2.WPF.Ferramentas.Internas;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JacutemAAI2.WPF.ViewModel
{
    public class BinariosViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private List<ItemListasDeBinarios> _binarios { get; set; }
        private List<ItemListasDeBinarios> _listasDeImportacao { get; set; }
        public Dictionary<string, DelegateCommand> MyCommand { get; set; }
        private bool _btnExportarEstaAtivo;
        private bool _btnImportarEstaAtivo;
        private bool _btnExportarTodosEstaAtivo = false;
        private bool _btnImportarTodosEstaAtivo = false;
        private bool _listaEstaAtiva = true;
        private bool _animacaoBotaoEstaAtiva = false;
        private bool _animacaoBotaoImportarEstaAtiva = false;
        private ItemListasDeBinarios _binarioSelecionado;
        private ItemListasDeBinarios _listaImportcaoSelecionada;


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

        public ItemListasDeBinarios BinarioSelecionado 
        { 
            get
            { 
                return _binarioSelecionado; 
            } 
            set 
            {               
                _binarioSelecionado = value;
                NotifyPropertyChanged("BinarioSelecionado");
                BtnExportarEstaAtivo = true;
            } 
        }

        public ItemListasDeBinarios ListaImportacaoSelecionada
        {
            get
            {
                return _listaImportcaoSelecionada;
            }
            set
            {
                _listaImportcaoSelecionada = value;
                NotifyPropertyChanged("BinarioSelecionado");
                BtnImportarEstaAtivo = true;
            }
        }

        public List<ItemListasDeBinarios> ListasDeImportacao 
        { get {return _listasDeImportacao ; }
          set 
            {
                _listasDeImportacao = value;
                NotifyPropertyChanged("ListasDeImportacao");
            } 
        }

        public List<ItemListasDeBinarios> Binarios
        {
            get { return _binarios; }
            set
            {
                _binarios = value;
                NotifyPropertyChanged("Binarios");
            }
        }

        public BinariosViewModel()
        {
            MyCommand = new Dictionary<string, DelegateCommand>()
            {
                ["ExportarBin"] = new DelegateCommand(ExportarSelecionado).ObservesCanExecute(() => BtnExportarEstaAtivo),
                ["ExportarTodos"] = new DelegateCommand(ExportarTodos).ObservesCanExecute(() => BtnExportarTodosEstaAtivo),
                ["ImportarSelecionado"] = new DelegateCommand(ImportarSelecionado).ObservesCanExecute(() => BtnImportarEstaAtivo),
                ["ImportarTodos"] = new DelegateCommand(ImportarTodos).ObservesCanExecute(() => BtnImportarTodosEstaAtivo)

            };

            VerificarListas();

        }

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

       

        public async void ExportarSelecionado()
        {
            AnimacaoBotaoEstaAtiva = true;
            DesativarComponentes();
           // await Task.Run(() => AAIBin.Exportar(_binarioSelecionado.Diretorio));
            VerificarListas();
            AtivarComponentes();
            MessageBox.Show(AAIBin.Mensagem);
            AnimacaoBotaoEstaAtiva = false;
            

        }

        public async void ExportarTodos()
        {
            AnimacaoBotaoEstaAtiva = true;
            DesativarComponentes();
            await Task.Run(() => AAIBin.ExportarTodos(Binarios));                     
            VerificarListas();
            AtivarComponentes();
            MessageBox.Show(AAIBin.Mensagem);

            AnimacaoBotaoEstaAtiva = false;
        }

        public async void ImportarSelecionado()
        {
            AnimacaoBotaoImportarEstaAtiva = true;
            DesativarComponentes();
            await Task.Run(() => AAIBin.Importar(_listaImportcaoSelecionada.Diretorio));
            AtivarComponentes();
            AnimacaoBotaoImportarEstaAtiva = false;
            MessageBox.Show(AAIBin.Mensagem);

        }

        public async void ImportarTodos()
        {
            AnimacaoBotaoImportarEstaAtiva = true;
            DesativarComponentes();
            await Task.Run(() => AAIBin.ImportarTodos(ListasDeImportacao));
            AtivarComponentes();
            AnimacaoBotaoImportarEstaAtiva = false;
            MessageBox.Show(AAIBin.Mensagem);

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
            ListasDeImportacao = AAIBin.ObtenhaListasDEImportaco(Properties.Settings.Default.PastaInfoBinarios);
            Binarios = AAIBin.ObtenhaBinariosDaRom(Properties.Settings.Default.DiretorioRomDesmonstada);
           

            if (Binarios.Count > 0)
            {
                BtnExportarTodosEstaAtivo = true;
            }

            if (ListasDeImportacao.Count > 0)
            {
                BtnImportarTodosEstaAtivo = true;
            }
        }
    }

    
}
