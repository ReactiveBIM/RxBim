namespace PikTools.Shared.Ui.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    /// <summary>
    /// Convert Null to Boolean. To inverse set parameter "inverse"
    /// </summary>
    public class NullToBooleanConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isInverse = parameter != null && parameter.ToString().ToLower() == "inverse";
            return value == null || !isInverse;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
