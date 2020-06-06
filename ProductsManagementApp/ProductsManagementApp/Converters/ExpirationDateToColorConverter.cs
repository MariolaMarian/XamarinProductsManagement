using ProductsManagementApp.Models;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace ProductsManagementApp.Converters
{
    public class ExpirationDateToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ExpirationDate tmp = (ExpirationDate)value;

            if (tmp != null)
            {
                if (tmp.Collected)
                {
                    return Color.FromHex("#dd444444");
                }

                int days = (int)(tmp.EndDate - DateTime.Now).TotalDays;

                if (days > 30)
                {
                    return Color.FromHex("#dd00cc66");
                }
                else if (days > 7)
                {
                    return Color.FromHex("#ddffb366");
                }
                else
                {
                    return Color.FromHex("#ddff6666");
                }
            }
            else
            {
                return Color.White;
            }
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
