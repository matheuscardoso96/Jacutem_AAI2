﻿using FormatosNitro.Imagens;
using JacutemAAI2.WPF.Ferramentas.Internas;
using JacutemAAI2.WPF.Images.ImagesMapPath;
using Prism.Commands;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using JacutemAAI2.WPF.Images;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace JacutemAAI2.WPF.ViewModel
{
    public class TexturesViewModel : ImageViewModelBase
    {
        private Btx _btx;
        private TextureInfo _textureInfo;

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


        public TexturesViewModel():base(new TexturesMap())
        {
            ScreenCommands = new Dictionary<string, DelegateCommand>()
            {
                ["SaveChanges"] = new DelegateCommand(SaveChanges).ObservesCanExecute(() => IsSaveButtonEnabled),//.ObservesCanExecute(() => BtnExportarEstaAtivo),
                ["CancelChanges"] = new DelegateCommand(CancelChanges).ObservesCanExecute(() => IsCancelButtonEnabled),
                ["ExportSelectedTexture"] = new DelegateCommand(ExportSelectedTexture).ObservesCanExecute(() => IsExportButtonEnabled),
                ["ImportSelectedTexture"] = new DelegateCommand(ImportSelectedTexture).ObservesCanExecute(() => IsImportButtonEnabled),
                ["ExportAllTexturesFromBtx"] = new DelegateCommand(ExportAllTexturesFromBtx).ObservesCanExecute(() => IsExportButtonEnabled),
                ["ImportAllTexturesToBtx"] = new DelegateCommand(ImportAllTexturesToBtx).ObservesCanExecute(() => IsImportButtonEnabled),
                ["ExportarAllBtx"] = new DelegateCommand(ExportarAllBtx),
                ["ImportAllBtx"] = new DelegateCommand(ImportAllBtx)

            };
        }
       
        public async void LoadBtx()
        {
            string args = SelectedPath.Value;
            Btx = await Task.Run(() =>NDSImageFactory.LoadBtx(args));
            if (Btx.Errors.Count > 0)
            {
                MessageBox.Show(string.Join("\r\n",Btx.Errors)); 
                Btx = null;
            }

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

        public override void SaveChanges()
        {
            if (!IsExecutingBatchOperation)
            {
                Btx.SaveToFile();
                _ = MessageBox.Show($"{Path.GetFileName(Btx.BtxPath)} salvo com sucesso.");
                DisableCancelAndSave();
            }
            
        }

        public override void CancelChanges()
        {
            DisableCancelAndSave();
            LoadBtx();
        }

        public async void ExportarAllBtx()
        {
            await Task.Run(() => FilePaths.List.Values.ToList().ForEach(arg => new Btx(arg).ExportAllTextures()));
            _ = MessageBox.Show("Texutras exportadas com sucesso.");
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
            IsExecutingBatchOperation = true;
            CommonOpenFileDialog commonOpenFileDialog = new CommonOpenFileDialog();
            commonOpenFileDialog.IsFolderPicker = true;
            
            if (commonOpenFileDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
              await BtxBatchImport(commonOpenFileDialog.FileName);                
            }
            IsExecutingBatchOperation = false;

        }

        private async Task BtxBatchImport(string folderPaths) 
        {
            var importErros = new List<string>();
            EnableStatus("Importando texturas...");
            DisableViewComponents();
            
            string[] folders = Directory.GetDirectories(folderPaths);
            foreach (var finalDir in folders)
            {
                string arg;
                string btxName = $"{finalDir.Split('\\').Last()}.btx";
                FilePaths.List.TryGetValue(btxName, out arg);
                if (arg != null)
                {
                    Btx tmp = new Btx(arg);
                    
                    if (tmp.Errors.Count == 0)
                    {
                        await Task.Run(() =>  tmp.ImportMultipleTextures(Directory.GetFiles(finalDir)));
                        if (tmp.Errors.Count == 0)
                        {
                            tmp.SaveToFile();
                        }
                        else
                        {
                            importErros.AddRange(tmp.Errors);
                        }

                    }
                    else
                    {
                        importErros.AddRange(tmp.Errors);
                    }

                }
                else
                {
                    importErros.AddRange(Btx.Errors);
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

        public void SetImageMetada(TextureInfo selectedTextureInfo)
        {
            ImageMetaData = new ImageMetadata(selectedTextureInfo.Width, selectedTextureInfo.Height, selectedTextureInfo.Bpp.ToString(), selectedTextureInfo.ColorCount);
        }     
       
    }

   
}
