using SmartSchoolboyApp.Classes;
using SmartSchoolboyApp.Converters;
using SmartSchoolboyApp.MVVM.Core;
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
        public List<Course> Courses
        {
            get { return _courses; }
            set { _courses = value; OnPropertyChanged(nameof(Course)); }
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
        #endregion

        #region Commands
        public ICommand UpdateDataCommand { get; }

        #endregion

        #region Constructor
        public AddEditGroupViewModel(Group group)
        {
            _group = group;

            if (_group is null)
                WindowName = "Add Group";
            else
            {
                WindowName = "Edit Group";
                Students = _group.students;
                IndexCourse = _group.courseId - 1;
            }

            UpdateDataCommand = new RelayCommand(ExecuteUpdateDataCommand);
            ExecuteUpdateDataCommand(null);
        }

        private async void ExecuteUpdateDataCommand(object obj)
        {
            Courses = await App.ApiConnector.GetTAsync<List<Course>>("Courses");
        }
        #endregion
    }
}
