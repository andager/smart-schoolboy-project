using Microsoft.Office.Interop.Excel;
using SmartSchoolboyApp.Classes;
using SmartSchoolboyApp.Commands;
using SmartSchoolboyApp.MVVM.Core;
using SmartSchoolboyApp.MVVM.View;
using SmartSchoolboyApp.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using Excel = Microsoft.Office.Interop.Excel;

namespace SmartSchoolboyApp.MVVM.ViewModel
{
    public class GroupViewModel : ObservableObject
    {
        #region Fields
        private readonly NavigationStore _navigationStore;
        private ObservableObject _currentChildView;
        private List<Group> _group;
        private Group _selectedGroup;
        private RelayCommand _viewGroup;
        private RelayCommand _addEditGroup;
        private RelayCommand _deleteGroup;
        private string _search;
        private bool _isLoading;
        private bool _isSearchNull;
        #endregion

        #region Properties
        public ObservableObject CurrentChildView => _navigationStore.CurrentViewModel;
        public bool IsSearchNull
        {
            get { return _isSearchNull; }
            set { _isSearchNull = value; OnPropertyChanged(nameof(IsSearchNull)); }
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
        public string Search
        {
            get { return _search; }
            set { _search = value; OnPropertyChanged(nameof(Search)); }
        }
        public bool IsLoading
        {
            get { return _isLoading; }
            set { _isLoading = value; OnPropertyChanged(nameof(IsLoading)); }
        }
        #endregion

        #region Commands
        public ICommand ShowHomeGroupViewCommnad { get; }
        public RelayCommand DeleteGroupCommand
        {
            get
            {
                return _deleteGroup ?? new RelayCommand(async obj =>
                {
                    var group = obj as Group;
                    if (group != null)
                        await App.ApiConnector.DeleteAsync("Groups", group.id);
                    ExecuteUpdateDataCommand(null);
                });
            }
        }
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
                    ExecuteUpdateDataCommand(null);
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
                        _navigationStore.CurrentViewModel = new StudentViewModel();
                    }
                });
            }
        }
        public ICommand SearchCommand { get; }
        public ICommand SearchNullCommnad { get; }
        public ICommand UpdateDataCommand { get; }
        public ICommand ExportGroupCommand { get; }
        #endregion

        #region Constructor
        public GroupViewModel(NavigationStore navigationStore)
        {
            ShowHomeGroupViewCommnad = new NavigateCommand<HomeGroupViewModel>(navigationStore, () => new HomeGroupViewModel());
            SearchCommand = new RelayCommand(ExecuteSearchCommand, CanExecuteSearchCommand);
            SearchNullCommnad = new RelayCommand(ExecuteSearchNullCommnad, CanExecuteSearchNullCommnad);
            UpdateDataCommand = new RelayCommand(ExecuteUpdateDataCommand);
            ExportGroupCommand = new RelayCommand(ExecuteExportGroupCommand, CanExecuteExportGroupCommand);

            ExecuteUpdateDataCommand(null);
        }

        private bool CanExecuteSearchNullCommnad(object obj)
        {
            if (string.IsNullOrWhiteSpace(Search))
                IsSearchNull = false;
            else
                IsSearchNull = true;
            return IsSearchNull;
        }

        private void ExecuteSearchNullCommnad(object obj)
        {
            Search = null;
            ExecuteUpdateDataCommand(null);
        }

        private bool CanExecuteSearchCommand(object obj)
        {
            if (string.IsNullOrWhiteSpace(Search))
                return false;
            else return true;
        }

        private void ExecuteSearchCommand(object obj)
        {
            ExecuteUpdateDataCommand(null);
        }

        private async void ExecuteUpdateDataCommand(object obj)
        {
            IsLoading = true;
            if (string.IsNullOrWhiteSpace(Search))
                Group = await App.ApiConnector.GetTAsync<List<Group>>("Groups");
            else Group = await App.ApiConnector.SearchAsync<List<Group>>("Groups", Search);
            IsLoading = false;

        }
        private bool CanExecuteExportGroupCommand(object obj)
        {
            if (IsLoading != true)
                return true;
            return false;
        }

        private void ExecuteExportGroupCommand(object obj)
        {
            try
            {
                using (SaveFileDialog save = new SaveFileDialog() { Filter = "Книга Excel|*.xlsx", FileName = "Группы", ValidateNames = true })
                {
                    if (save.ShowDialog() == DialogResult.OK)
                    {
                        Excel.Application applicationExcel = new Excel.Application();
                        Workbook workbookExcel = applicationExcel.Workbooks.Add(XlSheetType.xlWorksheet);
                        Worksheet worksheet = (Worksheet)applicationExcel.ActiveSheet;
                        applicationExcel.Visible = false;

                        worksheet.Cells[1, 1] = "Фамилия";

                        int r = 2;
                        foreach (var item in Group)
                        {
                            worksheet.Cells[r, 1] = item.name;
                            r++;
                        }
                        workbookExcel.SaveAs(save.FileName, XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing, false, false, XlSaveAsAccessMode.xlNoChange, XlSaveConflictResolution.xlLocalSessionChanges, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                        applicationExcel.Quit();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorView errorView = new ErrorView($"Ошибка экспорта в Excel\n\n{ex.Message}");
                errorView.ShowDialog();
            }
        }
        #endregion
    }
}
