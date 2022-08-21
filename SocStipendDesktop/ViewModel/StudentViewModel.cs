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
    public class StudentViewModel : INotifyPropertyChanged
    {
        public StudentViewModel()
        {

        }
        public Stipend currentStudent;
        public Stipend CurrentStudent
        {
            get { return currentStudent; }
            set
            {
                currentStudent = value;
                OnPropertyChanged("CurrentStudent");
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
