using JacutemAAI2.WPF.Managers;
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
            ResourceManager.VerfyDirectories();
            InitializeComponent();
        }
    }
}
