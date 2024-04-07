using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SmartSchoolboyApp.Helpers
{
    public static class NotifyPropertyChangedExtetion
    {
        public static void Mutateverbose<TField>(this INotifyPropertyChanged instance, TField field, TField newValue, Action<PropertyChangedEventArgs> ralse, [CallerMemberName] string propertyNeme = null)
        {
            if (EqualityComparer<TField>.Default.Equals(field, newValue))
                return;
            field = newValue;
            ralse?.Invoke(new PropertyChangedEventArgs(propertyNeme));
        }
    }
}
