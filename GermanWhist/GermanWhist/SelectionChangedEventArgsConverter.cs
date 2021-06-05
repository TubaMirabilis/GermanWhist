using System;
using System.Globalization;
using Xamarin.Forms;

namespace GermanWhist
{
    class SelectionChangedEventArgsConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is SelectionChangedEventArgs eventArgs)
                return eventArgs.CurrentSelection;
            return null;
        }
    }
}