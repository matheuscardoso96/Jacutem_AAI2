using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using JacutemAAI2.WPF.Ferramentas.Externas;
using Prism.Commands;
using System.Collections;
using Microsoft.Win32;
using MaterialDesignThemes.Wpf;
using System.Windows.Forms;
using JacutemAAI2.WPF.Gerenciadores;
using JacutemAAI2.WPF.Views;

namespace JacutemAAI2.WPF.ViewModel
{
    public class RomViewModel : INotifyPropertyChanged
    {  
        private string _diretorioDaRom;
        private string _diretorioDestino;
        private string _diretorioRomDesmontada = Properties.Settings.Default.DiretorioRomDesmonstada;
        private bool _podeExtrairRom = false;
        private bool _podeCriarNovaRom = false;
        private bool _txtDirNdsEstaAtivo = true;
        private bool _btnExportarEstaAtivo  = true;
        private bool _btnSelecionarEstaAtivo = true;
        private bool _btnGerarNovaRomEstaAtivo = true;
        public Dictionary<string, DelegateCommand> MyCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public RomViewModel()
        {
            MyCommand = new Dictionary<string,DelegateCommand>() 
            { 
             ["ExportarRom"] =  new DelegateCommand(ExportarRom).ObservesCanExecute(() => PodeExtrairRom), 
             ["AdicionarCaminho"] = new DelegateCommand(AdicionarCaminho),
             ["AdicionarCaminhoDestino"] = new DelegateCommand(AdicionarCaminhoDestino), 
             ["AdicionarCaminhoRomDesmotada"] = new DelegateCommand(AdicionarCaminhoRomDesmotada),
             ["GerarNovaRom"] = new DelegateCommand(GerarNovaRom).ObservesCanExecute(() => PodeCriarNovaRom)
            };
        }

        public string DiretorioDaRom 
        { 
            get 
            { 
                return _diretorioDaRom; 
            } 
            set 
            {
                _diretorioDaRom = value;
                PodeExtrairRom = TemComoApertar();
                NotifyPropertyChanged(nameof(DiretorioDaRom));
            } 
        }

        public string DiretorioDestino
        {
            get
            {
                return _diretorioDestino;
            }
            set
            {
                _diretorioDestino = value;
                PodeExtrairRom = TemComoApertar();
                NotifyPropertyChanged(nameof(DiretorioDestino));
            }
        }

        public string DiretorioRomDesmontada
        {
            get
            {
                PodeCriarNovaRom = TemComoApertarBotaoCriarNovaRom();
                return _diretorioRomDesmontada;
            }
            set
            {
                PodeCriarNovaRom = TemComoApertarBotaoCriarNovaRom();
                _diretorioRomDesmontada = value;
                NotifyPropertyChanged(nameof(DiretorioRomDesmontada));
            }
        }

        public bool PodeExtrairRom
        {
            get
            {
                return _podeExtrairRom;
            }
            set
            {
                _podeExtrairRom = value;
                NotifyPropertyChanged(nameof(PodeExtrairRom));
            }
        }

        public bool PodeCriarNovaRom
        {
            get
            {
                return _podeCriarNovaRom;
            }
            set
            {
                _podeCriarNovaRom = value;
                NotifyPropertyChanged(nameof(PodeCriarNovaRom));
            }
        }

        public bool TxtDirNdsEstaAtivo
        {
            get
            {
                return _txtDirNdsEstaAtivo;
            }
            set
            {
                _txtDirNdsEstaAtivo = value;
                NotifyPropertyChanged(nameof(TxtDirNdsEstaAtivo));
            }
        }

        public bool BtnSelecionarEstaAtivo
        {
            get
            {
                return _btnSelecionarEstaAtivo;
            }
            set
            {
                _btnSelecionarEstaAtivo = value;
                NotifyPropertyChanged(nameof(BtnSelecionarEstaAtivo));
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
                NotifyPropertyChanged(nameof(BtnExportarEstaAtivo));
            }
        }

        public bool BtnGerarNovaRomEstaAtivo
        {
            get
            {
                return _btnGerarNovaRomEstaAtivo;
            }
            set
            {
                _btnGerarNovaRomEstaAtivo = value;
                NotifyPropertyChanged(nameof(BtnGerarNovaRomEstaAtivo));
            }
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public bool TemComoApertar()
        {
            if (string.IsNullOrWhiteSpace(_diretorioDaRom) || string.IsNullOrWhiteSpace(_diretorioDestino))
                return false;
            

            return true;
        }

        public bool TemComoApertarBotaoCriarNovaRom()
        {
            if (string.IsNullOrWhiteSpace(_diretorioRomDesmontada))
                return false;


            return true;
        }

        public void AdicionarCaminho() 
        {

            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                DiretorioDaRom = openFileDialog.FileName;
           

        }


        public void AdicionarCaminhoDestino()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.SelectedPath = GerenciadoDeConfiguracoes.ObtenhaUltimoDiretorioSelecionado();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK) 
            {
                DiretorioDestino = folderBrowserDialog.SelectedPath;
                GerenciadoDeConfiguracoes.SetaUltimoDiretorioSelecionado(DiretorioDestino);
                GerenciadoDeConfiguracoes.SalvarConfiguracoes();
            }
               
        }

        public void AdicionarCaminhoRomDesmotada()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.SelectedPath = GerenciadoDeConfiguracoes.ObtenhaUltimoDiretorioSelecionado();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                DiretorioRomDesmontada = folderBrowserDialog.SelectedPath;
                GerenciadoDeConfiguracoes.SetaUltimoDiretorioSelecionado(DiretorioRomDesmontada);
                GerenciadoDeConfiguracoes.SalvarConfiguracoes();
            }

        }



        public async void ExportarRom() 
        {
            TxtDirNdsEstaAtivo = false; 
            BtnExportarEstaAtivo = false;
            BtnSelecionarEstaAtivo  = false;
            GerenciadoDeConfiguracoes.SetarDiretorioRomDesmotanda($"{_diretorioDestino}\\ROM_Desmontada");
            GerenciadoDeConfiguracoes.SalvarConfiguracoes();
            await Task.Run(() => NdsTool.DesmontarArquivoNds(_diretorioDaRom, GerenciadoDeConfiguracoes.ObtenhaDiretorioRomDesmotanda()));
            GerenciadoEstaticoDeViewsModels.BinariosViewModel.VerificarListas();
            System.Windows.MessageBox.Show(NdsTool.Mensagem);
            DiretorioDaRom = "";
            DiretorioDestino = "";
            TxtDirNdsEstaAtivo = true;
            BtnExportarEstaAtivo = true;
            BtnSelecionarEstaAtivo = true;


        }

        public async void GerarNovaRom()
        {
            TxtDirNdsEstaAtivo = false;
            BtnExportarEstaAtivo = false;
            BtnSelecionarEstaAtivo = false;
            BtnGerarNovaRomEstaAtivo = false;
            GerenciadoDeConfiguracoes.SetarDiretorioRomDesmotanda($"{_diretorioRomDesmontada}");
            GerenciadoDeConfiguracoes.SalvarConfiguracoes();
            await Task.Run(() => NdsTool.NovoArquivoNds(GerenciadoDeConfiguracoes.ObtenhaDiretorioRomDesmotanda(), "AAI2_"));
            System.Windows.MessageBox.Show(NdsTool.Mensagem);
            TxtDirNdsEstaAtivo = true;
            BtnExportarEstaAtivo = true;
            BtnSelecionarEstaAtivo = true;
            BtnGerarNovaRomEstaAtivo = true;


        }

   
    }
}
