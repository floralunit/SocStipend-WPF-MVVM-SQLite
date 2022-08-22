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

        //сохранить изменения
        private RelayCommand saveChangesClickCommand;
        public RelayCommand SaveChangesClickCommand => saveChangesClickCommand ??
                  (saveChangesClickCommand = new RelayCommand(obj =>
                  {
                      if (CurrentStudent.Id == 0)
                      {
                          if (CurrentStudent.Name == null || CurrentStudent.Name == "")
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
                              MessageBox.Show("Студент успешно добавлен! \nСоздайте для него справку, не закрывая это окно, чтобы студент отобразился в перечне справок.", "Ура!", MessageBoxButton.OK, MessageBoxImage.Information);
                          }
                      }
                      else
                      {
                          if (CurrentStudent.Name == null || CurrentStudent.Name == "")
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
                      var stipends = App.Context.Stipends.ToList();
                      StipendCollection = new ObservableCollection<Stipend>(stipends.Where(p => p.StudentId == CurrentStudent.Id).OrderBy(s => s.DtAssign));
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
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
