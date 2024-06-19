using SmartSchoolboyApp.Classes;
using SmartSchoolboyApp.Commands;
using SmartSchoolboyApp.Converters;
using SmartSchoolboyApp.MVVM.Core;
using SmartSchoolboyApp.MVVM.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace SmartSchoolboyApp.MVVM.ViewModel
{
    public class AddEditGroupViewModel : ObservableObject
    {
        #region Fields
        private Group _group;
        private string _windowName;
        private bool _isViewVisible = true;
        private int _indexCourse;
        private Course _selectedCourse;
        private List<Course> _courses;
        private string _courseName;
        private List<Student> _students;
        private List<Student> studentView;
        private int _selectedIndexStudent = 0;
        private Student _selectedItemStudent;
        #endregion

        #region Properties
        public string WindowName
        {
            get { return _windowName; }
            set { _windowName = value; OnPropertyChanged(nameof(WindowName)); }
        }
        public bool IsViewVisible
        {
            get { return _isViewVisible; }
            set { _isViewVisible = value; OnPropertyChanged(nameof(IsViewVisible)); }
        }
        public int IndexCourse
        {
            get { return _indexCourse; }
            set { _indexCourse = value; OnPropertyChanged(nameof(IndexCourse)); }
        }
        public Course SelectedCourse
        {
            get { return _selectedCourse; }
            set { _selectedCourse = value; OnPropertyChanged(nameof(SelectedCourse)); }
        }
        public List<Course> Coursess
        {
            get { return _courses; }
            set { _courses = value; OnPropertyChanged(nameof(Coursess)); }
        }
        public string CourseName
        {
            get { return _courseName; }
            set { _courseName = value; OnPropertyChanged(nameof(CourseName)); }
        }
        public List<Student> Students
        {
            get { return _students; }
            set { _students = value; OnPropertyChanged(nameof(Students)); }
        }
        public List<Student> StudentView
        {
            get { return _students; }
            set { _students = value; OnPropertyChanged(nameof(StudentView)); }
        }
        public int SelectedIndexStudent
        {
            get { return _selectedIndexStudent; }
            set { _selectedIndexStudent = value; OnPropertyChanged(nameof(SelectedIndexStudent)); }
        }
        public Student SelectedItemStudent
        {
            get { return _selectedItemStudent; }
            set { _selectedItemStudent = value; OnPropertyChanged(nameof(SelectedItemStudent)); }
        }

        #endregion

        #region Commands
        public ICommand UpdateDataCommand { get; }
        public ICommand AddStudentCommnad { get; }
        public ICommand SaveCommand { get; }
        #endregion

        #region Constructor
        public AddEditGroupViewModel(Group group)
        {
            _group = group;
            ExecuteUpdateDataCommand(null);

            if (_group is null)
                WindowName = "Add Group";
            else
            {
                WindowName = "Edit Group";
                CourseName = _group.name;
                Students = _group.students;
                IndexCourse = _group.courseId - 1;
            }
            UpdateDataCommand = new RelayCommand(ExecuteUpdateDataCommand);
            AddStudentCommnad = new RelayCommand(ExecuteAddStudentCommnad);
            SaveCommand = new RelayCommand(ExecuteSaveCommand);


        }

        private async void ExecuteSaveCommand(object obj)
        {
            string _error = String.Empty;


            if (string.IsNullOrWhiteSpace(_error))
            {
                try
                {
                    if (_group is null)
                    {
                        _group = new Group()
                        {
                            name = CourseName,
                            courseId = SelectedCourse.id,
                            course = SelectedCourse,
                            isActive = true,
                            students = Students
                        };
                        await App.ApiConnector.PostTAsync(_group, "Groups");
                    }
                    else
                    {
                        _group = new Group()
                        {
                            id = _group.id,
                            name = CourseName,
                            courseId = SelectedCourse.id,
                            course = SelectedCourse,
                            isActive = true,
                            students = Students
                        };
                        await App.ApiConnector.PutTAsync(_group, "Groups", _group.id);
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

        private void ExecuteAddStudentCommnad(object obj)
        {
            if (SelectedItemStudent != null)
                Students.Add(SelectedItemStudent);
        }

        private async void ExecuteUpdateDataCommand(object obj)
        {
            Coursess = await App.ApiConnector.GetTAsync<List<Course>>("Courses");
            StudentView = await App.ApiConnector.GetTAsync<List<Student>>("Students");
        }
        #endregion
    }
}
