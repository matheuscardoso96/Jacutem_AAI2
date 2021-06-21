using System.IO;


namespace Jacutem_AAI2.Arquivos
{
    public class EntradaAAIBin
    {
        public uint Endereco { get; set; }
        public uint Tamanho { get; set; }
        public bool Comprimido { get; set; }

        public EntradaAAIBin(BinaryReader br, bool ehUltimo)
        {
            Endereco = br.ReadUInt32();
            Tamanho = br.ReadUInt32();
            Comprimido = Tamanho > 0x80000000? true : false;
            if (Comprimido)
            {
                uint proximoEndereco = 0;

                if (ehUltimo)
                    proximoEndereco = (uint)br.BaseStream.Length;
                else
                    proximoEndereco = br.ReadUInt32();


                Tamanho = proximoEndereco - Endereco;
                br.BaseStream.Position = br.BaseStream.Position - 4;
            }
            
        }

        public EntradaAAIBin(uint endereco, uint tamanho, bool comprimido)
        {
            Endereco = endereco;
            Tamanho = comprimido ? tamanho + 0x80000000 : tamanho;
            Comprimido = comprimido;
        }

        public void EscreverEntrada(BinaryWriter bw)
        {
            bw.Write(Endereco);
            bw.Write(Comprimido ? Tamanho + 0x80000000 : Tamanho);
        }
    }
}
