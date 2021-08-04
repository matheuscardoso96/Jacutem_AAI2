using ImageLibGbaDS.Paleta;
using LibDeImagensGbaDs.Formatos.Indexado;
using LibDeImagensGbaDs.Sprites;
using LibDeImagensGbaDs.TileMap;
using LibDeImagensGbaDs.Util;
using nQuant;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace LibDeImagensGbaDs.Conversor
{
    public class ConversorDeTiposDeGraficos
    {
        public int Largura { get; set; }
        public int Altura { get; set; }
        public List<ushort> TileMap { get; set; }
        public List<Oam> Oams { get; set; }
        public ConversorDeTiposDeGraficos(int altura, int largura, List<ushort> tileMap)
        {
            Largura = largura;
            Altura = altura;
            TileMap = tileMap;
        }

        public ConversorDeTiposDeGraficos(List<Oam> valoresOam)
        {

        }


        public Bitmap Converta1DComOuSemTileMap(IConversorDeProfundidadeDeCor formatoIndexado, IPaleta paleta)
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
                FerramentaDeTileMap.MontarImagemComTileMap(TileMap, tiles, imagemFinal);
            else
                FerramentaDeTileMap.MontarImagemComTileMapGerado(tiles, imagemFinal);


            return imagemFinal;

        }

        public Bitmap Converta2D(IConversorDeProfundidadeDeCor formatoIndexado, IPaleta paleta)
        {
            Bitmap final = new Bitmap(Largura, Altura, PixelFormat.Format32bppArgb);
            byte[] valoresAlpha = null;
            if (paleta.TemAlpha)
            {
                valoresAlpha = new byte[formatoIndexado.Indices.Length];
                Array.Copy(formatoIndexado.AlphaValues, 0, valoresAlpha, 0, valoresAlpha.Length);
            }

            ManipuladorDeImagem.GerarBitmap(final, formatoIndexado.Indices, paleta.Cores, paleta.TemAlpha, valoresAlpha);
            return final;

        }


        public List<object> Gerar1DComOuSemTileMap(IConversorDeProfundidadeDeCor formatoIndexado, IPaleta paleta, Bitmap imagem)
        {
           

            List<Bitmap> tiles = ManipuladorDeImagem.DividaImagemEmTiles(imagem);


            if (TileMap != null)
                TileMap = FerramentaDeTileMap.GerarTileMap(tiles);

            List<byte> indices = new List<byte>();
            foreach (var tile in tiles)
            {
                Color[] cores = ManipuladorDeImagem.ObtenhaCoresDeImagem(tile);
                foreach (var cor in cores)
                    indices.Add(paleta.ObtenhaIndexCorMaisProxima(cor));

            }

            List<object> final = new List<object>() { formatoIndexado.GereIndices(indices.ToArray()) };

            if (TileMap != null)
                final.Add(TileMap);

            return final;
        }

        public List<object> Gerar2D(IConversorDeProfundidadeDeCor formatoIndexado, IPaleta paleta, Bitmap imagem)
        {
            List<byte> indices = new List<byte>();
            Color[] cores = ManipuladorDeImagem.ObtenhaCoresDeImagem(imagem);

            foreach (var cor in cores)
                indices.Add(paleta.ObtenhaIndexCorMaisProxima(cor));

            List<object> final = new List<object>() { formatoIndexado.GereIndices(indices.ToArray()) };
            return final;
        }



    }
}
