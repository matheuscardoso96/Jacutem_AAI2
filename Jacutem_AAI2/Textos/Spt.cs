using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


namespace Jacutem_AAI2.Textos
{
    public static class Spt
    {

        private static Dictionary<ushort, string> __tabela;
        private static Dictionary<ushort, string> __tabelaDlg = LerTabela(@"__Textos\_Tabelas\aai2.tbl");
        private static Dictionary<ushort, string> __tabelaDesc = LerTabela(@"__Textos\_Tabelas\aai2_descricoes.tbl");
        private static Dictionary<ushort, string> __tabelaBtn = LerTabela(@"__Textos\_Tabelas\aai2_botoes.tbl");

        public static string ExportarSpt(string dirSpt, string tpTabela, bool modoJapao)
        {
            if (tpTabela.Contains("descricoes"))
            {
                __tabela = __tabelaDesc;
            }
            else if (tpTabela.Contains("botoes"))
            {
                __tabela = __tabelaBtn;
            }
            else
            {
                __tabela = __tabelaDlg;
            }
            dirSpt = "__Binarios\\jpn_spt\\" + dirSpt;
            var ponteiros = ObtenhaSections(dirSpt, modoJapao);
            StringBuilder secoes = new StringBuilder();

            //  StringBuilder Raw = new StringBuilder();

            using (BinaryReader br = new BinaryReader(new MemoryStream(File.ReadAllBytes(dirSpt))))
            {
                int contaSecao = 0;

                foreach (var item in ponteiros)
                {
                    if (br.BaseStream.Position == br.BaseStream.Length)
                    {
                        break;
                    }
                    int contador = 0;

                    br.BaseStream.Position = item.Key;

                    StringBuilder secao = new StringBuilder();
                    secao.Append("{{" + contaSecao + "}}\r\n");
                    // Raw.Append("[" + contaSecao + "]\r\n");

                    while (contador < item.Value)
                    {
                        ushort valor = br.ReadUInt16();
                        contador += 2;

                        string value = "";
                        __tabela.TryGetValue(valor, out value);
                        string valorTabela = value;

                        if (valorTabela != null)
                        {

                            string caractereTabela = valorTabela.Split('=')[1];
                            secao.Append(caractereTabela);

                            int argumentos = int.Parse(valorTabela.Split('=')[2].Replace("{", "").Replace("}", ""));

                            /* if (!caractereTabela.Contains("{"))
                            {
                                Raw.Append(caractereTabela);
                            }



                             if (valor.Contains("55A0"))
                              {
                                  Raw.Append(" ");
                              }

                              if (valor.Contains("B4A8") || valor.Contains("B4AE") || valor.Contains("B4AC"))
                              {
                                  Raw.Append("\n");
                              }*/


                            for (int i = 0; i < argumentos; i++)
                            {
                                ushort arginho = br.ReadUInt16();
                                contador += 2;


                                if (valorTabela.Contains("{wait") || valorTabela.Contains("{speed"))
                                {
                                    arginho = (ushort)((arginho & 0xff) ^ 0xaa);
                                }




                                secao.Append(" " + arginho);



                                if (i < argumentos - 1)
                                {
                                    secao.Append(",");
                                }
                            }

                            if (caractereTabela.Contains("{"))
                            {
                                secao.Append("}");

                            }


                            if (caractereTabela.Contains("{name") || caractereTabela.Contains("{janela"))
                            {

                                secao.Append("\r\n");
                            }



                            if (caractereTabela.Contains("{endjmp") || caractereTabela.Contains("{sptendjmp") || caractereTabela.Contains("{preenchimento"))
                            {

                                secao.Append("\r\n\r\n");
                            }
                        }
                        else
                        {
                            secao.Append("{" + valor + "}");
                        }

                    }




                    string secaoFinal = secao.ToString();

                    secaoFinal = secaoFinal.Replace("{b}", "{b}\r\n");
                    secaoFinal = secaoFinal.Replace("{p}", "{p}\r\n\r\n");
                    secaoFinal = secaoFinal.Replace("{nextpage_button}", "{nextpage_button}\r\n\r\n");
                    secaoFinal = secaoFinal.Replace("{nextpage_nobutton}", "{nextpage_nobutton}\r\n\r\n");

                    //  Raw.Append("\r\n");


                    secoes.Append(secaoFinal);
                    contaSecao++;
                }


                string TextoFinal = ConvertaTags(secoes.ToString());



                return TextoFinal;

            }
        }

        private static Dictionary<uint, int> ObtenhaSections(string dirSpt, bool modoJapao)
        {
            Dictionary<uint, int> ponteiros = new Dictionary<uint, int>();

            using (BinaryReader br = new BinaryReader(new MemoryStream(File.ReadAllBytes(dirSpt))))
            {
                br.BaseStream.Position = 0x6;
                int numeroDeSections = br.ReadInt16();
                int posTabela = 0xc;
                uint ponteiro = 0;
                for (int i = 0; i < numeroDeSections; i++)
                {
                    br.BaseStream.Position = posTabela;
                    if (modoJapao)
                    {
                        ponteiro = br.ReadUInt16();
                    }
                    else
                    {
                        ponteiro = (uint)(br.ReadUInt16() * 2);
                    }

                    int tamanhoSection = (br.ReadInt16() * 2) + 2;
                    ponteiros.Add(ponteiro, tamanhoSection);
                    posTabela += 8;
                }
            }

            return ponteiros;
        }

        public static void ImportaSpt(string dirSpt, string tpTabela, bool modoJapao)
        {

            if (tpTabela.Contains("descricoes"))
            {
                __tabela = __tabelaDesc;
            }
            else if (tpTabela.Contains("botoes"))
            {
                __tabela = __tabelaBtn;
            }
            else
            {
                __tabela = __tabelaDlg;
            }
            string textosComTagsConvertidas = ConvertaTagsAoContario(File.ReadAllText(dirSpt, Encoding.UTF8));
            string[] textos = textosComTagsConvertidas.Replace("\n", "").Replace("\r", "").Split(new[] { "{{" }, StringSplitOptions.RemoveEmptyEntries);
            MemoryStream sptOG = new MemoryStream(File.ReadAllBytes(@"__Binarios\jpn_spt\" + Path.GetFileName(dirSpt).Replace(".txt", ".spt")));
            byte[] backupTabela;
            int posSecaoMarcada = 0;
            string acumula = "";
            using (BinaryReader br = new BinaryReader(sptOG))
            {
                br.BaseStream.Position = 0x6;
                int NumSecoes = (br.ReadInt16() * 8) + 12;
                br.BaseStream.Position = 0;
                backupTabela = br.ReadBytes(NumSecoes);

                int posInicialTabela = 0x8;

                br.BaseStream.Position = posInicialTabela;
                int valorMarcado = br.ReadUInt16();

                posInicialTabela = 0xE;

                for (int i = 0; i < textos.Length; i++)
                {

                    br.BaseStream.Position = posInicialTabela;
                    int tamanhoSecao = br.ReadUInt16();

                    if (tamanhoSecao == valorMarcado)
                    {
                        posSecaoMarcada = posInicialTabela;
                    }

                    posInicialTabela += 8;
                }
            }

            Dictionary<int, int> infomacoesSecoes = new Dictionary<int, int>();

            List<ushort> textoConvertido = new List<ushort>();

            int valorBasePOnteiro = backupTabela.Length;

            foreach (var secao in textos)
            {
                string aSerconvertido = secao.Split(new[] { "}}" }, StringSplitOptions.RemoveEmptyEntries).Last();
                int tamanhoSecao = 0;
                int posInicial = valorBasePOnteiro;

                for (int i = 0; i < aSerconvertido.Length; i++)
                {
                    if (aSerconvertido[i] == '{')
                    {
                        string tag = aSerconvertido[i] + "";
                        i++;

                        do
                        {
                            tag += aSerconvertido[i];
                            i++;

                        } while (aSerconvertido[i] != '}');


                        if (tag.Contains(":"))
                        {
                            tag = tag.Replace(" ", "");



                            ushort valorDoCaractere = __tabela.FirstOrDefault(x => x.Value.Contains("=" + tag.Split(':')[0] + ":" + "=")).Key;
                            textoConvertido.Add(valorDoCaractere);
                            valorBasePOnteiro += 2;
                            tamanhoSecao += 2;
                            string[] argumentos = tag.Split(':')[1].Split(',');

                            for (int y = 0; y < argumentos.Length; y++)
                            {
                                if (tag.Contains("{speed") || tag.Contains("{wait"))
                                {

                                    textoConvertido.Add((ushort)(0x55AA ^ Convert.ToUInt16(argumentos[y])));

                                }
                                else
                                {
                                    textoConvertido.Add(Convert.ToUInt16(argumentos[y]));
                                }


                                valorBasePOnteiro += 2;
                                tamanhoSecao += 2;
                            }
                        }
                        else
                        {

                            ushort caractereTabela = __tabela.FirstOrDefault(x => x.Value.Contains("=" + tag + "=")).Key;


                            if (caractereTabela > 0)
                            {


                                textoConvertido.Add(caractereTabela);
                                valorBasePOnteiro += 2;
                                tamanhoSecao += 2;
                            }
                            else
                            {

                                textoConvertido.Add(Convert.ToUInt16("0x" + tag.Replace("{", ""), 16));
                                valorBasePOnteiro += 2;
                                tamanhoSecao += 2;
                            }
                        }

                    }
                    else
                    {
                        // string valor = br.ReadUInt16().ToString("X4");
                        // contador += 2;
                        char c = aSerconvertido[i];



                        ushort valorDoCaractere = __tabela.FirstOrDefault(b => b.Value.Contains("=" + c + "=")).Key;

                        if (valorDoCaractere > 0)
                        {
                            textoConvertido.Add(valorDoCaractere);
                            valorBasePOnteiro += 2;
                            tamanhoSecao += 2;
                        }
                        else
                        {
                            textoConvertido.Add(0x65B5);
                            valorBasePOnteiro += 2;
                            tamanhoSecao += 2;
                        }
                    }
                }

                infomacoesSecoes.Add(posInicial, tamanhoSecao);

            }


            byte[] final = new byte[backupTabela.Length + textoConvertido.Count * 2];
            MemoryStream ms = new MemoryStream(final);

            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                bw.Write(backupTabela);
                foreach (var item in textoConvertido)
                {
                    bw.Write(item);
                }

                int posInicial = 0xC;




                foreach (var item in infomacoesSecoes)
                {
                    bw.BaseStream.Position = posInicial;

                    if (modoJapao)
                    {
                        bw.Write((ushort)(item.Key));
                    }
                    else
                    {
                        bw.Write((ushort)(item.Key / 2));
                    }



                    if (bw.BaseStream.Position == posSecaoMarcada)
                    {
                        int salvaPos = (int)bw.BaseStream.Position;
                        bw.BaseStream.Position = 0x8;
                        bw.Write((ushort)((item.Value - 2) / 2));
                        bw.BaseStream.Position = salvaPos;

                    }
                    bw.Write((ushort)((item.Value - 2) / 2));
                    posInicial += 8;

                }



                File.WriteAllBytes(@"__Binarios\jpn_spt\" + Path.GetFileName(dirSpt).Replace(".txt", ".spt"), ms.ToArray());
            }

        }


        private static Dictionary<ushort, string> LerTabela(string dirTabela)
        {
            Dictionary<ushort, string> tabela = new Dictionary<ushort, string>();
            string[] caracteres = File.ReadAllLines(dirTabela);
            ushort itemAtual = 0;

            try
            {
                foreach (var item in caracteres)
                {
                    ushort chave = Convert.ToUInt16("0x" + item.Split('=')[0], 16);
                    string valor = "=" + item.Split('=')[1] + "=" + item.Split('=')[2];
                    itemAtual = chave;
                    tabela.Add(chave, valor);
                }

                return tabela;
            }
            catch (Exception e)
            {

                throw new Exception(itemAtual + " " + e.Message);
            }


        }



        private static string ConvertaTags(string texto)
        {
            string[] tabelaModificada = File.ReadAllLines(@"__Textos\_Tabelas\tabelaTag.tbl");

            foreach (var item in tabelaModificada)
            {
                texto = texto.Replace(item.Split('=')[0], item.Split('=')[1]);
            }

            return texto;
        }

        private static string ConvertaTagsAoContario(string texto)
        {
            string[] tabelaModificada = File.ReadAllLines(@"__Textos\_Tabelas\tabelaTag.tbl");

            foreach (var item in tabelaModificada)
            {
                texto = texto.Replace(item.Split('=')[1], item.Split('=')[0]);
            }

            return texto;
        }

        public static void AtualizarTabelas()
        {

            __tabelaDlg = LerTabela(@"__Textos\_Tabelas\aai2.tbl");
            __tabelaDesc = LerTabela(@"__Textos\_Tabelas\aai2_descricoes.tbl");
            __tabelaBtn = LerTabela(@"__Textos\_Tabelas\aai2_botoes.tbl");
        }
        
    }
}
