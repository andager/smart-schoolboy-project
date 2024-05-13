using SmartSchoolboyApp.Classes;
using SmartSchoolboyApp.MVVM.Core;
using SmartSchoolboyApp.MVVM.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSchoolboyApp.MVVM.ViewModel
{
    public class CourseViewModel : ObservableObject
    {
        #region Fields
        private List<Course> _courses;
        private RelayCommand _addEditCourse;
        private RelayCommand _deleteCourse;
        private Course _selectedCourse;
        private bool _isLoading;
        #endregion

        #region Properties
        public bool IsLoading
        {
            get { return _isLoading; }
            set { _isLoading = value; OnPropertyChanged(nameof(IsLoading)); }
        }
        public List<Course> Courses
        {
            get { return _courses; }
            set { _courses = value; OnPropertyChanged(nameof(Courses)); }
        }
        public Course SelectedCourse
        {
            get { return _selectedCourse; }
            set { _selectedCourse = value; OnPropertyChanged(nameof(SelectedCourse)); }
        }
        #endregion

        #region Commands
        public RelayCommand AddEditCourseCommand
        {
            get
            {
                return _addEditCourse ?? new RelayCommand(obj =>
                {
                    AddEditCourseView addEditCourse = new AddEditCourseView(obj as Course);
                    addEditCourse.ShowDialog();
                    if (addEditCourse.IsVisible == false && addEditCourse.IsLoaded)
                        addEditCourse.Close();
                    UpdateList();
                });
            }
        }
        public RelayCommand DeleteCourseCommand
        {
            get
            {
                return _deleteCourse ?? new RelayCommand(obj =>
                {
                    var course = obj as Course;
                    if (course != null )
                    {

                    }
                });
            }
        }
        #endregion

        #region Constructor
        public CourseViewModel()
        {
            UpdateList();
        }

        private async void UpdateList()
        {
            IsLoading = true;
            Courses = await App.ApiConnector.GetTAsync<List<Course>>("Courses");
            IsLoading = false;
        }
        #endregion

    }
}
