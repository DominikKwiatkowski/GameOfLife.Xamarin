using System;
using GameOfLifeXamarin.Enums;
using Xamarin.Forms;

namespace GameOfLifeXamarin.Converters
{
    public class StatusToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Status status = (Status)value;
            if (status == Status.Alive)
                return Brush.Green;
            if (status == Status.Dead)
                return Brush.Gray;
            if (status == Status.Born)
                return Brush.GreenYellow;
            if (status == Status.WillRise)
                return Brush.LightGray;
            if (status == Status.WillDie)
                return Brush.Red;
            if (status == Status.Died)
                return Brush.Black;
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
