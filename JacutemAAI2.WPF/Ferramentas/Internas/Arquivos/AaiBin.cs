using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using dsdecmp.Formats.Nitro;

namespace JacutemAAI2.WPF.Ferramentas.Internas
{
    public static class AaiBin
    {
        public static string Mensagem;

        public static void Exportar(string dirBin)
        {          
            string subPasta = dirBin.Contains("jpn") ? "jpn" : "com";
            if (!File.Exists(dirBin))
            {
                Mensagem = $"Não foi possível encontrar o arquivo {Path.GetFileName(dirBin)}";
                return;
            }

            string dirExt = $"__Binarios\\{subPasta}_{Path.GetFileName(dirBin).Replace(".bin", "")}";

            if (!Directory.Exists(dirExt))
                Directory.CreateDirectory(dirExt);

            AbraArquivoEhExporte(dirBin, dirExt, subPasta);          

            Mensagem = $"Arquivo {Path.GetFileName(dirBin)} exportado com sucesso para a pasta __Binarios\\{Path.GetFileName(dirBin).Replace(Path.GetExtension(dirBin),"")}";

        }

        private static void AbraArquivoEhExporte(string dirBin, string dirExt, string subPasta) 
        {
            LZ11 lZ11 = new LZ11();
            var listaDeArquivos = new List<string>();

            using (BinaryReader br = new BinaryReader(File.Open(dirBin, FileMode.Open)))
            {

                List<EntradaAAIBin> entradas = LerEntradas(br);
                int contador = 0;

                foreach (EntradaAAIBin entrada in entradas)
                {
                    br.BaseStream.Position = entrada.Endereco;
                    byte[] arquivo = br.ReadBytes((int)entrada.Tamanho);
                    if (entrada.Comprimido)
                        arquivo = DescomprimirComLZ11(lZ11, arquivo);

                    int ext = 0;

                    if (arquivo.Length >= 4)
                        ext = BitConverter.ToInt32(arquivo, 0);

                    string extensao = ext == 0 ? ".bin" : ObtenhaExtensao(ext);
                    string prefixoCompressao = entrada.Comprimido ? "LZ11_" : "";
                    File.WriteAllBytes($"{dirExt}\\{contador:0000}_{Path.GetFileName(dirBin).Replace(".bin", extensao)}", arquivo);
                    listaDeArquivos.Add($"{dirExt}\\{contador:0000}{prefixoCompressao}{Path.GetFileName(dirBin).Replace(".bin", extensao)}");
                    contador++;
                }

                File.WriteAllLines($"__Binarios\\_InfoBinarios\\{subPasta}_{Path.GetFileName(dirBin).Replace(".bin", ".txt")}", listaDeArquivos);
            }

        }

        public static void ExportarTodos(string diretorioRomDesmontada) 
        {
            string[] listaDeArquivos = Directory.GetFiles($"{diretorioRomDesmontada}\\data", "*.bin", SearchOption.AllDirectories);

            foreach (var dir in listaDeArquivos)
            {
                if (dir.Contains("data\\data\\"))
                    continue;
             
                Exportar(dir);

            }

            Mensagem = $"Arquivos exportados com sucesso para a pasta __Binarios";

        }



        private static List<EntradaAAIBin> LerEntradas(BinaryReader br)
        {
            var entradas = new List<EntradaAAIBin>();
            int quantidadeDeEntradas = br.ReadInt32() / 8;
            br.BaseStream.Position = 0;

            for (int i = 0; i < quantidadeDeEntradas; i++)
            {
                bool ehUltimo = false;
                if (i == quantidadeDeEntradas - 1)
                    ehUltimo = true;

                entradas.Add(new EntradaAAIBin(br, ehUltimo));

            }

            return entradas;

        }

        private static string ObtenhaExtensao(int ext)
        {
            switch (ext)
            {
                case 0x4E434752:return ".ncgr";
                case 0x4E434C52:return ".nclr";
                case 0x4E534352:return ".nscr";
                case 0x4E414E52:return ".nanr";
                case 0x4E434552:return ".ncer"; 
                case 0x53505420:return ".spt";
                case 0x30444D42:return ".bmd";
                case 0x30585442:return ".btx";
                case 0x30414342:return ".bca";
                case 0x5441444D:return ".mdat";
                case 0x30414D42:return ".bma";  
                case 0x30415642:return ".bva";
                default: return ".bin";
            }
        }

        private static byte[] DescomprimirComLZ11(LZ11 LZ11, byte[] arquivo)
        {
            MemoryStream output = new MemoryStream();
            LZ11.Decompress(new MemoryStream(arquivo), arquivo.Length, output);
            return output.ToArray();
        }

        #region importarAAIBin

        public static string Importar(string dirTxt)
        {

            string prefixo = Path.GetFileName(dirTxt.Replace(".txt", ".bin")).Contains("jpn") ? "jpn\\" : "com\\";
            string dirNovoArquivo = $"ROM_Desmontada\\data\\{prefixo}{Path.GetFileName(dirTxt.Replace(".txt", ".bin").Replace("com_", "").Replace("jpn_", ""))}";
            var listaDeArquivos = File.ReadAllLines(dirTxt).ToList();
            string resultado = CriarNovoBinario(listaDeArquivos, dirNovoArquivo);

            if (resultado.Length > 0)
                return resultado;

            return $"{Path.GetFileName(dirTxt.Replace(".txt", ".bin"))} criado e substituidona pasta Rom_Desmotada com sucesso.";
        }

        private static string CriarNovoBinario(List<string> listaDeArquivos, string dirNovoArquivo)
        {
            var entradas = new List<EntradaAAIBin>();
            LZ11 LZ11 = new LZ11();
            uint tamanhoTabela = (uint)listaDeArquivos.Count * 8;
            uint endereco = tamanhoTabela;

            using (BinaryWriter bw = new BinaryWriter(File.Open("temp.bin", FileMode.Append)))
            {

                File.Create("temp.bin").Close();

                foreach (var caminho in listaDeArquivos)
                {
                    bool comprimido = caminho.Contains("LZ11_");
                    FileInfo infoArquivo = new FileInfo(comprimido ? caminho.Replace("LZ11_", "") : caminho);
               
                    if (infoArquivo == null)
                        return $"Arquivo não encontrado:\r\n {caminho}";

                    byte[] arquivo = File.ReadAllBytes(infoArquivo.Directory.FullName);
                    uint tamanhoArquivo  = (uint)arquivo.Length;
                    
                    if (comprimido)
                        arquivo = ComprimirComLZ11(LZ11, arquivo);

                    bw.Write(arquivo);
                    entradas.Add(new EntradaAAIBin(endereco, tamanhoArquivo, comprimido));
                    endereco += (uint)arquivo.Length;


                }

            }

            byte[] tabelaNova = new byte[tamanhoTabela];
            tabelaNova = CrieUmaNovaTabela(tabelaNova, entradas);
            CrieNovoBinarioComTabela(dirNovoArquivo, tabelaNova);

            return "";
        }

        private static byte[] CrieUmaNovaTabela(byte[] tabelaNova, List<EntradaAAIBin> entradas)
        {
            MemoryStream ms = new MemoryStream(tabelaNova);

            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                foreach (var entrada in entradas)
                    entrada.EscreverEntrada(bw);

                tabelaNova = ms.ToArray();
            }

            return tabelaNova;
        }

        private static void CrieNovoBinarioComTabela(string dirNovoArquivo, byte[] tabelaNova)
        {
            File.Create(dirNovoArquivo).Close();

            using (BinaryWriter bw = new BinaryWriter(File.Open(dirNovoArquivo, FileMode.Append)))
            {
                bw.Write(tabelaNova);
                byte[] temp = File.ReadAllBytes("temp.bin");
                bw.Write(temp);
                File.Delete("temp.bin");

            }
        }

        private static byte[] ComprimirComLZ11(LZ11 LZ11, byte[] arquivo)
        {

            MemoryStream output = new MemoryStream();
            LZ11.Compress(new MemoryStream(arquivo), arquivo.Length, output);
            return output.ToArray();
        }

        #endregion

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
