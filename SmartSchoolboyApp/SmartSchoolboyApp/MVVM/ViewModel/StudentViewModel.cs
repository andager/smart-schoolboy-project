using SmartSchoolboyApp.Classes;
using SmartSchoolboyApp.MVVM.Core;
using SmartSchoolboyApp.MVVM.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSchoolboyApp.MVVM.ViewModel
{
    public class StudentViewModel : ObservableObject
    {
        #region Fields
        private List<Student> _students;
        private RelayCommand _addStudent;
        private RelayCommand _editStudent;
        private RelayCommand _deleteStudent;
        private Student _selectedStudent;
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
        #endregion

        #region Commands
        public RelayCommand AddStudentCommand
        {
            get
            {
                return _addStudent ?? new RelayCommand(obj =>
                {
                    AddEditStudentView addEditStudent = new AddEditStudentView(null);
                    addEditStudent.ShowDialog();
                });
            }
        }
        public RelayCommand EditStudentCommand
        {
            get
            {
                return _editStudent ?? new RelayCommand(obj =>
                {
                    var student = obj as Student;
                    if (student != null)
                    {
                        AddEditStudentView addEditStudent = new AddEditStudentView(student);
                        addEditStudent.ShowDialog();
                    }
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
            UpdateList();
        }

        private async void UpdateList()
        {
            Students = await App.ApiConnector.GetTAsync<List<Student>>("Students");
        }
        #endregion
    }
}
