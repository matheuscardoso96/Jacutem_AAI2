using LibDeImagensGbaDs.Util;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;


namespace LibDeImagensGbaDs.TileMap
{
    public static class TileMapTool
    {
        public static Bitmap CreateWithGenericTilemap(List<Bitmap> tiles, Bitmap imagemFinal)
        {
            List<ushort> tileMap = Enumerable.Range(0, tiles.Count).Select(x => (ushort)x).ToList();
            return CreateWithTilemap(tileMap, tiles, imagemFinal, true);
        }

        public static Bitmap CreateWithTilemap(List<ushort> tileMap, List<Bitmap> tiles, Bitmap imagemFinal, bool foiGerado = false)
        {
            using (Graphics g = Graphics.FromImage(imagemFinal))
            {
                int contador = 0;
                for (int y = 0; y < imagemFinal.Height; y += 8)
                {
                    for (int x = 0; x < imagemFinal.Width; x += 8)
                    {
                        Bitmap tile;
                        if (foiGerado)
                        {
                            tile = tiles[tileMap[contador]];
                        }
                        else
                        {
                            int valor = tileMap[contador];
                            int tileNum = valor & 0x3FF;
                            valor >>= 10;
                            tile = tiles[tileNum].Clone(new Rectangle(0, 0, 8, 8), PixelFormat.Format32bppArgb);
                            int horizotal = valor & 1;
                            valor >>= 1;
                            int vertical = valor & 1;

                            if (horizotal == 1)
                                tile.RotateFlip(RotateFlipType.Rotate180FlipY);

                            if (vertical == 1)
                                tile.RotateFlip(RotateFlipType.Rotate180FlipX);


                        }

                        g.DrawImage(tile, x, y);
                        contador++;


                    }
                }

                tiles.Clear();
            }


            return imagemFinal;
        }



        public static List<ushort> GenerateTileMap(List<Bitmap> tiles)
        {
            List<ushort> tilemap = new List<ushort>();
            List<Bitmap> unicas = ObtenhaTilesUnicas(tiles);

            foreach (Bitmap tile in tiles)
            {
                int contador = 0;

                foreach (Bitmap tileEscolhida in unicas)
                {

                    if (CompareTiles(tile, tileEscolhida))
                    {
                        break;
                    }

                    contador++;
                }

                tilemap.Add((ushort)contador);
            }

            tiles = unicas;

            return tilemap;
        }

        private static List<Bitmap> ObtenhaTilesUnicas(List<Bitmap> tilesImg)
        {
            List<Bitmap> tiles = new List<Bitmap>();

            foreach (var item in tilesImg)
            {
                bool existe = false;

                foreach (var tl in tiles)
                {
                    if (CompareTiles(item, tl))
                    {
                        existe = true;
                        break;
                    }
                }

                if (!existe)
                {
                    tiles.Add(item);
                }


            }

            return tiles;
        }

        private static bool CompareTiles(Bitmap bmp1, Bitmap bmp2)
        {           
            Color[] cores1 = BitmapExtesions.GetColors(bmp1);
            Color[] cores2 = BitmapExtesions.GetColors(bmp2);          
          
            int contador = 0;

            for (int i = 0; i < cores1.Length; i++)
            {
                if (CoresSaoIguais(cores1[i], cores2[i]))
                    contador++;
            }


            return contador == 63;
        }

        private static bool CoresSaoIguais(Color color1, Color color2)
        {
            return color1.R == color2.R && color1.G == color2.G && color1.B == color2.B;
        }
    }
   
    
}

