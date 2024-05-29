using SmartSchoolboyApp.Classes;
using SmartSchoolboyApp.MVVM.Core;
using SmartSchoolboyApp.MVVM.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SmartSchoolboyApp.MVVM.ViewModel
{
    public class AddEditCourseViewModel : ObservableObject
    {
        #region Fields
        private string _windoWname;
        private RelayCommand _selectThemePlane;
        private List<ControlThemePlane> _controlThemes;
        private List<Teacher> _teachers;
        private int _indexTeacher;
        private Teacher _selectedTeacher;
        private string _courseName;
        private bool _isViewVisible = true;
        #endregion

        #region Properties
        public string WindowName
        {
            get { return _windoWname; }
            set { _windoWname = value; OnPropertyChanged(nameof(WindowName)); }
        }
        public List<ControlThemePlane> ControlThemes
        {
            get { return _controlThemes; }
            set { _controlThemes = value; OnPropertyChanged(nameof(ControlThemes)); }
        }
        public List<Teacher> Teachers
        {
            get { return _teachers; }
            set { _teachers = value; OnPropertyChanged(nameof(Teachers)); }
        }
        public string CourseName
        {
            get { return _courseName; }
            set { _courseName = value; OnPropertyChanged(nameof(CourseName)); }
        }
        public int IndexTeacher
        {
            get { return _indexTeacher; }
            set { _indexTeacher = value; OnPropertyChanged(nameof(IndexTeacher)); }
        }
        public Teacher SelectedTeacher
        {
            get { return _selectedTeacher; }
            set { _selectedTeacher = value; OnPropertyChanged(nameof(SelectedTeacher)); }
        }
        public bool IsViewVisible
        {
            get { return _isViewVisible; }
            set { _isViewVisible = value; OnPropertyChanged(nameof(IsViewVisible)); }
        }
        #endregion

        #region Commands
        public RelayCommand AddEditThemePlaneCommand
        {
            get
            {
                return _selectThemePlane ?? new RelayCommand(obj =>
                {
                    AddEditControlThemePlaneView themePlaneView = new AddEditControlThemePlaneView(obj as ControlThemePlane);
                    themePlaneView.ShowDialog();
                });
            }
        }
        public RelayCommand RemoveThemePlaneCommand
        {
            get
            {
                return _selectThemePlane ?? new RelayCommand(async obj =>
                {
                    await App.ApiConnector.DeleteAsync("ControlThemePlanes", (obj as ControlThemePlane).id);
                });
            }
        }
        public ICommand CourseSaveCommand { get; }
        #endregion

        #region Constructor
        Course _course;
        public AddEditCourseViewModel(Course course)
        {
            _course = course;   
            UpdateList();
            if (course is null)
            {
                WindowName = "Add Course";
            }
            else
            {
                WindowName = "Edit Course";
                IndexTeacher = course.teacherId - 1;
                CourseName = course.name;
                ControlThemes = course.controlThemePlane;
            }
            CourseSaveCommand = new RelayCommand(ExecuteCourseSaveCommand);
        }

        private async void ExecuteCourseSaveCommand(object obj)
        {
            string _error = String.Empty;
            if (string.IsNullOrWhiteSpace(CourseName)) _error += "\nЗаполните название курса";
            
            if (!string.IsNullOrWhiteSpace(_error))
            {
                ErrorView errorView = new ErrorView(_error);
                errorView.ShowDialog();
            }
            else
            {
                try
                {
                    if (_course is null)
                    {
                        _course = new Course()
                        {
                            name = CourseName,
                            teacherId = SelectedTeacher.id,
                            teacher = SelectedTeacher,
                        };
                        await App.ApiConnector.PostTAsync(_course, "Courses");
                    }
                    else
                    {
                        _course = new Course()
                        {
                            id = _course.id,
                            name = CourseName,
                            teacherId = SelectedTeacher.id,
                            teacher = SelectedTeacher,
                            isActive = _course.isActive,
                        };
                        await App.ApiConnector.PutTAsync(_course, "Courses", _course.id);
                    }
                    IsViewVisible = false;
                }
                catch (Exception ex)
                {
                    ErrorView errorView = new ErrorView(ex.Message);
                    errorView.ShowDialog();
                }
            }

        }

        private async void UpdateList()
        {
            Teachers = await App.ApiConnector.GetTAsync<List<Teacher>>("Teachers");
        }

        #endregion

    }
}
