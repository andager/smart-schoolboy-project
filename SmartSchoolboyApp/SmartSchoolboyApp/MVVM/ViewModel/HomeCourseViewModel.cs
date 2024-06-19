using Microsoft.Office.Interop.Excel;
using SmartSchoolboyApp.Classes;
using SmartSchoolboyApp.MVVM.Core;
using SmartSchoolboyApp.MVVM.View;
using SmartSchoolboyApp.Stores;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using Excel = Microsoft.Office.Interop.Excel;

namespace SmartSchoolboyApp.MVVM.ViewModel
{
    public class HomeCourseViewModel : ObservableObject
    {
        #region Fields
        private List<Course> cccourses;
        private int _id;
        private Course _course;
        private string _courseName;
        private string _teacherName;
        private RelayCommand _selectControlPlane;
        private List<ControlThemePlane> _themePlanes;
        #endregion

        #region Properties
        public string CourseName
        {
            get { return _courseName; }
            set { _courseName = value; OnPropertyChanged(nameof(CourseName)); }
        }
        public string TeacherName
        {
            get { return _teacherName; }
            set { _teacherName = value; OnPropertyChanged(nameof(TeacherName)); }
        }
        public List<ControlThemePlane> ThemePlanes
        {
            get { return _themePlanes; }
            set { _themePlanes = value; OnPropertyChanged(nameof(ThemePlanes)); }
        }
        #endregion

        #region Commands
        public ICommand ExportExelCommand { get; }
        public ICommand UpdateDataCommand { get; }
        public RelayCommand AddEditControlPlaneCommnad
        {
            get
            {
                return _selectControlPlane ?? new RelayCommand(obj =>
                {
                    AddEditControlThemePlaneView addEditControl = new AddEditControlThemePlaneView(obj as ControlThemePlane, _course);
                    addEditControl.ShowDialog();
                    if (addEditControl.IsVisible == false && addEditControl.IsLoaded)
                        addEditControl.Close();
                });
            }
        }
        public RelayCommand RemoveControlPlaneCommnad
        {
            get
            {
                return _selectControlPlane ?? new RelayCommand(async obj =>
                {
                    if (obj != null)
                        await App.ApiConnector.DeleteAsync("ControlThemePlanes", (obj as ControlThemePlane).id);
                });
            }
        }
        #endregion

        #region Constructor
        public HomeCourseViewModel(Course course)
        {
            _course = course;
            CourseName = _course.name;
            TeacherName = _course.teacher.fullName;
            ThemePlanes = _course.controlThemePlanes;
            UpdateDataCommand = new RelayCommand(ExecuteUpdateDataCommand);
            ExportExelCommand = new RelayCommand(ExecuteExportExelCommand, CanExecuteExportExelCommand);
        }

        private bool CanExecuteExportExelCommand(object obj)
        {
            return true;
        }

        private void ExecuteExportExelCommand(object obj)
        {
            try
            {
                using (SaveFileDialog save = new SaveFileDialog() { Filter = "Книга Excel|*.xlsx", FileName = $"Курс - {CourseName}", ValidateNames = true })
                {
                    if (save.ShowDialog() == DialogResult.OK)
                    {
                        Excel.Application applicationExcel = new Excel.Application();
                        Workbook workbookExcel = applicationExcel.Workbooks.Add(XlSheetType.xlWorksheet);
                        Worksheet worksheet = (Worksheet)applicationExcel.ActiveSheet;
                        applicationExcel.Visible = false;

                        worksheet.Cells[1, 1] = "Название курса";
                        worksheet.Cells[2, 1] = CourseName;
                        worksheet.Cells[1, 2] = "Ведет";
                        worksheet.Cells[2, 2] = TeacherName;
                        worksheet.Cells[1, 3] = "Название темы";
                        worksheet.Cells[1, 4] = "Описание темы";

                        int r = 2;
                        foreach (var item in ThemePlanes)
                        {
                            worksheet.Cells[r, 3] = item.lessonName;
                            worksheet.Cells[r, 4] = item.lessonDescription;
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

        private async void ExecuteUpdateDataCommand(object obj)
        {
            _course = await App.ApiConnector.SearchAsync<Course>("Courses", _id.ToString());
            ThemePlanes = _course.controlThemePlanes;

        }
        #endregion
    }
}
