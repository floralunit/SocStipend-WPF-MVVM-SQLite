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
    public class RefViewModel : INotifyPropertyChanged
    {
        public Stipend currentStipend;
        public Stipend CurrentStipend
        {
            get { return currentStipend; }
            set
            {
                currentStipend = value;
                OnPropertyChanged("CurrentStipend");
            }
        }

        //сохранить изменения
        private RelayCommand saveChangesClickCommand;
        public RelayCommand SaveChangesClickCommand => saveChangesClickCommand ??
                  (saveChangesClickCommand = new RelayCommand(obj =>
                  {
                      if (CurrentStipend.Id == 0)
                      {
                          if (CurrentStipend.DtAssign == null)
                          {
                              MessageBox.Show("Не заполнена дата назначения стипендии!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                              return;
                          }
                          else
                          {
                              App.Context.Stipends.Add(CurrentStipend);
                              App.Context.SaveChanges();
                              this.OnClosingRequest();
                              MessageBox.Show("Справка успешно создана!", "Ура!", MessageBoxButton.OK, MessageBoxImage.Information);
                          }
                      }
                      else
                      {
                          if (CurrentStipend.DtAssign == null)
                          {
                              MessageBox.Show("Не заполнена дата назначения стипендии!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                              return;
                          }
                          else
                          {
                              var stipend = App.Context.Stipends.FirstOrDefault(s => s.Id == CurrentStipend.Id);
                              stipend = CurrentStipend;
                              App.Context.SaveChanges();
                              this.OnClosingRequest();
                              MessageBox.Show("Информация о справке успешно обновлена!", "Ура!", MessageBoxButton.OK, MessageBoxImage.Information);
                          }
                      }
                      App.Context.SaveChanges();
                  }));
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
