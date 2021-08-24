namespace RxBim.Shared.RevitExtensions.Helpers
{
    using System;
    using System.Collections.Generic;
    using Autodesk.Revit.DB;

    /// <summary>
    /// Сравниватель точек <see cref="XYZ"/>
    /// </summary>
    public class XyzComparer : IComparer<XYZ>
    {
        private const double Tolerance = 0.0001;

        /// <inheritdoc/>
        public int Compare(XYZ x, XYZ y)
        {
            int d = Compare(x.X, y.X);

            if (d == 0)
            {
                d = Compare(x.Y, y.Y);

                if (d == 0)
                    d = Compare(x.Z, y.Z);
            }

            return d;
        }

        private static int Compare(double a, double b)
        {
            return IsEqual(a, b) ? 0 : (a < b ? -1 : 1);
        }

        private static bool IsZero(double a)
        {
            return Math.Abs(a) < Tolerance;
        }

        private static bool IsEqual(double a, double b)
        {
            return IsZero(b - a);
        }
    }
}
