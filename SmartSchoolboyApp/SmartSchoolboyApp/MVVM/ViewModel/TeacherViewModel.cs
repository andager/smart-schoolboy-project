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
using System.Windows.Input;

namespace SmartSchoolboyApp.MVVM.ViewModel
{
    public class TeacherViewModel : ObservableObject
    {
        #region Fields
        private List<Teacher> _teachers;
        private RelayCommand _addEditCommand;
        private RelayCommand _removeCommand;
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
        public ICommand SearchCommand { get; }
        public RelayCommand EditTeacherCommand
        {
            get
            {
                return _addEditCommand ?? new RelayCommand(obj =>
                {
                    var teacher = obj as Teacher;
                    if (teacher != null)
                    {
                        AddEditTeacherView addEdit = new AddEditTeacherView(teacher);
                        addEdit.ShowDialog();
                        if (addEdit.IsVisible == false && addEdit.IsLoaded)
                            addEdit.Close();
                        UpdateDataGrid();
                    }
                });
            }
        }

        private RelayCommand addTeacherCommand;

        public RelayCommand AddTeacherCommand
        {
            get 
            {
                return addTeacherCommand ?? new RelayCommand(obj =>
                {
                    AddEditTeacherView addEditTeacherView = new AddEditTeacherView(null);
                    addEditTeacherView.ShowDialog();
                    if (addEditTeacherView.IsVisible == false && addEditTeacherView.IsLoaded)
                        addEditTeacherView.Close();
                    UpdateDataGrid();
                });
            }
        }


        public RelayCommand RemoveCommand
        {
            get
            {
                return _removeCommand ?? new RelayCommand(async obj =>
                {
                    var teacher = obj as Teacher;
                    if (teacher != null)
                    {
                        teacher.isActive = false;
                        await App.ApiConnector.PutTAsync(teacher, "Teachers", teacher.id);
                    }
                });
            }
        }
        #endregion

        #region Constructor
        public TeacherViewModel()
        {
            UpdateDataGrid();
            SearchCommand = new RelayCommand(ExecuteSearchCommand, CanExecuteSearchCommand);
        }

        private async void UpdateDataGrid()
        {
            IsLoading = true;
            Teachers = await App.ApiConnector.GetTAsync<List<Teacher>>("Teachers");
            IsLoading = false;
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
