using SmartSchoolboyApp.Classes;
using SmartSchoolboyApp.MVVM.Core;
using SmartSchoolboyApp.Stores;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSchoolboyApp.MVVM.ViewModel
{
    public class HomeCourseViewModel : ObservableObject
    {
        #region Fields
        private string _courseName;
        private string _teacherName;
        private List<ControlThemePlane> _themePlanes;
        #endregion

        #region Properties
        public string CourseName
        {
            get { return _courseName; }
            set { _courseName = value; OnPropertyChanged(nameof(CourseName)); }
        }
        public string TeacherName
        {
            get { return _teacherName; }
            set { _teacherName = value; OnPropertyChanged(nameof(TeacherName)); }
        }
        public List<ControlThemePlane> ThemePlanes
        {
            get { return _themePlanes; }
            set { _themePlanes = value; OnPropertyChanged(nameof(ThemePlanes)); }
        }
        #endregion

        #region Commands

        #endregion

        #region Constructor
        public HomeCourseViewModel(NavigationStore navigationStore, Course course)
        {
            Update();
        }

        private async void Update()
        {
            ThemePlanes = await App.ApiConnector.GetTAsync<List<ControlThemePlane>>("ControlThemePlanes");
        }

        #endregion
    }
}
