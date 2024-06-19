using Microsoft.Win32;
using SmartSchoolboyApp.Classes;
using SmartSchoolboyApp.MVVM.Core;
using SmartSchoolboyApp.MVVM.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private string _password; // пароль пользователя
        private List<Gender> _gender; // пол пользователя
        private Gender _selectedGender;
        private int _indexGender;
        private DateTime _dateOfBirtch; // дата рождения пользователя
        private DateTime _dateStart = DateTime.Today.AddYears(-100);
        private DateTime _dateEnd = DateTime.Today.AddYears(-18);
        private List<Role> _role; // должность пользователя
        private Role _selectedRole;
        private int _indexRole;
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
        public string Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged(nameof(Password)); }
        }
        public List<Gender> Gender
        {
            get { return _gender; }
            set { _gender = value; OnPropertyChanged(nameof(Gender)); }
        }
        public Gender SelectedGender
        {
            get { return _selectedGender; }
            set { _selectedGender = value; OnPropertyChanged(nameof(SelectedGender)); }
        }
        public int IndexGender
        {
            get { return _indexGender; }
            set { _indexGender = value; OnPropertyChanged(nameof(IndexGender)); }
        }
        public DateTime DateOfBirtch
        {
            get { return _dateOfBirtch; }
            set { _dateOfBirtch = value; OnPropertyChanged(nameof(DateOfBirtch));}
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
        public List<Role> Role
        {
            get { return _role; }
            set { _role = value; OnPropertyChanged(nameof(Role)); }
        }
        public Role SelectedRole
        {
            get { return _selectedRole; }
            set { _selectedRole = value; OnPropertyChanged(nameof(SelectedRole)); }
        }
        public int IndexRole
        {
            get { return _indexRole; }
            set { _indexRole = value; OnPropertyChanged(nameof(IndexRole)); }
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
        Teacher _teacher;
        Teacherphoto _teacherhoto;
        public AddEditTeacherViewModel(Teacher teacher)
        {
            UpdateWindow();
            _teacher = teacher;
            if (teacher != null)
            {
                WindowName = "EDIT Teacher";
                LastName = teacher.lastName;
                FirstName = teacher.firstName;
                Patronymic = teacher.patronymic;
                Phone = teacher.numberPhone;
                DateOfBirtch = teacher.dateOfBirtch;
                WorkExperience = teacher.workExperience;
                IndexGender = teacher.genderId - 1;
                IndexRole = teacher.roleId - 1;
                TeacherPhoto = teacher.teacherPhoto.photo;
                _teacherhoto = teacher.teacherPhoto;
                Password = teacher.password;
                
            }
            else
            {
                WindowName = "ADD Teachers";
            }
            AddEditCommand = new RelayCommand(ExecuteAddEditCommand);
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

        private async void ExecuteAddEditCommand(object obj)
        {
            string _error = String.Empty;
            //if (string.IsNullOrWhiteSpace(LastName)) _error += "Заполните имя учителя";
            //if (string.IsNullOrWhiteSpace(FirstName)) _error += "\nЗаполните фамилию учителя";
            //if (string.IsNullOrWhiteSpace(Phone)) _error += "\nЗаполните номер телефона";
            //if (!Int32.TryParse(Phone, out int resultPhone)) _error += "\nНомер телефона введен не коректно";
            //if (string.IsNullOrWhiteSpace(Password)) _error += "\nЗаполните пароль пользователя";
            ////if (Password.Length < 4) _error += "\nДлинна пароля должна быть больше 4 символов";
            //if (string.IsNullOrWhiteSpace(WorkExperience)) _error += "\nЗаполните стаж работы";
            //if (!Double.TryParse(WorkExperience, out double resultWorkExperience)) _error += "\nСтаж работы заполнен не коректно";

            if (string.IsNullOrWhiteSpace(_error))
            {
                try
                {
                    if (_teacher is null)
                    {
                        if (_teacherhoto is null)
                        {
                            _teacherhoto = new Teacherphoto()
                            {
                                photo = TeacherPhoto
                            };
                            await App.ApiConnector.PostTAsync(_teacherhoto, "TeacherPhotos");
                        }
                        _teacher = new Teacher()
                        {
                            lastName = LastName,
                            firstName = FirstName,
                            patronymic = Patronymic,
                            numberPhone = Phone,
                            password = "111111",
                            genderId = SelectedGender.id,
                            gender = SelectedGender,
                            dateOfBirtch = DateOfBirtch,
                            roleId = SelectedRole.id,
                            role = SelectedRole,
                            workExperience = WorkExperience,
                            teacherPhotoId = _teacherhoto.id,
                            teacherPhoto = _teacherhoto
                        };
                        await App.ApiConnector.PostTAsync(_teacher, "Teachers");
                    }
                    else
                    {
                        _teacherhoto = new Teacherphoto()
                        {
                            id = _teacherhoto.id,
                            photo = TeacherPhoto
                        };
                        await App.ApiConnector.PutTAsync(_teacherhoto, "TeacherPhotos", _teacherhoto.id);
                        _teacher = new Teacher()
                        {
                            id = _teacher.id,
                            lastName = LastName,
                            firstName = FirstName,
                            patronymic = Patronymic,
                            numberPhone = Phone,
                            password = "111111",
                            genderId = SelectedGender.id,
                            gender = SelectedGender,
                            dateOfBirtch = DateOfBirtch,
                            roleId = SelectedRole.id,
                            role = SelectedRole,
                            workExperience = WorkExperience,
                            teacherPhotoId = _teacherhoto.id,
                            teacherPhoto = _teacherhoto,
                            isActive = _teacher.isActive
                        };
                        await App.ApiConnector.PutTAsync(_teacher, "Teachers", _teacher.id);
                    }
                    IsViewVisible = false;
                }
                catch (Exception ex)
                {
                    ErrorView errorView = new ErrorView(ex.Message);
                    errorView.ShowDialog();
                }
            }
            else
            {
                ErrorView errorView = new ErrorView(_error);
                errorView.ShowDialog();
            }
        }
        #endregion
    }
}
