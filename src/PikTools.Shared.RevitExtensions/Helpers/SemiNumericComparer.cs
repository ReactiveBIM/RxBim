namespace PikTools.Shared.RevitExtensions.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Сравниватель чисел в строковом виде
    /// </summary>
    /// <remarks>https://stackoverflow.com/a/33330715/8252345</remarks>
    public class SemiNumericComparer : IComparer<string>
    {
        private const string NumericPattern = @"[-+]?\d*\.\d+|\d+";

        /// <inheritdoc/>
        public int Compare(string s1, string s2)
        {
            var s1n = IsNumeric(s1, out var s1r);
            var s2n = IsNumeric(s2, out var s2r);

            if (s1n && s2n)
                return (int)(s1r - s2r);
            else if (s1n)
                return -1;
            else if (s2n)
                return 1;

            var num1 = Regex.Match(s1, NumericPattern);
            var num2 = Regex.Match(s2, NumericPattern);

            var onlyString1 = s1.Remove(num1.Index, num1.Length).Trim();
            var onlyString2 = s2.Remove(num2.Index, num2.Length).Trim();

            if (onlyString1 == onlyString2)
            {
                if (num1.Success && num2.Success)
                    return double.Parse(num1.Value, CultureInfo.InvariantCulture).CompareTo(double.Parse(num2.Value, CultureInfo.InvariantCulture));
                else if (num1.Success)
                    return 1;
                else if (num2.Success)
                    return -1;
            }

            return string.Compare(
                s1, s2, true, CultureInfo.InvariantCulture);
        }

        private bool IsNumeric(string value, out double result)
        {
            return double.TryParse(value, out result);
        }
    }
}
