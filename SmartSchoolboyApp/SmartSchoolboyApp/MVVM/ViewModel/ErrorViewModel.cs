using SmartSchoolboyApp.MVVM.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSchoolboyApp.MVVM.ViewModel
{
    public class ErrorViewModel : ObservableObject
    {
        #region Fields
        private string _messageError;
        #endregion

        #region Properties
        public string MessageError
        {
            get { return _messageError; }
            set { _messageError = value; OnPropertyChanged(nameof(MessageError)); }
        }
        #endregion

        #region Commands

        #endregion

        #region Constructor
        public ErrorViewModel(string messageError)
        {
            MessageError = messageError;
        }
        #endregion
    }
}
