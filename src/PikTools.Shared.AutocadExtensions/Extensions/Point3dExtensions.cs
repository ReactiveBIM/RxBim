namespace PikTools.Shared.AutocadExtensions.Extensions
{
    using Autodesk.AutoCAD.ApplicationServices.Core;
    using Autodesk.AutoCAD.Geometry;
    using CoordinateSystems;
    using JetBrains.Annotations;
    using AcRx = Autodesk.AutoCAD.Runtime;

    /// <summary>
    /// Расширения для 3D точек
    /// </summary>
    [PublicAPI]
    public static class Point3dExtensions
    {
        /// <summary>
        /// Возвращает точку, смещённую относительно исходной
        /// </summary>
        /// <param name="basePoint">Исходная точка</param>
        /// <param name="x">Смещение по X</param>
        /// <param name="y">Смещение по Y</param>
        /// <param name="z">Смещение по Z</param>
        public static Point3d OffsetPoint(this Point3d basePoint, double x, double y, double z = 0)
        {
            return basePoint + new Vector3d(x, y, z);
        }

        /// <summary>
        /// Возвращает 2D точку с координатами X и Y исходной 3D точки
        /// </summary>
        /// <param name="point">Исходная 3D точка</param>
        public static Point2d ConvertTo2d(this Point3d point)
        {
            return new Point2d(point.X, point.Y);
        }

        /// <summary>
        /// Возвращает 3D точку с координатами X и Y исходной 3D точки и нулевой координатой Z
        /// </summary>
        /// <param name="pt">Исходная 3D точка</param>
        /// <returns></returns>
        public static Point3d Flatten(this Point3d pt)
        {
            return new Point3d(pt.X, pt.Y, 0.0);
        }

        /// <summary>
        /// Возвращает точку, полученную трансформацией исходной точки из пользовательской системы координат в мировую
        /// </summary>
        /// <param name="pt">Исходная точка</param>
        public static Point3d TransformFromUcsToWcs(this Point3d pt)
        {
            return pt.Transform(CoordinateSystemType.UCS, CoordinateSystemType.WCS);
        }

        /// <summary>
        /// Возвращает точку, полученную трансформацией исходной точки из мировой системы координат в пользовательскую
        /// </summary>
        /// <param name="pt">Исходная точка</param>
        public static Point3d TransformFromWcsToUcs(this Point3d pt)
        {
            return pt.Transform(CoordinateSystemType.WCS, CoordinateSystemType.UCS);
        }

        /// <summary>
        /// Возвращает точку, полученную трансформацией исходной точки из одной системы координат в другую
        /// </summary>
        /// <param name="pt">Исходная точка</param>
        /// <param name="from">Исходная система координат</param>
        /// <param name="to">Целевая система координат</param>
        /// <exception cref="Autodesk.AutoCAD.Runtime.Exception">
        /// eInvalidInput - если трансформировать из PSDCS в любую систему координат, отличную от DCS
        /// </exception>
        public static Point3d Transform(this Point3d pt, CoordinateSystemType from, CoordinateSystemType to)
        {
            var ed = Application.DocumentManager.MdiActiveDocument.Editor;
            return ed.TransformPoint(pt, from, to);
        }
    }
}