using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace QuanLyTruyenTranh.Helper.Converter
{
    public class ImageConverter : IValueConverter
    {
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                string imageName = value.ToString();
                if (imageName != null)
                {
                    string des = Directory.GetCurrentDirectory();
                    Uri uri = new Uri(des + imageName);
                    BitmapFrame bitmapFrame = BitmapFrame.Create(uri);
                    return bitmapFrame;
                }
                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
