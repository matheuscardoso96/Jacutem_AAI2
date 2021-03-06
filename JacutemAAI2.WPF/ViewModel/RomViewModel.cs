using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using JacutemAAI2.WPF.Ferramentas.Externas;
using Prism.Commands;
using System.Windows.Forms;
using JacutemAAI2.WPF.Managers;

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
            folderBrowserDialog.SelectedPath = ConfigurationManager.ObtenhaUltimoDiretorioSelecionado();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK) 
            {
                DiretorioDestino = folderBrowserDialog.SelectedPath;
                ConfigurationManager.SetaUltimoDiretorioSelecionado(DiretorioDestino);
                ConfigurationManager.SalvarConfiguracoes();
            }
               
        }

        public void AdicionarCaminhoRomDesmotada()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.SelectedPath = ConfigurationManager.ObtenhaUltimoDiretorioSelecionado();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                DiretorioRomDesmontada = folderBrowserDialog.SelectedPath;
                ConfigurationManager.SetaUltimoDiretorioSelecionado(DiretorioRomDesmontada);
                ConfigurationManager.SalvarConfiguracoes();
            }

        }



        public async void ExportarRom() 
        {
            TxtDirNdsEstaAtivo = false; 
            BtnExportarEstaAtivo = false;
            BtnSelecionarEstaAtivo  = false;
            ConfigurationManager.SetarDiretorioRomDesmotanda($"{_diretorioDestino}\\ROM_Desmontada");
            ConfigurationManager.SalvarConfiguracoes();
            await Task.Run(() => NdsTool.ExportNdsFiles(_diretorioDaRom, ConfigurationManager.ObtenhaDiretorioRomDesmotanda()));
            await Task.Run(() => BinaryManager.Initialize());
           // GerenciadoEstaticoDeViewsModels.BinariosViewModel.VerificarListas();
            System.Windows.MessageBox.Show(NdsTool.Message);
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
            ConfigurationManager.SetarDiretorioRomDesmotanda($"{_diretorioRomDesmontada}");
            ConfigurationManager.SalvarConfiguracoes();
            await Task.Run(() => NdsTool.CreateNewNdsFile(ConfigurationManager.ObtenhaDiretorioRomDesmotanda(), "AAI2_"));
            System.Windows.MessageBox.Show(NdsTool.Message);
            TxtDirNdsEstaAtivo = true;
            BtnExportarEstaAtivo = true;
            BtnSelecionarEstaAtivo = true;
            BtnGerarNovaRomEstaAtivo = true;


        }

   
    }
}
