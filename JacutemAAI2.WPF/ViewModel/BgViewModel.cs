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
using Microsoft.WindowsAPICodePack.Dialogs;
using FormatosNitro.Imagens.FNcgr;

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
                NotifyPropertyChanged("LoadedNgcr");
            }
        }


        public BgViewModel():base(new BGSPaths())
        {
            LoadFile = LoadNcgr;
            BatchImportOp = NgcrBatchImport;
            ScreenCommands = new Dictionary<string, DelegateCommand>()
            {
                ["SaveChanges"] = new DelegateCommand(SaveChanges).ObservesCanExecute(() => IsSaveButtonEnabled),
                ["CancelChanges"] = new DelegateCommand(CancelChanges).ObservesCanExecute(() => IsCancelButtonEnabled),
                ["ImportImageToNgcr"] = new DelegateCommand(ImportImageToNgcr).ObservesCanExecute(() => IsExportButtonEnabled),
                ["ExportNcgrImage"] = new DelegateCommand(ExportNcgrImage).ObservesCanExecute(() => IsExportButtonEnabled),
                ["ExportAllNcgr"] = new DelegateCommand(ExportAllNcgr),
                ["BatchImportOperation"] = new DelegateCommand(BatchImportOperation)
            };

        }


        public async void LoadNcgr()
        {
            DisableCancelAndSave();
            EnableViewComponents();
            string args = SelectedPath.Value;
            LoadedNgcr = await Task.Run(() => NDSImageFactory.LoadNgcr(args));

            if (LoadedNgcr.AllErrors.Count == 0)
            {
                LoadedImage = LoadedNgcr.ConvertedImage.ToImageSource();

                ImageMetaData = new ImageMetadata(
                    LoadedNgcr.ConvertedImage.Width,
                    LoadedNgcr.ConvertedImage.Height,
                    LoadedNgcr.Char.IntensidadeDeBits == 3 ? "4" : "8",
                    LoadedNgcr.ArquivoNclr.Pltt.Paleta.Length / 2);
                Palette = PaletteVisualGenerator.CreateImage(LoadedNgcr.ArquivoNclr.Colors);
            }
            else
            {
                MessageBox.Show($"Alguns erros foram encontrados: {string.Join("\r\n", LoadedNgcr.AllErrors)}");
                LoadedNgcr = null;
            }
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
            MessageBox.Show($"{Path.GetFileName(LoadedNgcr.NitroFilePath)} salvo.");
            
        }

        public override void CancelChanges()
        {
            LoadNcgr();
            DisableCancelAndSave();
            MessageBox.Show("Operação cancelada.");
        }


        public async void ExportAllNcgr()
        {
            EnableStatus("Exportando ncgrs...");
            await Task.Run(() => FilePaths.List.Values.ToList().ForEach(arg => NDSImageFactory.LoadNgcr(arg).ExportarImagem()));
            _ = MessageBox.Show("Bgs exportados com sucesso.");
            DisableStatus();
        }

        public async void ImportImageToNgcr()
        {
            OpenFileDialog dlg = new OpenFileDialog();
           dlg.DefaultExt = ".png";
           dlg.Filter = "Imagens (.png)|*.png";

            var result = dlg.ShowDialog();

            if (result == true)
            {
                EnableStatus("Importando imagem...");
                await Task.Run(() => LoadedNgcr.ImportarNgcr.Invoke(dlg.FileName));
                if (LoadedNgcr.AllErrors.Count == 0)
                {
                    LoadedImage = LoadedNgcr.ConvertedImage.ToImageSource();
                    EnableCancelAndSave();
                }
                else
                {
                    MessageBox.Show($"Alguns erros foram encontrados: {string.Join("\r\n", LoadedNgcr.AllErrors)}");
                    LoadedNgcr.AllErrors.Clear();
                }
                DisableStatus();
            }
           

        }

        private void NgcrBatchImport(string[] pngsDirectory)
        {

            foreach (var path in pngsDirectory)
            {
                string arg;
                string ngcrName = $"{path.Split('\\').Last().Replace(".png","").Replace("com_","").Replace("jpn_","")}.ncgr";
                FilePaths.List.TryGetValue(ngcrName, out arg);
                if (arg != null)
                {
                    Ncgr tmpNcgr = NDSImageFactory.LoadNgcr(arg);

                    if (tmpNcgr.Errors.Count == 0)
                    {
                        tmpNcgr.ImportarNgcr.Invoke(path);
                        if (tmpNcgr.Errors.Count == 0)
                        {
                            tmpNcgr.SalvarNCGR(false);
                        }
                        else
                        {
                            ErrorsLog.AddRange(tmpNcgr.Errors);
                        }

                    }
                    else
                    {
                        ErrorsLog.AddRange(tmpNcgr.Errors);
                    }

                }
                else
                {
                    ErrorsLog.Add($"{ngcrName} não encontrado.");
                }
            }

            if (ErrorsLog.Count == 0)
            {
                _ = MessageBox.Show($"Imagens importadas com sucesso.");
            }
            else
            {
                _ = MessageBox.Show($"{string.Join("\r\n", ErrorsLog)}");
                ErrorsLog.Clear();
            }

        }



    }

   
}
