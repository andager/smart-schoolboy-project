using Microsoft.Win32;
using SmartSchoolboyApp.Classes;
using SmartSchoolboyApp.MVVM.Core;
using SmartSchoolboyApp.MVVM.View;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SmartSchoolboyApp.MVVM.ViewModel
{
    public class AddEditTeacherViewModel : ObservableObject
    {
        #region Fields
        private string _windowName; // заголовок открытого окна
        private string _errorPhotoMessage; // сообщение об ошибке добавления фото
        private string _lastName; // фамилия пользователя
        private string _firstName; // имя пользователя
        private string _patronymic; // отчество пользователя
        private string _phone; // номер телефона пользователя
        private SecureString _password; // пароль пользователя
        private List<Gender> _gender; // пол пользователя
        private DateTime _dateOfBirtch; // дата рождения пользователя
        private List<Role> _role; // должность пользователя
        private string _workExperience; // стаж работы поьзователя
        private byte[] _teacherPhoto; // фото пользователя
        private string _errorMessage; // сообщение об ошибке
        private bool _isViewVisible = true; // видимость окна авторизации
        #endregion

        #region Properties
        public string WindowName
        {
            get { return _windowName; }
            set { _windowName = value; OnPropertyChanged(nameof(WindowName)); }
        }
        public string ErrorPhotoMessage
        {
            get { return _errorPhotoMessage; }
            set { _errorPhotoMessage = value; OnPropertyChanged(nameof(ErrorPhotoMessage)); }
        }
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
        public string Phone
        {
            get { return _phone; }
            set { _phone = value; OnPropertyChanged(nameof(Phone)); }
        }
        public SecureString Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged(nameof(Password)); }
        }
        public List<Gender> Gender
        {
            get { return _gender; }
            set { _gender = value; OnPropertyChanged(nameof(Gender)); }
        }
        public DateTime DateOfBirtch
        {
            get { return _dateOfBirtch; }
            set { _dateOfBirtch = value; OnPropertyChanged(nameof(DateOfBirtch));}
        }
        public List<Role> Role
        {
            get { return _role; }
            set { _role = value; OnPropertyChanged(nameof(Role)); }
        }
        public string WorkExperience
        {
            get { return _workExperience; }
            set { _workExperience = value; OnPropertyChanged(nameof(WorkExperience)); }
        }
        public byte[] TeacherPhoto
        {
            get { return _teacherPhoto; }
            set { _teacherPhoto = value; OnPropertyChanged(nameof(TeacherPhoto)); }
        }
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; OnPropertyChanged(nameof(ErrorMessage)); }
        }
        public bool IsViewVisible
        {
            get { return _isViewVisible; }
            set { _isViewVisible = value; OnPropertyChanged(nameof(IsViewVisible)); }
        }
        #endregion

        #region Command
        public ICommand AddEditCommand { get; }
        public ICommand AddPhotoCommand { get; }
        #endregion

        #region Constructor
        public AddEditTeacherViewModel(Teacher teacher)
        {
            UpdateWindow();
            if (teacher != null)
            {
                WindowName = "EDIT Teacher";
                LastName = teacher.lastName;
                FirstName = teacher.firstName;
                Patronymic = teacher.patronymic;
                Phone = teacher.numberPhone;
                
            }
            else
            {
                WindowName = "ADD Teachers";
            }
            AddEditCommand = new RelayCommand(ExecuteAddEditCommand/*, CanExecuteAddEditCommand*/);
            AddPhotoCommand = new RelayCommand(ExecuteAddPhotoCommand);
        }

        private async void UpdateWindow()
        {
            try
            {
                Gender = await App.ApiConnector.GetTAsync<List<Gender>>("Genders");
                Role = await App.ApiConnector.GetTAsync<List<Role>>("Roles");

                if (TeacherPhoto == null)
                    TeacherPhoto = File.ReadAllBytes("D:\\[A] Coding\\[A] Диплом\\SmartSchoolboyApp\\SmartSchoolboyApp\\Images\\noImage.png");
            }
            catch
            {
                ErrorMessage = "Проверьте подключение к сети";
            }
        }

        private void ExecuteAddPhotoCommand(object obj)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg";
            if (fileDialog.ShowDialog() == true)
            {
                BitmapImage image = new BitmapImage(new Uri(fileDialog.FileName));
                if (Math.Round(image.Height / image.Width, 2) == 1.33)
                {
                    TeacherPhoto = File.ReadAllBytes(fileDialog.FileName);
                    ErrorPhotoMessage = null;
                }
                else ErrorPhotoMessage = "Фото не соответствует соотношению сторон (3:4)";
            }
        }

        private bool CanExecuteAddEditCommand(object obj)
        {
            if (string.IsNullOrWhiteSpace(LastName)) return false;
            if (string.IsNullOrWhiteSpace(FirstName)) return false;
            if (string.IsNullOrWhiteSpace(Phone)) return false;
            if (!Int32.TryParse(Phone, out int resultPhone)) return false;
            if (Password == null || Password.Length < 4) return false;
            if (string.IsNullOrWhiteSpace(WorkExperience)) return false;
            if (!Double.TryParse(WorkExperience, out double resultWorkExperience)) return false;
            
            return true;
        }

        private void ExecuteAddEditCommand(object obj)
        {

        }
        #endregion
    }
}
