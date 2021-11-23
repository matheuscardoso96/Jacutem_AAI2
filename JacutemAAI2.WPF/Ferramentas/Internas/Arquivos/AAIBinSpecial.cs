using System.Collections.Generic;
using System.IO;

namespace JacutemAAI2.WPF.Ferramentas.Internas
{
    public class AAIBinSpecial
    {
        public static void ExporteBin(string binario)
        {
            using (BinaryReader br = new BinaryReader(new MemoryStream(File.ReadAllBytes(binario))))
            {
                int tamanhoArquivo = (int)br.BaseStream.Length;
                List<int> ponteiros = new List<int>();
                List<int> tamanhos = new List<int>();
                int ponteiro1 = br.ReadInt32();
                int ponteiro2 = br.ReadInt32();
                int ponteiro3 = br.ReadInt32();
                ponteiros.Add(ponteiro1);
                tamanhos.Add(ponteiro2 - ponteiro1);
                ponteiros.Add(ponteiro2);
                tamanhos.Add(ponteiro3 - ponteiro2);
                ponteiros.Add(ponteiro3);
                tamanhos.Add(tamanhoArquivo - ponteiro3);


                if (!Directory.Exists(binario.Replace(".bin", "")))
                {
                    Directory.CreateDirectory(binario.Replace(".bin", ""));

                }

                br.BaseStream.Position = ponteiros[0];
                byte[] ncer = br.ReadBytes(tamanhos[0]);


                File.WriteAllBytes(binario.Replace(".bin", "") + "\\" + Path.GetFileName(binario).Replace(".bin", ".ncer"), ncer);

                br.BaseStream.Position = ponteiros[1];
                byte[] nanr = br.ReadBytes(tamanhos[1]);
                File.WriteAllBytes(binario.Replace(".bin", "") + "\\" + Path.GetFileName(binario).Replace(".bin", ".nanr"), nanr);

                br.BaseStream.Position = ponteiros[2];
                byte[] ncgr = br.ReadBytes(tamanhos[2]);
                File.WriteAllBytes(binario.Replace(".bin", "") + "\\" + Path.GetFileName(binario).Replace(".bin", ".ncgr"), ncgr);


            }
        }

        public static void ImporteBin(string dir, string dirSalva)
        {
            string[] dirArquivos = Directory.GetFiles(dir);

            byte[] ncer = new byte[] { };
            byte[] nanr = new byte[] { };
            byte[] ncgr = new byte[] { };

            foreach (var item in dirArquivos)
            {
                if (item.Contains("ncer"))
                {
                    ncer = File.ReadAllBytes(item);
                }
                else if (item.Contains("nanr"))
                {
                    nanr = File.ReadAllBytes(item);
                }
                else
                {
                    ncgr = File.ReadAllBytes(item);
                }
            }

            byte[] arquivoBinarioFinal = new byte[ncer.Length + nanr.Length + ncgr.Length + 12];
            MemoryStream ms = new MemoryStream(arquivoBinarioFinal);
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                bw.BaseStream.Position = 0xC;
                bw.Write(ncer);
                bw.Write(nanr);
                bw.Write(ncgr);
                bw.BaseStream.Position = 0x0;
                bw.Write(0xC);
                bw.Write(0xC + ncer.Length);
                bw.Write(0xC + ncer.Length + nanr.Length);

                arquivoBinarioFinal = ms.ToArray();
            }


            File.WriteAllBytes(dirSalva, arquivoBinarioFinal);

        }
    }
}