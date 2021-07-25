using LibDeImagensGbaDs.Formatos.Indexado;
using ImageLibGbaDS.Paleta;
using System.Drawing;
using LibDeImagensGbaDs.Enums;
using System.Collections.Generic;

namespace LibDeImagensGbaDs.Conversor
{
    public class ConversorFormatoIndexado : IConversor
    {
        public IPaleta Paleta { get; private set;}
        public IFormatoIndexado BitsPorPixel { get; set; }
        public IConversorIndex IndexConverter { get; set; }
        public int Altura { get; set; }
        public int Largura { get; set; }

        public ConversorFormatoIndexado(byte[] paleta, EFormatoPaleta formatoPaleta , int altura, int largura, EIndexFormat eIndexFormat, EModoDimensional modoDimensional, List<ushort> tilemap = null, bool temAlpha = false)
        {
            Altura = altura;
            Largura = largura;

            switch (modoDimensional)
            {
                case EModoDimensional.M1D:
                    IndexConverter = new Index1D(altura, largura, tilemap);
                    break;
                case EModoDimensional.M2D:
                    IndexConverter = new Index2D(altura, largura);
                    break;

            }

            switch (eIndexFormat)
            {
                case EIndexFormat.F1BBP:
                    BitsPorPixel = new F1BPP(altura, largura);
                    break;
                case EIndexFormat.F2BBP:
                    BitsPorPixel = new F2BPP(altura, largura);
                    break;
                case EIndexFormat.F4BBP:
                    BitsPorPixel = new F4BPP(altura, largura);
                    break;
                case EIndexFormat.F8BBP:
                    BitsPorPixel = new F8BPP(altura, largura);
                    break;
                case EIndexFormat.FA3I5:
                    BitsPorPixel = new FA3I5(altura, largura);
                    break;
                case EIndexFormat.FA5I3:
                    BitsPorPixel = new FA5I3(altura, largura);
                    break;
            }

            switch (formatoPaleta)
            {
                case EFormatoPaleta.BGR565:
                    Paleta = new BGR565(paleta, temAlpha);
                    break;
                default:
                    break;
            }
        }
        
        public Bitmap ConvertaParaBmp(byte[] arquivo, int tamanho , int enderecoInicial) 
        {
            BitsPorPixel.ObtenhaIndicesPorPixel(arquivo,tamanho, enderecoInicial);
            var imagemFinal = IndexConverter.ConvertaIndexado(BitsPorPixel, Paleta);

            return imagemFinal;
        }

        
    }
}
