using SmartSchoolboyApp.MVVM.Core;
using SmartSchoolboyApp.MVVM.Model.Repositories;
using System;
using System.Net;
using System.Security;
using System.Threading;
using System.Windows.Input;
using System.Security.Principal;

namespace SmartSchoolboyApp.MVVM.ViewModel
{
    public class LoginViewModel : ObservableObject
    {
        #region Fields
        private string _username; // номер телефона ползователя
        private SecureString _password; // пароль пользователя
        private string _errorMessage; // сообщение об ошибке
        private bool _isViewVisible = true; // видимость окна авторизации
        private bool _rememberMe;
        private bool _isLoading;
        private UserRepositories userRepositories; // иницилизация interface авторизации

        #endregion

        #region Properties
        public bool IsLoading
        {
            get { return _isLoading; }
            set { _isLoading = value; OnPropertyChanged(nameof(IsLoading)); }
        }
        public string Username // номер телефона ползователя
        {
            get
            {
                return _username;
            }

            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        public SecureString Password // пароль пользователя
        {
            get
            {
                return _password;
            }

            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public string ErrorMessage // сообщение об ошибке
        {
            get
            {
                return _errorMessage;
            }

            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public bool IsViewVisible // видимость окна авторизации
        {
            get
            {
                return _isViewVisible;
            }

            set
            {
                _isViewVisible = value;
                OnPropertyChanged(nameof(IsViewVisible));
            }
        }

        public bool RememberMe
        {
            get
            {
                return _rememberMe;
            }

            set
            {
                _rememberMe = value;
                OnPropertyChanged(nameof(RememberMe));
            }
        }

        #endregion

        #region Commands
        public ICommand LoginCommand { get; } // command авторизация
        public ICommand RememberMeCommand { get; } // command остаться в системе
        #endregion

        #region Constructor

        public LoginViewModel()
        {
            userRepositories = new UserRepositories();
            LoginCommand = new RelayCommand(ExecuteLoginCommand, CanExecuteLoginCommand);
            RememberMeCommand = new RelayCommand(ExecuteRememberMeCommand);
        }
        /// <summary>
        /// Метод проверки введеных данных для авторизации
        /// </summary>
        /// <returns>Возвращает true если данные пользователя коректны или false в случае если есть ошибки</returns>
        private bool CanExecuteLoginCommand(object obj)
        {
            bool validData;
            if (string.IsNullOrWhiteSpace(Username) || Username.Length != 11 || Password == null || Password.Length < 4)
                validData = false;
            else
                validData = true;
            return validData;
        }
        /// <summary>
        /// Метод авторизации пользователя
        /// </summary>
        private async void ExecuteLoginCommand(object obj)
        {
            IsLoading = true;
            if (await userRepositories.Authenticateuser(new NetworkCredential(Username, Password))) IsViewVisible = false;
            else ErrorMessage = "* Invalid number Phone or password";
            IsLoading = false;
        }

        private void ExecuteRememberMeCommand(object obj)
        {
            if (RememberMe == true)
            {
                Properties.Settings.Default.NumberPhone = Username;
                Properties.Settings.Default.Save();
            }
            else Properties.Settings.Default.Reset();
        }

        #endregion
    }
}
