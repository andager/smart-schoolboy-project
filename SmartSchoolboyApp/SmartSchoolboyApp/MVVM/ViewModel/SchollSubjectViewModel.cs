using SmartSchoolboyApp.Classes;
using SmartSchoolboyApp.MVVM.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SmartSchoolboyApp.MVVM.ViewModel
{
    public class SchollSubjectViewModel : ObservableObject
    {
        #region Fields
        private List<SchoolSubject> _schoolSubjects;
        private RelayCommand _changeCommand;
        private SchoolSubject _selectSubject;
        private bool _isLoading;
        #endregion

        #region Properties
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
        public bool IsLoading
        {
            get { return _isLoading; }
            set { _isLoading = value; OnPropertyChanged(nameof(IsLoading)); }
        }
        #endregion

        #region Commands
        public RelayCommand ChangeCommand
        {
            get
            {
                return _changeCommand ?? new RelayCommand(obj =>
                {
                    var subject = obj as SchoolSubject;
                    if (subject != null)
                    {
                        Console.WriteLine(subject.name);
                    }
                });
            }
            
        }
        #endregion

        #region Constructor
        public SchollSubjectViewModel()
        {
            UpdateList();
        }


        private async void UpdateList()
        {
            IsLoading = true;
            SchoolSubjects = await App.ApiConnector.GetTAsync<List<SchoolSubject>>("SchoolSubjects");
            IsLoading = false;
        }
        #endregion
    }
}
