using Microsoft.Office.Interop.Excel;
using SmartSchoolboyApp.Classes;
using SmartSchoolboyApp.MVVM.Core;
using SmartSchoolboyApp.MVVM.View;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using Excel = Microsoft.Office.Interop.Excel;

namespace SmartSchoolboyApp.MVVM.ViewModel
{
    public class SchollSubjectViewModel : ObservableObject
    {
        #region Fields
        private List<SchoolSubject> _schoolSubjects;
        private RelayCommand _addEditSchollSubject;
        private SchoolSubject _selectSubject;
        private string _search;
        private bool _isLoading;
        private bool _isSearchNull;
        #endregion

        #region Properties
        public bool IsSearchNull
        {
            get { return _isSearchNull; }
            set { _isSearchNull = value; OnPropertyChanged(nameof(IsSearchNull)); }
        }

        public List<SchoolSubject> SchoolSubjects
        {
            get { return _schoolSubjects; }
            set { _schoolSubjects = value; OnPropertyChanged(nameof(SchoolSubjects)); }
        }
        public SchoolSubject SelectSubject
        {
            get { return _selectSubject; }
            set { _selectSubject = value; OnPropertyChanged(nameof(SelectSubject)); }
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
        public ICommand SearchCommand { get; }
        public ICommand SearchNullCommnad { get; }
        public ICommand UpdateDataCommand { get; }
        public ICommand ExportSchollSubjectCommand { get; }
        public RelayCommand AddEditSchollSubjectCommand
        {
            get
            {
                return _addEditSchollSubject ?? new RelayCommand(obj =>
                {
                    var subject = obj as SchoolSubject;
                    AddEditSchoolSubjectView addEditSchool = new AddEditSchoolSubjectView(subject);
                    addEditSchool.ShowDialog();
                    ExecuteUpdateDataCommand(null);
                });
            }
            
        }
        #endregion

        #region Constructor
        public SchollSubjectViewModel()
        {
            SearchCommand = new RelayCommand(ExecuteSearchCommand, CanExecuteSearchCommand);
            SearchNullCommnad = new RelayCommand(ExecuteSearchNullCommnad, CanExecuteSearchNullCommnad);
            UpdateDataCommand = new RelayCommand(ExecuteUpdateDataCommand);
            ExportSchollSubjectCommand = new RelayCommand(ExecuteExportSchollSubjectCommand, CanExecuteExportSchollSubjectCommand);

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
                SchoolSubjects = await App.ApiConnector.GetTAsync<List<SchoolSubject>>("SchoolSubjects");
            else SchoolSubjects = await App.ApiConnector.SearchAsync<List<SchoolSubject>>("SchoolSubjects", Search);
            IsLoading = false;
        }

        private bool CanExecuteExportSchollSubjectCommand(object obj)
        {
            if (IsLoading != true)
                return true;
            return false;
        }

        private void ExecuteExportSchollSubjectCommand(object obj)
        {
            try
            {
                using (SaveFileDialog save = new SaveFileDialog() { Filter = "Книга Excel|*.xlsx", FileName = "Школьные предметы", ValidateNames = true })
                {
                    if (save.ShowDialog() == DialogResult.OK)
                    {
                        Excel.Application applicationExcel = new Excel.Application();
                        Workbook workbookExcel = applicationExcel.Workbooks.Add(XlSheetType.xlWorksheet);
                        Worksheet worksheet = (Worksheet)applicationExcel.ActiveSheet;
                        applicationExcel.Visible = false;

                        worksheet.Cells[1, 1] = "Название предмета";

                        int r = 2;
                        foreach (var item in SchoolSubjects)
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
