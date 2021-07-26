using JacutemAAI2.WPF.ViewModel;
using JacutemAAI2.WPF.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JacutemAAI2.WPF.Controls
{
    /// <summary>
    /// Interação lógica para Rom.xam
    /// </summary>
    public partial class Rom : UserControl
    {
        public Rom()
        {
            InitializeComponent();
            DataContext = GerenciadoEstaticoDeViewsModels.RomViewModel;
        }

      
    }
}
