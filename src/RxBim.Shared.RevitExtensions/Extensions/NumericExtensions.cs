namespace RxBim.Shared.RevitExtensions.Extensions
{
    using System;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.DB.Structure;

    /// <summary>
    /// Утилиты конвертирования числовых значений
    /// </summary>
    public static class NumericExtensions
    {
        /// <summary>
        /// Конвертировать миллиметры в футы
        /// </summary>
        /// <param name="mm">Значение в миллиметрах</param>
        public static double MmToFt(this double mm)
        {
            return UnitUtils.ConvertToInternalUnits(mm, DisplayUnitType.DUT_MILLIMETERS);
        }

        /// <summary>
        /// Конвертировать футы в миллиметры
        /// </summary>
        /// <param name="ft">Значение в футах</param>
        public static double FtToMm(this double ft)
        {
            return UnitUtils.ConvertFromInternalUnits(ft, DisplayUnitType.DUT_MILLIMETERS);
        }

        /// <summary>
        /// Конвертировать миллиметры в футы
        /// </summary>
        /// <param name="mm">Значение в миллиметрах</param>
        public static double MmToFt(this int mm)
        {
            return UnitUtils.ConvertToInternalUnits(mm, DisplayUnitType.DUT_MILLIMETERS);
        }

        /// <summary>
        /// Конвертировать футы в миллиметры
        /// </summary>
        /// <param name="ft">Значение в футах</param>
        public static double FtToMm(this int ft)
        {
            return UnitUtils.ConvertFromInternalUnits(ft, DisplayUnitType.DUT_MILLIMETERS);
        }

        /// <summary>
        /// Конвертировать градусы в радианы
        /// </summary>
        /// <param name="degree">Значение угла в градусах</param>
        public static double DegreeToRadian(this double degree)
        {
            return degree * Math.PI / 180.0;
        }

        /// <summary>
        /// Конвертировать градусы в радианы
        /// </summary>
        /// <param name="degree">Значение угла в градусах</param>
        public static double DegreeToRadian(this int degree)
        {
            return degree * Math.PI / 180.0;
        }

        /// <summary>
        /// Конвертировать радианы в градусы
        /// </summary>
        /// <param name="radian">Значение угла в радианах</param>
        public static double RadianToDegree(this double radian)
        {
            return radian * 180.0 / Math.PI;
        }

        /// <summary>
        /// Возвращает диаметр арматуры, полученный из <see cref="RebarBarType"/>, в миллиметрах с округлением
        /// до одного знака после запятой в сторону четного числа
        /// </summary>
        /// <param name="rbt">Instance of <see cref="RebarBarType"/></param>
        public static double GetDiameterInMm(this RebarBarType rbt)
        {
            var d = rbt.BarDiameter.FtToMm();
            return Math.Round(d, 1, MidpointRounding.ToEven);
        }
    }
}
