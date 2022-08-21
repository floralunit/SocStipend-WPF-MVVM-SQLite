using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using SocStipendDesktop.Model;
using System.Windows;
using SocStipendDesktop.View;

namespace SocStipendDesktop.ViewModel
{
    public class StipendCollectionViewModel : INotifyPropertyChanged
    {
        public StipendCollectionViewModel()
        {
            StudentCheck = true;
            DtAssignCheck = true;
            ActualStipendCheck = true;
            UpdateStipendColection();
        }
        public void UpdateStipendColection()
        {
            var stipends = App.Context.Stipends.ToList();
            var students = App.Context.Students.ToList();
            foreach (Stipend item in stipends)
            {
                item.StudentName = students.FirstOrDefault(x => x.Id == item.StudentId).Name;
                item.StudentGroup = students.FirstOrDefault(x => x.Id == item.StudentId).StudentGroup;
                item.Status = students.FirstOrDefault(x => x.Id == item.StudentId).Status;
            }
            if (ActualStipendCheck == true)
            {
                stipends = stipends.Where(p => p.DtEnd == null || p.DtEnd >= DateTime.Now).ToList();
            }
            if (DateTo != null)
            {
                if (DtAssignCheck == true) stipends = stipends.Where(p => p.DtAssign != null && p.DtAssign <= DateTo).ToList();
                if (DtEndCheck == true) stipends = stipends.Where(p => p.DtEnd != null && p.DtEnd <= DateTo).ToList();
            }
            if (DateFrom != null)
            {
                if (DtAssignCheck == true) stipends = stipends.Where(p => p.DtAssign != null && p.DtAssign >= DateFrom).ToList();
                if (DtEndCheck == true) stipends = stipends.Where(p => p.DtEnd != null && p.DtEnd >= DateFrom).ToList();
            }
            if (SearchBox != null)
            {
                if (StudentCheck == true) stipends = stipends.Where(p => p.StudentName != null && p.StudentName.ToLower().Contains(SearchBox.ToLower())).ToList();
                if (GroupCheck == true) stipends = stipends.Where(p => p.StudentGroup != null && p.StudentGroup.ToLower().Contains(SearchBox.ToLower())).ToList();
            }
            StipendCollection = new ObservableCollection<Stipend>(stipends.OrderBy(p => p.StudentName));
        }
        public ObservableCollection<Stipend> stipendcol;
        public ObservableCollection<Stipend> StipendCollection
        {
            get { return stipendcol; }
            set
            {
                stipendcol = value;
                OnPropertyChanged("StipendCollection");
            }
        }
        public Stipend selectedstipend;
        public Stipend SelectedStipend
        {
            get { return selectedstipend; }
            set
            {
                selectedstipend = value;
                OnPropertyChanged("SelectedStipend");
            }
        }
        public bool _ActualStipendCheck;
        public bool ActualStipendCheck
        {
            get { return _ActualStipendCheck; }
            set
            {
                _ActualStipendCheck = value;
                OnPropertyChanged("ActualStipendCheck");
            }
        }
        public bool dtAssignCheck;
        public bool DtAssignCheck
        {
            get { return dtAssignCheck; }
            set
            {
                dtAssignCheck = value;
                OnPropertyChanged("DtAssignCheck");
            }
        }
        public bool dtEndCheck;
        public bool DtEndCheck
        {
            get { return dtEndCheck; }
            set
            {
                dtEndCheck = value;
                OnPropertyChanged("DtEndCheck");
            }
        }
        public DateTime? _DateTo;
        public DateTime? DateTo
        {
            get { return _DateTo; }
            set
            {
                _DateTo = value;
                OnPropertyChanged("DateTo");
            }
        }
        public DateTime? _DateFrom;
        public DateTime? DateFrom
        {
            get { return _DateFrom; }
            set
            {
                _DateFrom = value;
                OnPropertyChanged("DateFrom");
            }
        }
        public bool groupcheck;
        public bool GroupCheck
        {
            get { return groupcheck; }
            set
            {
                groupcheck = value;
                OnPropertyChanged("GroupCheck");
            }
        }
        public bool studentcheck;
        public bool StudentCheck
        {
            get { return studentcheck; }
            set
            {
                studentcheck = value;
                OnPropertyChanged("StudentCheck");
            }
        }
        public string searchbox;
        public string SearchBox 
        {
            get { return searchbox; }
            set
            {
                searchbox = value;
                OnPropertyChanged("SearchBox");
            }
        }

        //выбор записи
        private RelayCommand selectedStipendClickCommand;
        public RelayCommand SelectedStipendClickCommand
        {
            get
            {
                return selectedStipendClickCommand ??
                  (selectedStipendClickCommand = new RelayCommand(obj =>
                  {
                      MessageBox.Show(SelectedStipend.StudentName);
                      var studentView = new StudentView();
                      var studentModel = studentView.DataContext as StudentViewModel;
                      studentModel.CurrentStudent = SelectedStipend;
                      studentView.Show();
                  }));
            }
        }
        // поиск
        private RelayCommand searchTextChangedCommand;
        public RelayCommand SearchTextChangedCommand
        {
            get
            {
                return searchTextChangedCommand ??
                  (searchTextChangedCommand = new RelayCommand(obj =>
                  {
                      UpdateStipendColection();
                  }));
            }
        }
        // поиск по студенту
        private RelayCommand studentCheckedCommand;
        public RelayCommand StudentCheckedCommand
        {
            get
            {
                return studentCheckedCommand ??
                  (studentCheckedCommand = new RelayCommand(obj =>
                  {
                      StudentCheck = true;
                      GroupCheck = false;
                      UpdateStipendColection();
                  }));
            }
        }
        // поиск по группе
        private RelayCommand groupCheckedCommand;
        public RelayCommand GroupCheckedCommand
        {
            get
            {
                return groupCheckedCommand ??
                  (groupCheckedCommand = new RelayCommand(obj =>
                  {
                      StudentCheck = false;
                      GroupCheck = true;
                      UpdateStipendColection();
                  }));
            }
        }
        // изменение даты От
        private RelayCommand dtFromChangedCommand;
        public RelayCommand DtFromChangedCommand
        {
            get
            {
                return dtFromChangedCommand ??
                  (dtFromChangedCommand = new RelayCommand(obj =>
                  {
                      UpdateStipendColection();
                  }));
            }
        }
        // изменение даты До
        private RelayCommand dtToChangedCommand;
        public RelayCommand DtToChangedCommand
        {
            get
            {
                return dtToChangedCommand ??
                  (dtToChangedCommand = new RelayCommand(obj =>
                  {
                      UpdateStipendColection();
                  }));
            }
        }
        // поиск по дате назначения
        private RelayCommand dtAssignCheckedCommand;
        public RelayCommand DtAssignCheckedCommand
        {
            get
            {
                return dtAssignCheckedCommand ??
                  (dtAssignCheckedCommand = new RelayCommand(obj =>
                  {
                      DtAssignCheck = true;
                      DtEndCheck = false;
                      UpdateStipendColection();
                  }));
            }
        }
        // поиск по дате окончания
        private RelayCommand dtEndCheckedCommand;
        public RelayCommand DtEndCheckedCommand
        {
            get
            {
                return dtEndCheckedCommand ??
                  (dtEndCheckedCommand = new RelayCommand(obj =>
                  {
                      DtAssignCheck = false;
                      DtEndCheck = true;
                      UpdateStipendColection();
                  }));
            }
        }
        // актуальная стипендия
        private RelayCommand actualStipendCheckedCommand;
        public RelayCommand ActualStipendCheckedCommand
        {
            get
            {
                return actualStipendCheckedCommand ??
                  (actualStipendCheckedCommand = new RelayCommand(obj =>
                  {
                      UpdateStipendColection();
                  }));
            }
        }
        private RelayCommand actualStipendUncheckedCommand;
        public RelayCommand ActualStipendUncheckedCommand
        {
            get
            {
                return actualStipendUncheckedCommand ??
                  (actualStipendUncheckedCommand = new RelayCommand(obj =>
                  {
                      UpdateStipendColection();
                  }));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        //private void UpdateProducts()
        //{
        //    var products = App.Context.Product.ToList();
        //    //sort by price
        //    if (ComboSortBy.SelectedIndex == 0)
        //        products = products.OrderBy(p => p.ProductCost).ToList();
        //    else
        //        products = products.OrderByDescending(p => p.ProductCost).ToList();
        //    //skidka
        //    if (ComboDiscount.SelectedIndex == 1)
        //        products = products.Where(p => p.ProductDiscountAmount >= 0 && p.ProductDiscountAmount < 9.99).ToList();
        //    if (ComboDiscount.SelectedIndex == 2)
        //        products = products.Where(p => p.ProductDiscountAmount >= 10 && p.ProductDiscountAmount < 14.99).ToList();
        //    if (ComboDiscount.SelectedIndex == 3)
        //        products = products.Where(p => p.ProductDiscountAmount >= 15 && p.ProductDiscountAmount < 100).ToList();
        //    //поиск по названию
        //    products = products.Where(p => p.ProductName.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();
        //    foreach (Product product in products)
        //    {
        //        product.ProductManufacturerName = App.Context.Manufacturer.FirstOrDefault(p => p.Id_Manufacturer == product.ProductManufacturer).ProductManufacturer;
        //    }
        //    LViewProducts.ItemsSource = products;
        //}
    }
}
