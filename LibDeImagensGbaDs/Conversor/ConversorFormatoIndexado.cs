using LibDeImagensGbaDs.Formatos.Indexado;
using ImageLibGbaDS.Paleta;
using System.Drawing;
using LibDeImagensGbaDs.Enums;
using System.Collections.Generic;
using LibDeImagensGbaDs.Util;
using System.Drawing.Imaging;
using LibDeImagensGbaDs.Sprites;

namespace LibDeImagensGbaDs.Conversor
{
    public class ConversorFormatoIndexado : IConversor
    {
        public IPaleta Paleta { get; private set;}
        public IConversorDeProfundidadeDeCor ConversorDeProfundidadeDeCor { get; set; }
        public EFormatoPaleta FormatoPaleta { get; set; }
        public ConversorDeTiposDeGraficos ConversorDeTipos { get; set; }
        public int Altura { get; set; }
        public int Largura { get; set; }

        public ConversorFormatoIndexado(byte[] paleta, EFormatoPaleta formatoPaleta , int altura, int largura, ProfundidaDeCor profundidadeDeCor, EModoDimensional modoDimensional, List<ushort> tilemap = null, bool temAlpha = false)
        {
            Altura = altura;
            Largura = largura;
            ObtenhaFormatoPaleta(formatoPaleta, paleta, temAlpha);
            ObtenhaProfundidadeDeCor(profundidadeDeCor);
        }

        public ConversorFormatoIndexado(byte[] paleta, EFormatoPaleta formatoPaleta, ProfundidaDeCor profundidadeDeCor, EModoDimensional modoDimensional, List<Oam> oams)
        {
            ObtenhaFormatoPaleta(formatoPaleta, paleta, false);
            ObtenhaProfundidadeDeCor(profundidadeDeCor);
        }

        private void ObtenhaFormatoPaleta(EFormatoPaleta formatoPaleta, byte[] paleta, bool temAlpha) 
        {
            switch (formatoPaleta)
            {
                case EFormatoPaleta.NintendoDS:
                    Paleta = new BGR565(paleta, temAlpha);
                    break;
                case EFormatoPaleta.GBA:
                    Paleta = new BGR565(paleta, temAlpha);
                    break;
                default:
                    break;
            }
        }

        private void ObtenhaProfundidadeDeCor(ProfundidaDeCor profundidadeCor)
        {
            switch (profundidadeCor)
            {
                case ProfundidaDeCor.F1BBP:
                    ConversorDeProfundidadeDeCor = new F1BPP();
                    break;
                case ProfundidaDeCor.F2BBP:
                    ConversorDeProfundidadeDeCor = new F2BPP();
                    break;
                case ProfundidaDeCor.F4BBP:
                    ConversorDeProfundidadeDeCor = new F4BPP();
                    break;
                case ProfundidaDeCor.F8BBP:
                    ConversorDeProfundidadeDeCor = new F8BPP();
                    break;
                case ProfundidaDeCor.FA3I5:
                    ConversorDeProfundidadeDeCor = new FA3I5();
                    break;
                case ProfundidaDeCor.FA5I3:
                    ConversorDeProfundidadeDeCor = new FA5I3();
                    break;
            }
        }

       

        private void ObtenhaModoDimensional(EModoDimensional eModoDimensional) 
        {
            switch (eModoDimensional)
            {
                case EModoDimensional.M1D:
                    ConversorIndexado = new Index1D(Altura, Largura, TileMap);
                    break;
                case EModoDimensional.M2D:
                    ConversorIndexado = new Index2D(Altura, Largura);
                    break;

            }

        }

        public Bitmap ConvertaParaBmp(byte[] arquivo, int tamanho , int enderecoInicial) 
        {
            ConversorDeProfundidadeDeCor.ObtenhaIndicesPorPixel(arquivo,tamanho, enderecoInicial);
            var imagemFinal = ConversorIndexado.ConvertaIndexado(ConversorDeProfundidadeDeCor, Paleta);

            return imagemFinal;
        }

        public List<object> ConvertaParaBin(Bitmap imagem)
        {
            if (imagem.PixelFormat != PixelFormat.Format32bppArgb)
            {
                imagem = ManipuladorDeImagem.MudarPixelFormatPra32Bpp(imagem);
            }

            List<object> resultado = ConversorIndexado.GerarIndeces(ConversorDeProfundidadeDeCor, Paleta,imagem);        
            return resultado;
        }
    }
}
