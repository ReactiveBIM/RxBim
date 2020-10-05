namespace PikTools.Shared.RevitExtensions.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Autodesk.Revit.DB;

    /// <summary>
    /// Расширения для геометрии в Revit
    /// </summary>
    public static class GeometryExtensions
    {
        private const double Tolerance = 0.0001;

        /// <summary>
        /// Получить базовую точку проекта
        /// </summary>
        /// <param name="doc">Документ Revit</param>
        /// <returns>Базовая точка проекта</returns>
        public static XYZ GetProjectBasePoint(this Document doc)
        {
            var elements = new FilteredElementCollector(doc)
                .WherePasses(new ElementCategoryFilter(BuiltInCategory.OST_ProjectBasePoint))
                .ToElements();

            var element = elements.FirstOrDefault();
            if (element == null)
                return null;

            var x = element.get_Parameter(BuiltInParameter.BASEPOINT_EASTWEST_PARAM).AsDouble();
            var y = element.get_Parameter(BuiltInParameter.BASEPOINT_NORTHSOUTH_PARAM).AsDouble();
            var elevation = element.get_Parameter(BuiltInParameter.BASEPOINT_ELEVATION_PARAM).AsDouble();

            return new XYZ(x, y, elevation);
        }

        /// <summary>
        /// Средняя точка линии
        /// </summary>
        /// <param name="line">The line.</param>
        public static XYZ GetCenterPoint(this Line line)
        {
            var pt1 = line.GetEndPoint(0);
            var pt2 = line.GetEndPoint(1);
            return (pt1 + pt2) / 2;
        }

        /// <summary>
        /// Проверка что два отрезка параллельны
        /// </summary>
        /// <param name="line">Первый отрезок</param>
        /// <param name="otherLine">Второй отрезок</param>
        public static bool IsParallelTo(this Line line, Line otherLine)
        {
            return Math.Abs(Math.Abs(line.Direction.DotProduct(otherLine.Direction)) - 1) < Tolerance;
        }

        /// <summary>
        /// Проверка, что отрезок параллелен вектору
        /// </summary>
        /// <param name="line">Отрезок</param>
        /// <param name="vector">Вектор</param>
        public static bool IsParallelTo(this Line line, XYZ vector)
        {
            return Math.Abs(Math.Abs(line.Direction.DotProduct(vector.Normalize())) - 1) < Tolerance;
        }

        /// <summary>
        /// Проверка наличия точки в списке через сверку расстояния
        /// </summary>
        /// <param name="points">Список точек</param>
        /// <param name="point">Проверяемая точка</param>
        public static bool HasSimilarPoint(this List<XYZ> points, XYZ point)
        {
            foreach (var xyz in points)
            {
                if (Math.Abs(xyz.DistanceTo(point)) < Tolerance)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Получение наружного <see cref="Face"/> для стены
        /// </summary>
        /// <param name="wall">Стена</param>
        /// <param name="shellLayerType">Тип получаемого Face</param>
        public static Face GetSideFaceFromWall(this Wall wall, ShellLayerType shellLayerType)
        {
            Face face = null;
            IList<Reference> sideFaces = null;
            if (shellLayerType == ShellLayerType.Exterior)
            {
                sideFaces = HostObjectUtils.GetSideFaces(wall, ShellLayerType.Exterior);
            }

            if (shellLayerType == ShellLayerType.Interior)
            {
                sideFaces = HostObjectUtils.GetSideFaces(wall, ShellLayerType.Interior);
            }

            if (sideFaces != null)
            {
                face = wall.GetGeometryObjectFromReference(sideFaces[0]) as Face;
            }

            return face;
        }

        /// <summary>
        /// Возвращает отрезок, спроецированный на плоскость
        /// </summary>
        /// <param name="plane">Плоскость</param>
        /// <param name="line">Исходный отрезок</param>
        public static Line ProjectOnto(this Plane plane, Line line)
        {
            return Line.CreateBound(
                plane.ProjectOnto(line.GetEndPoint(0)),
                plane.ProjectOnto(line.GetEndPoint(1)));
        }

        /// <summary>
        /// Возвращает 3D точку <see cref="XYZ"/>, спроецированную на плоскость <see cref="Plane"/> 
        /// </summary>
        /// <remarks>
        /// http://thebuildingcoder.typepad.com/blog/2014/09/planes-projections-and-picking-points.html
        /// </remarks>
        /// <param name="plane">The plane.</param>
        /// <param name="p">The point.</param>
        public static XYZ ProjectOnto(this Plane plane, XYZ p)
        {
            var d = plane.SignedDistanceTo(p);

            var q = p - (d * plane.Normal);

            Debug.Assert(
                IsZero(plane.SignedDistanceTo(q)),
                "expected point on plane to have zero distance to plane");

            return q;
        }

        /// <summary>
        /// Return signed distance from plane to a given point.
        /// </summary>
        /// <param name="plane">The plane.</param>
        /// <param name="p">The point.</param>
        private static double SignedDistanceTo(this Plane plane, XYZ p)
        {
            Debug.Assert(
                IsEqual(plane.Normal.GetLength(), 1),
                "expected normalized plane normal");

            var v = p - plane.Origin;
            return plane.Normal.DotProduct(v);
        }

        private static bool IsZero(double a, double tolerance = 1.0e-9)
        {
            return tolerance > Math.Abs(a);
        }

        private static bool IsEqual(double a, double b)
        {
            return IsZero(b - a);
        }
    }
}
