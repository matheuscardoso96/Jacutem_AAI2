using JacutemAAI2.WPF.Gerenciadores;
using System.Windows;

namespace JacutemAAI2.WPF.Views
{
    /// <summary>
    /// Lógica interna para TelaPrincipalView.xaml
    /// </summary>
    public partial class TelaPrincipalView : Window
    {
        public TelaPrincipalView()
        {
            GerenciadorDeRecursos.VerficarFicarPastasDoPrograma();
            InitializeComponent();
        }
    }
}
