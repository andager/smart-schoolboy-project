using SmartSchoolboyApp.Classes;
using SmartSchoolboyApp.MVVM.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSchoolboyApp.MVVM.ViewModel
{
    public class HomeGroupViewModel : ObservableObject
    {
        #region Fields
        private Group _group;
        private string _groupName;
        private string _courseName;
        private List<Student> _students;
        #endregion

        #region Properties
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
        private List<Student> StudentGroup
        {
            get { return _students; }
            set { _students = value; OnPropertyChanged(nameof(StudentGroup)); }
        }
        #endregion

        #region Commands

        #endregion

        #region Constructor
        public HomeGroupViewModel(Group group)
        {
            _group = group;
            GroupName = _group.name;
            CourseName = _group.course.name;
            StudentGroup = _group.students;
        }
        #endregion
    }
}
