using SmartSchoolboyApp.MVVM.Core;
using SmartSchoolboyApp.Stores;
using System;

namespace SmartSchoolboyApp.Commands
{
    public class NavigateCommand<TViewModel> : BaseCommand where TViewModel : ObservableObject
    {
        private readonly NavigationStore _navigationStore;
        private readonly Func<TViewModel> _createViewModel;
        public NavigateCommand(NavigationStore navigationStore, Func<TViewModel> createViewModel)
        {
            _navigationStore = navigationStore;
            _createViewModel = createViewModel;
        }

        public override void Execute(object parameter)
        {
            _navigationStore.CurrentViewModel = _createViewModel();
        }
    }
}
