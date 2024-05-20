using Microsoft.Office.Interop.Excel;
using SmartSchoolboyApp.Classes;
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
        private ObservableObject _currentChildView;
        private List<Group> _group;
        private Group _selectedGroup;
        private RelayCommand _viewGroup;
        private RelayCommand _addEditGroup;
        private RelayCommand _deleteGroup;
        private bool _isLoading;
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
        public bool IsLoading
        {
            get { return _isLoading; }
            set { _isLoading = value; OnPropertyChanged(nameof(IsLoading)); }
        }
        #endregion

        #region Commands
        public RelayCommand DeleteGroupCommand
        {
            get
            {
                return _deleteGroup ?? new RelayCommand(async obj =>
                {
                    var group = obj as Group;
                    if (group != null)
                        await App.ApiConnector.DeleteAsync("Groups", group.id);
                    UpdateList();
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
        public ICommand ExportGroupCommand { get; }
        #endregion

        #region Constructor
        public GroupViewModel()
        {
            UpdateList();
            ExportGroupCommand = new RelayCommand(ExecuteExportGroupCommand, CanExecuteExportGroupCommand);
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
                using (SaveFileDialog save = new SaveFileDialog() { Filter = "Книга Excel|*.xlsx", ValidateNames = true })
                {
                    save.FileName = "Группы";

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

        private async void UpdateList()
        {
            IsLoading = true;
            Group = await App.ApiConnector.GetTAsync<List<Group>>("Groups");
            IsLoading = false;
        }
        #endregion
    }
}
