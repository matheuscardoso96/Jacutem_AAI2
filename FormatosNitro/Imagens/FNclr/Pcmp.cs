using System.IO;
using System.Text;

namespace FormatosNitro.Imagens.FNclr
{
    public class Pcmp
    {
        public string Id { get; set; }
        public uint TamanhoPcmp { get; set; }
        public ushort QuatidadeDePaletas { get; set; }
        public ushort Desconhecido { get; set; }
        public uint EnderecoIdPaletas { get; set; }
        public ushort[] IdsDePaletas { get; set; }

        public Pcmp(BinaryReader br)
        {

            Id = Encoding.ASCII.GetString(br.ReadBytes(4));
            TamanhoPcmp = br.ReadUInt32();
            QuatidadeDePaletas = br.ReadUInt16();
            Desconhecido = br.ReadUInt16();
            EnderecoIdPaletas = br.ReadUInt16();
            IdsDePaletas = new ushort[QuatidadeDePaletas];
            for (int i = 0; i < QuatidadeDePaletas; i++)
            {
                IdsDePaletas[i] = br.ReadUInt16();
            }
        }

        public void EscreverPropiedades(BinaryWriter bw)
        {
            bw.Write(Encoding.ASCII.GetBytes(Id));
            bw.Write(TamanhoPcmp);
            bw.Write(QuatidadeDePaletas);
            bw.Write(Desconhecido);
            bw.Write(EnderecoIdPaletas);
            for (int i = 0; i < QuatidadeDePaletas; i++)
            {
                bw.Write(IdsDePaletas[i]);
            }


        }

    }
}
