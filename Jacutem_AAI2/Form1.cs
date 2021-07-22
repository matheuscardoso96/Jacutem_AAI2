using Jacutem_AAI2.Arquivos;
using Jacutem_AAI2.Imagens;
using Jacutem_AAI2.Imagens.Enums;
using Jacutem_AAI2.Textos;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Linq;
using System.Drawing.Drawing2D;
using Jacutem_AAI2.Imagens.MapaDeArquivos;

namespace Jacutem_AAI2
{
    public partial class Form1 : Form
    {
        private GerenciadorDeAcessoAsTabs _gerenciadorDeTabs = new GerenciadorDeAcessoAsTabs();
        private const string _textoAviso = "1- Só é necessário desmontar a ROM uma única vez.\r\n\r\n2- Após a pasta \"ROM_Desmontada\" ser criada, não será possível \"desmontar\" a ROM.\r\n\r\n3- Caso queira desmonstar a ROM novamente, mova ou apague a pasta \"ROM_Desmontada\".";
        private const string _textoAvisoArquivo = "1- Os arquivos estão localizos no diretório ROM_Desmontada\\data\\.\r\n\r\n" +
                "2- Para que as modificações sejam aplicadas no jogo, é preciso criar um novo binário cada vez que imagens ou textos forem modificadas.";
        private Dictionary<string,MapaDeArquivos> _mapasDeArquivos;

        private Ncgr _ncgrBgCarregado;
        private Ncgr _ncgrSpriteCarregado;
        private Btx _btxCarregado;

        public Form1()
        {
            InitializeComponent();
            AdicionarTextosNosTabs();
            VerificarLiberacaoAcessoAsTabs();
            InicializarMapasDeArquivos();
            PrencherCombosDeImagens();

            
            AdicionarResolucoesOamAmDicionario();
           
         
          //  PreencherComboBoxResolucoesOam();
           // PreencherComboBoxTabelaCarac();

           
        }
        
        #region Funções auxiliares de telas
        private void InicializarMapasDeArquivos()
        {
            var bGTileComMap =  new BGTileComMap();
            var bGTileSemMap = new BGTileSemMap();
            var spritesModo2D = new SpritesModo2D();
            var spritesBinariosModo2D = new SpritesBinariosModo2D();
            var spritesBinariosModoTile = new SpritesBinariosModoTile();
            var spritesModoTile = new SpritesModoTile();
            var texturas = new Texturas();

            _mapasDeArquivos = new Dictionary<string, MapaDeArquivos>() {
               [bGTileComMap.Tipo] = bGTileComMap,
                [bGTileSemMap.Tipo] = bGTileSemMap,
                [spritesModo2D.Tipo] = spritesModo2D,
                [spritesBinariosModo2D.Tipo] = spritesBinariosModo2D,
                [spritesBinariosModoTile.Tipo] = spritesBinariosModoTile,
                [spritesModoTile.Tipo] = spritesModoTile,
                [texturas.Tipo] = texturas,
            };            
           
        }

        private void PrencherCombosDeImagens() 
        {
            Dictionary<string, List<string>> tipoImagem = new Dictionary<string, List<string>>() { };
            List<string> bg = new List<string>();
            List<string> sprites = new List<string>();
            List<string> texturas = new List<string>();

            foreach (var mapa in _mapasDeArquivos)
            {
                if (mapa.Value.Tipo.Contains("BG"))
                    bg.Add(mapa.Value.Tipo);
                else if (mapa.Value.Tipo.Contains("Sprites"))
                    sprites.Add(mapa.Value.Tipo);
                else if (mapa.Value.Tipo.Contains("Texturas"))
                    texturas.Add(mapa.Value.Tipo);
            }

            PreencherListOuCombo(bg, comboBoxBGetc);
            PreencherListOuCombo(sprites, comboBoxSprites);
            PreencherListOuCombo(texturas, comboBoxTexturas);
        }

        private void AdicionarTextosNosTabs()
        {
            textBoxAviso.Text = _textoAviso;
            textBoxAvisoArquivo.Text = _textoAvisoArquivo;
        }

        private void VerificarLiberacaoAcessoAsTabs()
        {
            _gerenciadorDeTabs.ExcutarVerificacoes();


            if (_gerenciadorDeTabs.ArquivosBinarios)
            {
                VoltarPrimeiraAba();
                textBoxDirRomDesmontada.Text = "ROM_Desmontada";
            }

            if (_gerenciadorDeTabs.Imagens)
            {
                var listaDeResolucoes = new List<string>() { "8x8", "16x16", "32x32", "64x64", "16x8", "32x8", "32x16", "64x32", "8x16", "8x32", "16x32", "32x64" };
                PreencherListOuCombo(listaDeResolucoes, comboBoxResolucoesOam);
                PreencherListOuCombo(listaDeResolucoes, comboBoxResolucaoOamAdicionar);
                
            }
               

            if (_gerenciadorDeTabs.Textos)
            {
                PreencherListOuCombo(_gerenciadorDeTabs.TextosDir, "*.spt", listBoxSpt);
                textBoxPreviaSpt.ScrollBars = ScrollBars.Both;
            }
                

            if (_gerenciadorDeTabs.ImportarBinarios)
            {
                PreencherListOuCombo(_gerenciadorDeTabs.ImportarBinariosDir, "*.txt", comboBoxImportarBinario);
               
            }
               

            if (_gerenciadorDeTabs.Fontes)
            {
                var listaFontes = new List<string>() { "Fonte 1", "Fonte 2", "Fonte 3" };
                PreencherListOuCombo(listaFontes, comboBoxFonte);
            }
                

        }

        private void PreencherListOuCombo(string dir,string extensao, ListControl listbox)
        { 
                listbox.DataSource = Directory.GetFiles(dir, extensao);                      
        }

        private void PreencherListOuCombo<T>(List<T> dir, ListControl listbox)
        {
           
                listbox.DataSource = dir;
           
                  
        }

        private void PreencherListOuCombo(Dictionary<string, string> dir, ListControl listbox)
        {
            
                listbox.DataSource = dir;
            

        }
        #endregion

        #region Funções da aba ROM
        private void button2_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog opf = new OpenFileDialog())
            {

                opf.Filter = "Arquivo .nds (*.nds)|*.nds|All files (*.*)|*.*";

                if (opf.ShowDialog() == DialogResult.OK)
                {
                    textBoxDirNds.Text = opf.FileName;
                }
            }
        }

        private void textBoxDirNds_TextChanged(object sender, EventArgs e)
        {
            if (textBoxDirNds.TextLength > 0)
            {
                if (textBoxDirNds.Text.Contains(".nds"))
                {
                    buttonDesmontar.Enabled = true;
                }
            }
            else
            {
                buttonDesmontar.Enabled = false;
            }



            if (textBoxDirRomDesmontada.TextLength > 0)
            {

                buttonRemontar.Enabled = true;

            }
        }


        private async void buttonDesmontar_Click(object sender, EventArgs e)
        {
            DesativarOuDesativaBotoesAbaRom(false);
            textBoxStatusRom.Text = "Aguarde...";
            var resultado = await Task.Run(() => IntegracaoComNdsTool.DesmontarArquivoNds(textBoxDirNds.Text, "ROM_Desmontada"));
            textBoxStatusRom.Text = "Concluido!";
            MessageBox.Show(this, resultado,"Resultado", MessageBoxButtons.OK,MessageBoxIcon.Information);
            DesativarOuDesativaBotoesAbaRom(true);
            VerificarLiberacaoAcessoAsTabs();
            textBoxStatusRom.Text = "";

        }

        private async void buttonRemontar_Click(object sender, EventArgs e)
        {
               
            string dirRomDes = textBoxDirRomDesmontada.Text;
            DesativarOuDesativaBotoesAbaRom(false);
            textBoxStatusRom.Text = "Aguarde...";
            var resultado = await Task.Run(() => IntegracaoComNdsTool.NovoArquivoNds(dirRomDes, "AAI2_"));
            textBoxStatusRom.Text = "Concluido!";
            MessageBox.Show(this, resultado,"Aviso", MessageBoxButtons.OK,MessageBoxIcon.Information);
            DesativarOuDesativaBotoesAbaRom(true);
            textBoxStatusRom.Text = "";


        }

        private void DesativarOuDesativaBotoesAbaRom(bool ativa)
        {
            buttonDesmontar.Enabled = ativa;
            buttonRemontar.Enabled = ativa;
        }

        #endregion

        private void AdicionarResolucoesOamAmDicionario()
        {
            _resolucoesOam.Add("8x8", 0);
            _resolucoesOam.Add("16x16", 1);
            _resolucoesOam.Add("32x32", 2);
            _resolucoesOam.Add("64x64", 3);
            _resolucoesOam.Add("16x8", 4);
            _resolucoesOam.Add("32x8", 5);
            _resolucoesOam.Add("32x16", 6);
            _resolucoesOam.Add("64x32", 7);
            _resolucoesOam.Add("8x16", 8);
            _resolucoesOam.Add("8x32", 9);
            _resolucoesOam.Add("16x32", 10);
            _resolucoesOam.Add("32x64", 11);

        }

        private void DesativeComponentes()
        {
            //  checkBox1.ForeColor = Color.Gray; // Read-only appearance
           /* checkBox4Bpp.AutoCheck = false;
            //  checkBox2.ForeColor = Color.Gray; // Read-only appearance
            checkBox8Bpp.AutoCheck = false;
            //  checkBox2.ForeColor = Color.Gray; // Read-only appearance
            checkBox8Bpp.AutoCheck = false;
            //  checkBox3.ForeColor = Color.Gray; // Read-only appearance
            checkBoxCoresDiretas.AutoCheck = false;
            checkBoxEditarPosicao.Enabled = false;
            checkBoxTex4bpp.AutoCheck = false;
            checkBoxTex8bpp.AutoCheck = false;
            checkBox4bppOam.AutoCheck = false;
            checkBox8bppOam.AutoCheck = false;
            checkBoxAtivarBordas.AutoCheck = false;
            buttonExportarFonte.Enabled = false;
            buttonImportarFonte.Enabled = false;
            buttonSalvarVwf.Enabled = false;
            buttonConverteSeleTexto.Enabled = false;
            buttonAdicionarOam.Enabled = false;
            buttonSalvarModifcaoesOam.Enabled = false;
            checkBoxNumerosOam.Enabled = false;
            checkBoxFundoTransparenteOam.Enabled = false;
            buttonExportarSelecionaTextura.Enabled = false;
            numericUpDownPosOamX.Minimum = 0;
            numericUpDownPosOamX.Maximum = 511;
            numericUpDownPosOamX.Enabled = false;
            numericUpDownPosOamY.Enabled = false;
            numericUpDownPosOamY.Minimum = 0;
            numericUpDownPosOamY.Maximum = 255;
            checkedListBoxSpriteSelecionado.Enabled = false;*/

        }



        #region Funções da aba Arquivos Binários

        private async void button5_Click(object sender, EventArgs e)
        {
            DesativarOuAtivarBotoesBin(false);
            await ExportarBinarios();          
            MessageBox.Show(this, @"Exportação de binários concluída para o diretório: _Binarios\\",
                                  "Aviso", MessageBoxButtons.OK,
                                  MessageBoxIcon.Information);
            VerificarLiberacaoAcessoAsTabs();
            DesativarOuAtivarBotoesBin(true);
            textBoxStatusBin.Text = "";

        }

        private async void buttonBnJpnCom_Click(object sender, EventArgs e)
        {

            textBoxStatusBin.Text = Path.GetFileName((string)comboBoxImportarBinario.SelectedItem);
            string listaSelecionada = (string)comboBoxImportarBinario.SelectedItem;
            MessageBox.Show(this, await Task.Run(() => AaiBin.Importar(listaSelecionada)),
                                 "Aviso", MessageBoxButtons.OK,
                                 MessageBoxIcon.Information);
        }

        private async void btnCriarTodosBinarios_Click_3(object sender, EventArgs e)
        {
            DesativarOuAtivarBotoesBin(false);
            await RecriarBinarios();
            progressBarBin.Value = 0;
            DesativarOuAtivarBotoesBin(true);
        }

        private async Task ExportarBinarios()
        {
           
            string[] listaDeArquivos = Directory.GetFiles(@"ROM_Desmontada\data", "*.bin",SearchOption.AllDirectories);

            progressBarBin.Minimum = 0;
            progressBarBin.Maximum = listaDeArquivos.Length;

            foreach (var dir in listaDeArquivos)
            {
                if (dir.Contains("data\\data\\"))
                    continue;
                
                string jpnOuCom = dir.Contains("jpn") ? "jpn" : "com";
                await Task.Run(() => AaiBin.Exportar(dir, jpnOuCom));
                progressBarBin.Value += 1;
                textBoxStatusBin.Text = Path.GetFileName(dir);
            }

            progressBarBin.Value = 0;            

        }



        private async Task RecriarBinarios()
        {

            var dirListaArquivos =(string[])comboBoxImportarBinario.DataSource;

            progressBarBin.Minimum = 0;
            progressBarBin.Maximum = dirListaArquivos.Length;

            foreach (var dirLista in dirListaArquivos)
            {
                textBoxStatusBin.Text = Path.GetFileName(dirLista);
                progressBarBin.Value += 1;
                await Task.Run(() => AaiBin.Importar(dirLista));              
            }

        }


        private void DesativarOuAtivarBotoesBin(bool ativa)
        {
            buttonExportarBin.Enabled = ativa;
            buttonBnJpnCom.Enabled = ativa;
            comboBoxImportarBinario.Enabled = ativa;
            btnCriarTodosBinarios.Enabled = ativa;

        }

        #endregion

        #region Funções da aba Imagens

        private void comboBoxBGetc_SelectionChangeCommitted(object sender, EventArgs e)
        {
            PreencherListOuCombo(_mapasDeArquivos[comboBoxBGetc.SelectedItem.ToString()].Lista.Keys.ToList(), listBoxBGetc);
        }

        private void comboBoxTexturas_SelectionChangeCommitted(object sender, EventArgs e)
        {
            PreencherListOuCombo(_mapasDeArquivos[comboBoxTexturas.SelectedItem.ToString()].Lista.Keys.ToList(), listBoxTexturasDePasta);
        }

        private void listBoxBGetc_SelectedIndexChanged(object sender, EventArgs e)
        {
            string argumentosImg = _mapasDeArquivos[comboBoxBGetc.SelectedItem.ToString()].Lista[listBoxBGetc.SelectedItem.ToString()];
            if (_ncgrBgCarregado!= null)
            _ncgrBgCarregado.Imagem.Dispose();

             CarregarNcgr(argumentosImg, comboBoxBGetc.SelectedItem.ToString());
             AdicionarImagemPictureEInformacoes(pictureBoxBgetc, _ncgrBgCarregado);

            
        }

        private void listBoxTexturasDePasta_SelectedIndexChanged(object sender, EventArgs e)
        {
            string dirBtx = _mapasDeArquivos[comboBoxTexturas.SelectedItem.ToString()].Lista[listBoxTexturasDePasta.SelectedItem.ToString()];
            if (_btxCarregado != null)
                _btxCarregado = null;
            _btxCarregado = new Btx(dirBtx);
            PreencherListOuCombo(_btxCarregado.Texturas, listBoxTexturasDoBtx);
            //string argumentosImg = listaTexturasCarregadas[listBoxTexturasDePasta.SelectedIndex];
            //AdicionarImagemAPicitureBoxTexturas(argumentosImg);
            //buttonExportarSelecionaTextura.Enabled = true;
            //buttonImportarTexturaSelecionada.Enabled = true;
        }

        private void listBoxTexturasDoBtx_SelectedIndexChanged(object sender, EventArgs e)
        {
           // pictureBoxTexturas.Image = _btxCarregado.Texturas[listBoxTexturasDoBtx.SelectedIndex].Textura;
            AdicionarImagemPictureTexturaEhInfos(pictureBoxTexturas, _btxCarregado, listBoxTexturasDoBtx.SelectedIndex);
            

           // PaletaNaPicture(pictureBoxPaletaTextura, labelQtdCoresTextura, _btxCarregado.Texturas[listBoxTexturasDoBtx.SelectedIndex].PaletaImg);
        }

        private void AdicionarImagemPictureEInformacoes(PictureBox pictureBox, Ncgr ncgr)
        {
            if (pictureBoxBgetc.Image != null)
                pictureBoxBgetc.Image.Dispose();
            labelResX.Text = ncgr.Imagem.Width + "";
            labelResY.Text = ncgr.Imagem.Height + "";
            pictureBoxBgetc.Width = ncgr.Imagem.Width + 10;
            pictureBoxBgetc.Height = ncgr.Imagem.Height + 10;
            pictureBoxBgetc.Image = ncgr.Imagem;
        }

        private void AdicionarImagemPictureTexturaEhInfos(PictureBox pictureBox, Btx btx, int index)
        {
           

            pictureBox.Image = btx.Texturas[index].Textura;

            ResXTextura.Text = pictureBox.Image.Width + "";
            ResYTextura.Text = pictureBox.Image.Height + "";
            checkBoxTex4bpp.Checked = false;
            checkBoxTex8bpp.Checked = false;

            if (btx.Texturas[index].Bpp == Bpp.bpp4)
                checkBoxTex4bpp.Checked = true;
            else
                checkBoxTex8bpp.Checked = true;
        }

        private void buttonExportarImgSel_Click(object sender, EventArgs e)
        {
            _ncgrBgCarregado.Exportar();             
            
            MessageBox.Show(this, $"Imagem exportada para: {_ncgrBgCarregado.LocalParaExportacao}",
                                  "Aviso", MessageBoxButtons.OK,
                                  MessageBoxIcon.Information);
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            DesativarOuAtivarBotoesImagensBg(false);

            DialogResult dialogResult = MessageBox.Show("Tem certeza que deseja exportar todas as pastas?", "Aviso", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes) 
            {
               
                await Task.Run(() => ExportarImagensdeTodosCombos(comboBoxBGetc, progressBarExportImgs));
                MessageBox.Show(this, $"Imagens exportadas para: __imagens\\",
                              "Aviso", MessageBoxButtons.OK,
                              MessageBoxIcon.Information);
            }
            else
                MessageBox.Show(this, @"Operação cancelada.",
                              "Aviso", MessageBoxButtons.OK,
                              MessageBoxIcon.Information);
            
            DesativarOuAtivarBotoesImagensBg(true);

        }

        private void DesativarOuAtivarBotoesImagensBg(bool ativa)
        {
            buttonExportarImgSel.Enabled = ativa;
            buttonExportaPastaImg.Enabled = ativa;
            buttonTodasAsPastaImg.Enabled = ativa;
            buttonImportaSelecionadoImg.Enabled = ativa;
            buttonImportaSelecionadoImg.Enabled = ativa;
            listBoxBGetc.Enabled = ativa;
            buttonImportarBgLote.Enabled = ativa;

        }

        private void ExportarImagensdeTodosCombos(ComboBox combox, ProgressBar progresso) 
        {
          
            foreach (var item in combox.Items)
            {
                string tipo = item.ToString();

              //  progresso.Minimum = 0;
               // progresso.Maximum = _mapasDeArquivos[tipo].Lista.Count;

                foreach (var imagemLista in _mapasDeArquivos[tipo].Lista)
                {
                    Ncgr ncgr = new Ncgr(imagemLista.Value, _mapasDeArquivos[tipo].Tipo);
                    ncgr.Exportar();
                   // progresso.Value++;
                }
                    //progresso.Value = 0;
            }


           

           
        }

        #endregion

        private void PreencherComboBoxResolucoesOam()
        {
            

            


            comboBoxResolucaoOamAdicionar.Items.Add("8x8");
            comboBoxResolucaoOamAdicionar.Items.Add("16x16");
            comboBoxResolucaoOamAdicionar.Items.Add("32x32");
            comboBoxResolucaoOamAdicionar.Items.Add("64x64");
            comboBoxResolucaoOamAdicionar.Items.Add("16x8");
            comboBoxResolucaoOamAdicionar.Items.Add("32x8");
            comboBoxResolucaoOamAdicionar.Items.Add("32x16");
            comboBoxResolucaoOamAdicionar.Items.Add("64x32");
            comboBoxResolucaoOamAdicionar.Items.Add("8x16");
            comboBoxResolucaoOamAdicionar.Items.Add("8x32");
            comboBoxResolucaoOamAdicionar.Items.Add("16x32");
            comboBoxResolucaoOamAdicionar.Items.Add("32x64");
        }

       

    

       

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private string textoNaPrevia = "";

        private async void listBoxSpt_SelectedIndexChanged(object sender, EventArgs e)
        {
           /*DesativarBotoesTexto();
            textBoxPreviaSpt.Text = "";
            listBoxSpt.Enabled = false;
            string alvo = listBoxSpt.SelectedItem.ToString();
            textBoxUltimoScript.Text = "Convertendo " + alvo + "...";
            textoNaPrevia = await Task.Run(() => Spt.ExportarSpt(alvo, ProcurarArgSpt(alvo), false));
            textBoxPreviaSpt.Text = textoNaPrevia;
            textBoxUltimoScript.Text = alvo;
            AtivarBotoesTexto();
            listBoxSpt.Enabled = true;
            buttonConverteSeleTexto.Enabled = true;*/
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // string texto = Spt.ExportarSpt(listBoxSpt.SelectedItem.ToString(), false);
            // textBoxPreviaSpt.Text = texto;
            if (!Directory.Exists("__Textos\\Scripts_Originais\\"))
            {
                Directory.CreateDirectory("__Textos\\Scripts_Originais\\");
            }
            File.WriteAllText("__Textos\\Scripts_Originais\\" + listBoxSpt.SelectedItem.ToString().Replace(".spt", ".txt"), textoNaPrevia);
            MessageBox.Show(this, "Script convertido para .txt para o seguinte diretório: __Textos\\Scripts_Originais\\",
                                  "Aviso", MessageBoxButtons.OK,
                                  MessageBoxIcon.Information);

        }

        private async void button2_Click_1(object sender, EventArgs e)
        {
            string[] arquivosSpt = Directory.GetFiles(@"__Binarios\jpn_spt\", "*.spt");
            DesativarBotoesTexto();
            progressBarExportacaoTextos.Minimum = 0;
            progressBarExportacaoTextos.Maximum = arquivosSpt.Length;

            if (!Directory.Exists("__Textos\\Scripts_Originais\\"))
            {
                Directory.CreateDirectory("__Textos\\Scripts_Originais\\");
            }

            foreach (var spt in arquivosSpt)
            {
                string sptSemDir = Path.GetFileName(spt);
                string script = await Task.Run(() => Spt.ExportarSpt(sptSemDir, ProcurarArgSpt(spt), false));
                progressBarExportacaoTextos.Value += 1;
                File.WriteAllText("__Textos\\Scripts_Originais\\" + sptSemDir.Replace(".spt", ".txt"), script);
                textBoxUltimoScript.Text = sptSemDir;
            }

            MessageBox.Show(this, "Scripts convertidos para .txt para o seguinte diretório: __Textos\\Scripts_Originais\\",
                                  "Aviso", MessageBoxButtons.OK,
                                  MessageBoxIcon.Information);
            progressBarExportacaoTextos.Value = 0;
            AtivarBotoesTexto();
        }

        private async void buttonImportaTextos_Click(object sender, EventArgs e)
        {
            string[] arquivosTxt = Directory.GetFiles(@"__Textos\Scripts_Traduzidos\", "*.txt");
            DesativarBotoesTexto();
            progressBarExportacaoTextos.Minimum = 0;
            progressBarExportacaoTextos.Maximum = arquivosTxt.Length;


            foreach (var txt in arquivosTxt)
            {

                string txtSemdir = Path.GetFileName(txt);
                await Task.Run(() => Spt.ImportaSpt(txt, ProcurarArgSpt(txt), false));
                progressBarExportacaoTextos.Value += 1;
                textBoxUltimoScript.Text = txtSemdir;
            }

            MessageBox.Show(this, "Importação dos scripts concluída!\nCrie um novo jpn_spt.bin na aba \"Arquivos Binários\".",
                                  "Aviso", MessageBoxButtons.OK,
                                  MessageBoxIcon.Information);

            progressBarExportacaoTextos.Value = 0;
            AtivarBotoesTexto();


        }

        private string ProcurarArgSpt(string txt)
        {
            List<string> infoScripts = File.ReadAllLines(@"__Textos\_Tabelas\_infoScripts.txt").ToList();
            string temArg = "";

            foreach (var item in infoScripts)
            {
                if (item.Contains(Path.GetFileName(txt).Replace(".txt", ".spt")))
                {
                    temArg = item.Split(',')[1];
                }
            }

            return temArg;
        }

        private void DesativarBotoesTexto()
        {
            buttonConverteSeleTexto.Enabled = false;
            buttonExportaTTextos.Enabled = false;
            buttonImportaTextos.Enabled = false;

        }

        private void AtivarBotoesTexto()
        {
            buttonConverteSeleTexto.Enabled = true;
            buttonExportaTTextos.Enabled = true;
            buttonImportaTextos.Enabled = true;

        }

        

        private void DesativarBotoesImagem()
        {
           

        }

        private void AtivarBotoesImagem()
        {
            buttonExportarImgSel.Enabled = true;
            buttonExportaPastaImg.Enabled = true;
            buttonTodasAsPastaImg.Enabled = true;
            buttonImportaSelecionadoImg.Enabled = true;
            buttonImportaSelecionadoImg.Enabled = true;
            listBoxBGetc.Enabled = true;

        }



       

        private List<string> listaCarregadaDeImagens = new List<string>();
        private List<string> listaCarregadaDeSprites = new List<string>();
        private List<string> listaTexturasCarregadas = new List<string>();

       /* private void PreenchaListBoxBgEtc(string lista)
        {
            string[] listaDeArquivos = File.ReadAllLines(@"__Imagens\_Info_Imagens\" + lista + ".txt");
            listBoxBGetc.Items.Clear();
            listaCarregadaDeImagens.Clear();
            // comboBoxItensAexportar.Items.Add("Todos");

            foreach (var item in listaDeArquivos)
            {
                listaCarregadaDeImagens.Add(item);
                if (item.Split(',')[0].Contains("com_"))
                {
                    listBoxBGetc.Items.Add("com_" + item.Split(',')[0].Split('\\')[2]);
                }
                else
                {
                    listBoxBGetc.Items.Add("jpn_" + item.Split(',')[0].Split('\\')[2]);
                }

            }
        }*/

   

        private void CarregarNcgr(string argumentosImg, string tipo)
        {
            Ncgr ncgr = new Ncgr(argumentosImg, tipo);

            if (ncgr.EhSprite)
                _ncgrSpriteCarregado = ncgr;            
            else
                _ncgrBgCarregado = ncgr;
                      
      
        }




        private void PaletaNaPicture(PictureBox pb, Label l, List<Color> cores)
        {
            int x = 0;
            int y = 0;

            if (pb.Image != null)
            {
                pb.Image.Dispose();
            }

            Bitmap paleta = new Bitmap(128, 128);

            // Draw line to screen.
            using (var graphics = Graphics.FromImage(paleta))
            {

                foreach (var item in cores)
                {
                    SolidBrush brush = new SolidBrush(item);
                    Rectangle rect = new Rectangle(x, y, 8, 8);
                    graphics.FillRectangle(brush, rect);

                    brush.Dispose();

                    x += 8;
                    if (x == 128)
                    {
                        x = 0;
                        y += 8;
                    }
                }

            }

            pb.Image = paleta;

            l.Text = cores.Count + "";
        }

        

        private async void button2_Click_2(object sender, EventArgs e)
        {
            DesativarBotoesImagem();

            DialogResult dialogResult = MessageBox.Show("Tem certeza que deseja exportar pasta?", "Aviso", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {


                progressBarExportImgs.Minimum = 0;
                progressBarExportImgs.Maximum = _mapasDeArquivos[comboBoxBGetc.SelectedItem.ToString()].Lista.Count;

                foreach (var item in _mapasDeArquivos[comboBoxBGetc.SelectedItem.ToString()].Lista)
                {
                    string combo = comboBoxBGetc.SelectedItem.ToString();
                    Ncgr ncgr = await Task.Run(() => new Ncgr(item.Value, combo));                   
                    ncgr.Exportar();

                    progressBarExportImgs.Value += 1;
                }

                progressBarExportImgs.Value = 0;

                MessageBox.Show(this, @"Imagens exportadas",
                                    "Aviso", MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
            }



            AtivarBotoesImagem();
        }

        private Bitmap ExportarSomenteImagem(string arg, string combo)
        {


            //if (combo.Contains("2_mapa_bgs_sem_tilemap")
            //    || combo.Contains("7_mapa_name_tags")
            //    || combo.Contains("8_mapa_bg_upcut")
            //    || combo.Contains("9_mapa_bg_upcut_local"))
            //{
            //    string[] argumentos = arg.Split(',');
            //    Ncgr ncgr = new Ncgr(argumentos[0]);
            //    Bitmap img = ncgr.ExportarNCGR(argumentos[1]);
            //    return img;


            //}
            //else if (combo.Contains("1_mapa_bgs_tilemap"))
            //{
            //    string[] argumentos = arg.Split(',');
            //    Ncgr ncgr = new Ncgr(argumentos[0]);
            //    Bitmap img = ncgr.ExportarNCGRTile(argumentos[1], argumentos[2]);
            //    return img;
            //}
            //else
            //{
            //    return null;
            //}



            return null;



        }

       

        private async void buttonImportaSelecionadoImg_Click(object sender, EventArgs e)
        {
            DesativarBotoesImagem();
            using (OpenFileDialog opf = new OpenFileDialog())
            {
                opf.Filter = "Arquivos .png (*.png)|*.png|All files (*.*)|*.*";
                if (opf.ShowDialog() == DialogResult.OK)
                {
                    string listaSelecionadaNaCombo = comboBoxBGetc.SelectedItem.ToString();
                    await Task.Run(() => ImportarImagemSelecionada(opf.FileName, listaSelecionadaNaCombo));

                    string argumentosImg = listaCarregadaDeImagens[listBoxBGetc.SelectedIndex];
                    //CarregarNcgr(argumentosImg);
                }
            }

            AtivarBotoesImagem();
        }

        private void buttonImportaImgLote_Click(object sender, EventArgs e)
        {
            DesativarBotoesImagem();
            ImportarImagemEmLote();
            AtivarBotoesImagem();
        }

        private void comboBoxSprites_SelectedIndexChanged(object sender, EventArgs e)
        {
          /*  if (pictureBoxSprite.Image != null)
            {
                pictureBoxSprite.Image.Dispose();
                pictureBoxSprite.Image = null;
            }
            PreenchaListBoxSprites(comboBoxSprites.SelectedItem.ToString());

            listBoxSpritesDoNgcr.Items.Clear();
            DesativarBotoesSprite();*/



            //checkBoxAtivarBordas = new CheckBox();
            // checkBoxFundoTransparenteOam = new CheckBox();
            // checkBoxNumerosOam = new CheckBox();
            // checkBoxEditarPosicao = new CheckBox();


            // panelOam = new Panel();
            // panelOam.Width = 512;
            // panelOam.Height = 256;

        }

        private void listBoxSprites_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxSprites.SelectedIndex > -1)
            {
                string argumentosImg = listaCarregadaDeSprites[listBoxSprites.SelectedIndex];
                _adicionouOam = false;
                _imagemNovaTemporaria = null;
                LimparComponentesTelaDeSprite();

                ExportarSpritesDoNcgrEhAdicionarLista(argumentosImg);
                listBoxSpritesDoNgcr.SelectedIndex = 0;
                _adicionouOam = false;
                checkBoxAtivarBordas.Enabled = true;
                checkBoxEditarPosicao.Enabled = true;
                checkBoxFundoTransparenteOam.Enabled = true;
                checkBoxNumerosOam.Enabled = true;
            }

            //checkBoxAtivarBordas.Checked = false;
            // checkBoxAtivarBordas.Enabled = false;
            //comboBoxSpritesLista.Enabled = true;
        }

        private void LimparComponentesTelaDeSprite()
        {
            checkedListBoxSpriteSelecionado.Items.Clear();
            checkBoxEditarPosicao.Checked = false;
            checkBoxEditarPosicao.Enabled = false;
            buttonExportOam.Enabled = true;
            buttonImportOam.Enabled = true;
            checkBoxAtivarBordas.AutoCheck = true;
            numericUpDownPosOamX.Value = 0;
            numericUpDownPosOamY.Value = 0;
            comboBoxResolucoesOam.SelectedIndex = -1;

            //labelOamResX.Text = 0 + "";
            //labelOamResY.Text = 0 + "";
        }


        private void ExportarSpritesDoNcgrEhAdicionarLista(string argumentosImg)
        {
            listBoxSpritesDoNgcr.Items.Clear();

            ExportarSpritesDoNcgr(argumentosImg);


    

            int contador = 0;

  

            //foreach (var item in _ncgrSpriteCarregado.ArquivoNcer.GrupoDeTabelasOam)
            //{
               
            //    listBoxSpritesDoNgcr.Items.Add(Path.GetFileName(argumentosImg.Split(',')[0]) + "_" + contador++);
            //}

           

        }

        private void ExportarSpritesDoNcgr(string argumentosImg)
        {

            ExporteSprites(argumentosImg, comboBoxSprites.SelectedItem.ToString(), false);
        }

      //  private Ncgr _ncgrSpriteCarregado;

        private void ExporteSprites(string argumentosImg, string listaSelecionada, bool fundoTransparente)
        {
            if (_ncgrSpriteCarregado != null)
            {
                _ncgrSpriteCarregado = null;
            }
            // comboBoxSprites.SelectedItem.ToString()
            if (listaSelecionada.Contains("3_mapa_sprites_modo_1"))
            {
                string[] argumentos = argumentosImg.Split(',');
                Ncgr ncgr = null;// new Ncgr(argumentos[0]);
                ncgr.ExportarNCGRSprite1D(argumentos[1], argumentos[2], argumentos[3], fundoTransparente);
                _ncgrSpriteCarregado = ncgr;
                // CoresDaPaleta = ncgr.CoresConvertidas;
            }
            else if (listaSelecionada.Contains("4_mapa_sprites_modo_2"))
            {
                string[] argumentos = argumentosImg.Split(',');
                Ncgr ncgr = null; //new Ncgr(argumentos[0]);
                ncgr.ExportarNCGRSprite2D(argumentos[1], argumentos[2], argumentos[3], fundoTransparente);
                _ncgrSpriteCarregado = ncgr;
                // CoresDaPaleta = ncgr.CoresConvertidas;
            }
            else if (listaSelecionada.Contains("5_Gerais_sprites_binarios")
                || listaSelecionada.Contains("11_Botoes_sprites")
                || listaSelecionada.Contains("12_Logicas_provas_sprites")
                || listaSelecionada.Contains("13_Personagens_sprites"))
            {
                string[] argumentos = argumentosImg.Split(',');
                // if (!Directory.Exists(argumentos[0].Replace(".bin", "")))
                // {
                AaiBin.ExporteBin(argumentos[0]);
                // }
                Ncgr ncgr = null; //new Ncgr(argumentos[0].Replace(".bin", "") + "\\" + Path.GetFileName(argumentos[0].Replace(".bin", ".ncgr")));
                ncgr.ExportarNCGRSprite1D(argumentos[1], argumentos[0].Replace(".bin", "") + "\\" + Path.GetFileName(argumentos[0].Replace(".bin", ".ncer")), argumentos[2], fundoTransparente);
                _ncgrSpriteCarregado = ncgr;

            }
            else if (listaSelecionada.Contains("6_mapa_sprites_binarios_modo_2"))
            {
                string[] argumentos = argumentosImg.Split(',');
                // if (!Directory.Exists(argumentos[0].Replace(".bin", "")))
                // {

                // }
                AaiBin.ExporteBin(argumentos[0]);
                Ncgr ncgr = null; //new Ncgr(argumentos[0].Replace(".bin", "") + "\\" + Path.GetFileName(argumentos[0].Replace(".bin", ".ncgr")));
                ncgr.ExportarNCGRSprite2D(argumentos[1], argumentos[0].Replace(".bin", "") + "\\" + Path.GetFileName(argumentos[0].Replace(".bin", ".ncer")), argumentos[2], fundoTransparente);
                _ncgrSpriteCarregado = ncgr;
            }


            //return sprites;

        }



        private void AdicionarImagemNaPictureOam(List<Oam> valoresOam)
        {
            //  panelOam.Controls.Clear();
            if (pictureBoxSprite.Image != null)
            {
                pictureBoxSprite.Image.Dispose();
            }
            pictureBoxSprite.Image = MonteOamERetorneImagem(valoresOam);

        }

        private Bitmap MonteOamERetorneImagem(List<Oam> valoresOam)
        {
            bool temBorda = checkBoxAtivarBordas.Checked;

            Bitmap imagemDoOam = new Bitmap(512, 256);
            using (Graphics g = Graphics.FromImage(imagemDoOam))
            {
                int conta = 0;
                foreach (var item in valoresOam)
                {

                    /* PictureBoxCustom pers = new PictureBoxCustom(temBorda);
                     pers.Location = new Point((int)(item.X), (int)(item.Y));
                     pers.Width = item.Imagem.Width;
                     pers.Height = item.Imagem.Height;
                     pers.Image = item.Imagem;
                     Bitmap background = new Bitmap(item.Imagem.Width, item.Imagem.Height);
                     pers.BackColor = Color.Transparent;
                     panelOam.Controls.Add(pers);*/

                    if (_componentesDeVisibilidadeDoSprite[conta] == false)
                    {
                        conta++;
                        continue;
                    }

                    Rectangle rectangle = new Rectangle((int)item.X, (int)item.Y, item.Imagem.Width, item.Imagem.Height);
                    if (temBorda)
                    {


                        Bitmap copiaItem = new Bitmap(item.Imagem);
                        if (checkBoxFundoTransparenteOam.Checked)
                        {
                            copiaItem.MakeTransparent(item.Paleta.First());
                        }
                        using (Graphics gg = Graphics.FromImage(copiaItem))
                        {
                            Rectangle rect = new Rectangle(0, 0, copiaItem.Width, copiaItem.Height);
                            gg.DrawRectangle(new Pen(Brushes.Red, 2), rect);
                            gg.Dispose();

                        }
                        if (checkedListBoxSpriteSelecionado.SelectedItem != null)
                        {
                            if (conta == checkedListBoxSpriteSelecionado.SelectedIndex)
                            {
                                using (Graphics gg = Graphics.FromImage(copiaItem))
                                {
                                    Rectangle rect = new Rectangle(0, 0, copiaItem.Width, copiaItem.Height);
                                    gg.DrawRectangle(new Pen(Brushes.Yellow, 6), rect);
                                    gg.Dispose();

                                }
                            }
                        }
                        if (checkBoxNumerosOam.Checked)
                        {
                            using (Graphics gg = Graphics.FromImage(copiaItem))
                            {
                                using (Font arialFont = new Font("Arial", 8, FontStyle.Bold))
                                {
                                    gg.DrawString(conta.ToString(), arialFont, Brushes.DarkBlue, 0, 0);

                                }

                            }
                        }
                        g.DrawImage(copiaItem, rectangle);
                        copiaItem.Dispose();

                    }
                    else
                    {

                        Bitmap copiaItem = new Bitmap(item.Imagem);
                        if (checkBoxFundoTransparenteOam.Checked)
                        {
                            copiaItem.MakeTransparent(item.Paleta.First());
                        }
                        if (checkedListBoxSpriteSelecionado.SelectedItem != null)
                        {
                            if (conta == checkedListBoxSpriteSelecionado.SelectedIndex)
                            {
                                using (Graphics gg = Graphics.FromImage(copiaItem))
                                {
                                    Rectangle rect = new Rectangle(0, 0, copiaItem.Width, copiaItem.Height);
                                    gg.DrawRectangle(new Pen(Brushes.Yellow, 6), rect);
                                    gg.Dispose();

                                }
                            }
                        }
                        if (checkBoxNumerosOam.Checked)
                        {
                            using (Graphics gg = Graphics.FromImage(copiaItem))
                            {
                                using (Font arialFont = new Font("Arial", 8, FontStyle.Bold))
                                {
                                    gg.DrawString(conta.ToString(), arialFont, Brushes.DarkBlue, 0, 0);

                                }

                            }
                        }
                        g.DrawImage(copiaItem, rectangle);
                        copiaItem.Dispose();
                    }


                    conta++;
                }
            }

            return imagemDoOam;

        }

        private List<Bitmap> MonteOamERetorneImagemParaExportacao(List<Oam> valoresOam)
        {
            List<Oam> valoresOamClone = new List<Oam>(valoresOam);
            bool temBorda = checkBoxAtivarBordas.Checked;
            List<Bitmap> camadas = new List<Bitmap>();

            List<List<Oam>> Layers = SepararCamadas(valoresOamClone);



            foreach (var llayer in Layers)
            {
                Bitmap lr = new Bitmap(512, 256);
                using (Graphics g = Graphics.FromImage(lr))
                {
                    int conta = 0;
                    foreach (var item in llayer)
                    {



                        Rectangle rectangle = new Rectangle((int)item.X, (int)item.Y, item.Imagem.Width, item.Imagem.Height);
                        g.DrawImage(item.Imagem, rectangle);




                        conta++;
                    }
                }

                camadas.Add(lr);


            }




            return camadas;

        }

        private List<List<Oam>> SepararCamadas(List<Oam> valoresOamClone)
        {

            List<List<Oam>> Layers = new List<List<Oam>>();
            List<Oam> layer = new List<Oam>();

            while (valoresOamClone.Count > 0)
            {
                bool iterc = false;
                foreach (var item in layer)
                {
                    if (valoresOamClone[0].Retangulo.IntersectsWith(item.Retangulo))
                    {
                        iterc = true;
                        break;

                    }

                }



                if (!iterc)
                {
                    layer.Add(valoresOamClone[0]);
                    valoresOamClone.Remove(valoresOamClone[0]);
                    if (valoresOamClone.Count == 0)
                    {
                        Layers.Add(layer);
                    }

                }
                else
                {
                    Layers.Add(layer);
                    layer = new List<Oam>();
                    layer.Add(valoresOamClone[0]);

                    valoresOamClone.Remove(valoresOamClone[0]);

                    if (valoresOamClone.Count == 0)
                    {
                        Layers.Add(layer);
                    }

                }





            }

            return Layers;
        }

        private Dictionary<string, int> _resolucoesOam = new Dictionary<string, int>();

        private void checkedListBoxSpriteSelecionado_SelectedIndexChanged(object sender, EventArgs e)
        {
            // numericUpDownPosOamX.Enabled = true;
            // numericUpDownPosOamY.Enabled = true;
            //  checkBoxEditarPosicao.Checked = false;
            // checkBoxEditarPosicao.Enabled = true;
            //checkBoxEditarPosicao.Enabled = true;
            if (checkedListBoxSpriteSelecionado.SelectedIndex > -1)
            {
                checkBoxEditarPosicao.Enabled = true;
                string resX = _ncgrSpriteCarregado.ArquivoNcer.GrupoDeTabelasOam[listBoxSpritesDoNgcr.SelectedIndex].TabelaDeOams[checkedListBoxSpriteSelecionado.SelectedIndex].Imagem.Width.ToString();
                string resY = _ncgrSpriteCarregado.ArquivoNcer.GrupoDeTabelasOam[listBoxSpritesDoNgcr.SelectedIndex].TabelaDeOams[checkedListBoxSpriteSelecionado.SelectedIndex].Imagem.Height.ToString();
                int indexListaResOam = _resolucoesOam[resX + "x" + resY];
                comboBoxResolucoesOam.SelectedIndex = indexListaResOam;
                //labelOamResX.Text = "" + ;
                // labelOamResY.Text = "" + ;
                numericUpDownPosOamX.Value = _ncgrSpriteCarregado.ArquivoNcer.GrupoDeTabelasOam[listBoxSpritesDoNgcr.SelectedIndex].TabelaDeOams[checkedListBoxSpriteSelecionado.SelectedIndex].X;
                numericUpDownPosOamY.Value = _ncgrSpriteCarregado.ArquivoNcer.GrupoDeTabelasOam[listBoxSpritesDoNgcr.SelectedIndex].TabelaDeOams[checkedListBoxSpriteSelecionado.SelectedIndex].Y;
                IndexOamCarregado = checkedListBoxSpriteSelecionado.SelectedIndex;
                pictureBoxSprite.Image.Dispose();
                pictureBoxSprite.Image = null;
                pictureBoxSprite.Image = MonteOamERetorneImagem(_ncgrSpriteCarregado.ArquivoNcer.GrupoDeTabelasOam[listBoxSpritesDoNgcr.SelectedIndex].TabelaDeOams);

            }


        }

        private List<bool> _componentesDeVisibilidadeDoSprite = new List<bool>();

        private void checkedListBoxSpriteSelecionado_ItemCheck(object sender, ItemCheckEventArgs e)
        {


            if (checkedListBoxSpriteSelecionado.SelectedIndex > -1)
            {
                if (checkedListBoxSpriteSelecionado.GetItemChecked(checkedListBoxSpriteSelecionado.SelectedIndex) == true)
                {
                    _componentesDeVisibilidadeDoSprite[checkedListBoxSpriteSelecionado.SelectedIndex] = false;
                    AdicionarImagemNaPictureOam(_ncgrSpriteCarregado.ArquivoNcer.GrupoDeTabelasOam[listBoxSpritesDoNgcr.SelectedIndex].TabelaDeOams);

                }
                else
                {
                    _componentesDeVisibilidadeDoSprite[checkedListBoxSpriteSelecionado.SelectedIndex] = true;
                    AdicionarImagemNaPictureOam(_ncgrSpriteCarregado.ArquivoNcer.GrupoDeTabelasOam[listBoxSpritesDoNgcr.SelectedIndex].TabelaDeOams);

                }
            }


        }

        private void listBoxSpritesDoNgcr_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxSpritesDoNgcr.SelectedIndex > -1)
            {

                checkedListBoxSpriteSelecionado.Items.Clear();

                checkBox8bppOam.Checked = false;
                checkBox4bppOam.Checked = false;
                List<bool> visiblidadeDosComponentes = new List<bool>();
                for (int i = 0; i < _ncgrSpriteCarregado.ArquivoNcer.GrupoDeTabelasOam[listBoxSpritesDoNgcr.SelectedIndex].TabelaDeOams.Count; i++)
                {
                    checkedListBoxSpriteSelecionado.Items.Add("OAM " + i);
                    checkedListBoxSpriteSelecionado.SetItemChecked(i, true);
                    visiblidadeDosComponentes.Add(true);

                }
                _componentesDeVisibilidadeDoSprite = visiblidadeDosComponentes;
                AdicionarImagemNaPictureOam(_ncgrSpriteCarregado.ArquivoNcer.GrupoDeTabelasOam[listBoxSpritesDoNgcr.SelectedIndex].TabelaDeOams);

                if (_ncgrSpriteCarregado.ArquivoNcer.GrupoDeTabelasOam[listBoxSpritesDoNgcr.SelectedIndex].TabelaDeOams.Count > 0)
                {
                    PaletaNaPicture(pictureBoxPaletaOam, labelQuantidadeCoresOam, _ncgrSpriteCarregado.ArquivoNcer.GrupoDeTabelasOam[listBoxSpritesDoNgcr.SelectedIndex].TabelaDeOams[0].Paleta);
                    if (_ncgrSpriteCarregado.ArquivoNcer.GrupoDeTabelasOam[listBoxSpritesDoNgcr.SelectedIndex].TabelaDeOams[0].Bpp == Bpp.bpp4)
                    {
                        checkBox4bppOam.Checked = true;
                    }
                    else
                    {
                        checkBox8bppOam.Checked = true;
                    }
                }


            }

            numericUpDownPosOamX.Value = 0;
            numericUpDownPosOamY.Value = 0;


        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            /*  List<bool> visibilidade = new List<bool>();
              List<Point> backupPosicoes = new List<Point>();

              if (checkBoxAtivarBordas.Checked)
              {


                  foreach (Control item in panelOam.Controls)
                  {

                      visibilidade.Add(item.Visible);
                      backupPosicoes.Add(item.Location);
                  }

                  panelOam.Controls.Clear();


                  int contador = 0;
                  foreach (var item in _ncgrSpriteCarregado.ArquivoNcer.Oams[listBoxSpritesDoNgcr.SelectedIndex].VariosOam)
                  {
                      PictureBoxCustom pers = new PictureBoxCustom(true);
                      pers.Location = backupPosicoes[contador];
                      pers.Width = item.Imagem.Width;
                      pers.Height = item.Imagem.Height;
                      pers.Image = item.Imagem;
                      pers.Visible = visibilidade[contador];
                      panelOam.Controls.Add(pers);
                      contador++;
                  }
              }
              else
              {
                  foreach (Control item in panelOam.Controls)
                  {

                      visibilidade.Add(item.Visible);
                      backupPosicoes.Add(item.Location);
                  }
                  panelOam.Controls.Clear();
                  int contador = 0;
                  foreach (var item in _ncgrSpriteCarregado.ArquivoNcer.Oams[listBoxSpritesDoNgcr.SelectedIndex].VariosOam)
                  {
                      PictureBoxCustom pers = new PictureBoxCustom(false);
                      pers.Location = backupPosicoes[contador];
                      pers.Width = item.Imagem.Width;
                      pers.Height = item.Imagem.Height;
                      pers.Image = item.Imagem;
                      pers.Visible = visibilidade[contador];
                      panelOam.Controls.Add(pers);
                      contador++;
                  }
              }
              */
            if (listBoxSpritesDoNgcr.SelectedIndex > -1)
            {
                if (checkBoxAtivarBordas.Checked)
                {
                    AdicionarImagemNaPictureOam(_ncgrSpriteCarregado.ArquivoNcer.GrupoDeTabelasOam[listBoxSpritesDoNgcr.SelectedIndex].TabelaDeOams);
                }
                else
                {
                    AdicionarImagemNaPictureOam(_ncgrSpriteCarregado.ArquivoNcer.GrupoDeTabelasOam[listBoxSpritesDoNgcr.SelectedIndex].TabelaDeOams);
                }
            }





        }

        private void label25_Click(object sender, EventArgs e)
        {

        }

        int IndexOamCarregado = 0;

        private void numericUpDownPosOamX_ValueChanged(object sender, EventArgs e)
        {
            if (checkBoxEditarPosicao.Checked)
            {
                if (checkedListBoxSpriteSelecionado.SelectedIndex > -1 && listBoxSpritesDoNgcr.SelectedIndex > -1)
                {
                    int x = (int)numericUpDownPosOamX.Value;
                    _ncgrSpriteCarregado.ArquivoNcer.GrupoDeTabelasOam[listBoxSpritesDoNgcr.SelectedIndex].TabelaDeOams[checkedListBoxSpriteSelecionado.SelectedIndex].X = (uint)x;
                    pictureBoxSprite.Image.Dispose();
                    pictureBoxSprite.Image = null;
                    pictureBoxSprite.Image = MonteOamERetorneImagem(_ncgrSpriteCarregado.ArquivoNcer.GrupoDeTabelasOam[listBoxSpritesDoNgcr.SelectedIndex].TabelaDeOams);
                }


            }

        }

        private void numericUpDownPosOamX_Click(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (listBoxSpritesDoNgcr.SelectedIndex > -1)
            {
                if (checkBoxEditarPosicao.Checked)
                {
                    numericUpDownPosOamX.Enabled = true;
                    numericUpDownPosOamY.Enabled = true;
                    buttonAdicionarOam.Enabled = true;
                    buttonSalvarModifcaoesOam.Enabled = true;
                    comboBoxResolucaoOamAdicionar.SelectedIndex = 0;
                    buttonExportOam.Enabled = false;
                    buttonImportOam.Enabled = false;
                    buttonImportSpritesLote.Enabled = false;
                    checkedListBoxSpriteSelecionado.Enabled = true;
                    checkedListBoxSpriteSelecionado.SelectedIndex = 0;
                }
                else
                {
                    buttonExportOam.Enabled = true;
                    buttonImportOam.Enabled = true;
                    buttonImportSpritesLote.Enabled = true;
                    numericUpDownPosOamX.Enabled = false;
                    numericUpDownPosOamY.Enabled = false;
                    buttonAdicionarOam.Enabled = false;
                    buttonSalvarModifcaoesOam.Enabled = false;
                    checkedListBoxSpriteSelecionado.Enabled = false;
                    checkedListBoxSpriteSelecionado.SelectedIndex = -1;
                    AdicionarImagemNaPictureOam(_ncgrSpriteCarregado.ArquivoNcer.GrupoDeTabelasOam[listBoxSpritesDoNgcr.SelectedIndex].TabelaDeOams);
                }
            }


        }

        private void numericUpDownPosOamY_ValueChanged(object sender, EventArgs e)
        {
            if (checkedListBoxSpriteSelecionado.SelectedIndex > -1 && listBoxSpritesDoNgcr.SelectedIndex > -1)
            {
                if (checkBoxEditarPosicao.Checked)
                {
                    int y = (int)numericUpDownPosOamY.Value;
                    _ncgrSpriteCarregado.ArquivoNcer.GrupoDeTabelasOam[listBoxSpritesDoNgcr.SelectedIndex].TabelaDeOams[checkedListBoxSpriteSelecionado.SelectedIndex].Y = (uint)y;
                    pictureBoxSprite.Image.Dispose();
                    pictureBoxSprite.Image = null;
                    pictureBoxSprite.Image = MonteOamERetorneImagem(_ncgrSpriteCarregado.ArquivoNcer.GrupoDeTabelasOam[listBoxSpritesDoNgcr.SelectedIndex].TabelaDeOams);
                }
            }

        }

       

        List<string> listaArgumentosBtx;

       

        

        private void AdicionarImagemAPicitureBoxTexturas(string argumentosImg)
        {

            _btxCarregado = new Btx(argumentosImg);
            //  if (imagemCarregada != null)
            //  {
            ///      labelResX.Text = imagemCarregada.Width + "";
            //      labelResY.Text = imagemCarregada.Height + "";
            //  }
            //
            // pictureBoxBgetc.Width = imagemCarregada.Width;
            //  pictureBoxBgetc.Height = imagemCarregada.Height;


            int contador = 0;
            listBoxTexturasDoBtx.Items.Clear();

            //   comboBoxSpritesLista.Items.Clear();

            foreach (var item in _btxCarregado.Texturas)
            {
                // comboBoxSpritesLista.Items.Add(Path.GetFileName(argumentosImg.Split(',')[0]) + "_" + contador++);

                listBoxTexturasDoBtx.Items.Add(Path.GetFileName(argumentosImg.Split(',')[0]) + "_" + contador++);
            }

            listBoxTexturasDoBtx.SelectedIndex = 0;

            //  PaletaNaPicture();

        }

        

        private void ImportarImagemSelecionada(string imagem, string combo)
        {
            List<string> listaImg = File.ReadAllLines(@"__Imagens\_Info_Imagens\" + combo + ".txt").ToList();
            string nomeNgcr = Path.GetFileName(imagem).Replace("png", "ncgr").Replace("jpn_", "").Replace("com_", "");
            string prefixo = Path.GetFileName(imagem).Split('_')[0] + "_";
            string listaDImagem = Path.GetFileName(imagem).Replace("png", "ncgr").Split('_')[0] + "_";
            bool achou = false;
            foreach (var item in listaImg)
            {

                if (item.Contains(nomeNgcr))
                {
                    if (item.Contains(listaDImagem))
                    {
                        if (listaDImagem.Contains(prefixo))
                        {
                            ImporteImagemParaBin(imagem, item, combo);
                            achou = true;
                            break;
                        }

                    }

                }

                if (achou)
                {
                    break;
                }
            }
        }

        private void ImportarImagemEmLote()
        {
            string combo = "";
            List<string> imagens = Directory.GetFiles(@"__Imagens\_Imagens_BGS_Editados\").ToList();
            List<string> listas = Directory.GetFiles(@"__Imagens\_Info_Imagens\", "*.txt").ToList();
            // List<string> listaImg = File.ReadAllLines(@"__Imagens\_Info_Imagens\" + combo + ".txt").ToList();



            List<string[]> todasAsListas = new List<string[]>();

            foreach (var item in listas)
            {
                if (item.Contains("sprites"))
                {
                    continue;
                }
                todasAsListas.Add(File.ReadAllLines(item));
            }



            foreach (var imagem in imagens)
            {
                string nomeNgcr = Path.GetFileName(imagem).Replace("png", "ncgr").Replace("jpn_", "").Replace("com_", "");
                string listaDImagem = Path.GetFileName(imagem).Replace("png", "ncgr").Split('_')[0] + "_";
                int conta = 0;
                bool encontrou = false;
                string prefixo = Path.GetFileName(imagem).Split('_')[0] + "_";

                foreach (var lista in todasAsListas)
                {


                    foreach (var argh in lista)
                    {

                        if (argh.Contains(nomeNgcr))
                        {
                            if (argh.Contains(listaDImagem))
                            {
                                if (argh.Contains(prefixo))
                                {
                                    ImporteImagemParaBin(imagem, argh, Path.GetFileName(listas[conta]).Replace(".txt", ""));
                                    encontrou = true;
                                    break;
                                }


                            }

                        }
                    }

                    if (encontrou)
                    {
                        break;
                    }

                    conta++;

                }
            }


        }



        private void ImporteImagemParaBin(string imagem, string arg, string lista)
        {
            if (lista.Contains("2_mapa_bgs_sem_tilemap")
                || lista.Contains("7_mapa_name_tags")
                || lista.Contains("8_mapa_bg_upcut")
                || lista.Contains("9_mapa_bg_upcut_local"))
            {
                string[] argumentos = arg.Split(',');
              //  Ncgr ncgr = new Ncgr(argumentos[0]);
              //  ncgr.ImportaNCGRSemMap(imagem, argumentos[1]);
            }
            else if (lista.Contains("1_mapa_bgs_tilemap"))
            {
                string[] argumentos = arg.Split(',');
               // Ncgr ncgr = new Ncgr(argumentos[0]);
               // ncgr.ImportaNCGRComMap(imagem, argumentos[1], argumentos[2]);
            }
        }

        private void button2_Click_3(object sender, EventArgs e)
        {
            //terminar

            SalvarImagemSprite(_ncgrSpriteCarregado.ArquivoNcer.GrupoDeTabelasOam[listBoxSpritesDoNgcr.SelectedIndex].TabelaDeOams);


        }

        private void SalvarImagemSprite(List<Oam> valoresOam)
        {

            List<Bitmap> imagemFinal = MonteOamERetorneImagemParaExportacao(valoresOam);

            string dirSave = listaCarregadaDeSprites[listBoxSprites.SelectedIndex].Split(',').Last();
            string nome = listBoxSpritesDoNgcr.SelectedItem.ToString().Replace(".bin", "").Replace(".ncgr", "");
            string prefixo = listaCarregadaDeSprites[listBoxSprites.SelectedIndex].Contains("com_") ? "com_" : "jpn_";
            if (!Directory.Exists(dirSave))
            {
                Directory.CreateDirectory(dirSave);
            }

            int contaCamadas = 0;

            foreach (var item in imagemFinal)
            {
                item.Save(dirSave + prefixo + nome + "_camada_" + contaCamadas + "_.png");
                contaCamadas++;
            }



            textBoxStatusSprite.Text = "Exportado para: " + dirSave;
        }

        private async void button4_Click(object sender, EventArgs e)
        {

            LimparTelaDeSprite();
            buttonExportSpriteLista.Enabled = false;
            string lista = comboBoxSprites.SelectedItem.ToString();
            textBoxStatusSprite.Text = "Exportando lista de sprites...";
            DesativarBotoesSprite();
            DesativaListBoxSprites();
            await Task.Run(() => ExportarSpritesEmlote(lista));
            textBoxStatusSprite.Text = "Concluído.";
            AtivarBotoesSprite();
            AtivaListBoxSprites();


        }

        private void LimparTelaDeSprite()
        {
            if (_ncgrSpriteCarregado != null)
            {
                _ncgrSpriteCarregado = null;
                listBoxSprites.SelectedIndex = -1;
                if (listBoxSpritesDoNgcr.Items.Count > 0)
                {
                    listBoxSpritesDoNgcr.Items.Clear();
                }

                if (checkedListBoxSpriteSelecionado.Items.Count > 0)
                {
                    checkedListBoxSpriteSelecionado.Items.Clear();
                }



                if (pictureBoxSprite.Image != null)
                {
                    pictureBoxSprite.Image.Dispose();
                    pictureBoxSprite.Image = null;
                    pictureBoxPaletaOam.Image.Dispose();
                    pictureBoxPaletaOam.Image = null;
                }


            }
        }

        private void DesativarBotoesSprite()
        {
            buttonExportOam.Enabled = false;
            buttonImportOam.Enabled = false;
            buttonImportSpritesLote.Enabled = false;
            checkBoxAtivarBordas.AutoCheck = false;
            checkBoxFundoTransparenteOam.Enabled = false;
            checkedListBoxSpriteSelecionado.Items.Clear();
            checkBoxAtivarBordas.Checked = false;
            checkBoxEditarPosicao.Checked = false;
            checkBoxFundoTransparenteOam.Checked = false;
            checkBoxNumerosOam.Checked = false;
            checkBoxAtivarBordas.Enabled = false;
            checkBoxEditarPosicao.Enabled = false;
            checkBoxFundoTransparenteOam.Enabled = false;
            checkBoxNumerosOam.Enabled = false;
        }

        private void DesativaListBoxSprites()
        {
            listBoxSprites.Enabled = false;
            listBoxSpritesDoNgcr.Enabled = false;
        }

        private void AtivaListBoxSprites()
        {
            listBoxSprites.Enabled = true;
            listBoxSpritesDoNgcr.Enabled = true;
        }

        private void AtivarBotoesSprite()
        {
            buttonImportSpritesLote.Enabled = true;
            buttonExportSpriteLista.Enabled = true;
            checkBoxAtivarBordas.AutoCheck = true;
            checkBoxFundoTransparenteOam.Enabled = true;
            checkBoxAtivarBordas.Enabled = true;
            checkBoxEditarPosicao.Enabled = true;
            checkBoxFundoTransparenteOam.Enabled = true;
            checkBoxNumerosOam.Enabled = true;
        }

        private void ExportarSpritesEmlote(string listaSelecionada)
        {
            string argumento = "";

            foreach (var item in listaCarregadaDeSprites)
            {
                argumento = item;
                ExporteSprites(item, listaSelecionada, false);
                int indexDoOam = 0;
                foreach (var oam in _ncgrSpriteCarregado.ArquivoNcer.GrupoDeTabelasOam)
                {
                    Oams oamsPareExportar = oam;
                    // Bitmap imagemFinal = new Bitmap(512, 256);
                    //oamsPareExportar.TabelaDeOams.Reverse();

                    if (oamsPareExportar.TabelaDeOams.Count > 0)
                    {
                        List<Bitmap> imagemFinal = MonteOamERetorneImagemParaExportacao(oamsPareExportar.TabelaDeOams);

                        string dirSave = item.Split(',').Last();
                        string nome = Path.GetFileName(item.Split(',')[0]).Replace(".bin", "").Replace(".ncgr", "");
                        string prefixo = item.Split(',')[0].Contains("com_") ? "com_" : "jpn_";
                        if (!Directory.Exists(dirSave))
                        {
                            Directory.CreateDirectory(dirSave);
                        }

                        int contaCamadas = 0;

                        foreach (var camada in imagemFinal)
                        {
                            camada.Save(dirSave + prefixo + nome + "_" + indexDoOam + "_camada_" + contaCamadas + "_.png");
                            contaCamadas++;

                            camada.Dispose();
                        }


                    }
                    indexDoOam++;


                }

            }
            //  }
            // catch (Exception e)
            // {

            //throw new Exception(e.Message);
            //   }


        }

        private async void button3_Click_1(object sender, EventArgs e)
        {
            List<bool> oamsAtivados = new List<bool>();

            int index = listBoxSprites.SelectedIndex;

            using (OpenFileDialog opf = new OpenFileDialog())
            {
                opf.Filter = "Arquivos .png (*.png)|*.png|All files (*.*)|*.*";
                if (opf.ShowDialog() == DialogResult.OK)
                {
                    List<Oam> oamsParaInserir = new List<Oam>();
                    string listaSelecionadaNaCombo = comboBoxSprites.SelectedItem.ToString();
                    List<List<Oam>> Layers = SepararCamadas(_ncgrSpriteCarregado.ArquivoNcer.GrupoDeTabelasOam[listBoxSpritesDoNgcr.SelectedIndex].TabelaDeOams);

                    /*for (int i = 0; i < _componentesDeVisibilidadeDoSprite.Count; i++)
                    {
                        if (_componentesDeVisibilidadeDoSprite[i])
                        {
                            oamsParaInserir.Add(_ncgrSpriteCarregado.ArquivoNcer.GrupoDeTabelasOam[listBoxSpritesDoNgcr.SelectedIndex].TabelaDeOams[i]);
                        }


                    }*/

                    string[] args = Path.GetFileName(opf.FileName).Split('_');
                    int indexQualLayer = Convert.ToInt32(args[args.Length - 2]);

                    oamsParaInserir = Layers[indexQualLayer];
                    // string listaSelecionadaNaCombo = comboBoxBGetc.SelectedItem.ToString();
                    string argImg = listaCarregadaDeSprites[listBoxSprites.SelectedIndex];
                    await Task.Run(() => ImportaSprites(opf.FileName, argImg, listaSelecionadaNaCombo, oamsParaInserir));
                    textBoxStatusSprite.Text = "Sprite Importado com sucesso";
                    int indexUpdate = listBoxSprites.SelectedIndex;
                    listBoxSprites.SelectedIndex = -1;
                    listBoxSprites.SelectedIndex = indexUpdate;

                }
            }

            listBoxSprites.SelectedIndex = index;





        }




        private void ImportaSprites(string dirImg, string argumentosImg, string listaSelecionada, List<Oam> oams)
        {
            // comboBoxSprites.SelectedItem.ToString()
            if (listaSelecionada.Contains("3_mapa_sprites_modo_1"))
            {
                string[] argumentos = argumentosImg.Split(',');
                Ncgr ncgr = new Ncgr(argumentos[0], argumentos[2]);
                ncgr.ImportaNCGRSprites(dirImg, argumentos[1], oams);
            }
            else if (listaSelecionada.Contains("4_mapa_sprites_modo_2"))
            {
                string[] argumentos = argumentosImg.Split(',');
                Ncgr ncgr = new Ncgr(argumentos[0], argumentos[2]);
                ncgr.ImportaNCGRSpritesModo2(dirImg, argumentos[1], oams);
            }
            else if (listaSelecionada.Contains("5_Gerais_sprites_binarios")
                || listaSelecionada.Contains("11_Botoes_sprites")
                || listaSelecionada.Contains("12_Logicas_provas_sprites")
                || listaSelecionada.Contains("13_Personagens_sprites"))
            {
                string[] argumentos = argumentosImg.Split(',');
                // if (!Directory.Exists(argumentos[0].Replace(".bin", "")))
                //  {
                //    AaiBin.ExporteBin(argumentos[0]);
                //  }
                Ncgr ncgr = new Ncgr(argumentos[0].Replace(".bin", "") + "\\" + Path.GetFileName(argumentos[0].Replace(".bin", ".ncgr")), argumentos[0].Replace(".bin", "") + "\\" + Path.GetFileName(argumentos[0].Replace(".bin", ".ncer")));
                ncgr.ImportaNCGRSprites(dirImg, argumentos[1], oams);
                AaiBin.ImporteBin(argumentos[0].Replace(".bin", ""), argumentos[0]);

            }
            else if (listaSelecionada.Contains("6_mapa_sprites_binarios_modo_2"))
            {
                string[] argumentos = argumentosImg.Split(',');
                // if (!Directory.Exists(argumentos[0].Replace(".bin", "")))
                // {
                //     AaiBin.ExporteBin(argumentos[0]);
                // }

                Ncgr ncgr = new Ncgr(argumentos[0].Replace(".bin", "") + "\\" + Path.GetFileName(argumentos[0].Replace(".bin", ".ncgr")), argumentos[0].Replace(".bin", "") + "\\" + Path.GetFileName(argumentos[0].Replace(".bin", ".ncer")));
                ncgr.ImportaNCGRSpritesModo2(dirImg, argumentos[1], oams);
                AaiBin.ImporteBin(argumentos[0].Replace(".bin", ""), argumentos[0]);
            }


            //return sprites;

        }

        private async void button5_Click_1(object sender, EventArgs e)
        {
            string lista = comboBoxSprites.SelectedItem.ToString();
            LimparTelaDeSprite();
            textBoxStatusSprite.Text = "Importando sprites da pasta Editados...";
            DesativarBotoesSprite();
            await Task.Run(() => ImnpotarSpriteEmLote());
            textBoxStatusSprite.Text = "Concluído.";
            AtivarBotoesSprite();

        }

        private void ImnpotarSpriteEmLote()
        {
            List<string> imagens = Directory.GetFiles(@"__Imagens\_imagens_sprites_Editados\").ToList();
            List<string> listas = Directory.GetFiles(@"__Imagens\_Info_Imagens\", "*.txt").ToList();
            // List<string> listaImg = File.ReadAllLines(@"__Imagens\_Info_Imagens\" + combo + ".txt").ToList();

            List<string> listasUpdate = new List<string>();

            List<string[]> todasAsListas = new List<string[]>();

            foreach (var item in listas)
            {
                if (item.Contains("sprites"))
                {
                    todasAsListas.Add(File.ReadAllLines(item));
                    listasUpdate.Add(item);
                }

            }



            foreach (var imagem in imagens)
            {
                string nomeNgcr = Path.GetFileName(imagem).Replace("png", "ncgr").Replace("jpn_", "").Replace("com_", "");
                string[] partes = nomeNgcr.Split('_');
                string nomeNgcrFinal = "";
                for (int i = 0; i < partes.Length - 4; i++)
                {
                    if (i < partes.Length - 5)
                    {
                        nomeNgcrFinal += partes[i] + "_";
                    }
                    else
                    {
                        nomeNgcrFinal += partes[i] + ".ncgr";
                    }

                }

                nomeNgcr = nomeNgcrFinal;

                string listaDImagem = Path.GetFileName(imagem).Replace("png", "ncgr").Split('_')[0] + "_";
                int conta = 0;
                bool encontrou = false;
                string prefixo = Path.GetFileName(imagem).Split('_')[0] + "_";

                foreach (var lista in todasAsListas)
                {


                    foreach (var argh in lista)
                    {

                        if (argh.Contains(nomeNgcr) || argh.Contains(nomeNgcr.Replace(".ncgr", ".bin")))
                        {
                            if (argh.Contains(listaDImagem))
                            {
                                if (argh.Contains(prefixo))
                                {
                                    ImportaSpritesLote(imagem, argh, Path.GetFileName(listasUpdate[conta]).Replace(".txt", ""));
                                    encontrou = true;
                                    break;
                                }


                            }

                        }
                    }

                    if (encontrou)
                    {
                        break;
                    }

                    conta++;

                }
            }

        }

        private void ImportaSpritesLote(string dirImg, string argumentosImg, string listaSelecionada)
        {
            // comboBoxSprites.SelectedItem.ToString()
            string nomeImg = Path.GetFileName(dirImg);
            string[] args = nomeImg.Split('_');
            int indexTabela = Convert.ToInt32(args[args.Length - 4]);
            int indexQualLayer = Convert.ToInt32(args[args.Length - 2]);

            if (listaSelecionada.Contains("3_mapa_sprites_modo_1"))
            {
                string[] argumentos = argumentosImg.Split(',');
                Ncgr ncgr = new Ncgr(argumentos[0], argumentos[2]);
                List<Oam> oams = ncgr.ArquivoNcer.GrupoDeTabelasOam[indexTabela].TabelaDeOams;
                oams.Reverse();
                List<List<Oam>> Layers = SepararCamadas(oams);
                oams = Layers[indexQualLayer];
                ncgr.ImportaNCGRSprites(dirImg, argumentos[1], oams);

            }
            else if (listaSelecionada.Contains("4_mapa_sprites_modo_2"))
            {
                string[] argumentos = argumentosImg.Split(',');
                Ncgr ncgr = new Ncgr(argumentos[0], argumentos[2]);
                List<Oam> oams = ncgr.ArquivoNcer.GrupoDeTabelasOam[indexTabela].TabelaDeOams;
                oams.Reverse();
                List<List<Oam>> Layers = SepararCamadas(oams);
                oams = Layers[indexQualLayer];
                ncgr.ImportaNCGRSpritesModo2(dirImg, argumentos[1], oams);
            }
            else if (listaSelecionada.Contains("5_Gerais_sprites_binarios")
                || listaSelecionada.Contains("11_Botoes_sprites")
                || listaSelecionada.Contains("12_Logicas_provas_sprites")
                || listaSelecionada.Contains("13_Personagens_sprites"))
            {
                string[] argumentos = argumentosImg.Split(',');
                // if (!Directory.Exists(argumentos[0].Replace(".bin", "")))
                //  {
                //    AaiBin.ExporteBin(argumentos[0]);
                //  }
                Ncgr ncgr = new Ncgr(argumentos[0].Replace(".bin", "") + "\\" + Path.GetFileName(argumentos[0].Replace(".bin", ".ncgr")), argumentos[0].Replace(".bin", "") + "\\" + Path.GetFileName(argumentos[0].Replace(".bin", ".ncer")));
                List<Oam> oams = ncgr.ArquivoNcer.GrupoDeTabelasOam[indexTabela].TabelaDeOams;
                oams.Reverse();
                List<List<Oam>> Layers = SepararCamadas(oams);
                oams = Layers[indexQualLayer];
                ncgr.ImportaNCGRSprites(dirImg, argumentos[1], oams);
                AaiBin.ImporteBin(argumentos[0].Replace(".bin", ""), argumentos[0]);
                //   _ncgrSpriteCarregado = ncgr;

            }
            else if (listaSelecionada.Contains("6_mapa_sprites_binarios_modo_2"))
            {
                string[] argumentos = argumentosImg.Split(',');
                //  if (!Directory.Exists(argumentos[0].Replace(".bin", "")))
                //  {
                //      AaiBin.ExporteBin(argumentos[0]);
                //   }

                Ncgr ncgr = new Ncgr(argumentos[0].Replace(".bin", "") + "\\" + Path.GetFileName(argumentos[0].Replace(".bin", ".ncgr")), argumentos[0].Replace(".bin", "") + "\\" + Path.GetFileName(argumentos[0].Replace(".bin", ".ncer")));
                List<Oam> oams = ncgr.ArquivoNcer.GrupoDeTabelasOam[indexTabela].TabelaDeOams;
                oams.Reverse();
                List<List<Oam>> Layers = SepararCamadas(oams);
                oams = Layers[indexQualLayer];
                ncgr.ImportaNCGRSpritesModo2(dirImg, argumentos[1], oams);
                AaiBin.ImporteBin(argumentos[0].Replace(".bin", ""), argumentos[0]);
            }


            //return sprites;

        }

        private void button6_Click(object sender, EventArgs e)
        {

            _btxCarregado.Exportar();
            textBoxStatusTexturas.Text = "Texturas exportadas";
        }

        private async void button7_Click(object sender, EventArgs e)
        {
            //Terminar
            textBoxStatusTexturas.Text = "Exportando lista de texturas...";
            DesativarBotoesTextura();
            await Task.Run(() => ExportarListaDeBtx());
            textBoxStatusTexturas.Text = "Concluído.";
            AtivarBotoesTextura();
        }

        private void DesativarBotoesTextura()
        {
            buttonExportarSelecionaTextura.Enabled = false;
            buttonExportarListaTextura.Enabled = false;
            buttonImportarTexturaSelecionada.Enabled = false;
            buttonImportarTexturaLote.Enabled = false;
            listBoxTexturasDePasta.Enabled = false;
            listBoxTexturasDoBtx.Enabled = false;
        }

        private void AtivarBotoesTextura()
        {
            buttonExportarSelecionaTextura.Enabled = true;
            buttonExportarListaTextura.Enabled = true;
            buttonImportarTexturaSelecionada.Enabled = true;
            buttonImportarTexturaLote.Enabled = true;
            listBoxTexturasDePasta.Enabled = true;
            listBoxTexturasDoBtx.Enabled = true;
        }

        private void ExportarListaDeBtx()
        {

            List<string> listaBtx = File.ReadAllLines(@"__Imagens\_Info_Imagens\" + "10_mapa_texturas.txt").ToList();
            foreach (var item in listaBtx)
            {
                Btx btx = new Btx(item);

                string pastaSave = item.Split(',').Last();
                if (!Directory.Exists(pastaSave))
                {
                    Directory.CreateDirectory(pastaSave);
                }

                int contador = 0;

                foreach (var bt in btx.Texturas)
                {
                    bt.Textura.Save(pastaSave + Path.GetFileName(item.Split(',')[0]).Replace(".btx", "_") + contador + ".png");
                    contador++;
                }

            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string argumentos = listaArgumentosBtx[listBoxTexturasDePasta.SelectedIndex];

            using (OpenFileDialog opf = new OpenFileDialog())
            {
                opf.Filter = "Arquivos .png (*.png)|*.png|All files (*.*)|*.*";
                if (opf.ShowDialog() == DialogResult.OK)
                {
                    Btx btx = new Btx();
                    btx.ImporteImagemParaBtx(argumentos, opf.FileName);

                }
            }

            textBoxStatusTexturas.Text = "Textura importada com sucesso.";


        }

        private async void buttonImportarTexturaLote_Click(object sender, EventArgs e)
        {
            textBoxStatusTexturas.Text = "Importando texturas da pasta Editados...";
            DesativarBotoesTextura();
            await Task.Run(() => ImportarTexturaEmLote());
            textBoxStatusTexturas.Text = "Concluído.";
            AtivarBotoesTextura();
        }


        private void ImportarTexturaEmLote()
        {
            List<string> texturasEditadas = Directory.GetFiles(@"__Imagens\_imagens_Texturas_Editadas\").ToList();
            List<string> argumentos = File.ReadAllLines(@"__Imagens\_Info_Imagens\10_mapa_texturas.txt").ToList();
            foreach (var dirTextura in texturasEditadas)
            {
                string nomeTextura = Path.GetFileName(dirTextura).Split('_')[0] + "_" + Path.GetFileName(dirTextura).Split('_')[1] + ".btx";
                for (int i = 0; i < argumentos.Count; i++)
                {
                    if (argumentos[i].Contains(nomeTextura))
                    {

                        Btx btx = new Btx();
                        btx.ImporteImagemParaBtx(argumentos[i], dirTextura);
                        break;

                    }
                }
            }


        }
        private void CarregarTabelaDeCaracteres(string tabela)
        {
            buttonSalvarTbl.Enabled = false;
            List<string> tabelaDeCaracteres = File.ReadAllLines(@"__Textos\_Tabelas\" + tabela).ToList();
            foreach (var linha in tabelaDeCaracteres)
            {
                string[] arg = linha.Split('=');
                string[] row = new string[3];
                row[0] = arg[0];
                row[1] = arg[1];
                row[2] = arg[2].Replace("{", "").Replace("}", "");


                dataGridView1.Rows.Add(row);
            }

        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            buttonSalvarTbl.Enabled = true;
        }

        private void buttonSalvarTbl_Click(object sender, EventArgs e)
        {
            List<string> novaTabela = new List<string>();

            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                string codigo = dataGridView1.Rows[i].Cells[0].Value.ToString();
                string caractere = dataGridView1.Rows[i].Cells[1].Value.ToString();
                string argumentos = dataGridView1.Rows[i].Cells[2].Value.ToString();
                novaTabela.Add(codigo + "=" + caractere + "=" + "{" + argumentos + "}");
            }

            dataGridView1.Rows.Clear();

            File.WriteAllLines(@"__Textos\_Tabelas\" + comboBoxTabelaCarac.SelectedItem.ToString(), novaTabela);
            CarregarTabelaDeCaracteres(comboBoxTabelaCarac.SelectedItem.ToString());
            Spt.AtualizarTabelas();
        }

        private List<PropriedadeDaFonte> _propriedadeDasFontes;
        private Bitmap _fonteCarregada;

        private void CarregueFonte1()
        {
            List<PropriedadeDaFonte> propriedadeDasFontes = new List<PropriedadeDaFonte>();
            ConversorDeImagens cv = new ConversorDeImagens();
            Bitmap fontetotal = new Bitmap(1024, 512);
            using (BinaryReader br = new BinaryReader(new MemoryStream(File.ReadAllBytes(@"ROM_Desmontada\arm9.bin"))))
            {
                int x = 0;
                int y = 0;


                using (Graphics g = Graphics.FromImage(fontetotal))
                {
                    int posTable = 0x45B4C;
                    int contador = 0;

                    for (int i = 0; i < 2012; i++)
                    {
                        br.BaseStream.Position = posTable;
                        short codigoCaractere = (short)(br.ReadInt16() ^ 0x55aa);
                        byte largura = br.ReadByte();
                        br.BaseStream.Seek(1, SeekOrigin.Current);
                        int ponteiroFonte = br.ReadInt32() - 0x2000000;

                        br.BaseStream.Position = ponteiroFonte;
                        Bitmap fonte = cv.Exporte1bpp2D(16, 16, br.ReadBytes(0x20));
                        Rectangle rect = new Rectangle(x, y, 16, 16);
                        PropriedadeDaFonte propriedadeDaFonte = new PropriedadeDaFonte(codigoCaractere, x, y, largura, posTable, 16, 16, ponteiroFonte);
                        propriedadeDasFontes.Add(propriedadeDaFonte);
                        g.DrawImage(fonte, rect);
                        fonte.Dispose();
                        contador++;
                        x += 16;
                        if (contador == 64)
                        {
                            x = 0;
                            y += 16;
                            contador = 0;
                        }

                        posTable += 8;
                    }
                }


            }

            _propriedadeDasFontes = propriedadeDasFontes;
            Bitmap copiaFonte = new Bitmap(fontetotal);
            _fonteCarregada = copiaFonte;
            pictureBoxFonte.Image = fontetotal;

            foreach (var item in propriedadeDasFontes)
            {
                listBoxFonteProp.Items.Add(item.Codigo.ToString("X4"));
            }

        }


        private void CarregueFonte2()
        {
            List<PropriedadeDaFonte> propriedadeDasFontes = new List<PropriedadeDaFonte>();
            ConversorDeImagens cv = new ConversorDeImagens();
            Bitmap fontetotal = new Bitmap(1024, 512);
            using (BinaryReader br = new BinaryReader(new MemoryStream(File.ReadAllBytes(@"ROM_Desmontada\arm9.bin"))))
            {
                int x = 0;
                int y = 0;


                using (Graphics g = Graphics.FromImage(fontetotal))
                {
                    int posTable = 0x553E4;
                    int contador = 0;

                    for (int i = 0; i < 849; i++)
                    {
                        br.BaseStream.Position = posTable;
                        short codigoCaractere = (short)(br.ReadInt16() ^ 0x55aa);
                        byte largura = br.ReadByte();
                        br.BaseStream.Seek(1, SeekOrigin.Current);
                        int ponteiroFonte = br.ReadInt32() - 0x2000000;

                        br.BaseStream.Position = ponteiroFonte;
                        Bitmap fonte = cv.Exporte2bpp2D(16, 14, br.ReadBytes(0x56));
                        // fonte.Save("fonte2.png");
                        Rectangle rect = new Rectangle(x, y, 16, 14);
                        PropriedadeDaFonte propriedadeDaFonte = new PropriedadeDaFonte(codigoCaractere, x, y, largura, posTable, 16, 14, ponteiroFonte);
                        propriedadeDasFontes.Add(propriedadeDaFonte);
                        g.DrawImage(fonte, rect);
                        fonte.Dispose();
                        contador++;
                        x += 16;
                        if (contador == 64)
                        {
                            x = 0;
                            y += 14;
                            contador = 0;
                        }

                        posTable += 8;
                    }
                }


            }

            _propriedadeDasFontes = propriedadeDasFontes;
            Bitmap copiaFonte = new Bitmap(fontetotal);
            _fonteCarregada = copiaFonte;
            pictureBoxFonte.Image = fontetotal;

            foreach (var item in propriedadeDasFontes)
            {
                listBoxFonteProp.Items.Add(item.Codigo.ToString("X4"));
            }

        }


        private void CarregueFonte3()
        {
            List<PropriedadeDaFonte> propriedadeDasFontes = new List<PropriedadeDaFonte>();
            ConversorDeImagens cv = new ConversorDeImagens();
            Bitmap fontetotal = new Bitmap(1024, 512);
            using (BinaryReader br = new BinaryReader(new MemoryStream(File.ReadAllBytes(@"ROM_Desmontada\arm9.bin"))))
            {
                int x = 0;
                int y = 0;


                using (Graphics g = Graphics.FromImage(fontetotal))
                {
                    int posTable = 0x5C81C;
                    int contador = 0;

                    for (int i = 0; i < 820; i++)
                    {
                        br.BaseStream.Position = posTable;
                        short codigoCaractere = (short)(br.ReadInt16() ^ 0x55aa);
                        byte largura = br.ReadByte();
                        br.BaseStream.Seek(1, SeekOrigin.Current);
                        int ponteiroFonte = br.ReadInt32() - 0x2000000;

                        br.BaseStream.Position = ponteiroFonte;
                        Bitmap fonte = cv.Exporte1bpp2D(16, 14, br.ReadBytes(0x20));
                        Rectangle rect = new Rectangle(x, y, 16, 14);
                        PropriedadeDaFonte propriedadeDaFonte = new PropriedadeDaFonte(codigoCaractere, x, y, largura, posTable, 16, 14, ponteiroFonte);
                        propriedadeDasFontes.Add(propriedadeDaFonte);
                        g.DrawImage(fonte, rect);
                        fonte.Dispose();
                        contador++;
                        x += 16;
                        if (contador == 64)
                        {
                            x = 0;
                            y += 14;
                            contador = 0;
                        }

                        posTable += 8;
                    }
                }


            }

            _propriedadeDasFontes = propriedadeDasFontes;
            Bitmap copiaFonte = new Bitmap(fontetotal);
            _fonteCarregada = copiaFonte;
            pictureBoxFonte.Image = fontetotal;

            foreach (var item in propriedadeDasFontes)
            {
                listBoxFonteProp.Items.Add(item.Codigo.ToString("X4"));
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LimparComponentes();
            EditeFonte(comboBoxFonte.SelectedItem.ToString());
            buttonExportarFonte.Enabled = true;
            buttonImportarFonte.Enabled = true;
            buttonSalvarVwf.Enabled = false;

        }

        private void LimparComponentes()
        {
            if (_propriedadeDasFontes != null)
            {
                _propriedadeDasFontes.Clear();
            }

            if (listBoxFonteProp.Items.Count > 0)
            {
                listBoxFonteProp.Items.Clear();
            }

            if (pictureBoxFonte.Image != null)
            {
                pictureBoxFonte.Image.Dispose();
            }
            if (_fonteCarregada != null)
            {
                _fonteCarregada.Dispose();
            }
        }

        private void EditeFonte(string fonteSelecionada)
        {
            if (fonteSelecionada.Contains("Fonte 1"))
            {
               // CarregueFonte1();
            }
            else if (fonteSelecionada.Contains("Fonte 2"))
            {
               // CarregueFonte2();
            }
            else if (fonteSelecionada.Contains("Fonte 3"))
            {
              //  CarregueFonte3();
            }

        }

       

        private void listBoxFonteProp_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonSalvarVwf.Enabled = true;
            AtualizarImagemCarregada(listBoxFonteProp.SelectedIndex, _propriedadeDasFontes[listBoxFonteProp.SelectedIndex].ResFonteX - 1, _propriedadeDasFontes[listBoxFonteProp.SelectedIndex].ResFonteY - 1);
            ObterFonte(_propriedadeDasFontes[listBoxFonteProp.SelectedIndex].PosicaoX, _propriedadeDasFontes[listBoxFonteProp.SelectedIndex].PosicaoY,
                _propriedadeDasFontes[listBoxFonteProp.SelectedIndex].Largura,
                _propriedadeDasFontes[listBoxFonteProp.SelectedIndex].ResFonteX,
                _propriedadeDasFontes[listBoxFonteProp.SelectedIndex].ResFonteY);
            textBoxLarguraDaFonte.Text = _propriedadeDasFontes[listBoxFonteProp.SelectedIndex].Largura + "";
        }

        private void AtualizarImagemCarregada(int indexPropidade, int larguraCaixa, int alturaCaixa)
        {
            if (pictureBoxFonte.Image != null)
            {
                pictureBoxFonte.Image.Dispose();
            }
            Bitmap copiaFonte = new Bitmap(_fonteCarregada);
            using (Graphics g = Graphics.FromImage(copiaFonte))
            {
                Pen selPen = new Pen(Color.Red);
                g.DrawRectangle(selPen, _propriedadeDasFontes[indexPropidade].PosicaoX, _propriedadeDasFontes[indexPropidade].PosicaoY, larguraCaixa, alturaCaixa);
                selPen.Dispose();
                g.Dispose();
            }
            pictureBoxFonte.Image = copiaFonte;

        }

        private void ObterFonte(int x, int y, int largura, int resX, int resY)
        {
            if (pictureBoxFonteSelecionada.Image != null)
            {
                pictureBoxFonteSelecionada.Image.Dispose();
            }
            Rectangle rectangle = new Rectangle(x, y, resX, resY);
            Bitmap fonteSelecionada = _fonteCarregada.Clone(rectangle, _fonteCarregada.PixelFormat);
            using (Graphics g = Graphics.FromImage(fonteSelecionada))
            {
                Pen selPen = new Pen(Color.Red);
                g.DrawLine(selPen, new Point(largura, 0), new Point(largura, resY));
            }

            pictureBoxFonteSelecionada.Image = ScaleImage(fonteSelecionada, 32, 32);
        }

        public static Bitmap ScaleImage(Bitmap bmp, int maxWidth, int maxHeight)
        {
            Bitmap newImage = new Bitmap(maxWidth, maxHeight);
            using (Graphics gr = Graphics.FromImage(newImage))
            {
                gr.SmoothingMode = SmoothingMode.HighQuality;
                gr.InterpolationMode = InterpolationMode.NearestNeighbor;
                gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                gr.DrawImage(bmp, new Rectangle(0, 0, maxWidth, maxHeight));
            }

            return newImage;
        }

        private void buttonDiminuirLargura_Click(object sender, EventArgs e)
        {
            if (pictureBoxFonteSelecionada.Image != null)
            {
                pictureBoxFonteSelecionada.Image.Dispose();
            }
            int valorVwf = int.Parse(textBoxLarguraDaFonte.Text);

            if (valorVwf > 0)
            {
                valorVwf -= 1;
            }

            ObterFonte(_propriedadeDasFontes[listBoxFonteProp.SelectedIndex].PosicaoX,
                _propriedadeDasFontes[listBoxFonteProp.SelectedIndex].PosicaoY, valorVwf,
                _propriedadeDasFontes[listBoxFonteProp.SelectedIndex].ResFonteX,
                _propriedadeDasFontes[listBoxFonteProp.SelectedIndex].ResFonteY);
            textBoxLarguraDaFonte.Text = valorVwf + "";

        }

        private void buttonAumentarLargura_Click(object sender, EventArgs e)
        {
            if (pictureBoxFonteSelecionada.Image != null)
            {
                pictureBoxFonteSelecionada.Image.Dispose();
            }
            int valorVwf = int.Parse(textBoxLarguraDaFonte.Text);

            if (valorVwf < 15)
            {
                valorVwf += 1;
            }

            ObterFonte(_propriedadeDasFontes[listBoxFonteProp.SelectedIndex].PosicaoX,
                _propriedadeDasFontes[listBoxFonteProp.SelectedIndex].PosicaoY,
                valorVwf,
                _propriedadeDasFontes[listBoxFonteProp.SelectedIndex].ResFonteX,
                _propriedadeDasFontes[listBoxFonteProp.SelectedIndex].ResFonteY);
            textBoxLarguraDaFonte.Text = valorVwf + "";
        }

        private void button2_Click_4(object sender, EventArgs e)
        {
            if (!Directory.Exists(@"__Imagens\Fontes\"))
            {
                Directory.CreateDirectory(@"__Imagens\Fontes\");
            }

            ExportarFonte(comboBoxFonte.SelectedItem.ToString() + ".png", _fonteCarregada);
        }

        private void ExportarFonte(string nomeDaFonte, Bitmap fonte)
        {
            fonte.Save(@"__Imagens\Fontes\" + nomeDaFonte);
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            int valorVwf = int.Parse(textBoxLarguraDaFonte.Text);
            _propriedadeDasFontes[listBoxFonteProp.SelectedIndex].Largura = valorVwf;
            SalvarVwf(_propriedadeDasFontes[listBoxFonteProp.SelectedIndex].PosTabela + 2, (byte)valorVwf);
        }

        private void SalvarVwf(int offsetLargura, byte valorSalvar)
        {
            using (BinaryWriter bw = new BinaryWriter(File.Open(@"ROM_Desmontada\arm9.bin", FileMode.Open)))
            {
                bw.BaseStream.Position = offsetLargura;
                bw.Write(valorSalvar);
            }
        }

        private void button3_Click_2(object sender, EventArgs e)
        {
            using (OpenFileDialog opf = new OpenFileDialog())
            {
                opf.Filter = "Arquivos .png (*.png)|*.png|All files (*.*)|*.*";
                if (opf.ShowDialog() == DialogResult.OK)
                {

                    Importefonte(comboBoxFonte.SelectedItem.ToString(), opf.FileName);

                }
            }


        }


        private void Importefonte(string fonteSelecionada, string dirIamgem)
        {
            if (fonteSelecionada.Contains("Fonte 1") || fonteSelecionada.Contains("Fonte 3"))
            {
                ImporteFonte1Bpp(dirIamgem);
            }
            else if (fonteSelecionada.Contains("Fonte 2"))
            {
                ImporteFonte2Bpp(dirIamgem);
            }
        }

        private void ImporteFonte1Bpp(string dirIamgem)
        {

            Bitmap fonte = new Bitmap(dirIamgem);

            if (fonte.Height != 512)
            {
                throw new Exception("A imagem possui mais ou menos que 512px de altura.");
            }

            if (fonte.Width != 1024)
            {
                throw new Exception("A imagem possui mais ou menos que 1024px de largura.");
            }

            ConversorDeImagens ci = new ConversorDeImagens();

            using (BinaryWriter bw = new BinaryWriter(File.Open(@"ROM_Desmontada\arm9.bin", FileMode.Open)))
            {
                foreach (var item in _propriedadeDasFontes)
                {
                    Rectangle rectangle = new Rectangle(item.PosicaoX, item.PosicaoY, item.ResFonteX, item.ResFonteY);
                    Bitmap fonteSelecionada = fonte.Clone(rectangle, fonte.PixelFormat);
                    byte[] imagemEm1bpp = ci.ConvertaPara1bppBitmap(fonteSelecionada);
                    fonteSelecionada.Dispose();
                    //File.WriteAllBytes("testeF.bin",imagemEm1bpp);
                    bw.BaseStream.Seek(item.PonteiroImagem, SeekOrigin.Begin);
                    bw.Write(imagemEm1bpp);
                }
            }
            LimparComponentes();
            EditeFonte(comboBoxFonte.SelectedItem.ToString());
        }

        private void ImporteFonte2Bpp(string dirIamgem)
        {

            Bitmap fonte = new Bitmap(dirIamgem);
            if (fonte.Height != 512)
            {
                throw new Exception("A imagem possui mais ou menos que 512px de altura.");
            }

            if (fonte.Width != 1024)
            {
                throw new Exception("A imagem possui mais ou menos que 1024px de largura.");
            }
            ConversorDeImagens ci = new ConversorDeImagens();

            using (BinaryWriter bw = new BinaryWriter(File.Open(@"ROM_Desmontada\arm9.bin", FileMode.Open)))
            {
                foreach (var item in _propriedadeDasFontes)
                {
                    Rectangle rectangle = new Rectangle(item.PosicaoX, item.PosicaoY, item.ResFonteX, item.ResFonteY);
                    Bitmap fonteSelecionada = fonte.Clone(rectangle, fonte.PixelFormat);
                    byte[] imagemEm2bpp = ci.ConvertaPara2bppBitmap(fonteSelecionada);
                    fonteSelecionada.Dispose();
                    bw.BaseStream.Seek(item.PonteiroImagem, SeekOrigin.Begin);
                    bw.Write(imagemEm2bpp);
                }
            }
            LimparComponentes();
            EditeFonte(comboBoxFonte.SelectedItem.ToString());
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            CarregarTabelaDeCaracteres(comboBoxTabelaCarac.SelectedItem.ToString());
        }

        private void PreencherComboBoxTabelaCarac()
        {
            comboBoxTabelaCarac.Items.Add("aai2.tbl");
            comboBoxTabelaCarac.Items.Add("aai2_descricoes.tbl");
            comboBoxTabelaCarac.Items.Add("aai2_botoes.tbl");
        }

        private void checkBoxFundoTransparenteOam_CheckedChanged(object sender, EventArgs e)
        {
            if (listBoxSpritesDoNgcr.SelectedIndex > -1)
            {
                if (checkBoxFundoTransparenteOam.Checked)
                {

                    AdicionarImagemNaPictureOam(_ncgrSpriteCarregado.ArquivoNcer.GrupoDeTabelasOam[listBoxSpritesDoNgcr.SelectedIndex].TabelaDeOams);
                }
                else
                {
                    AdicionarImagemNaPictureOam(_ncgrSpriteCarregado.ArquivoNcer.GrupoDeTabelasOam[listBoxSpritesDoNgcr.SelectedIndex].TabelaDeOams);
                }
            }

        }

        private void tabPage5_Click(object sender, EventArgs e)
        {

        }

        private bool _adicionouOam = false;
        private byte[] _imagemNovaTemporaria;
        private void button2_Click_5(object sender, EventArgs e)
        {
            ushort resX = Convert.ToUInt16(comboBoxResolucaoOamAdicionar.SelectedItem.ToString().Split('x')[0]);
            ushort resY = Convert.ToUInt16(comboBoxResolucaoOamAdicionar.SelectedItem.ToString().Split('x')[1]);
            //  _ncgrSpriteCarregado.ArquivoNcer.GrupoDeTabelasOam[listBoxSpritesDoNgcr.SelectedIndex].TabelaDeOams.Reverse();
            _ncgrSpriteCarregado.ArquivoNcer.GrupoDeTabelasOam[listBoxSpritesDoNgcr.SelectedIndex].TabelaDeOams.Add(new Oam(_ncgrSpriteCarregado.ArquivoNcer.GrupoDeTabelasOam[listBoxSpritesDoNgcr.SelectedIndex].TabelaDeOams.First(),
                resX,
               resY));
            _ncgrSpriteCarregado.ArquivoNcer.GrupoDeTabelasOam[listBoxSpritesDoNgcr.SelectedIndex].TabelaDeOams.Last().X = 0;
            _ncgrSpriteCarregado.ArquivoNcer.GrupoDeTabelasOam[listBoxSpritesDoNgcr.SelectedIndex].TabelaDeOams.Last().Y = 0;

            bool borda = checkBoxAtivarBordas.Checked;
            bool transparete = checkBoxFundoTransparenteOam.Checked;
            int indexSpritesNcgr = listBoxSpritesDoNgcr.SelectedIndex;
            listBoxSpritesDoNgcr.SelectedIndex = -1;
            listBoxSpritesDoNgcr.SelectedIndex = indexSpritesNcgr;
            checkBoxEditarPosicao.Checked = true;
            checkBoxAtivarBordas.Checked = borda;
            checkBoxFundoTransparenteOam.Checked = transparete;
            _adicionouOam = true;

            byte[] tmp;

            if (_imagemNovaTemporaria != null)
            {


                if (_ncgrSpriteCarregado.Bpp == Bpp.bpp4)
                {
                    tmp = new byte[_imagemNovaTemporaria.Length + (resX * resY / 2)];

                }
                else
                {
                    tmp = new byte[_imagemNovaTemporaria.Length + (resX * resY)];

                }

                Array.Copy(_imagemNovaTemporaria, tmp, _imagemNovaTemporaria.Length);
                _imagemNovaTemporaria = tmp;


            }
            else
            {
                if (_ncgrSpriteCarregado.Bpp == Bpp.bpp4)
                {
                    _imagemNovaTemporaria = new byte[_ncgrSpriteCarregado.ImagemEmBytes.Length + (resX * resY / 2)];


                }
                else
                {
                    _imagemNovaTemporaria = new byte[_ncgrSpriteCarregado.ImagemEmBytes.Length + resX * resY];
                }

                Array.Copy(_ncgrSpriteCarregado.ImagemEmBytes, _imagemNovaTemporaria, _ncgrSpriteCarregado.ImagemEmBytes.Length);

            }

            int valorParaCalcularTiuleId = 0;

            if (_ncgrSpriteCarregado.Bpp == Bpp.bpp4)
            {
                valorParaCalcularTiuleId = _imagemNovaTemporaria.Length - (resX * resY / 2);
            }
            else
            {
                valorParaCalcularTiuleId = _imagemNovaTemporaria.Length - (resX * resY);
            }
            int tileId = valorParaCalcularTiuleId / 128;
            ushort tileIdNovo = (ushort)(_ncgrSpriteCarregado.ArquivoNcer.GrupoDeTabelasOam[listBoxSpritesDoNgcr.SelectedIndex].TabelaDeOams.Last()._atributosOBJ2 -
                ((int)_ncgrSpriteCarregado.ArquivoNcer.GrupoDeTabelasOam[listBoxSpritesDoNgcr.SelectedIndex].TabelaDeOams.Last()._atributosOBJ2 & 0x3FF) + tileId);
            _ncgrSpriteCarregado.ArquivoNcer.
                GrupoDeTabelasOam[listBoxSpritesDoNgcr.SelectedIndex]
                .TabelaDeOams.Last()._atributosOBJ2 = tileIdNovo;

            _ncgrSpriteCarregado.ArquivoNcer.GrupoDeTabelasOam[listBoxSpritesDoNgcr.SelectedIndex].TabelaDeOams.Last();

            int obt0Backup = _ncgrSpriteCarregado.ArquivoNcer.GrupoDeTabelasOam[listBoxSpritesDoNgcr.SelectedIndex].TabelaDeOams.Last()._atributosOBJ0;
            int obt1Backup = _ncgrSpriteCarregado.ArquivoNcer.GrupoDeTabelasOam[listBoxSpritesDoNgcr.SelectedIndex].TabelaDeOams.Last()._atributosOBJ1;
            int objShape = 0;
            int objSize = 0;

            if (resX == resY)
            {
                objShape = 0;
                switch (resX)
                {
                    case 8:
                        objSize = 0;
                        break;
                    case 16:
                        objSize = 1;
                        break;
                    case 32:
                        objSize = 2;
                        break;
                    case 64:
                        objSize = 3;
                        break;
                    default:
                        break;
                }
            }
            else if (resX > resY)
            {
                objShape = 1;
                if (resX == 16)
                {
                    objSize = 0;
                }
                else if (resX == 32)
                {
                    if (resY == 8)
                    {
                        objSize = 1;
                    }
                    else
                    {
                        objSize = 2;
                    }
                }
                else
                {
                    objSize = 3;
                }
            }
            else
            {
                switch (resY)
                {
                    case 16:
                        objSize = 0;
                        break;
                    case 32:
                        if (resX == 8)
                        {
                            objSize = 1;
                        }
                        else
                        {
                            objSize = 2;
                        }

                        break;
                    case 64:
                        objSize = 3;
                        break;
                    default:
                        break;
                }

            }

            obt0Backup = obt0Backup & 0x3FF;
            obt0Backup = obt0Backup | (objShape << 14);

            obt1Backup &= ~(3 << 14);
            obt1Backup = obt0Backup | (objSize << 14);

            _ncgrSpriteCarregado.ArquivoNcer.GrupoDeTabelasOam[listBoxSpritesDoNgcr.SelectedIndex].TabelaDeOams.Last()._atributosOBJ0 = (ushort)obt0Backup;
            _ncgrSpriteCarregado.ArquivoNcer.GrupoDeTabelasOam[listBoxSpritesDoNgcr.SelectedIndex].TabelaDeOams.Last()._atributosOBJ1 = (ushort)obt1Backup;
            //  _ncgrSpriteCarregado.ArquivoNcer.GrupoDeTabelasOam[listBoxSpritesDoNgcr.SelectedIndex].TabelaDeOams.Reverse();

        }

        private void checkBoxNumerosOam_CheckedChanged(object sender, EventArgs e)
        {
            if (listBoxSpritesDoNgcr.SelectedIndex > -1)
            {
                if (checkBoxNumerosOam.Checked)
                {
                    AdicionarImagemNaPictureOam(_ncgrSpriteCarregado.ArquivoNcer.GrupoDeTabelasOam[listBoxSpritesDoNgcr.SelectedIndex].TabelaDeOams);
                }
                else
                {
                    AdicionarImagemNaPictureOam(_ncgrSpriteCarregado.ArquivoNcer.GrupoDeTabelasOam[listBoxSpritesDoNgcr.SelectedIndex].TabelaDeOams);
                }
            }

        }

        private void buttonSalvarModifcaoesOam_Click(object sender, EventArgs e)
        {


            string argumentosImg = listaCarregadaDeSprites[listBoxSprites.SelectedIndex];
            string dirNcer = "";
            string dirNcgr = "";

            if (argumentosImg.Split(',')[0].Contains(".bin"))
            {
                string dirBase = argumentosImg.Split(',')[0].Replace(".bin", "");
                dirNcer = dirBase + "\\" + Path.GetFileName(argumentosImg.Split(',')[0]).Replace(".bin", ".ncer");
                dirNcgr = dirBase + "\\" + Path.GetFileName(argumentosImg.Split(',')[0]).Replace(".bin", ".ncgr");
            }
            else
            {
                dirNcgr = argumentosImg.Split(',')[0];
                dirNcer = argumentosImg.Split(',')[2];
            }

            _ncgrSpriteCarregado.ArquivoNcer.SalvarNcer(dirNcer);

            if (_adicionouOam)
            {
                _ncgrSpriteCarregado.GerarNovoNgcrParaOamAdicionado(dirNcgr, _imagemNovaTemporaria);
            }

            if (argumentosImg.Split(',')[0].Contains(".bin"))
            {
                AaiBin.ImporteBin(argumentosImg.Split(',')[0].Replace(".bin", ""), argumentosImg.Split(',')[0]);
            }

            checkBoxEditarPosicao.Checked = false;
            buttonExportOam.Enabled = true;
            buttonImportOam.Enabled = true;
            buttonImportSpritesLote.Enabled = true;

            _adicionouOam = false;
            _imagemNovaTemporaria = null;

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ControlarAcesso();
        }

        private void ControlarAcesso()
        {
            switch (tabControl1.SelectedIndex)
            {

                case 1:
                    if (!_gerenciadorDeTabs.ArquivosBinarios)
                        VoltarPrimeiraAba();
                    break;
                case 2:
                    if (!_gerenciadorDeTabs.Imagens)
                        VoltarPrimeiraAba();
                    break;
                case 3:
                    if (!_gerenciadorDeTabs.Textos)
                        VoltarPrimeiraAba();
                    break;
                case 4:
                    if (!_gerenciadorDeTabs.Fontes)
                        VoltarPrimeiraAba();
                    break;
                default:
                    VoltarPrimeiraAba();
                    break;
            }

        }

        private void VoltarPrimeiraAba()
        {
            tabControl1.SelectedIndex = 0;
        }

        
    }

    public class PropriedadeDaFonte
    {
        public short Codigo { get; set; }
        public int PosicaoX { get; set; }
        public int PosicaoY { get; set; }
        public int Largura { get; set; }
        public int PosTabela { get; set; }
        public int ResFonteX { get; set; }
        public int ResFonteY { get; set; }
        public int PonteiroImagem { get; set; }

        public PropriedadeDaFonte(short codigo, int posicaoX, int posicaoY, int largura, int posTabela, int resFonteX, int resFonteY, int ponteiroImagem)
        {
            Codigo = codigo;
            PosicaoX = posicaoX;
            PosicaoY = posicaoY;
            Largura = largura;
            PosTabela = posTabela;
            ResFonteX = resFonteX;
            ResFonteY = resFonteY;
            PonteiroImagem = ponteiroImagem;
        }
    }
}
