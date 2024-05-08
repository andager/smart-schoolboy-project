using SmartSchoolboyApp.Classes;
using SmartSchoolboyApp.MVVM.Core;
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
        #endregion

        #region Properties
        public List<Course> Courses
        {
            get { return _courses; }
            set { _courses = value; OnPropertyChanged(nameof(Courses)); }
        }
        #endregion

        #region Commands

        #endregion

        #region Constructor
        public CourseViewModel()
        {
            UpdateList();
        }

        private async void UpdateList()
        {
            Courses = await App.ApiConnector.GetTAsync<List<Course>>("Courses");
        }
        #endregion

    }
}
