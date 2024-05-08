using SmartSchoolboyApp.Classes;
using SmartSchoolboyApp.MVVM.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SmartSchoolboyApp.MVVM.ViewModel
{
    public class GroupViewModel : ObservableObject
    {
        #region Fields
        private List<Group> _group;
        #endregion

        #region Properties
        public List<Group> Group
        {
            get { return _group; }
            set { _group = value; OnPropertyChanged(nameof(Group)); }
        }
        #endregion

        #region Commands
        public ICommand AddEditGroupCommand { get; }
        public ICommand DeleteGroupCommand { get; }
        #endregion

        #region Constructor
        public GroupViewModel()
        {
            UpdateList();
            AddEditGroupCommand = new RelayCommand(ExecuteAddEditGroupCommand);
            DeleteGroupCommand = new RelayCommand(ExecuteDeleteGroupCommand);
        }

        private void ExecuteDeleteGroupCommand(object obj)
        {
            throw new NotImplementedException();
        }

        private void ExecuteAddEditGroupCommand(object obj)
        {
            throw new NotImplementedException();
        }

        private async void UpdateList()
        {
            Group = await App.ApiConnector.GetTAsync<List<Group>>("Groups");
        }
        #endregion
    }
}
