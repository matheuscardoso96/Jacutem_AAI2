using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace FormatosNitro.Imagens.FNclr
{
    public class Nclr : NitroBase
    {
        public Pltt Pltt { get; set; }
        public Pcmp Pcmp { get; set; }
        public Color[] Colors { get; set; }


        public Nclr(BinaryReader br, string diretorio) : base(br, diretorio)
        {
            if (Errors.Count == 0)
            {
                Pltt = new Pltt(br);
                if (SectionCount > 1)
                    Pcmp = new Pcmp(br);
            }
            
            
            br.Close();         

        }

        public void SalvarNclr()
        {
            MemoryStream novoNclr = new MemoryStream();
            using (BinaryWriter bw = new BinaryWriter(novoNclr))
            {
                base.EscreverPropiedades(bw);
                Pltt.EscreverPropiedades(bw);
                if (Pcmp != null)
                {
                    Pcmp.EscreverPropiedades(bw);
                }
                
            }

            File.WriteAllBytes(base.NitroFilePath, novoNclr.ToArray());
        }
    }
}
