using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Malashok02.Models
{
    public class Person : INotifyPropertyChanged
    {
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }
        public DateTime? BirthDate { get; }

        public bool IsAdult { get; private set; }
        public string SunSign { get; private set; }
        public string ChineseSign { get; private set; }
        public bool IsBirthday { get; private set; }

        public Person(string firstName, string lastName, string email, DateTime birthDate)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            BirthDate = birthDate;
            CalculateProperties();
        }

        public Person(string firstName, string lastName, string email)
            : this(firstName, lastName, email, default) { }

        public Person(string firstName, string lastName, DateTime birthDate)
            : this(firstName, lastName, string.Empty, birthDate) { }

        private void CalculateProperties()
        {
            if (BirthDate.HasValue)
            {
                IsAdult = CalculateAge() >= 18;
                SunSign = GetWesternZodiac();
                ChineseSign = GetChineseZodiac();
                IsBirthday = BirthDate.Value.Day == DateTime.Today.Day && BirthDate.Value.Month == DateTime.Today.Month;
            }
        }

        private int CalculateAge()
        {
            if (!BirthDate.HasValue) return 0;
            DateTime today = DateTime.Today;
            int age = today.Year - BirthDate.Value.Year;
            if (BirthDate > today.AddYears(-age)) age--;
            return age;
        }

        private string GetWesternZodiac()
        {
            int day = BirthDate.Value.Day;
            int month = BirthDate.Value.Month;

            return month switch
            {
                1 => day <= 19 ? "Capricorn" : "Aquarius",
                2 => day <= 18 ? "Aquarius" : "Pisces",
                3 => day <= 20 ? "Pisces" : "Aries",
                4 => day <= 19 ? "Aries" : "Taurus",
                5 => day <= 20 ? "Taurus" : "Gemini",
                6 => day <= 20 ? "Gemini" : "Cancer",
                7 => day <= 22 ? "Cancer" : "Leo",
                8 => day <= 22 ? "Leo" : "Virgo",
                9 => day <= 22 ? "Virgo" : "Libra",
                10 => day <= 22 ? "Libra" : "Scorpio",
                11 => day <= 21 ? "Scorpio" : "Sagittarius",
                12 => day <= 21 ? "Sagittarius" : "Capricorn",
                _ => throw new NotImplementedException()
            };
        }

        private string GetChineseZodiac()
        {
            string[] zodiacSigns = { "Monkey", "Rooster", "Dog", "Pig", "Rat", "Ox", "Tiger", "Rabbit", "Dragon", "Snake", "Horse", "Goat" };
            return zodiacSigns[BirthDate.Value.Year % 12];
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
