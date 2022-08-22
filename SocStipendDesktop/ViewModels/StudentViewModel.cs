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
        public StudentViewModel()
        {
        }
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
