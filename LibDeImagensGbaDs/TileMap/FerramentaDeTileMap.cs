using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;


namespace LibDeImagensGbaDs.TileMap
{
    public static class FerramentaDeTileMap
    {
        public static Bitmap MontarImagemComTileMapGerado(List<Bitmap> tiles, Bitmap imagemFinal)
        {
            List<ushort> tileMap = Enumerable.Range(0, tiles.Count).Select(x => (ushort)x).ToList();
            return MontarImagemComTileMap(tileMap, tiles, imagemFinal);
        }
        public static Bitmap MontarImagemComTileMap(List<ushort> tileMap, List<Bitmap> tiles, Bitmap imagemFinal)
        {
            using (Graphics g = Graphics.FromImage(imagemFinal))
            {
                int contador = 0 ;
                for (int y = 0; y < imagemFinal.Height; y += 8)
                {
                    for (int x = 0; x < imagemFinal.Width; x += 8)
                    {
                        int valor = tileMap[contador];
                        int tileNum = valor & 0x3FF;
                        valor >>= 10;
                        var tile = tiles[tileNum].Clone(new Rectangle(0,0,8,8), PixelFormat.Format32bppArgb);
                        int horizotal = valor & 1;
                        valor >>= 1;
                        int vertical = valor & 1;

                        if (horizotal == 1)
                            tile.RotateFlip(RotateFlipType.Rotate180FlipY);

                        if (vertical == 1)
                            tile.RotateFlip(RotateFlipType.Rotate180FlipX);

                        g.DrawImage(tile, x, y);
                        contador++;
                    }
                }

                tiles.Clear();
            }

            
            return imagemFinal; 
        }
        
    }
}
