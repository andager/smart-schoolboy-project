using Microsoft.Office.Interop.Excel;
using SmartSchoolboyApp.Classes;
using SmartSchoolboyApp.MVVM.Core;
using SmartSchoolboyApp.MVVM.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using Excel = Microsoft.Office.Interop.Excel;

namespace SmartSchoolboyApp.MVVM.ViewModel
{
    public class TeacherViewModel : ObservableObject
    {
        #region Fields
        private List<Teacher> _teachers;
        private RelayCommand _addEditTeacher;
        private RelayCommand _deleteTeacher;
        private Teacher _selectTeacher;
        private string _search;
        private bool _isLoading;
        #endregion

        #region Properties
        public List<Teacher> Teachers { get { return _teachers; } set { _teachers = value; OnPropertyChanged(nameof(Teachers)); } }
        public Teacher SelectedTeacher
        {
            get { return _selectTeacher; }
            set { _selectTeacher = value; OnPropertyChanged(nameof(SelectedTeacher));}
        }
        public string Search { get { return _search; } set { _search = value; OnPropertyChanged(nameof(Search)); } }
        public bool IsLoading
        {
            get { return _isLoading;}
            set { _isLoading = value; OnPropertyChanged(nameof(IsLoading)); }
        }
        #endregion

        #region Commands
        public ICommand UpdateDataCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand ExportTeatcherCommnad { get; }
        public RelayCommand AddEditTeacherCommand
        {
            get
            {
                return _addEditTeacher ?? new RelayCommand(obj =>
                {
                    AddEditTeacherView addEdit = new AddEditTeacherView(obj as Teacher);
                    addEdit.ShowDialog();
                    if (addEdit.IsVisible == false && addEdit.IsLoaded)
                        addEdit.Close();
                    ExecuteUpdateDataCommand(null);
                });
            }
        }

        public RelayCommand RemoveCommand
        {
            get
            {
                return _deleteTeacher ?? new RelayCommand(async obj =>
                {
                    var teacher = obj as Teacher;
                    if (teacher != null)
                        await App.ApiConnector.DeleteAsync("Teachers", teacher.id);
                    ExecuteUpdateDataCommand(null);
                });
            }
        }
        #endregion

        #region Constructor
        public TeacherViewModel()
        {
            UpdateDataCommand = new RelayCommand(ExecuteUpdateDataCommand);
            SearchCommand = new RelayCommand(ExecuteSearchCommand, CanExecuteSearchCommand);
            ExportTeatcherCommnad = new RelayCommand(ExecuteExportTeatcherCommnad, CanExecuteExportTeatcherCommnad);

            ExecuteUpdateDataCommand(null);
        }

        private async void ExecuteUpdateDataCommand(object obj)
        {
            IsLoading = true;
            Teachers = await App.ApiConnector.GetTAsync<List<Teacher>>("Teachers");
            IsLoading = false;
        }

        private bool CanExecuteExportTeatcherCommnad(object obj)
        {
            if (IsLoading != true)
                return true;
            return false;
        }

        private void ExecuteExportTeatcherCommnad(object obj)
        {
            try
            {
                using (SaveFileDialog save = new SaveFileDialog() { Filter = "Книга Excel|*.xlsx", FileName = "Учителя", ValidateNames = true })
                {
                    if (save.ShowDialog() == DialogResult.OK)
                    {
                        Excel.Application applicationExcel = new Excel.Application();
                        Workbook workbookExcel = applicationExcel.Workbooks.Add(XlSheetType.xlWorksheet);
                        Worksheet worksheet = (Worksheet)applicationExcel.ActiveSheet;
                        applicationExcel.Visible = false;

                        worksheet.Cells[1, 1] = "Фамилия";
                        worksheet.Cells[1, 2] = "Имя";
                        worksheet.Cells[1, 3] = "Отчество";
                        worksheet.Cells[1, 4] = "Номер телефона";
                        worksheet.Cells[1, 5] = "Пароль";
                        worksheet.Cells[1, 6] = "Пол";
                        worksheet.Cells[1, 7] = "Дата рождения";
                        worksheet.Cells[1, 8] = "Должность";
                        worksheet.Cells[1, 9] = "Стаж работы";

                        int r = 2;
                        foreach (var item in Teachers)
                        {
                            worksheet.Cells[r, 1] = item.lastName;
                            worksheet.Cells[r, 2] = item.firstName;
                            worksheet.Cells[r, 3] = item.patronymic;
                            worksheet.Cells[r, 4] = item.numberPhone;
                            worksheet.Cells[r, 5] = item.password;
                            worksheet.Cells[r, 6] = item.gender.name;
                            worksheet.Cells[r, 7] = item.dateOfBirtch;
                            worksheet.Cells[r, 8] = item.role.name;
                            worksheet.Cells[r, 9] = item.workExperience;
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

        private bool CanExecuteSearchCommand(object obj)
        {
            if (string.IsNullOrWhiteSpace(Search))
                return false;

            else return true;
        }

        private async void ExecuteSearchCommand(object obj)
        {
            var ds = await App.ApiConnector.SearchAsync();
            throw new NotImplementedException();
        }
        #endregion

    }
}
