namespace RV.WM2.SlotEditor.UI
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    public class LeftTopToScreenModelMarginConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var left = (double)values[0] / 2;
            var top = (double)values[1] / 2;
            return new Thickness(left, top, 0, 0);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
