namespace RxBim.Shared.AutocadExtensions.Extensions
{
    using System;

    /// <summary>
    /// Расширения для вещественных чисел
    /// </summary>
    public static class DoubleExtensions
    {
        private const double Epsilon = 1e-6;
        private const double DegreesInRadian = 180D / Math.PI;

        /// <summary>
        /// Возвращает истину, если число равно другому числу с заданной точностью
        /// </summary>
        /// <param name="value">Число</param>
        /// <param name="otherValue">Другое число</param>
        /// <param name="precision">Точность сравнения</param>
        public static bool IsEqualTo(this double value, double otherValue, double precision = Epsilon)
        {
            return Math.Abs(value - otherValue) < precision;
        }

        /// <summary>
        /// Возвращает истину, если число меньше или равно другому числу с заданной точностью
        /// </summary>
        /// <param name="value">Число</param>
        /// <param name="otherValue">Другое число</param>
        /// <param name="precision">Точность сравнения</param>
        public static bool IsEqualOrLess(this double value, double otherValue, double precision = Epsilon)
        {
            return value.IsEqualTo(otherValue, precision) || value < otherValue;
        }

        /// <summary>
        /// Возвращает истину, если число больше или равно другому числу с заданной точностью
        /// </summary>
        /// <param name="value">Число</param>
        /// <param name="otherValue">Другое число</param>
        /// <param name="precision">Точность сравнения</param>
        public static bool IsEqualOrGreater(this double value, double otherValue, double precision = Epsilon)
        {
            return value.IsEqualTo(otherValue, precision) || value > otherValue;
        }

        /// <summary>
        /// Возвращает истину, если число равно нулю с заданной точностью
        /// </summary>
        /// <param name="value">Число</param>
        /// <param name="precision">Точность сравнения</param>
        public static bool IsZero(this double value, double precision = Epsilon)
        {
            return value.IsEqualTo(0D, precision);
        }

        /// <summary>
        /// Возвращает результат умножения числа на другое число
        /// </summary>
        /// <param name="value">Число</param>
        /// <param name="multiplier">Другое число (множитель)</param>
        public static double Multiple(this double value, double multiplier)
        {
            return value * multiplier;
        }

        /// <summary>
        /// Возвращает значение угла, переведённого из радианов в градусы
        /// </summary>
        /// <param name="radians">Значение угла в радианах</param>
        public static double RadiansToDegrees(this double radians)
        {
            return radians * DegreesInRadian;
        }

        /// <summary>
        /// Возвращает значение угла, переведённого из градусов в радианы
        /// </summary>
        /// <param name="degrees">Значение угла в градусах</param>
        public static double DegreesToRadians(this double degrees)
        {
            return degrees / DegreesInRadian;
        }
    }
}