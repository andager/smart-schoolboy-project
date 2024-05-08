using SmartSchoolboyApp.Classes;
using SmartSchoolboyApp.MVVM.Core;
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
        #endregion

        #region Properties
        public List<Student> Students
        {
            get { return _students; }
            set { _students = value; OnPropertyChanged(nameof(Students)); }
        }
        #endregion

        #region Commands

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
