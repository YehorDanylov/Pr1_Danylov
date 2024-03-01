using System;

using System.ComponentModel;

using System.Windows;

using System.Windows.Input;


namespace Pr1_Danylov
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private DateTime _birthDate;
        public DateTime BirthDate
        {
            get { return _birthDate; }
            set
            {
                _birthDate = value;
                OnPropertyChanged("BirthDate");
            }
        }

        private string _ageResult;
        public string AgeResult
        {
            get { return _ageResult; }
            set
            {
                _ageResult = value;
                OnPropertyChanged("AgeResult");
            }
        }

        private string _zodiacSignWestern;
        public string ZodiacSignWestern
        {
            get { return _zodiacSignWestern; }
            set
            {
                _zodiacSignWestern = value;
                OnPropertyChanged("ZodiacSignWestern");
            }
        }

        private string _zodiacSignChinese;
        public string ZodiacSignChinese
        {
            get { return _zodiacSignChinese; }
            set
            {
                _zodiacSignChinese = value;
                OnPropertyChanged("ZodiacSignChinese");
            }
        }

        private string _birthdayMessage;
        public string BirthdayMessage
        {
            get { return _birthdayMessage; }
            set
            {
                _birthdayMessage = value;
                OnPropertyChanged("BirthdayMessage");
            }
        }

        public ICommand CalculateAgeCommand { get; private set; }

        public MainViewModel()
        {
            CalculateAgeCommand = new RelayCommand(CalculateAge, CanCalculateAge);
        }

        private void CalculateAge(object parameter)
        {
            if (BirthDate > DateTime.Now || BirthDate.Year < DateTime.Now.Year - 135)
            {
                MessageBox.Show("Please enter a valid birth date.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            TimeSpan ageDifference = DateTime.Now - BirthDate;
            int ageInYears = (int)(ageDifference.TotalDays / 365.25);

            if (ageInYears <= 0 || ageInYears > 135)
            {
                MessageBox.Show("Please enter a valid birth date.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            AgeResult = $"Your age is {ageInYears} years.";

            if (BirthDate.Month == DateTime.Now.Month && BirthDate.Day == DateTime.Now.Day)
                BirthdayMessage = "Happy Birthday!";
            else
                BirthdayMessage = ""; 

            CalculateZodiacSigns();
        }


        private bool CanCalculateAge(object parameter)
        {
            return BirthDate != default(DateTime);
        }

        private void CalculateZodiacSigns()
        {
           
            string[] westernZodiacSigns = { "Capricorn", "Aquarius", "Pisces", "Aries", "Taurus", "Gemini", "Cancer", "Leo", "Virgo", "Libra", "Scorpio", "Sagittarius", "Capricorn" };
            int indexWestern = (BirthDate.Month - 1 + (BirthDate.Day >= 20 ? 1 : 0)) % 12;
            ZodiacSignWestern = "Western Zodiac: " + westernZodiacSigns[indexWestern];

            
            string[] chineseZodiacSigns = { "Monkey", "Rooster", "Dog", "Pig", "Rat", "Ox", "Tiger", "Rabbit", "Dragon", "Snake", "Horse", "Goat" };
            int chineseYear = BirthDate.Year;
            ZodiacSignChinese = "Chinese Zodiac: " + chineseZodiacSigns[chineseYear % 12];
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}
