using SmartSchoolboyApp.MVVM.Core;
using System;

namespace SmartSchoolboyApp.Stores
{
    public class NavigationStore
    {
        private ObservableObject _currentViewModel;
        public event Action CurrentViewModelChanged;
        public ObservableObject CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;  
                OnCurrentViewModelChanged();
            }
        }
        private void OnCurrentViewModelChanged()
        {
            CurrentViewModelChanged?.Invoke();
        }
    }
}
