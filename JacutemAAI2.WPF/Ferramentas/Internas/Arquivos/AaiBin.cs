using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using dsdecmp.Formats.Nitro;

namespace JacutemAAI2.WPF.Ferramentas.Internas
{
    public class AAIBin
    {
        private const string _binaryExportPath = "__Binarios";
        private const string _jpn = "jpn";
        private const string _com = "com";
        public static string Message { get; set; }
        public List<AAIBinEntry> Entries { get; set; }
        public string BinPath { get; set; }
        public string ExportPath { get; set; }
        private readonly LZ11 _lz11;
       

        public AAIBin(string dirBin)
        {
            _lz11 = new LZ11();
            BinPath = dirBin;
            using (BinaryReader br = new BinaryReader(File.OpenRead(dirBin)))
            {
                Entries = ReadEntries(br);
            }

            if (IsExportNeeded(dirBin))
            {
                ExportFiles(dirBin, ExportPath);
            }
        }

        public bool IsExportNeeded(string dirBin)
        {
            string folderPrefix = dirBin.Contains(_jpn) ? _jpn : _com;
            string exportPath = $"{_binaryExportPath}\\{folderPrefix}_{Path.GetFileName(dirBin).Replace(".bin", "")}";
            ExportPath = exportPath;     

            if (!Directory.Exists(exportPath))
            {
                Directory.CreateDirectory(exportPath);
                return true;
            }
            
           return CheckFileCount(exportPath);
        }

        private bool CheckFileCount(string exportPath) 
        {
            if (Directory.GetFiles(exportPath, "*", SearchOption.TopDirectoryOnly).Length != Entries.Count)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static List<AAIBinEntry> ReadEntries(BinaryReader br)
        {
            var entries = new List<AAIBinEntry>();
            int entryCount = br.ReadInt32() / 8;
            br.BaseStream.Position = 0;

            for (int i = 0; i < entryCount; i++)
            {
                bool isLastEntry = false;
                if (i == entryCount - 1)
                    isLastEntry = true;

                entries.Add(new AAIBinEntry(br, isLastEntry));

            }

            return entries;

        }

        private void ExportFiles(string dirBin, string dirExt)
        {
            using (BinaryReader br = new BinaryReader(File.Open(dirBin, FileMode.Open)))
            {
                int contador = 0;

                foreach (AAIBinEntry entrada in Entries)
                {
                    br.BaseStream.Position = entrada.Offset;
                    byte[] file = br.ReadBytes((int)entrada.Size);
                    File.WriteAllBytes($"{dirExt}\\{contador:0000}_{Path.GetFileName(dirBin)}", file);
                    contador++;
                }

            }
        }


        public byte[] GetFile(AAIBinEntry entry,string fileName)
        {
            string path = GetFileFullPath(fileName);
            byte[] arquivo = File.ReadAllBytes(path);
            return entry.IsCompressed ? DecompressOrCompress(_lz11, arquivo,false) : arquivo;

        }

        public void ReplaceFile(AAIBinEntry fileEntryToChange, byte[] file, string fileName)
        {
            string path = GetFileFullPath(fileName);
            File.WriteAllBytes(path, fileEntryToChange.IsCompressed? file: DecompressOrCompress(_lz11,file, true));
        }

        private string GetFileFullPath(string fileName) 
        {
            return $"{ExportPath}\\{fileName.Replace(Path.GetExtension(fileName), ".bin")}";
        }

        public void PackIntoBinary() 
        {           
            uint tamanhoTabela = (uint)Entries.Count * 8;
            uint offset = tamanhoTabela;

            MemoryStream newBinary = new MemoryStream();
            using (BinaryWriter bw = new BinaryWriter(newBinary))
            {
                bw.BaseStream.Position = offset;

                for (int i = 0; i < Entries.Count; i++)
                {
                    byte[] file = File.ReadAllBytes($"{ExportPath}\\{i:0000}.bin");
                    Entries[i].Size = (uint)file.Length;
                    Entries[i].Offset = offset;
                    offset += (uint)file.Length;
                    bw.Write(file);
                }

                bw.BaseStream.Position = 0;
                Entries.ForEach(e => e.WriteEntry(bw));
            }

            File.WriteAllBytes(BinPath,newBinary.ToArray());
        }
        private static byte[] DecompressOrCompress(LZ11 LZ11, byte[] arquivo, bool compress)
        {
            MemoryStream output = new MemoryStream();

            if (compress)
            {
                _ = LZ11.Compress(new MemoryStream(arquivo), arquivo.Length, output);
            }
            else
            {
                _ = LZ11.Decompress(new MemoryStream(arquivo), arquivo.Length, output);
            }

            return output.ToArray();
        }

        

    }
}
