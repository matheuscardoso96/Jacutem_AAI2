using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using FormatosNitro.Imagens;
using JacutemAAI2.WPF.Ferramentas.Internas;
using JacutemAAI2.WPF.Images;
using JacutemAAI2.WPF.Images.ImagesMapPath;
using LibDeImagensGbaDs.Sprites;

namespace JacutemAAI2.WPF.ViewModel
{
    public class SpritesViewModel : ImageViewModelBase
    {
        private Ncgr _loadedNcgr;
        private List<Ebk> _ebks;
        private Ebk _selectedEbk;
        private List<Oam> _oams;
        private Oam _selectOam;
        private OamViewModel _oamVM;

        public Ncgr LoadedNgcr
        {
            get { return _loadedNcgr; }
            set
            {
                _loadedNcgr = value;
                NotifyPropertyChanged("LoadedNgcr");
            }
        }

        public List<Ebk> Ebks
        {
            get { return _ebks; }
            set
            {
                _ebks = value;
                NotifyPropertyChanged("Ebks");
            }
        }

        public Ebk SelectedEbk
        {
            get { return _selectedEbk; }
            set
            {
                _selectedEbk = value;
                NotifyPropertyChanged("SelectedEbk");
                LoadFrame(SelectedEbk);
            }
        }

        public List<Oam> Oams
        {
            get { return _oams; }
            set
            {
                _oams = value;
                NotifyPropertyChanged("Oams");
            }
        }

        public Oam SelectOam
        {
            get { return _selectOam; }
            set
            {
                _selectOam = value;
                NotifyPropertyChanged("SelectOam");
                if (SelectOam != null)
                {
                    LoadedImage = SpriteHelpers.MarkSelectOam(LoadedNgcr.ConvertedImage, SelectOam).ToImageSource();               
                    OamVM = new OamViewModel(SelectOam);
                    //OamVM.ComboResIndex = -1;
                    //OamVM.ComboResIndex = OamVM.OamResolutions[SelectOam.ToString()];
                }
                
            }
        }

        public OamViewModel OamVM
        {
            get { return _oamVM; }
            set
            {
                _oamVM = value;
                NotifyPropertyChanged("OamVM");

            }
        }

        public SpritesViewModel():base(new SpritesPaths())
        {
            LoadFile = LoadNcgr;
        }

        public async void LoadNcgr()
        {
            LoadedImage = null;
            Oams = null;          
            DisableCancelAndSave();
            EnableViewComponents();
            string args = SelectedPath.Value;
            LoadedNgcr = await Task.Run(() => NDSImageFactory.LoadNgcr(args));

            if (LoadedNgcr.AllErrors.Count > 0)
            {
                MessageBox.Show($"Alguns erros foram encontrados: {string.Join("\r\n", LoadedNgcr.AllErrors)}");
                LoadedNgcr = null;
            }
            else
            {
                Ebks = LoadedNgcr.ArquivoNcer.Cebk.Ebks;
            }

            

            //LoadedImage = LoadedNgcr.ConvertedImage.ToImageSource();

            //ImageMetaData = new ImageMetadata(
            //    LoadedNgcr.ConvertedImage.Width,
            //    LoadedNgcr.ConvertedImage.Height,
            //    LoadedNgcr.Char.IntensidadeDeBits == 3 ? "4" : "8",
            //    LoadedNgcr.ArquivoNclr.Pltt.Paleta.Length / 2);
            //Palette = PaletteVisualGenerator.CreateImage(LoadedNgcr.ArquivoNclr.Colors);
        }

        public async void LoadFrame(Ebk ebk)
        {
            if (ebk != null)
            {
                await Task.Run(() => LoadedNgcr.LoadNGCRImageWithNcer(ebk));
                LoadedImage = LoadedNgcr.ConvertedImage.ToImageSource();

                ImageMetaData = new ImageMetadata(
                    LoadedNgcr.ConvertedImage.Width,
                    LoadedNgcr.ConvertedImage.Height,
                    LoadedNgcr.Char.IntensidadeDeBits == 3 ? "4" : "8",
                    LoadedNgcr.ArquivoNclr.Pltt.Paleta.Length / 2);

                Oams = ebk.Oams;
            }

            
           // Palette = PaletteVisualGenerator.CreateImage(LoadedNgcr.ArquivoNclr.Colors);
        }
    }
}
