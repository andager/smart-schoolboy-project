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
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using Excel = Microsoft.Office.Interop.Excel;

namespace SmartSchoolboyApp.MVVM.ViewModel
{
    public class CourseViewModel : ObservableObject
    {
        #region Fields
        private List<Course> _courses;
        private RelayCommand _addEditCourse;
        private RelayCommand _deleteCourse;
        private Course _selectedCourse;
        private bool _isLoading;
        #endregion

        #region Properties
        public bool IsLoading
        {
            get { return _isLoading; }
            set { _isLoading = value; OnPropertyChanged(nameof(IsLoading)); }
        }
        public List<Course> Courses
        {
            get { return _courses; }
            set { _courses = value; OnPropertyChanged(nameof(Courses)); }
        }
        public Course SelectedCourse
        {
            get { return _selectedCourse; }
            set { _selectedCourse = value; OnPropertyChanged(nameof(SelectedCourse)); }
        }
        #endregion

        #region Commands
        public ICommand UpdateDataCommand { get; }
        public ICommand ExportExelCommand { get; }
        public RelayCommand AddEditCourseCommand
        {
            get
            {
                return _addEditCourse ?? new RelayCommand(obj =>
                {
                    AddEditCourseView addEditCourse = new AddEditCourseView(obj as Course);
                    addEditCourse.ShowDialog();
                    if (addEditCourse.IsVisible == false && addEditCourse.IsLoaded)
                        addEditCourse.Close();
                    ExecuteUpdateDataCommand(null);
                });
            }
        }
        public RelayCommand DeleteCourseCommand
        {
            get
            {
                return _deleteCourse ?? new RelayCommand(async obj =>
                {
                    var course = obj as Course;
                    if (course != null)
                        await App.ApiConnector.DeleteAsync("Courses", course.id);
                    ExecuteUpdateDataCommand(null);
                });
            }
        }
        #endregion

        #region Constructor
        public CourseViewModel()
        {
            UpdateDataCommand = new RelayCommand(ExecuteUpdateDataCommand);
            ExportExelCommand = new RelayCommand(ExecuteExportExelCommand, CanExecuteExportExelCommand);

            ExecuteUpdateDataCommand(null);
        }

        private async void ExecuteUpdateDataCommand(object obj)
        {
            IsLoading = true;
            Courses = await App.ApiConnector.GetTAsync<List<Course>>("Courses");
            IsLoading = false;
        }

        private bool CanExecuteExportExelCommand(object obj)
        {
            if (IsLoading != true)
                return true;
            return false;
        }

        private void ExecuteExportExelCommand(object obj)
        {
            try
            {
                using (SaveFileDialog save = new SaveFileDialog() { Filter = "Книга Excel|*.xlsx", FileName = "Курсы", ValidateNames = true })
                {
                    if (save.ShowDialog() == DialogResult.OK)
                    {
                        Excel.Application applicationExcel = new Excel.Application();
                        Workbook workbookExcel = applicationExcel.Workbooks.Add(XlSheetType.xlWorksheet);
                        Worksheet worksheet = (Worksheet)applicationExcel.ActiveSheet;
                        applicationExcel.Visible = false;

                        worksheet.Cells[1, 1] = "Название курса";
                        worksheet.Cells[1, 2] = "Учитель";

                        int r = 2;
                        foreach (var item in Courses)
                        {
                            worksheet.Cells[r, 1] = item.name;
                            worksheet.Cells[r, 2] = item.teacher.fullName;
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
