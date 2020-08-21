namespace PikTools.Shared.Ui.Converters
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    /// <summary>
    /// Конвертер Boolean в Visibility
    /// </summary>
    public class BooleanToVisibilityConverter : IValueConverter
    {
        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var flag = false;
            if (value != null)
                flag = (bool)value;

            var isInverse = parameter != null
                            && parameter.ToString().ToLower() == "inverse";

            return (flag ^ isInverse)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
