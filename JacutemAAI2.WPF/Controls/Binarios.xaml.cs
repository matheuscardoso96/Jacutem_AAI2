﻿using JacutemAAI2.WPF.ViewModel;
using JacutemAAI2.WPF.Managers;
using System;
using System.Collections.Generic;
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
    /// Interação lógica para Binarios.xam
    /// </summary>
    public partial class Binarios : UserControl
    {
        public Binarios()
        {
            InitializeComponent();
            //DataContext = GerenciadoEstaticoDeViewsModels.BinariosViewModel;
        }
    }
}
