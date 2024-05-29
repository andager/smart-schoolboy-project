using FontAwesome.Sharp;
using SmartSchoolboyApp.Classes;
using SmartSchoolboyApp.Commands;
using SmartSchoolboyApp.MVVM.Core;
using SmartSchoolboyApp.MVVM.View;
using SmartSchoolboyApp.Stores;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SmartSchoolboyApp.MVVM.ViewModel
{
    public class HomeViewModel : ObservableObject
    {
        #region Fields
        private readonly NavigationStore _navigationStore;
        private string _currentUserName; // ФИО активного пользователя
        private byte[] _currentUserPhoto; // Фото активного пользователя
        private string _caption; // Заголовок старници
        private IconChar _icon; // Иконка странци
        #endregion

        #region Properties
        public ObservableObject CurrentChildView => _navigationStore.CurrentViewModel;
        public string CurrentUser
        {
            get { return _currentUserName; }
            set { _currentUserName = value; OnPropertyChanged(nameof(CurrentUser)); }
        }
        public byte[] CurrentUserPhoto
        {
            get { return _currentUserPhoto; }
            set { _currentUserPhoto = value; OnPropertyChanged(nameof(CurrentUserPhoto)); }
        }
        public string Caption
        {
            get { return _caption; }
            set { _caption = value; OnPropertyChanged(nameof(Caption)); }
        }
        public IconChar _Icon
        {
            get { return _icon; }
            set { _icon = value; OnPropertyChanged(nameof(_Icon)); }
        }
        #endregion

        #region Commands
        public ICommand SchowCourseViewCommand { get; } // Команда для открытия страници: Курсов
        public ICommand SchowTeacherViewCommand { get; } // Команда для открытия страници: Учителей
        public ICommand SchowSchoolSubjectViewCommand { get; } // Команда для открытия страници: Предметов
        public ICommand SchowStudentCommand { get; } // Команда для открытия страници: Учеников
        public ICommand SchowGroupViewCommand { get; } // Команда для открытия страници: Груп
        public ICommand OutUserCommand { get; } // Команда для выхода пользователя из системы
        #endregion

        #region Constructor
        public HomeViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;

            if (App.currentUser != null)
            {
                CurrentUser = App.currentUser.fullName;
                CurrentUserPhoto = App.currentUser.teacherPhoto.photo;
            }
            // Initialize command

            //SchowCourseViewCommand = new RelayCommand(ExecuteSchowCourseViewCommand);
            //SchowTeacherViewCommand = new RelayCommand(ExecuteSchowTeacherViewCommand);
            SchowSchoolSubjectViewCommand = new RelayCommand(ExecuteSchowSchoolSubjectViewCommand);
            SchowStudentCommand = new RelayCommand(ExecuteSchowStudentCommand);
            //SchowGroupViewCommand = new RelayCommand(ExecuteSchowGroupViewCommand);
            OutUserCommand = new RelayCommand(ExecuteOutUserCommand);

            SchowCourseViewCommand = new NavigateCommand<CourseViewModel>(navigationStore, () => new CourseViewModel(navigationStore));
            SchowTeacherViewCommand = new NavigateCommand<TeacherViewModel>(navigationStore, () => new TeacherViewModel(navigationStore));
            SchowGroupViewCommand = new NavigateCommand<GroupViewModel>(navigationStore, () => new GroupViewModel(navigationStore));

            // Default view
            ExecuteSchowCourseViewCommand(null);
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentChildView));
        }

        private void ExecuteSchowCourseViewCommand(object obj)
        {
            //_navigationStore.CurrentViewModel = new CourseViewModel(_navigationStore);
            Caption = "Курсы";
            _Icon = IconChar.GraduationCap;
        }

        private void ExecuteSchowTeacherViewCommand(object obj)
        {
            //_navigationStore.CurrentViewModel = new TeacherViewModel(_navigationStore);
            Caption = "Учителя";
            _Icon = IconChar.ChalkboardTeacher;
        }

        private void ExecuteSchowSchoolSubjectViewCommand(object obj)
        {
            _navigationStore.CurrentViewModel = new SchollSubjectViewModel();
            Caption = "Предметы";
            _Icon = IconChar.BookBookmark;
        }

        private void ExecuteSchowStudentCommand(object obj)
        {
            _navigationStore.CurrentViewModel = new StudentViewModel();
            Caption = "Ученики";
            _Icon = IconChar.PeopleGroup;
        }

        private void ExecuteSchowGroupViewCommand(object obj)
        {
            _navigationStore.CurrentViewModel = new GroupViewModel(null);
            Caption = "Группы";
            _Icon = IconChar.LayerGroup;
        }

        private void ExecuteOutUserCommand(object obj)
        {

        }
        #endregion
    }
}
