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
    public class HomeGroupViewModel : ObservableObject
    {
        #region Fields
        private Group _group;
        private ObservableObject _currentChildView;
        private string _groupName;
        private string _courseName;
        #endregion

        #region Properties
        public ObservableObject CurrentChildView
        {
            get { return _currentChildView; }
            set { _currentChildView = value; OnPropertyChanged(nameof(CurrentChildView)); }
        }
        public string GroupName
        {
            get { return _groupName; }
            set { _groupName = value; OnPropertyChanged(nameof(GroupName)); }
        }
        public string CourseName
        {
            get { return _courseName; }
            set { _courseName = value; OnPropertyChanged(nameof(CourseName)); }
        }
        #endregion

        #region Commands
        public ICommand ShowStudentCommand { get; }
        public ICommand ShowScheduleCommand { get; }
        #endregion

        #region Constructor
        public HomeGroupViewModel(Group group)
        {
            _group = group;
            GroupName = _group.name;
            CourseName = _group.course.name;

            ShowStudentCommand = new RelayCommand(ExecuteShowStudentCommand);
            ShowScheduleCommand = new RelayCommand(ExecuteShowScheduleCommand);

            ExecuteShowStudentCommand(null);
        }

        private void ExecuteShowScheduleCommand(object obj)
        {
            CurrentChildView = new ScheduleViewModel(_group);
        }

        private void ExecuteShowStudentCommand(object obj)
        {
            CurrentChildView = new StudentGroupViewModel(_group);
        }
        #endregion
    }
}
