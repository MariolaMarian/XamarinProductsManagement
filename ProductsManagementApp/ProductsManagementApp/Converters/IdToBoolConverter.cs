using System;
using System.Globalization;
using Xamarin.Forms;

namespace ProductsManagementApp.Converters
{
    public class IdToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                if (value is int number)
                {
                    if (number > 0)
                        return true;
                }
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
