using Microsoft.EntityFrameworkCore;
using SocStipendDesktop.ViewModels;
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

namespace SocStipendDesktop.Views
{
    /// <summary>
    /// Логика взаимодействия для StudentView.xaml
    /// </summary>
    public partial class StudentView : Window
    {
        public StudentView()
        {
            InitializeComponent();
            var vm = new StudentViewModel();
            DataContext = vm;
            vm.ClosingRequest += (sender, e) => this.Close();
        }
    }
}
