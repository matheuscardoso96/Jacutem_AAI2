using System.IO;


namespace Jacutem_AAI2.Imagens
{
    public class Nclr
    {
        public byte[] PaletaEmBytes { get; set; }
        public int TamanhoPaleta { get; set; }

        public Nclr(string dir)
        {
            using (BinaryReader br = new BinaryReader(new MemoryStream(File.ReadAllBytes(dir))))
            {
                br.BaseStream.Position = 0x20;
                TamanhoPaleta = br.ReadInt32();
                br.BaseStream.Position = 0x28;
                PaletaEmBytes = br.ReadBytes(TamanhoPaleta);
            }
        }
    }
}
