using SmartSchoolboyApp.Classes;
using SmartSchoolboyApp.MVVM.Core;
using SmartSchoolboyApp.MVVM.View;
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

            if (_schedule is null)
                WindowName = "Add Schedule";
            else
            {
                WindowName = "Edit Schedule";
                DateSchedule = _schedule.date.ToString("dd:MM:yyyy");
                StartTime = _schedule.startTime.ToString();
                EndTime = _schedule.endTime.ToString();
            }
            SaveScheduleCommnad = new RelayCommand(ExecuteSaveScheduleCommnad);
        }

        private async void ExecuteSaveScheduleCommnad(object obj)
        {
            string _error = String.Empty;
            if (string.IsNullOrWhiteSpace(DateSchedule)) _error += "Заполните дату проведения занятия";
            if (DateTime.TryParse(DateSchedule, out DateTime dateSchedule)) _error += "\nПоле даты занятия, заполнено не коректно";
            if (string.IsNullOrWhiteSpace(StartTime)) _error += "\nЗаполните время начала занятия";
            if (!TimeSpan.TryParse(StartTime, out TimeSpan timeStart)) _error += "\nПоле начала занятия, заполнено не коректно";
            if (string.IsNullOrWhiteSpace(EndTime)) _error += "\nЗаполните поле окончания занятия";
            if (!TimeSpan.TryParse(EndTime, out TimeSpan timeEnd)) _error += "\nПоле окончания занятия, заполнено не коректно";
            if (timeStart > timeEnd) _error += "\nОкончание занятия не может быть раньше начала занятия";
            if (dateSchedule >= DateTime.Today) _error += "\nЗанятие не может быть в прошлом";

            if (!string.IsNullOrWhiteSpace(_error))
            {
                ErrorView errorView = new ErrorView(_error);
                errorView.ShowDialog();
            }
            else
            {
                try
                {
                    if (_schedule is null)
                    {
                        _schedule = new Schedule()
                        {
                            groupId = _group.id,
                            date = dateSchedule,
                            startTime = timeStart,
                            endTime = timeEnd,
                            group = _group,
                        };
                        await App.ApiConnector.PostTAsync(_schedule, "Schedules");
                    }
                    else
                    {
                        _schedule = new Schedule()
                        {
                            id = _schedule.id,
                            groupId = _group.id,
                            date = dateSchedule,
                            startTime = timeStart,
                            endTime = timeEnd,
                            group = _group,
                        };
                        await App.ApiConnector.PutTAsync(_schedule, "Schedules", _schedule.id);
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
