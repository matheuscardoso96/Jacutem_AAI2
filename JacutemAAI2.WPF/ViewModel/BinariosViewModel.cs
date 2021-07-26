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
        public Dictionary<string, DelegateCommand> MyCommand { get; set; }
        private bool _btnExportarEstaAtivo;
        private bool _btnExportarTodosEstaAtivo = true;
        private bool _listaEstaAtiva = true;
        private bool _animacaoBotaoEstaAtiva = false;
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

        private Binario _binarioSelecionado;

        public Binario BinarioSelecionado 
        { 
            get
            { 
                return _binarioSelecionado; 
            } 
            set 
            {               
                _binarioSelecionado = value;
                NotifyPropertyChanged("BinarioSelecionado");
                BtnExportarEstaAtivo = PodeExportarBinario();
            } 
        }

        public List<Binario> Binarios { get; set; }

        public BinariosViewModel()
        {
            MyCommand = new Dictionary<string, DelegateCommand>()
            {
                ["ExportarBin"] = new DelegateCommand(ExportarSelecionado).ObservesCanExecute(() => BtnExportarEstaAtivo),
                ["ExportarTodos"] = new DelegateCommand(ExportarTodos)
               
            };

            ObtenhaArquivos();
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ObtenhaArquivos()
        {
            Binarios = new List<Binario>();
            string[] bins = Directory.GetFiles($"{Properties.Settings.Default.DiretorioRomDesmonstada}\\data","*.bin",SearchOption.AllDirectories);

            foreach (var dirBin in bins)
            {
                if (dirBin.Contains("data\\data"))
                    continue;

                Binarios.Add(new Binario() { Nome = Path.GetFileName(dirBin), Diretorio = dirBin });
            }
        }

        private bool PodeExportarBinario() 
        {
            if (_binarioSelecionado == null)
                return false;
            

            return true;
        
        }

        public async void ExportarSelecionado() 
        {
            AnimacaoBotaoEstaAtiva = true;
            BtnExportarEstaAtivo = false;
            BtnExportarTodosEstaAtivo = false;
            ListaEstaAtiva = false;
            await Task.Run(() => AaiBin.Exportar(_binarioSelecionado.Diretorio));          
            BtnExportarEstaAtivo = true;
            BtnExportarTodosEstaAtivo = true;
            ListaEstaAtiva = true;
            AnimacaoBotaoEstaAtiva = false;
            MessageBox.Show(AaiBin.Mensagem);
        }

        public async void ExportarTodos()
        {
            AnimacaoBotaoEstaAtiva = true;
            BtnExportarEstaAtivo = false;
            BtnExportarTodosEstaAtivo = false;
            ListaEstaAtiva = false;
            await Task.Run(() => AaiBin.ExportarTodos(Properties.Settings.Default.DiretorioRomDesmonstada));
            ListaEstaAtiva = true;
            BtnExportarEstaAtivo = true;
            BtnExportarTodosEstaAtivo = true;
            AnimacaoBotaoEstaAtiva = false;
            MessageBox.Show(AaiBin.Mensagem);
           
            
        }
    }

    public class Binario
    {
        public string Nome { get; set; }
        public string Diretorio { get; set; }

    }
}
