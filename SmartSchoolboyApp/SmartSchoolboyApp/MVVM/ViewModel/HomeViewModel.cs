﻿using FontAwesome.Sharp;
using SmartSchoolboyApp.Classes;
using SmartSchoolboyApp.MVVM.Core;
using SmartSchoolboyApp.MVVM.View;
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
        private ObservableObject _currentChildView; // Базовое представленеи
        private string _currentUserName; // ФИО активного пользователя
        private byte[] _currentUserPhoto; // Фото активного пользователя
        private string _caption; // Заголовок старници
        private IconChar _icon; // Иконка странци
        #endregion

        #region Properties
        public ObservableObject CurrentChildView
        {
            get { return _currentChildView; }
            set { _currentChildView = value; OnPropertyChanged(nameof(CurrentChildView)); }
        }
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
        public ICommand SchowScheduleViewCommand { get; } // Команда для открытия страници: Расписания
        public ICommand SchowStudentCommand { get; } // Команда для открытия страници: Учеников
        public ICommand SchowGroupViewCommand { get; } // Команда для открытия страници: Груп
        public ICommand OutUserCommand { get; } // Команда для выхода пользователя из системы
        #endregion

        #region Constructor
        public HomeViewModel()
        {
            if (App.currentUser != null)
            {
                CurrentUser = App.currentUser.fullName;
                CurrentUserPhoto = App.currentUser.teacherPhoto.photo;
            }
            // Initialize command
            SchowCourseViewCommand = new RelayCommand(ExecuteSchowCourseViewCommand);
            SchowTeacherViewCommand = new RelayCommand(ExecuteSchowTeacherViewCommand);
            SchowSchoolSubjectViewCommand = new RelayCommand(ExecuteSchowSchoolSubjectViewCommand);
            SchowScheduleViewCommand = new RelayCommand(ExecuteSchowScheduleViewCommand);
            SchowStudentCommand = new RelayCommand(ExecuteSchowStudentCommand);
            SchowGroupViewCommand = new RelayCommand(ExecuteSchowGroupViewCommand);
            OutUserCommand = new RelayCommand(ExecuteOutUserCommand);
            // Default view
            ExecuteSchowCourseViewCommand(null);
        }
        private void ExecuteSchowCourseViewCommand(object obj)
        {
            CurrentChildView = new CourseViewModel();
            Caption = "Курсы";
            _Icon = IconChar.GraduationCap;
        }

        private void ExecuteSchowTeacherViewCommand(object obj)
        {
            CurrentChildView = new TeacherViewModel();
            Caption = "Учителя";
            _Icon = IconChar.ChalkboardTeacher;
        }

        private void ExecuteSchowSchoolSubjectViewCommand(object obj)
        {
            CurrentChildView = new SchollSubjectViewModel();
            Caption = "Предметы";
            _Icon = IconChar.BookBookmark;
        }

        private void ExecuteSchowScheduleViewCommand(object obj)
        {
            CurrentChildView = new ScheduleViewModel();
            Caption = "Расписанние";
            _Icon = IconChar.Calendar;
        }

        private void ExecuteSchowStudentCommand(object obj)
        {
            CurrentChildView = new StudentViewModel();
            Caption = "Ученики";
            _Icon = IconChar.PeopleGroup;
        }

        private void ExecuteSchowGroupViewCommand(object obj)
        {
            CurrentChildView = new GroupViewModel();
            Caption = "Группы";
            _Icon = IconChar.LayerGroup;
        }

        private void ExecuteOutUserCommand(object obj)
        {

        }
        #endregion
    }
}
