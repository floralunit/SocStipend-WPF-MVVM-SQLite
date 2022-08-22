using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using SocStipendDesktop.Models;
using System.Windows;
using SocStipendDesktop.Views;
using SocStipendDesktop.Services;
using System.Text.RegularExpressions;

namespace SocStipendDesktop.ViewModels
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
                stipends = stipends.Where(p => (p.DtEnd == null || p.DtEnd >= DateTime.Now) && p.DtStop == null).ToList();
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

        //открытие окна для работы с отдельным студентом
        private RelayCommand selectedStipendClickCommand;
        public RelayCommand SelectedStipendClickCommand => selectedStipendClickCommand ??
                  (selectedStipendClickCommand = new RelayCommand(obj =>
                  {
                      var studentView = new StudentView();
                      var studentModel = studentView.DataContext as StudentViewModel;
                      studentModel.CurrentStudent = App.Context.Students.FirstOrDefault(s => s.Id == SelectedStipend.StudentId);
                      studentView.Show();
                  }));
        //обновление групп в связи с новым учебным годом
        private RelayCommand updateGloupNameCommand;
        public RelayCommand UpdateGloupNameCommand => updateGloupNameCommand ??
                  (updateGloupNameCommand = new RelayCommand(obj =>
                  {
                      var month = DateTime.Now.Month;
                      if (month >= 8 && month <= 12)
                      {
                          var result = MessageBox.Show("Обновить группу для всех студентов в связи с новым уч. годом? \nЭто действие не отменить.", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                          if (result == MessageBoxResult.Yes)
                          {
                              foreach (var student in App.Context.Students)
                              {
                                  if (student.StudentGroup != null)
                                  {
                                      var group = student.StudentGroup;
                                      var groupNew = UpdateGroup(group);
                                      student.StudentGroup = groupNew;
                                  }
                              }
                              App.Context.SaveChanges();
                              UpdateStipendColection();
                          }
                          else
                              return;
                      }
                      else
                      {
                          MessageBox.Show("Обновление группы возможно только в период с 1 августа по 31 декабря", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                          return;
                      }
                  }));
        public string UpdateGroup (string groupOld)
        {
            string groupNew = groupOld;
            var year = DateTime.Now.Year;
            year = int.Parse(year.ToString().Substring(year.ToString().Length - 2));
            try
            {
                var groupNumOld = Regex.Match(groupOld, @"\d+").Value;
                var groupYear = int.Parse(groupNumOld.Substring(groupNumOld.Length - 2));
                int kurs;
                switch (groupYear, year)
                {
                    case ( > 0, > 0) when groupYear == year:
                        kurs = 1;
                        break;

                    case ( > 0, > 0) when groupYear == (year - 1):
                        kurs = 2;
                        break;

                    case ( > 0, > 0) when groupYear == (year - 2):
                        kurs = 3;
                        break;
                    case ( > 0, > 0) when groupYear == (year - 3):
                        kurs = 4;
                        break;

                    case ( > 0, > 0) when groupYear == (year - 4):
                        kurs = 5;
                        break;
                    default:
                        groupNew = "Выпускник";
                        return groupNew;
                }
                var groupNumNew = string.Format(kurs.ToString() + groupYear.ToString());
                int pos = groupOld.IndexOf(groupNumOld);
                if (pos < 0)
                {
                    return groupNew;
                }
                groupNew = groupOld.Substring(0, pos) + groupNumNew + groupOld.Substring(pos + groupNumOld.Length);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return groupNew;
        }
        // поиск
        private RelayCommand searchTextChangedCommand;
        public RelayCommand SearchTextChangedCommand => searchTextChangedCommand ??
                  (searchTextChangedCommand = new RelayCommand(obj =>
                  {
                      UpdateStipendColection();
                  }));
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
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
