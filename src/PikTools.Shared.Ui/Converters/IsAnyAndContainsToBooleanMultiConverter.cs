namespace PikTools.Shared.Ui.Converters
{
    using System;
    using System.Collections;
    using System.Globalization;
    using System.Linq;
    using System.Windows.Data;

    /// <summary>
    /// Конвертер проверки наличия элементов в коллекции
    /// </summary>
    public class IsAnyAndContainsToBooleanMultiConverter : IMultiValueConverter
    {
        /// <inheritdoc />
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null
                || values.Length < 2)
                return false;

            if (!(values[0] is IEnumerable collect)
                || values[1] == null)
                return false;

            return collect.Cast<object>().All(el => !el.Equals(values[1]));
        }

        /// <inheritdoc />
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
