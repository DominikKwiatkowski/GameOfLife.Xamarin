using System;
using GameOfLifeXamarin.Enums;
using Xamarin.Forms;

namespace GameOfLifeXamarin.Converters
{
    public class StatusToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Status status = (Status)value;
            return status.ToString("g");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
