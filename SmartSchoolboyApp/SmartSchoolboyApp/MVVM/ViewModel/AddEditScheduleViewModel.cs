using SmartSchoolboyApp.Classes;
using SmartSchoolboyApp.MVVM.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace SmartSchoolboyApp.MVVM.ViewModel
{
    public class AddEditScheduleViewModel : ObservableObject
    {
        #region Fields
        private Schedule _schedule;
        private Group _group;
        private string _windoWname;
        private bool _isViewVisible = true;
        private string _dateSchedule;
        private string _startTime;
        private string _endTime;

        #endregion

        #region Properties
        public string WindowName
        {
            get { return _windoWname; }
            set { _windoWname = value; OnPropertyChanged(nameof(WindowName)); }
        }
        public bool IsViewVisible
        {
            get { return _isViewVisible; }
            set { _isViewVisible = value; OnPropertyChanged(nameof(IsViewVisible)); }
        }
        public string DateSchedule
        {
            get { return _dateSchedule; }
            set { _dateSchedule = value; OnPropertyChanged(nameof(DateSchedule)); }
        }
        public string StartTime
        {
            get { return _startTime; }
            set { _startTime = value; OnPropertyChanged(nameof(StartTime)); }
        }
        public string EndTime
        {
            get { return _endTime; }
            set { _endTime = value; OnPropertyChanged(nameof(EndTime)); }
        }
        #endregion

        #region Commands
        public ICommand SaveScheduleCommnad { get; }
        #endregion

        #region Constructor
        public AddEditScheduleViewModel(Schedule schedule, Group group)
        {
            _schedule = schedule;
            _group = group;

            SaveScheduleCommnad = new RelayCommand(ExecuteSaveScheduleCommnad);
        }

        private void ExecuteSaveScheduleCommnad(object obj)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
