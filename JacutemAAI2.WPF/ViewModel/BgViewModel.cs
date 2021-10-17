using FormatosNitro.Imagens;
using JacutemAAI2.WPF.Ferramentas.Internas;
using JacutemAAI2.WPF.Images.ImagesMapPath;
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
using JacutemAAI2.WPF.Images;
using Microsoft.Win32;

namespace JacutemAAI2.WPF.ViewModel
{
    public class BgViewModel : ImageViewModelBase
    {

        private Ncgr _loadedNcgr;
        public Ncgr LoadedNgcr
        {
            get { return _loadedNcgr; }
            set
            {
                _loadedNcgr = value;
                NotifyPropertyChanged("NgcrCarregado");
            }
        }


        public BgViewModel():base(new BGSPaths())
        {
            LoadFile = LoadNcgr;
            ScreenCommands = new Dictionary<string, DelegateCommand>()
            {
                ["SaveChanges"] = new DelegateCommand(SaveChanges).ObservesCanExecute(() => IsSaveButtonEnabled),//.ObservesCanExecute(() => BtnExportarEstaAtivo),
                ["CancelChanges"] = new DelegateCommand(CancelChanges).ObservesCanExecute(() => IsCancelButtonEnabled),
                ["ImportImageToNgcr"] = new DelegateCommand(ImportImageToNgcr).ObservesCanExecute(() => IsExportButtonEnabled),
                ["ExportNcgrImage"] = new DelegateCommand(ExportNcgrImage).ObservesCanExecute(() => IsExportButtonEnabled),
                ["ExportAllNcgr"] = new DelegateCommand(ExportAllNcgr),
                ["ImportAllNgcr"] = new DelegateCommand(ImportAllNgcr)
            };

        }


        public async void LoadNcgr()
        {
            DisableCancelAndSave();
            EnableViewComponents();
            string args = SelectedPath.Value;
            LoadedNgcr = await Task.Run(() =>NDSImageFactory.LoadNgcr(args));
            LoadedImage = LoadedNgcr.Imagens[0].ToImageSource();
            
            ImageMetaData = new ImageMetadata(
                LoadedNgcr.Imagens[0].Width,
                LoadedNgcr.Imagens[0].Height,
                LoadedNgcr.Char.IntensidadeDeBits == 3 ? "4" : "8",
                LoadedNgcr.ArquivoNclr.Pltt.Paleta.Length / 2);
            Palette = PaletteVisualGenerator.CreateImage(LoadedNgcr.ArquivoNclr.Colors);


        }

        public void ExportNcgrImage()
        {
           LoadedNgcr.ExportarImagem.Invoke();
           MessageBox.Show($"Imagem exportada para: {LoadedNgcr.ExportPath}");

        }

        public async override void SaveChanges() 
        {
            await Task.Run(() => LoadedNgcr.SalvarNCGR(false));
            DisableCancelAndSave();
            MessageBox.Show($"{Path.GetFileName(LoadedNgcr.Diretorio)} salvo.");
            
        }

        public override void CancelChanges()
        {
            LoadNcgr();
            DisableCancelAndSave();
            MessageBox.Show("Operação cancelada.");
        }


        public async void ExportAllNcgr()
        {
           
            await Task.Run(() => FilePaths.List.Values.ToList().ForEach(arg => NDSImageFactory.LoadNgcr(arg).ExportarImagem()));          
            _ = MessageBox.Show("Bgs exportados com sucesso.");
        }

        public async void ImportImageToNgcr()
        {
            OpenFileDialog dlg = new OpenFileDialog();
           dlg.DefaultExt = ".png";
           dlg.Filter = "Imagens (.png)|*.png";

            var result = dlg.ShowDialog();

            if (result == true)
            {
                LoadedNgcr.ImportarNgcr.Invoke(dlg.FileName);
                LoadedImage = LoadedNgcr.Imagens[0].ToImageSource();
                EnableCancelAndSave();
            }
           

        }

        public async void ImportAllNgcr()
        {
          

        }


        
    }

   
}
