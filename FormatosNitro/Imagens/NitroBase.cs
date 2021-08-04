using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FormatosNitro.Imagens
{
    public abstract class NitroBase
    {
        public string Id { get; protected set; }
        public ushort OrdemDosBytes { get; protected set; }
        public ushort Versao { get; protected set; }
        public uint TamanhoDoArquivo { get; protected set; }
        public ushort EnderecoProximaSecao { get; protected set; }
        public ushort QuantidadeDeSecoes { get; protected set; }
        public string Diretorio { get; set; }

        public const int TamanhoCabecalho = 16;

        public NitroBase(BinaryReader br, string diretorio)
        {
            Diretorio = diretorio;
            Id = Encoding.ASCII.GetString(br.ReadBytes(4));
            OrdemDosBytes = br.ReadUInt16();
            Versao = br.ReadUInt16();
            TamanhoDoArquivo = br.ReadUInt32();
            EnderecoProximaSecao = br.ReadUInt16();
            QuantidadeDeSecoes = br.ReadUInt16();
        }

        public void EscreverPropiedades(BinaryWriter bw) 
        {
            bw.Write(Encoding.ASCII.GetBytes(Id));
            bw.Write(OrdemDosBytes);
            bw.Write(Versao);
            bw.Write(TamanhoDoArquivo);
            bw.Write(EnderecoProximaSecao);
            bw.Write(QuantidadeDeSecoes);
        
        }
    }
}
