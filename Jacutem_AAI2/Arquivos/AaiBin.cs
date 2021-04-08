using Jacutem_AAI2.Compressoes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jacutem_AAI2.Arquivos
{
    public static class AaiBin
    {
        public static void Exportar(string dirBin, string subPasta)
        {
            if (!Directory.Exists("__Binarios\\_InfoBinarios"))
            {
                Directory.CreateDirectory("__Binarios\\_InfoBinarios");
            }
            string dirExt = "__Binarios\\"+ subPasta + "_" + Path.GetFileName(dirBin).Replace(".bin", "");
            List<string> listaDeArquivos = new List<string>();


            if (!Directory.Exists(dirExt))
            {
                Directory.CreateDirectory(dirExt);
            }

            int quantidadeArquivos = 0;

            List<int> tamanhos = new List<int>();
            List<int> ponteiros = new List<int>();
            List<bool> flagsDeCompressao = new List<bool>();
            LZ11_DS lZ11_DS = new LZ11_DS();

            using (BinaryReader br = new BinaryReader(File.Open(dirBin, FileMode.Open)))
            {
                quantidadeArquivos = br.ReadInt32() / 8;
                int inicioTabela = 0;
                for (int i = 0; i < quantidadeArquivos; i++)
                {
                    br.BaseStream.Position = inicioTabela;
                    ponteiros.Add(br.ReadInt32());
                    uint flag = br.ReadUInt32();
                    if (flag > 0x80000000)
                    {
                        flagsDeCompressao.Add(true);
                    }
                    else
                    {
                        flagsDeCompressao.Add(false);
                    }
                    inicioTabela += 8;
                }

                for (int i = 0; i < ponteiros.Count(); i++)
                {
                    if (i < ponteiros.Count() - 1)
                    {
                        tamanhos.Add(ponteiros[i + 1] - ponteiros[i]);
                    }
                    else
                    {
                        tamanhos.Add((int)(br.BaseStream.Length - ponteiros[i]));
                    }

                }


                for (int i = 0; i < ponteiros.Count(); i++)
                {
                    bool temCompressao = false;
                    br.BaseStream.Position = ponteiros[i];
                    byte[] arquivo = br.ReadBytes(tamanhos[i]);
                    int ext = 0;
                    if (flagsDeCompressao[i])
                    {
                        
                        MemoryStream output = new MemoryStream();
                        lZ11_DS.Decompress(new MemoryStream(arquivo), arquivo.Length, output);                        
                        arquivo = output.ToArray();
                        temCompressao = true;

                    }

                    if (arquivo.Length > 4)
                    {
                        ext = BitConverter.ToInt32(arquivo, 0);
                    }


                    string extensao = "";
                    switch (ext)
                    {
                        case 0x4E434752:
                            extensao = ".ncgr";
                            break;
                        case 0x4E434C52:
                            extensao = ".nclr";
                            break;
                        case 0x4E534352:
                            extensao = ".nscr";
                            break;
                        case 0x4E414E52:
                            extensao = ".nanr";
                            break;
                        case 0x4E434552:
                            extensao = ".ncer";
                            break;
                        case 0x53505420:
                            extensao = ".spt";
                            break;
                        case 0x30444D42:
                            extensao = ".bmd";
                            break;
                        case 0x30585442:
                            extensao = ".btx";
                            break;
                        case 0x30414342:
                            extensao = ".bca";
                            break;
                        case 0x5441444D:
                            extensao = ".mdat";
                            break;
                        case 0x30414D42:
                            extensao = ".bma";
                            break;
                        case 0x30415642:
                            extensao = ".bva";
                            break;
                        default:
                            extensao = ".bin";
                            break;
                    }

                    File.WriteAllBytes(dirExt + "\\" + i.ToString("0000") + "_" + Path.GetFileName(dirBin).Replace(".bin", extensao), arquivo);
                    if (temCompressao)
                    {
                        listaDeArquivos.Add(dirExt + "\\" + i.ToString("0000") + "_LZ11_" + Path.GetFileName(dirBin).Replace(".bin", extensao));
                    }
                    else
                    {
                        listaDeArquivos.Add(dirExt + "\\" + i.ToString("0000") + "_" + Path.GetFileName(dirBin).Replace(".bin", extensao));
                    }

                }

                File.WriteAllLines("__Binarios\\_InfoBinarios\\" + subPasta + "_" + Path.GetFileName(dirBin).Replace(".bin", ".txt"), listaDeArquivos);
            }

        }


        public static void Importar(string dirTxt)
        {
           

           
            string prefixo = Path.GetFileName(dirTxt.Replace(".txt", ".bin")).Contains("jpn") ? "jpn\\" : "com\\";
            string dirNovoArquivo = @"ROM_Desmontada\data\" +  prefixo+ Path.GetFileName(dirTxt.Replace(".txt", ".bin").Replace("com_","").Replace("jpn_",""));
            List<string> listaDeArquivos = File.ReadAllLines(dirTxt).ToList();
            List<int> novosPonteiros = new List<int>();
            List<uint> tamanhoArquivo = new List<uint>();
            int tamanhoTabela = listaDeArquivos.Count * 8;
            int ponteiroInicial = tamanhoTabela;

            File.Create(dirNovoArquivo).Close();
            File.Create("temp.bin").Close();

            LZ11_DS lZ11_DS = new LZ11_DS();

            foreach (var item in listaDeArquivos)
            {
                using (BinaryWriter bw = new BinaryWriter(File.Open("temp.bin", FileMode.Append)))
                {
                    if (item.Contains("LZ11_"))
                    {
                        novosPonteiros.Add(ponteiroInicial);
                        string dir = item.Replace("LZ11_", "");
                        byte[] arquivo = File.ReadAllBytes(dir);
                        tamanhoArquivo.Add((uint)arquivo.Length + 0x80000000);
                        MemoryStream output = new MemoryStream();
                        lZ11_DS.Compress(new MemoryStream(arquivo), arquivo.Length, output);
                        arquivo = output.ToArray();
                        bw.Write(arquivo);
                        ponteiroInicial += arquivo.Length;
                    }
                    else
                    {
                        novosPonteiros.Add(ponteiroInicial);
                        byte[] arquivo = File.ReadAllBytes(item);
                        tamanhoArquivo.Add((uint)arquivo.Length);
                        bw.Write(arquivo);
                        ponteiroInicial += arquivo.Length;
                    }



                }
            }

            byte[] tabelaNova = new byte[tamanhoTabela];
            MemoryStream ms = new MemoryStream(tabelaNova);

            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                for (int i = 0; i < novosPonteiros.Count; i++)
                {
                    bw.Write(novosPonteiros[i]);
                    bw.Write(tamanhoArquivo[i]);
                }


                tabelaNova = ms.ToArray();
            }

            using (BinaryWriter bw = new BinaryWriter(File.Open(dirNovoArquivo, FileMode.Append)))
            {
                bw.Write(tabelaNova);
                byte[] temp = File.ReadAllBytes("temp.bin");
                bw.Write(temp);
                File.Delete("temp.bin");

            }

        }


        public static void ExporteBinComLista(string binario)
        {
            if (!Directory.Exists("__Binarios\\_InfoBinarios"))
            {
                Directory.CreateDirectory("__Binarios\\_InfoBinarios");
            }

            List<string> listaDeArquivos = new List<string>();

            string dirExt = "__Binarios\\"  + "com_" + Path.GetFileName(binario).Replace(".bin", "");

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


                if (!Directory.Exists(dirExt))
                {
                    Directory.CreateDirectory(dirExt);

                }

                br.BaseStream.Position = ponteiros[0];
                byte[] ncer = br.ReadBytes(tamanhos[0]);


                File.WriteAllBytes(dirExt + "\\" + Path.GetFileName(binario).Replace(".bin", ".ncer"), ncer);

                br.BaseStream.Position = ponteiros[1];
                byte[] nanr = br.ReadBytes(tamanhos[1]);
                File.WriteAllBytes(dirExt + "\\" + Path.GetFileName(binario).Replace(".bin", ".nanr"), nanr);

                br.BaseStream.Position = ponteiros[2];
                byte[] ncgr = br.ReadBytes(tamanhos[2]);
                File.WriteAllBytes(dirExt + "\\" + Path.GetFileName(binario).Replace(".bin", ".ncgr"), ncgr);
                listaDeArquivos.Add(dirExt + "\\" + Path.GetFileName(binario).Replace(".bin", ".ncer") + "," + dirExt + "\\" + Path.GetFileName(binario).Replace(".bin", ".nanr") + "," + dirExt + "\\" + Path.GetFileName(binario).Replace(".bin", ".ncgr"));

            }

            File.WriteAllLines("__Binarios\\_InfoBinarios\\"  + "com_" + Path.GetFileName(binario).Replace(".bin", ".txt"), listaDeArquivos);
        }

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
                else if(item.Contains("nanr"))
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
