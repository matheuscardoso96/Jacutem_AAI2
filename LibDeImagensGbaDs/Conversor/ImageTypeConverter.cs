using LibDeImagensGbaDs.Paleta;
using LibDeImagensGbaDs.Formats.Indexed;
using LibDeImagensGbaDs.Sprites;
using LibDeImagensGbaDs.TileMap;
using LibDeImagensGbaDs.Util;
using nQuant;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using LibDeImagensGbaDs.Tipos;

namespace LibDeImagensGbaDs.Conversor
{
    public static class ImageTypeConverter
    {
        public static Bitmap TiledToBmp(byte[] uncompressedIndices, int width, int height, IPalette palette, List<ushort> tileMap = null, byte [] alphaValues = null)
        {

            List<Bitmap> tiles = new List<Bitmap>();
            Bitmap finalImage = new Bitmap(width, height);

            for (int i = 0; i < uncompressedIndices.Length; i += 64)
            {
                byte[] indicesTile = new byte[64];
                Array.Copy(uncompressedIndices, i, indicesTile, 0, 64);
                Bitmap tile = new Bitmap(8, 8, PixelFormat.Format32bppArgb);

                tile.PoulateBitmap(indicesTile, palette.Colors, alphaValues);
                tiles.Add(tile);

            }

            if (tileMap != null)
                TileMapTool.CreateWithTilemap(tileMap, tiles, finalImage);
            else
                TileMapTool.CreateWithGenericTilemap(tiles, finalImage);

            tiles = null;
            return finalImage;

        }

        public static Bitmap NotTiledToBmp(byte[] uncompressedIndices, int width, int height, IPalette palette, byte[] alphaValues = null)
        {
            Bitmap finalImage = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            finalImage.PoulateBitmap(uncompressedIndices, palette.Colors, alphaValues);
            return finalImage;

        }

        public static Bitmap ConvertDirectToBmp()
        {
            return null;
        }


        public static byte[] GenerateTiledIndices(Bitmap imagem, IPalette paleta, TileMapType tileMap, bool hasTileMap)
        {
            List<Bitmap> tiles = BitmapExtesions.SlitIntoTiles(imagem,8,8);

             if (hasTileMap)
            {
                tileMap.Tiles = tiles;
                tileMap.Tilemap = TileMapTool.GenerateTileMap(tileMap);
                tiles = tileMap.Tiles;
            }       

            List<byte> indexes = new List<byte>();
            foreach (var tile in tiles)
            {
                Color[] colors = tile.GetColors();
                foreach (var cor in colors)
                    indexes.Add(paleta.GetNearColorIndex(cor));

            }

            return indexes.ToArray();
        }

        public static byte[] GenerateNotTiledToBmp(Bitmap imagem, IPalette paleta)
        {
            List<byte> indexes = new List<byte>();
            Color[] cores = imagem.GetColors();

            foreach (var cor in cores)
                indexes.Add(paleta.GetNearColorIndex(cor));

            return indexes.ToArray();
        }



    }
}
