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
    public class ScheduleViewModel : ObservableObject
    {
        #region Fields
        private Group _group;
        private List<Schedule> _schedules;
        private RelayCommand _selectedSchedule;
        private bool _isLoading;
        #endregion

        #region Properties
        public List<Schedule> Schedules
        {
            get { return _schedules; }
            set { _schedules = value; OnPropertyChanged(nameof(Schedules)); }
        }
        public bool IsLoading
        {
            get { return _isLoading; }
            set { _isLoading = value; OnPropertyChanged(nameof(IsLoading)); }
        }
        #endregion

        #region Commands
        public ICommand UpdateDataCommand { get; }
        public RelayCommand AddEditScheduleViewCommand
        {
            get
            {
                return _selectedSchedule ?? new RelayCommand(obj =>
                {
                    AddEditScheduleView addEditSchedule = new AddEditScheduleView(obj as Schedule, _group);
                    addEditSchedule.ShowDialog();
                    if (addEditSchedule.IsVisible == false && addEditSchedule.IsLoaded)
                        addEditSchedule.Close();
                });
            }
        }
        #endregion

        #region Constructor
        public ScheduleViewModel(Group group)
        {
            _group = group;

            UpdateDataCommand = new RelayCommand(ExecuteUpdateDataCommand);

            ExecuteUpdateDataCommand(null);
        }

        private async void ExecuteUpdateDataCommand(object obj)
        {
            IsLoading = true;
            Schedules = await App.ApiConnector.SearchAsync<List<Schedule>>("Schedules", _group.id.ToString());
            IsLoading = false;
        }
        #endregion
    }
}
