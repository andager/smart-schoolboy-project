using SmartSchoolboyApp.Classes;
using SmartSchoolboyApp.MVVM.Core;
using SmartSchoolboyApp.MVVM.View;
using SmartSchoolboyApp.Stores;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SmartSchoolboyApp.MVVM.ViewModel
{
    public class HomeCourseViewModel : ObservableObject
    {
        #region Fields
        private string _courseName;
        private string _teacherName;
        private RelayCommand _selectControlPlane;
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
        public ICommand UpdateDataCommand { get; }
        public RelayCommand AddEditControlPlaneCommnad
        {
            get
            {
                return _selectControlPlane ?? new RelayCommand(obj =>
                {
                    AddEditControlThemePlaneView addEditControl = new AddEditControlThemePlaneView(obj as ControlThemePlane);
                    addEditControl.ShowDialog();
                    if (addEditControl.IsVisible == false && addEditControl.IsLoaded)
                        addEditControl.Close();
                    ExecuteUpdateDataCommand(null);
                });
            }
        }
        public RelayCommand RemoveControlPlaneCommnad
        {
            get
            {
                return _selectControlPlane ?? new RelayCommand(async obj =>
                {
                    if (obj != null)
                        await App.ApiConnector.DeleteAsync("ControlThemePlanes", (obj as ControlThemePlane).id);
                    ExecuteUpdateDataCommand(null);
                });
            }
        }
        #endregion

        #region Constructor
        public HomeCourseViewModel(NavigationStore navigationStore, Course course)
        {
            UpdateDataCommand = new RelayCommand(ExecuteUpdateDataCommand);
            ExecuteUpdateDataCommand(null);
        }

        private async void ExecuteUpdateDataCommand(object obj)
        {
            ThemePlanes = await App.ApiConnector.GetTAsync<List<ControlThemePlane>>("ControlThemePlanes");
        }
        #endregion
    }
}
