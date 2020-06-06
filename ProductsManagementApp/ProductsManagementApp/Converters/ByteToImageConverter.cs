using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace ProductsManagementApp.Converters
{
    public class ByteToImageConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            ImageSource result = null;
            if(value!=null)
            {
                byte[] source = (byte[])value;
                result = ImageSource.FromStream(() => new MemoryStream(source));
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
