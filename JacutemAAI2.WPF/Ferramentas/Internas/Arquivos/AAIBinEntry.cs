using System.IO;


namespace JacutemAAI2.WPF.Ferramentas.Internas
{
    public class AAIBinEntry
    {
        public uint Offset { get; set; }
        public uint Size { get; set; }
        public bool IsCompressed { get; set; }

        public AAIBinEntry(BinaryReader br, bool isLastOffset)
        {
            Offset = br.ReadUInt32();
            Size = br.ReadUInt32();
            IsCompressed = Size > 0x80000000;
            if (IsCompressed)
            {
                uint nextOffset;

                if (isLastOffset)
                    nextOffset = (uint)br.BaseStream.Length;
                else
                    nextOffset = br.ReadUInt32();


                Size = nextOffset - Offset;
                br.BaseStream.Position = br.BaseStream.Position - 4;
            }
            
        }

        public AAIBinEntry(uint offser, uint size, bool isCompressed)
        {
            Offset = offser;
            Size = isCompressed ? size + 0x80000000 : size;
            IsCompressed = isCompressed;
        }

        public void WriteEntry(BinaryWriter bw)
        {
            bw.Write(Offset);
            bw.Write(IsCompressed ? Size + 0x80000000 : Size);
        }
    }
}
