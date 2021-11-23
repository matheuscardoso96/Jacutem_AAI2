using System.IO;

namespace FormatosNitro.Imagens.FNscr
{
    public class Nscr : NitroBase
    {
        public Scrn Scrn { get; set; }
       
        public Nscr(BinaryReader br, string diretorio) : base(br, diretorio)
        {
            if (Errors.Count == 0)
            {
                Scrn = new Scrn(br);
            }
            
            br.Close();

        }

        public void SalvarNscr() 
        {
            MemoryStream novoNscr = new MemoryStream();
            using (BinaryWriter bw = new BinaryWriter(novoNscr))
            {
                base.EscreverPropiedades(bw);
                Scrn.EscreverPropiedades(bw);
               
            }

            File.WriteAllBytes(base.NitroFilePath, novoNscr.ToArray());
        }
    }
}
