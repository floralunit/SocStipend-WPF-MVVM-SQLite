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
using Microsoft.EntityFrameworkCore;

namespace SocStipendDesktop.Views
{
    /// <summary>
    /// Логика взаимодействия для StipendCollectionView.xaml
    /// </summary>
    public partial class StipendCollectionView : Window
    {
        public StipendCollectionView()
        {
            // гарантируем, что база данных создана
            App.Context.Database.EnsureCreated();
            // загружаем данные из БД
            App.Context.Stipends.Load();
            App.Context.Students.Load();
            InitializeComponent();
            DataContext = new StipendCollectionViewModel();
        }
    }
}
