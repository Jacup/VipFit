namespace VipFit.Helpers
{
    using Microsoft.UI.Xaml.Data;

    class DecimalToDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value is decimal decimalValue ? System.Convert.ToDouble(decimalValue) : value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value is double doubleValue ? System.Convert.ToDecimal(doubleValue) : value;
        }
    }
}
