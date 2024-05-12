using SmartSchoolboyApp.Classes;
using SmartSchoolboyApp.MVVM.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SmartSchoolboyApp.MVVM.ViewModel
{
    public class AddEditStudentViewModel : ObservableObject
    {
        private Gender _selectedGender;
        public Gender SelectedGender
        {
            get { return _selectedGender; }
            set { _selectedGender = value; OnPropertyChanged(nameof(SelectedGender)); }
        }
        #region Fields
        private string _lastName;
        private string _firstName;
        private string _patronymic;
        private DateTime _dateOfBirtch;
        private List<Gender> _genders;
        private string _numberPhone;
        private object _telegramId;
        private DateTime _dateStart = DateTime.Today.AddYears(-100);
        private DateTime _dateEnd = DateTime.Today.AddYears(-6);
        #endregion

        #region Properties
        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; OnPropertyChanged(nameof(LastName)); }
        }
        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; OnPropertyChanged(nameof(FirstName)); }
        }
        public string Patronymic
        {
            get { return _patronymic; }
            set { _patronymic = value; OnPropertyChanged(nameof(Patronymic)); }
        }
        public DateTime DateOfBirtch
        {
            get { return _dateOfBirtch; }
            set { _dateOfBirtch = value; OnPropertyChanged(nameof(DateOfBirtch)); }
        }
        public List<Gender> Genders
        {
            get { return _genders; }
            set { _genders = value; OnPropertyChanged(nameof(Genders)); }
        }
        public string NumberPhone
        {
            get { return _numberPhone; }
            set { _numberPhone = value; OnPropertyChanged(nameof(NumberPhone)); }
        }
        public object TelegramId
        {
            get { return _telegramId; }
            set { _telegramId = value; OnPropertyChanged(nameof(TelegramId)); }
        }
        public DateTime DateStart
        {
            get { return _dateStart; }
            set { _dateStart = value; OnPropertyChanged(nameof(DateStart)); }
        }
        public DateTime DateEnd
        {
            get { return _dateEnd; }
            set { _dateEnd = value; OnPropertyChanged(nameof(DateEnd)); }
        }
        #endregion

        #region Commands
        public ICommand StudentSaveCommand { get; }
        #endregion

        #region Constructor
        private Student _student;
        public AddEditStudentViewModel(Student student)
        {
            _student = student; 
            UpdateList();
            if (student is null)
            {

            }
            else
            {
                LastName = student.lastName;
                FirstName = student.firstName;
                Patronymic = student.patronymic;
                DateOfBirtch = student.dateOfBirch;
                NumberPhone = student.numberPhone;
                TelegramId = student.telegramId;
            }
            StudentSaveCommand = new RelayCommand(ExecuteStudentSaveCommand);
        }

    private async void UpdateList()
        {
            Genders = await App.ApiConnector.GetTAsync<List<Gender>>("Genders");
        }

        private async void ExecuteStudentSaveCommand(object obj)
        {
            string _error = String.Empty;
            if (string.IsNullOrWhiteSpace(LastName)) _error += "Заполните имя ученика";
            if (string.IsNullOrWhiteSpace(FirstName)) _error += "Заполните фамилию ученика";
            _student = new Student()
            {
                id = _student.id,
                lastName = LastName,
                firstName = FirstName,
                patronymic = Patronymic,
                dateOfBirch = DateOfBirtch,
                genderId = SelectedGender.id,
                gender = SelectedGender,
                numberPhone = NumberPhone,
                telegramId = TelegramId,
                isActive = _student.isActive
            };
            await App.ApiConnector.PutTAsync(_student, "Students", _student.id);
            Console.WriteLine();
        }
        #endregion
    }
}
