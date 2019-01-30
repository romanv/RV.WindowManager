namespace RV.WM2.SlotEditor.UI
{
    using System;
    using System.Globalization;

    using RV.WM2.Infrastructure.UI;

    public class ActualToScreenModelCoordinateConverter : ParameterlessConverterBase<ActualToScreenModelCoordinateConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value.GetType() != typeof(double))
            {
                throw new ArgumentException("Incorrect or empty argumens");
            }

            return (double)value / 2.0;
        }
    }
}
