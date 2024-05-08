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
        #endregion

        #region Properties
        public List<Teacher> Teachers { get { return _teachers; } set { _teachers = value; OnPropertyChanged(nameof(Teachers)); } }
        public Teacher SelectedTeacher
        {
            get { return _selectTeacher; }
            set { _selectTeacher = value; OnPropertyChanged(nameof(SelectedTeacher));}
        }
        public string Search { get { return _search; } set { _search = value; OnPropertyChanged(nameof(Search)); } }
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
                        teacher.isActive = true;
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

        private void ExecuteAddEditTeacherCommand(object obj)
        {
            AddEditTeacherView addEdit = new AddEditTeacherView(obj as Teacher);
            addEdit.ShowDialog();
        }

        private async void UpdateDataGrid()
        {
            var teacher = await App.ApiConnector.GetTAsync<List<Teacher>>("Teachers");
            teacher.Where(p => p.lastName.ToLower().Trim().Contains(Search.ToLower().Trim()) ||
            p.firstName.ToLower().Trim().Contains(Search.ToLower().Trim()) ||
            p.patronymic.ToLower().Trim().Contains(Search.ToLower().Trim()));
            Teachers = teacher;
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
