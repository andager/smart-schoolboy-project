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
        private RelayCommand _addCourse;
        private RelayCommand _editCourse;
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
        public RelayCommand AddCourseCommand
        {
            get
            {
                return _addCourse ?? new RelayCommand(obj =>
                {
                    AddEditCourseView addEditCourse = new AddEditCourseView(null);
                    addEditCourse.ShowDialog();
                    if (addEditCourse.IsVisible == false && addEditCourse.IsLoaded)
                        addEditCourse.Close();
                    UpdateList();
                });
            }
        }
        public RelayCommand EditCourseCommand
        {
            get
            {
                return _editCourse ?? new RelayCommand(obj =>
                {
                    var course = obj as Course;
                    if (course != null)
                    {
                        AddEditCourseView addEditCourse = new AddEditCourseView(course);
                        addEditCourse.ShowDialog();
                        if (addEditCourse.IsVisible == false && addEditCourse.IsLoaded)
                            addEditCourse.Close();
                        UpdateList();
                    }
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
