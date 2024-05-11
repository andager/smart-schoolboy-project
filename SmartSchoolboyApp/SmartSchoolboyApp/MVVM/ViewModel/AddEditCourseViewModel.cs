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
        private List<Teacher> _teachers;
        private string _courseName;
        #endregion

        #region Properties
        public string WindowName
        {
            get { return _windoWname; }
            set { _windoWname = value; OnPropertyChanged(nameof(WindowName)); }
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
        #endregion

        #region Commands
        public ICommand CourseSaveCommand { get; }
        #endregion

        #region Constructor
        Course _course;
        public AddEditCourseViewModel(Course course)
        {
            UpdateList();
            if (course is null)
            {
                WindowName = "Add Course";
            }
            else
            {
                WindowName = "Edit Course";
                CourseName = course.name;
            }
            CourseSaveCommand = new RelayCommand(ExecuteCourseSaveCommand);
        }

        private void ExecuteCourseSaveCommand(object obj)
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
                _course = new Course()
                {
                    id = _course.id,
                    name = CourseName,
                    //teacherId
                    //teacher
                    isActive = _course.isActive,
                };


            }

        }

        private async void UpdateList()
        {
            Teachers = await App.ApiConnector.GetTAsync<List<Teacher>>("Teachers");
        }

        #endregion

    }
}
