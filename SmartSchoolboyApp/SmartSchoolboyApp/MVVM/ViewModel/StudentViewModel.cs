using Microsoft.Office.Interop.Excel;
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
using Excel = Microsoft.Office.Interop.Excel;

namespace SmartSchoolboyApp.MVVM.ViewModel
{
    public class StudentViewModel : ObservableObject
    {
        #region Fields
        private List<Student> _students;
        private RelayCommand _addEditStudent;
        private RelayCommand _deleteStudent;
        private Student _selectedStudent;
        private bool _isLoading;
        #endregion

        #region Properties
        public List<Student> Students
        {
            get { return _students; }
            set { _students = value; OnPropertyChanged(nameof(Students)); }
        }
        public Student SelectedStudent
        {
            get { return _selectedStudent; }
            set { _selectedStudent = value; OnPropertyChanged(nameof(SelectedStudent)); }
        }
        public bool IsLoading
        {
            get { return _isLoading; }
            set { _isLoading = value; OnPropertyChanged(nameof(IsLoading)); }
        }
        #endregion

        #region Commands
        public ICommand UpdateDataCommand { get; }
        public ICommand ExportStudentCommand { get; }
        public RelayCommand AddEditStudentCommand
        {
            get
            {
                return _addEditStudent ?? new RelayCommand(obj =>
                {
                    AddEditStudentView addEditStudent = new AddEditStudentView(obj as Student);
                    addEditStudent.ShowDialog();
                    if (addEditStudent.IsVisible == false && addEditStudent.IsLoaded)
                        addEditStudent.Close();
                    ExecuteUpdateDataCommand(null);
                });
            }
        }
        public RelayCommand DeleteStudentCommand
        {
            get
            {
                return _deleteStudent ?? new RelayCommand(obj =>
                {
                    var student = obj as Student;
                    if (student != null)
                    {

                    }
                });
            }
        }
        #endregion

        #region Constructor
        public StudentViewModel()
        {
            ExecuteUpdateDataCommand(null);
            UpdateDataCommand = new RelayCommand(ExecuteUpdateDataCommand);
            ExportStudentCommand = new RelayCommand(ExecuteExportStudentCommand, CanExecuteExportStudentCommand);

            ExecuteUpdateDataCommand(null);
        }

        private async void ExecuteUpdateDataCommand(object obj)
        {
            IsLoading = true;
            Students = await App.ApiConnector.GetTAsync<List<Student>>("Students");
            IsLoading = false;
        }

        private bool CanExecuteExportStudentCommand(object obj)
        {
            if (IsLoading != true)
                return true;
            return false;
        }

        private void ExecuteExportStudentCommand(object obj)
        {
            try
            {
                using (SaveFileDialog save = new SaveFileDialog() { Filter = "Книга Excel|*.xlsx", FileName = "Ученики", ValidateNames = true })
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
                        worksheet.Cells[1, 4] = "Дата рождения";
                        worksheet.Cells[1, 5] = "Пол";
                        worksheet.Cells[1, 6] = "Номер телефона";
                        worksheet.Cells[1, 7] = "Telegram ID";

                        int r = 2;
                        foreach (var item in Students)
                        {
                            worksheet.Cells[r, 1] = item.lastName;
                            worksheet.Cells[r, 2] = item.firstName;
                            worksheet.Cells[r, 3] = item.patronymic;
                            worksheet.Cells[r, 4] = item.dateOfBirch;
                            worksheet.Cells[r, 5] = item.gender.name;
                            worksheet.Cells[r, 6] = item.numberPhone;
                            worksheet.Cells[r, 7] = item.telegramId;
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
