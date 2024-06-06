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
    public class AddEditControlThemePlaneViewModel : ObservableObject
    {
        #region Fields
        private Course _course;
        private ControlThemePlane _themePlane;
        private string _windowName;
        private string _controlPlaneName;
        private string _controlPlaneDescription;
        private bool _isViewVisible = true;
        #endregion

        #region Properties
        public string WindowName
        {
            get { return _windowName; }
            set { _windowName = value; OnPropertyChanged(nameof(WindowName)); }
        }
        public string ControlPlaneName
        {
            get { return _controlPlaneName; }
            set { _controlPlaneName = value; OnPropertyChanged(nameof(ControlPlaneName)); }
        }
        public string ControlPlaneDescription
        {
            get { return _controlPlaneDescription; }
            set { _controlPlaneDescription = value; OnPropertyChanged(nameof(ControlPlaneDescription)); }
        }
        public bool IsViewVisible
        {
            get { return _isViewVisible; }
            set { _isViewVisible = value; OnPropertyChanged(nameof(IsViewVisible)); }
        }
        #endregion

        #region Commands
        public ICommand ThemePaneSaveCommand { get; }

        #endregion

        #region Constructor
        public AddEditControlThemePlaneViewModel(ControlThemePlane themePlane, Course course)
        {
            _themePlane = themePlane;
            _course = course;

            if (_themePlane is null)
                WindowName = "Add Control Theme plane";
            else
            {
                WindowName = "Edit Control Theme plane";
                ControlPlaneName = _themePlane.lessonName;
                ControlPlaneDescription = _themePlane.lessonDescription;
            }
            ThemePaneSaveCommand = new RelayCommand(ExecuteThemePaneSaveCommand);
            _course = course;
        }

        private async void ExecuteThemePaneSaveCommand(object obj)
        {
            string _error = String.Empty;
            if (string.IsNullOrWhiteSpace(ControlPlaneName)) _error += "\nЗаполните название темы";
            if (string.IsNullOrWhiteSpace(ControlPlaneDescription)) _error += "\nЗаполните описание темы";

            if (!string.IsNullOrWhiteSpace(_error))
            {
                ErrorView errorView = new ErrorView(_error);
                errorView.ShowDialog();
            }
            else
            {
                try
                {
                    if (_themePlane is null)
                    {
                        _themePlane = new ControlThemePlane()
                        {
                            lessonName = ControlPlaneName,
                            lessonDescription = ControlPlaneDescription
                        };
                        await App.ApiConnector.PostTAsyncE(_themePlane, "ControlThemePlanes", _course.id);
                    }
                    else
                    {
                        _themePlane = new ControlThemePlane()
                        {
                            id = _themePlane.id,
                            lessonName = ControlPlaneName,
                            lessonDescription = ControlPlaneDescription
                        };
                        await App.ApiConnector.PutTAsync(_themePlane, "ControlThemePlanes", _themePlane.id);
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
        #endregion
    }
}
