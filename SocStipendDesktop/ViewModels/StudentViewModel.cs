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

namespace SocStipendDesktop.ViewModels
{
    public class StudentViewModel : INotifyPropertyChanged
    { 
        //сохранить изменения
        private RelayCommand saveChangesClickCommand;
        public RelayCommand SaveChangesClickCommand => saveChangesClickCommand ??
                  (saveChangesClickCommand = new RelayCommand(obj =>
                  {
                      if (CurrentStudent.Id == 0)
                      {
                          if (CurrentStudent.StudentName == null || CurrentStudent.StudentName == "")
                          {
                              MessageBox.Show("Не заполнено ФИО!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                              return;
                          }
                          else if (CurrentStudent.StudentGroup == null || CurrentStudent.StudentGroup == "")
                          {
                              MessageBox.Show("Не заполнена группа!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                              return;
                          }
                          else
                          {
                              App.Context.Students.Add(CurrentStudent);
                              App.Context.SaveChanges();
                              StipendsEnabled = true;
                              MessageBox.Show("Студент успешно добавлен! \nСоздайте для него справку, не закрывая это окно, чтобы студент отобразился в перечне справок.", "Ура!", MessageBoxButton.OK, MessageBoxImage.Information);
                          }
                      }
                      else
                      {
                          if (CurrentStudent.StudentName == null || CurrentStudent.StudentName == "")
                          {
                              MessageBox.Show("Не заполнено ФИО!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                              return;
                          }
                          else if (CurrentStudent.StudentGroup == null || CurrentStudent.StudentGroup == "")
                          {
                              MessageBox.Show("Не заполнена группа!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                              return;
                          }
                          else
                          {
                              var student = App.Context.Students.FirstOrDefault(s => s.Id == CurrentStudent.Id);
                              student = CurrentStudent;
                              App.Context.SaveChanges();
                              MessageBox.Show("Информация о студенте успешно обновлена!");
                          }
                      }
                      App.Context.SaveChanges();
                  }));

        //удалить студента и его справки
        private RelayCommand studentDeleteClickCommand;
        public RelayCommand StudentDeleteClickCommand => studentDeleteClickCommand ??
                  (studentDeleteClickCommand = new RelayCommand(obj =>
                  {
                      var result = MessageBox.Show("Удалить выбранного студента и все его справки?", $"{CurrentStudent.StudentName}", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                      if (result == MessageBoxResult.Yes)
                      {
                          var student = App.Context.Students.FirstOrDefault(s => s.Id == CurrentStudent.Id);
                          var stipends = App.Context.Stipends.Where(s => s.StudentId == student.Id);
                          foreach (var stipend in stipends)
                          {
                              App.Context.Stipends.Remove(stipend);
                          }
                          App.Context.Students.Remove(student);
                          App.Context.SaveChanges();
                          MessageBox.Show($"Студент {CurrentStudent.StudentName} и все его справки были успешно удалены", "Ура!", MessageBoxButton.OK, MessageBoxImage.Information);
                          this.OnClosingRequest();
                      }
                      else
                          return;
                  }));


        //загрузка справок для студента
        private RelayCommand stipendColectionLoadedCommand;
        public RelayCommand StipendColectionLoadedCommand
        {
            get
            {
                return stipendColectionLoadedCommand ??
                  (stipendColectionLoadedCommand = new RelayCommand(obj =>
                  {
                      var stipends = App.Context.Stipends.ToList();
                      StipendCollection = new ObservableCollection<Stipend>(stipends.Where(p => p.StudentId == CurrentStudent.Id).OrderBy(s => s.DtAssign));
                  }));
            }
        }
        //создать новую справку
        private RelayCommand stipendCreateClickCommand;
        public RelayCommand StipendCreateClickCommand => stipendCreateClickCommand ??
                  (stipendCreateClickCommand = new RelayCommand(obj =>
                  {
                      var stipendView = new RefView();
                      var stipendModel = stipendView.DataContext as RefViewModel;
                      stipendModel.CurrentStipend = new Stipend();
                      stipendModel.CurrentStipend.StudentId = CurrentStudent.Id;
                      stipendModel.CurrentStipend.HasTravelCard = false;
                      stipendView.Show();
                  }));


        //редактировать справку
        private RelayCommand stipendUpdateClickCommand;
        public RelayCommand StipendUpdateClickCommand => stipendUpdateClickCommand ??
                  (stipendUpdateClickCommand = new RelayCommand(obj =>
                  {
                      if (SelectedStipend == null)
                      {
                          MessageBox.Show("Выберите справку!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
                          return;
                      }
                      else
                      {
                          var stipendView = new RefView();
                          var stipendModel = stipendView.DataContext as RefViewModel;
                          stipendModel.CurrentStipend = App.Context.Stipends.FirstOrDefault(s => s.Id == SelectedStipend.Id);
                          stipendModel.CurrentStipend.StudentId = CurrentStudent.Id;
                          stipendView.Show();
                      }
                  }));


        //удалить справку
        private RelayCommand stipendDeleteClickCommand;
        public RelayCommand StipendDeleteClickCommand => stipendDeleteClickCommand ??
                  (stipendDeleteClickCommand = new RelayCommand(obj =>
                  {
                      if (SelectedStipend == null)
                      {
                          MessageBox.Show("Выберите справку!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
                          return;
                      }
                      else
                      {
                          var result = MessageBox.Show("Удалить выбранную справку?", $"{SelectedStipend.StudentName} от {SelectedStipend.DtAssign}", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                          if (result == MessageBoxResult.Yes)
                          {
                              var stipend = App.Context.Stipends.FirstOrDefault(s => s.Id == SelectedStipend.Id);
                              App.Context.Stipends.Remove(stipend);
                              App.Context.SaveChanges();
                              MessageBox.Show($"Справка {CurrentStudent.StudentName} от {SelectedStipend.DtAssign} была успешно удалена", "Ура!", MessageBoxButton.OK, MessageBoxImage.Information);
                          }
                          else
                              return;
                      }
                  }));
        public Student currentStudent;
        public Student CurrentStudent
        {
            get { return currentStudent; }
            set
            {
                currentStudent = value;
                OnPropertyChanged("CurrentStudent");
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
        public bool stipendsEnabled;
        public bool StipendsEnabled
        {
            get { return stipendsEnabled; }
            set
            {
                stipendsEnabled = value;
                OnPropertyChanged("StipendsEnabled");
            }
        }

        public event EventHandler ClosingRequest;
        protected void OnClosingRequest()
        {
            if (this.ClosingRequest != null)
            {
                this.ClosingRequest(this, EventArgs.Empty);
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
