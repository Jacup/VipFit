namespace VipFit.Helpers
{
    using Microsoft.UI.Xaml.Data;
    using System;

    /// <summary>
    /// Converter for DateTime and DateOnly.
    /// </summary>
    internal class DateOnlyConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object? Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return null;

            var dateOnly = (DateOnly)value;
            var dt = dateOnly.ToDateTime(TimeOnly.MinValue);

            return dt.ToUniversalTime() <= DateTimeOffset.MinValue.UtcDateTime
                ? DateTimeOffset.MinValue
                : new DateTimeOffset(dt);
        }

        /// <inheritdoc/>
        public object? ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return DateOnly.FromDateTime(((DateTimeOffset)value).DateTime);
        }
    }
}
