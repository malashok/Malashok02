    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CommunityToolkit.Mvvm.Input;
    using System.Windows.Input;
    using System.ComponentModel;
    using Malashok02.Models;
    using System.Windows;

namespace Malshok02.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _firstName;
        private string _lastName;
        private string _email;
        private DateTime? _birthDate;
        private bool _isProcessing;
        private Person _person;

        public string FirstName
        {
            get => _firstName;
            set { _firstName = value; OnPropertyChanged(nameof(FirstName)); ValidateForm(); }
        }

        public string LastName
        {
            get => _lastName;
            set { _lastName = value; OnPropertyChanged(nameof(LastName)); ValidateForm(); }
        }

        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(nameof(Email)); ValidateForm(); }
        }

        public DateTime? BirthDate
        {
            get => _birthDate;
            set { _birthDate = value; OnPropertyChanged(nameof(BirthDate)); ValidateForm(); }
        }

        public bool IsProcessing
        {
            get => _isProcessing;
            set { _isProcessing = value; OnPropertyChanged(nameof(IsProcessing)); }
        }

        public Person Person
        {
            get => _person;
            private set { _person = value; OnPropertyChanged(nameof(Person)); }
        }

        public ICommand ProceedCommand { get; }
        public bool CanProceed { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public MainViewModel()
        {
            ProceedCommand = new RelayCommand(async () => await ProcessData(), () => CanProceed);
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ValidateForm()
        {
            CanProceed = !string.IsNullOrWhiteSpace(FirstName) &&
             !string.IsNullOrWhiteSpace(LastName) &&
             !string.IsNullOrWhiteSpace(Email) &&
             BirthDate.HasValue;
            OnPropertyChanged(nameof(CanProceed));

            ((RelayCommand)ProceedCommand).NotifyCanExecuteChanged();
        }

        private bool ValidateAge()
        {
            if (!BirthDate.HasValue) return false;
            int age = DateTime.Today.Year - BirthDate.Value.Year;
            if (BirthDate.Value.Date > DateTime.Today.AddYears(-age))
                age--;

            if (age < 0 || age > 135)
            {
                MessageBox.Show("Invalid date!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (BirthDate.Value.Day == DateTime.Today.Day && BirthDate.Value.Month == DateTime.Today.Month)
            {
                MessageBox.Show("Happy Birthday!", "Nice message", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            return true;
        }


        private async Task ProcessData()
        {
            if (!ValidateAge() || !CanProceed) return;

            IsProcessing = true;
            await Task.Delay(1000);
            Person = new Person(FirstName, LastName, Email, BirthDate.Value);
            IsProcessing = false;
        }

    }
}
