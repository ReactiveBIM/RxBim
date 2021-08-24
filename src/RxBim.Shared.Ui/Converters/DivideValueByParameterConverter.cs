namespace RxBim.Shared.Ui.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    /// <summary>
    /// Конвертер, который делит значение на значение в параметре
    /// </summary>
    public class DivideValueByParameterConverter : IValueConverter
    {
        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null
                || !int.TryParse(parameter.ToString(), out var divider))
                return value;

            if (divider == 0
                || value == null)
                return null;

            switch (value)
            {
                case int iValue:
                    return iValue / divider;

                case double dValue:
                    return dValue / divider;
            }

            return value;
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
