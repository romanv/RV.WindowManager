namespace RV.WM2.Infrastructure.UI
{
    using System;
    using System.Globalization;

    public class DummyConverter : ParameterlessConverterBase<DummyConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}