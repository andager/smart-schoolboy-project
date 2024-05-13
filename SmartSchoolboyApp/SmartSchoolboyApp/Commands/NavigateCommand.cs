using SmartSchoolboyApp.MVVM.Core;
using SmartSchoolboyApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SmartSchoolboyApp.Commands
{
    public class NavigateCommand<TViewModel> : BaseCommand where TViewModel : ObservableObject
    {
        private readonly NavigationService<TViewModel> _navigationService;
        public NavigateCommand(NavigationService<TViewModel> navigationService)
        {
            _navigationService = navigationService;
        }

        public override void Execute(object parameter)
        {
            _navigationService.Navigate();
        }

    }
}
