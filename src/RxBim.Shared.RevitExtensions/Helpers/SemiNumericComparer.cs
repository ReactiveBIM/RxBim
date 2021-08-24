namespace RxBim.Shared.RevitExtensions.Helpers
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Сравниватель чисел в строковом виде
    /// </summary>
    /// <remarks>https://stackoverflow.com/a/33330715/8252345</remarks>
    public class SemiNumericComparer : IComparer<string>
    {
        private readonly string _numericPattern;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="numericPattern">Регулярка для поиска числа в тексте</param>
        public SemiNumericComparer(string numericPattern = @"[-+]?\d*\.\d+|\d+")
        {
            _numericPattern = numericPattern;
        }

        /// <inheritdoc/>
        public int Compare(string s1, string s2)
        {
            var num1 = Regex.Match(s1, _numericPattern);
            var num2 = Regex.Match(s2, _numericPattern);

            if (num1.Success && num2.Success)
            {
                var dNum1 = double.Parse(num1.Value, CultureInfo.InvariantCulture);
                var dNum2 = double.Parse(num2.Value, CultureInfo.InvariantCulture);

                // Если два числа одинаковые, то проверяем длину исходного текста,
                // чтобы сравнить случаи "000" и "0000"
                if (dNum1.Equals(dNum2))
                    return s1.Length - s2.Length;

                return dNum1.CompareTo(dNum2);
            }

            if (num1.Success)
                return 1;
            if (num2.Success)
                return -1;

            return string.Compare(
                s1, s2, true, CultureInfo.InvariantCulture);
        }
    }
}
