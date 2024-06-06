using SmartSchoolboyApp.Classes;
using SmartSchoolboyApp.MVVM.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSchoolboyApp.MVVM.ViewModel
{
    public class AttendanceViewModel : ObservableObject
    {
        #region Fields
        private Group _group;
        private List<Attendance> _attendances;
        #endregion

        #region Properties
        public List<Attendance> Attendances
        {
            get { return _attendances; }
            set { _attendances = value; OnPropertyChanged(nameof(Attendances)); }
        }
        #endregion

        #region Commands

        #endregion

        #region Constructor
        public AttendanceViewModel(Group group)
        {
            _group = group;

        }
        #endregion
    }
}
