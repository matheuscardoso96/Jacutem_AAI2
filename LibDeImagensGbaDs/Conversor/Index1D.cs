using ImageLibGbaDS.Paleta;
using LibDeImagensGbaDs.Formatos.Indexado;
using LibDeImagensGbaDs.TileMap;
using LibDeImagensGbaDs.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace LibDeImagensGbaDs.Conversor
{
    public class Index1D : IConversorIndex
    {
        public int Largura { get; set; }
        public int Altura { get; set; }
        public List<ushort> TileMap { get; set; }
        public Index1D(int altura, int largura, List<ushort> tileMap)
        {
            Largura = largura;
            Altura = altura;
            TileMap = tileMap;
        }
        public Bitmap ConvertaIndexado(IFormatoIndexado formatoIndexado, IPaleta paleta)
        {
           
            List<Bitmap> tiles = new List<Bitmap>();
            Bitmap imagemFinal = new Bitmap(Largura, Altura);
            byte[] valoresAlpha = null;
            for (int i = 0; i < formatoIndexado.Indices.Length; i += 64)
            {
                byte[] indicesTile = new byte[64];
                Array.Copy(formatoIndexado.Indices, i, indicesTile, 0, 64);
                Bitmap tile = new Bitmap(8, 8, PixelFormat.Format32bppArgb);
                
                if (paleta.TemAlpha)
                {
                    valoresAlpha = new byte[64];
                    Array.Copy(formatoIndexado.AlphaValues, i, valoresAlpha, 0, 64);
                }

                ManipuladorDeImagem.GerarBitmap(tile, indicesTile, paleta.Cores, paleta.TemAlpha, valoresAlpha);
                tiles.Add(tile);

            }

            if (TileMap != null)
                FerramentaDeTileMap.MontarImagemComTileMap(TileMap,tiles,imagemFinal);
            else
                FerramentaDeTileMap.MontarImagemComTileMapGerado(tiles, imagemFinal);


            return imagemFinal;

        }
    }
}
