namespace PikTools.Shared.AutocadExtensions.Extensions
{
    using Autodesk.AutoCAD.Geometry;

    /// <summary>
    /// Расширения для 2D точек
    /// </summary>
    public static class Point2dExtensions
    {
        /// <summary>
        /// Возвращает точку, смещённую относительно исходной
        /// </summary>
        /// <param name="basePoint">Исходная точка</param>
        /// <param name="x">Смещение по X</param>
        /// <param name="y">Смещение по Y</param>
        public static Point2d OffsetPoint(this Point2d basePoint, double x, double y)
        {
            return basePoint + new Vector2d(x, y);
        }

        /// <summary>
        /// Возвращает 3D точку с координатами X и Y исходной 2D точки и нулевой координатой Z
        /// </summary>
        /// <param name="point">Исходная 2D точка</param>
        public static Point3d ConvertTo3d(this Point2d point)
        {
            return new Point3d(point.X, point.Y, 0d);
        }

        /// <summary>
        /// Возвращает точку посередине между точкой и другой точкой
        /// </summary>
        /// <param name="point">Точка</param>
        /// <param name="otherPoint">Другая точка</param>
        public static Point2d GetMiddlePoint(this Point2d point, Point2d otherPoint)
        {
            return point + point.GetVectorTo(otherPoint).DivideBy(2);
        }
    }
}