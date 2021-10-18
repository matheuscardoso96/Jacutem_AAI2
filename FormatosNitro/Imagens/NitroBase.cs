using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FormatosNitro.Imagens
{
    public abstract class NitroBase
    {
        public string Id { get; protected set; }
        public ushort ByteOrder { get; protected set; }
        public ushort Version { get; protected set; }
        public uint FileSize { get; protected set; }
        public ushort NextSectionOffset { get; protected set; }
        public ushort SectionCount { get; protected set; }
        public string NitroFilePath { get; set; }
        public List<string> Errors { get; set; }

        public const int HEADER_SIZE = 16;

        public NitroBase(BinaryReader br, string path)
        {
            Errors = new List<string>();
            ValidateNitroFile(br, path);
            if (Errors.Count == 0)
            {
                NitroFilePath = path;
                Id = Encoding.ASCII.GetString(br.ReadBytes(4));
                ByteOrder = br.ReadUInt16();
                Version = br.ReadUInt16();
                FileSize = br.ReadUInt32();
                NextSectionOffset = br.ReadUInt16();
                SectionCount = br.ReadUInt16();
            }
            
            
        }

        public void EscreverPropiedades(BinaryWriter bw)
        {
            bw.Write(Encoding.ASCII.GetBytes(Id));
            bw.Write(ByteOrder);
            bw.Write(Version);
            bw.Write(FileSize);
            bw.Write(NextSectionOffset);
            bw.Write(SectionCount);
        
        }

        private void ValidateNitroFile(BinaryReader br, string path)
        {
            string fileName = Path.GetFileName(path);

            if (br.BaseStream.Length < 32)
            {
                Errors.Add($"Arquivo {fileName} inválido.");
                return;
            }

            byte firstByte = br.ReadByte();
            if (firstByte == 0x52 || firstByte == 0x4E)
            {
                br.BaseStream.Position--;
                return;
            }
            else
            {
                br.BaseStream.Position--;
                Errors.Add($"Arquivo {fileName} inválido ou está comprimido.");
            }

            
        }
    }
}
