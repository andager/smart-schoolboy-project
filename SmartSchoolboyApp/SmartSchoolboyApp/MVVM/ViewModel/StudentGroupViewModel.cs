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
    public class StudentGroupViewModel : ObservableObject
    {
        #region Fields
        private Group _group;
        private List<Student> _students;
        private RelayCommand _selectedStudent;
        #endregion

        #region Properties
        public List<Student> StudentGroup
        {
            get { return _students; }
            set { _students = value; OnPropertyChanged(nameof(StudentGroup)); }
        }
        #endregion

        #region Commands
        public RelayCommand AddEditStudentCommand
        {
            get
            {
                return _selectedStudent ?? new RelayCommand(obj =>
                {
                    AddEditStudentView addEditStudent = new AddEditStudentView(obj as Student);
                    addEditStudent.ShowDialog();

                    if (addEditStudent.IsVisible == false && addEditStudent.IsLoaded)
                        addEditStudent.Close();
                });
            }
        }
        #endregion

        #region Constructor
        public StudentGroupViewModel(Group group)
        {
            _group = group;
            StudentGroup = _group.students;
        }
        #endregion
    }
}
