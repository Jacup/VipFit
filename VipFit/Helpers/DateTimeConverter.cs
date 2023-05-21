namespace VipFit.Helpers
{
    using Microsoft.UI.Xaml.Data;
    using System;

    /// <summary>
    /// Converter for DateTime and DateOnly.
    /// </summary>
    internal class DateTimeConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object? Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return null;

            var dateTime = (DateTime)value;

            return dateTime.ToUniversalTime() <= DateTimeOffset.MinValue.UtcDateTime
                ? DateTimeOffset.MinValue
                : new DateTimeOffset(dateTime);
        }

        /// <inheritdoc/>
        public object? ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return ((DateTimeOffset)value).DateTime;
        }
    }
}
