using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace FutuScanner
{
    public class IntToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color red = Color.FromArgb(255, 255, 76, 118);
            Color green = Color.FromArgb(255, 103, 223, 43);
            Color color;
            if (int.Parse(value.ToString()) == 0)
                color = red;
            else
                color = green;
            return new SolidColorBrush(color);
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ConvertItemToIndex : IValueConverter
    {
        #region IValueConverter Members
        //Convert the Item to an Index
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                //Get the CollectionView from the DataGrid that is using the converter
                DataGrid dg = (DataGrid)Application.Current.MainWindow.FindName("watchListGrid");
                CollectionView cv = (CollectionView)dg.Items;
                //Get the index of the item from the CollectionView
                int rowindex = cv.IndexOf(value) + 1;

                return rowindex.ToString();
            }
            catch (Exception e)
            {
                throw new NotImplementedException(e.Message);
            }
        }
        //One way binding, so ConvertBack is not implemented
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
