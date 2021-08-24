namespace RxBim.Shared.Ui.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    /// <summary>
    /// Конвертер перечисления в число
    /// </summary>
    public class EnumToIntConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var returnValue = 0;
            if (parameter is Type)
               returnValue = (int)Enum.Parse((Type)parameter, value.ToString());

            return returnValue;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var enumValue = default(Enum);
            if (parameter is Type)
                enumValue = (Enum)Enum.Parse((Type)parameter, value.ToString());

            return enumValue;
        }
    }
}
