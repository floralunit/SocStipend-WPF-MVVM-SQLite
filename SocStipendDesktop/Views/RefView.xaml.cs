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
using System.Windows.Shapes;
using SocStipendDesktop.ViewModels;

namespace SocStipendDesktop.Views
{
    /// <summary>
    /// Логика взаимодействия для RefView.xaml
    /// </summary>
    public partial class RefView : Window
    {
        public RefView()
        {
            InitializeComponent();
            DataContext = new RefViewModel();
        }
    }
}
