using System.Collections.Generic;
using System.IO;


namespace Jacutem_AAI2.Imagens
{
    public class Ncer
    {
        public List<Oams> GrupoDeTabelasOam = new List<Oams>();
        public byte[] Cabecalho { get; set; }
        public byte[] Lbal { get; set; }
        public byte[] Txeu { get; set; }

        public Ncer(string dir)
        {
            using (BinaryReader br = new BinaryReader(new MemoryStream(File.ReadAllBytes(dir))))
            {
                Cabecalho = br.ReadBytes(0x30);
                br.BaseStream.Position = 0x14;
                int offsetLbal = br.ReadInt32() + 0x10;
                br.BaseStream.Position = offsetLbal;
                byte v = br.ReadByte();
                int contador = 0;
                if (v != 0x4C)
                {
                    contador++;
                    while (true)
                    {
                        v = br.ReadByte();
                        if (v == 0x4C)
                        {
                            break;
                        }
                        contador++;
                    }

                    offsetLbal += contador;
                    br.BaseStream.Position = offsetLbal;

                }
                else
                {
                    br.BaseStream.Position -= 1;
                }
                br.BaseStream.Seek(4,SeekOrigin.Current);
                int tamanhoSecaoLb = br.ReadInt32();
                br.BaseStream.Position = offsetLbal;              
                Lbal = br.ReadBytes(tamanhoSecaoLb);
                br.BaseStream.Seek(4, SeekOrigin.Current);
                int tamanhoSecaoTx = br.ReadInt32();
                br.BaseStream.Position = offsetLbal + tamanhoSecaoLb;
                Txeu = br.ReadBytes(tamanhoSecaoTx);
                
                br.BaseStream.Position = 0x18;
                int numeroDeTabelasOam = br.ReadInt32();
                int posTabela = 0x30;
                int posicaoEntradas = (numeroDeTabelasOam * 8) + 0x30;

                for (int i = 0; i < numeroDeTabelasOam; i++)
                {
                    br.BaseStream.Position = posTabela;
                    int qtdEntradas = br.ReadInt16();
                    int id = br.ReadInt16();
                    int offsetEntrada = br.ReadInt32();
                    br.BaseStream.Position = offsetEntrada + posicaoEntradas;

                    Oams listaDeoams = new Oams(posTabela, (ushort)qtdEntradas, (ushort)id);


                    for (int y = 0; y < qtdEntradas; y++)
                    {
                        ushort atb0 = br.ReadUInt16();
                        ushort atb1 = br.ReadUInt16();
                        ushort atb2 = br.ReadUInt16();

                        Oam oam = new Oam(atb0, atb1, atb2);
                        listaDeoams.AdicionarOam(oam);
                    }

                   // listaDeoams.VariosOam.Reverse();

                    GrupoDeTabelasOam.Add(listaDeoams);



                    posTabela += 8;

                }

                


            }
        }

        public void SalvarNcer(string diretorio)
        {
            AtualizarPosicoesXYdeValoresOam();
            int tamanhoDaTabelaDeOams = GrupoDeTabelasOam.Count * 8;
            int tQtdEntras = TamanhoQtdDeEntradas();
            int tamanhoDoNovoNcer = tamanhoDaTabelaDeOams + tQtdEntras + Cabecalho.Length + Txeu.Length + Lbal.Length;
            if (tamanhoDoNovoNcer % 4 != 0)
            {
                while (tamanhoDoNovoNcer % 4 != 0)
                {
                    tamanhoDoNovoNcer++;
                }
            }
            byte[] tabelasOam = new byte[tamanhoDoNovoNcer];
            MemoryStream ms = new MemoryStream(tabelasOam);
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
               
                bw.Write(Cabecalho);

                foreach (var item in GrupoDeTabelasOam)
                {
                    bw.Write(item.QtdEntradas);
                    bw.Write(item.Id);
                    bw.Write(0);
                }

                int offsetCont = 0;
                Dictionary<int,short> ponteirosEhQtdEntradas = new Dictionary<int, short>();
                
                foreach (var item in GrupoDeTabelasOam)
                {
                    ponteirosEhQtdEntradas.Add(offsetCont, (short)item.TabelaDeOams.Count);
                    item.TabelaDeOams.Reverse();
                    foreach (var oamm in item.TabelaDeOams)
                    {
                        bw.Write(oamm._atributosOBJ0);
                        bw.Write(oamm._atributosOBJ1);
                        bw.Write(oamm._atributosOBJ2);

                    }

                    offsetCont = (int)bw.BaseStream.Position;
                }

                bw.Write(Lbal);
                bw.Write(Txeu);
                int Tamanhocebk = 0x20 + tQtdEntras + tamanhoDaTabelaDeOams;
                int TamanhoNcer = 0x20 + tQtdEntras + tamanhoDaTabelaDeOams + Txeu.Length + Lbal.Length + 0x10;

                bw.BaseStream.Position = 0x8;
                bw.Write(TamanhoNcer);
                bw.BaseStream.Position = 0x14;
                bw.Write(Tamanhocebk);

                bw.BaseStream.Position = 0x30;
                foreach (var item in ponteirosEhQtdEntradas)
                {
                    bw.Write(item.Value);
                    bw.BaseStream.Seek(2,SeekOrigin.Current);
                    bw.Write(item.Key);
                }

                tabelasOam = ms.ToArray();
            }

            File.WriteAllBytes(diretorio,tabelasOam);

           
        }

        private void AtualizarPosicoesXYdeValoresOam()
        {
            foreach (var tabelaOam in GrupoDeTabelasOam)
            {
                foreach (var entrada in tabelaOam.TabelaDeOams)
                {
                   int atb0SemValorY = entrada._atributosOBJ0 - (entrada._atributosOBJ0 & 0XFF);
                   int atb1SemValorX = entrada._atributosOBJ1 -(entrada._atributosOBJ1 & 0X1FF);
                    int y =(int)entrada.Y;
                    int x = (int)entrada.X;
                    if (y >= 128)
                        y -= 128;
                    else
                        y += 128;

                    if (x >= 256)
                        x -= 256;
                    else
                        x += 256;

                    entrada._atributosOBJ0 = (ushort)(atb0SemValorY + y);
                    entrada._atributosOBJ1 = (ushort)(atb1SemValorX + x);

                }
            }
        }

        public void SalvarNcerdicionando(string diretorio)
        {

        }

        private int TamanhoQtdDeEntradas()
        {
            int total = 0;

            foreach (var item in GrupoDeTabelasOam)
            {
                foreach (var v in item.TabelaDeOams)
                {
                    total += 6;
                }
            }

            return total;
        }
    }


    public class Oams
    {

        public List<Oam> TabelaDeOams;
        public int Posicao { get; set; }
        public int PosicaoEntradas { get; set; }
        public ushort QtdEntradas { get; set; }
        public ushort Id { get; set; }
        

        public Oams(int posicao, ushort qtdEntradas, ushort id)
        {
            TabelaDeOams = new List<Oam>();
            Posicao = posicao;
            Id = id;
            QtdEntradas = qtdEntradas;
        }

        public void AdicionarOam(Oam oam)
        {
            TabelaDeOams.Add(oam);
        }
    }
}
