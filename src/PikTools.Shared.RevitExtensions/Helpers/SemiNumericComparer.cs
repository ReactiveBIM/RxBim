namespace PikTools.Shared.RevitExtensions.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    /// <summary>
    /// Сравниватель чисел в строковом виде
    /// </summary>
    /// <remarks>https://stackoverflow.com/a/6397287</remarks>
    internal class SemiNumericComparer : IComparer<string>
    {
        /// <summary>
        /// Method to determine if a string is a number
        /// </summary>
        /// <param name="value">String to test</param>
        /// <returns>True if numeric</returns>
        public static bool IsNumeric(string value)
        {
            return int.TryParse(value, out _);
        }

        /// <inheritdoc />
        public int Compare(string s1, string s2)
        {
            const int s1GreaterThanS2 = 1;
            const int s2GreaterThanS1 = -1;

            var isNumeric1 = IsNumeric(s1);
            var isNumeric2 = IsNumeric(s2);

            if (isNumeric1
                && isNumeric2)
            {
                var i1 = Convert.ToInt32(s1);
                var i2 = Convert.ToInt32(s2);

                if (i1 > i2)
                    return s1GreaterThanS2;

                if (i1 < i2)
                    return s2GreaterThanS1;

                return 0;
            }

            if (isNumeric1)
                return s2GreaterThanS1;

            if (isNumeric2)
                return s1GreaterThanS2;

            return string.Compare(
                s1, s2, true, CultureInfo.InvariantCulture);
        }
    }
}
