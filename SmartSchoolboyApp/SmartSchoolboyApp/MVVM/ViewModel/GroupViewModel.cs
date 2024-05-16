using SmartSchoolboyApp.Classes;
using SmartSchoolboyApp.MVVM.Core;
using SmartSchoolboyApp.MVVM.View;
using SmartSchoolboyApp.Stores;
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
        private ObservableObject _currentChildView;
        private List<Group> _group;
        private Group _selectedGroup;
        private RelayCommand _viewGroup;
        private RelayCommand _addEditGroup;
        private RelayCommand _deleteGroup;
        #endregion

        #region Properties
        public ObservableObject CurrentChildView
        {
            get { return _currentChildView; }
            set { _currentChildView = value; OnPropertyChanged(nameof(CurrentChildView)); }
        }
        public List<Group> Group
        {
            get { return _group; }
            set { _group = value; OnPropertyChanged(nameof(Group)); }
        }
        public Group SelectedGroup
        {
            get { return _selectedGroup; }
            set { _selectedGroup = value; OnPropertyChanged(nameof(SelectedGroup)); }
        }
        #endregion

        #region Commands
        public ICommand DeleteGroupCommand { get; }
        public RelayCommand AddEditGroupCommand
        {
            get
            {
                return _addEditGroup ?? new RelayCommand(obj =>
                {
                    AddEditeGroupView addEditeGroup = new AddEditeGroupView(obj as Group);
                    addEditeGroup.ShowDialog();
                    if (addEditeGroup.IsVisible == false && addEditeGroup.IsLoaded)
                        addEditeGroup.Close();
                });
            }
        }
        public RelayCommand ViewGroup
        {
            get
            {
                return _viewGroup ?? new RelayCommand(obj =>
                {
                    var group = obj as Group;
                    if (group != null)
                    {
                        CurrentChildView = new HomeGroupViewModel(group);
                    }
                });
            }
        }
        #endregion

        #region Constructor
        public GroupViewModel()
        {
            UpdateList();
            DeleteGroupCommand = new RelayCommand(ExecuteDeleteGroupCommand);
        }
        private void ExecuteDeleteGroupCommand(object obj)
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
