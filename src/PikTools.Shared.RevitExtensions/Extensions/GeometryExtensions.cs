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
        private const double LigthTolerance = 0.001;

        private static readonly Options Options = new Options()
        {
            ComputeReferences = true,
        };

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
        /// Создает линию между двумя точками
        /// </summary>
        /// <param name="firstPoint">Первая точка</param>
        /// <param name="secondPoint">Вторая точка</param>
        /// <returns>Созданная линия между двумя точками</returns>
        /// <remarks>Если растояние между точками мало, то возвращает null</remarks>
        public static Line GetLine(this XYZ firstPoint, XYZ secondPoint)
            => firstPoint.DistanceTo(secondPoint) < 1.MmToFt()
                    ? null
                    : Line.CreateBound(firstPoint, secondPoint);

        /// <summary>
        /// Средняя точка линии
        /// </summary>
        /// <param name="line">The line.</param>
        public static XYZ GetCenterPoint(this Line line)
            => line.Evaluate(0.5, true);

        /// <summary>
        /// Проверка что два отрезка параллельны
        /// </summary>
        /// <param name="line">Первый отрезок</param>
        /// <param name="otherLine">Второй отрезок</param>
        public static bool IsParallelTo(this Line line, Line otherLine)
            => Math.Abs(Math.Abs(line.Direction.DotProduct(otherLine.Direction)) - 1) < Tolerance;

        /// <summary>
        /// Проверка, что отрезок параллелен вектору
        /// </summary>
        /// <param name="line">Отрезок</param>
        /// <param name="vector">Вектор</param>
        public static bool IsParallelTo(this Line line, XYZ vector)
            => Math.Abs(Math.Abs(line.Direction.DotProduct(vector.Normalize())) - 1) < Tolerance;

        /// <summary>
        /// Проверка наличия точки в списке через сверку расстояния
        /// </summary>
        /// <param name="points">Список точек</param>
        /// <param name="point">Проверяемая точка</param>
        public static bool HasSimilarPoint(this List<XYZ> points, XYZ point)
            => points.Any(xyz => Math.Abs(xyz.DistanceTo(point)) < Tolerance);

        /// <summary>
        /// Получение наружного <see cref="Face"/> для стены
        /// </summary>
        /// <param name="wall">Стена</param>
        /// <param name="shellLayerType">Тип получаемого Face</param>
        public static Face GetSideFaceFromWall(this Wall wall, ShellLayerType shellLayerType)
        {
            var sideFaces = shellLayerType switch
            {
                ShellLayerType.Exterior => HostObjectUtils.GetSideFaces(wall, ShellLayerType.Exterior),
                ShellLayerType.Interior => HostObjectUtils.GetSideFaces(wall, ShellLayerType.Interior),
                _ => throw new NotImplementedException($"Not implement {shellLayerType}"),
            };

            return wall.GetGeometryObjectFromReference(sideFaces[0]) as Face;
        }

        /// <summary>
        /// Возвращает отрезок, спроецированный на плоскость
        /// </summary>
        /// <param name="plane">Плоскость</param>
        /// <param name="line">Исходный отрезок</param>
        public static Line ProjectOnto(this Plane plane, Line line)
            => Line.CreateBound(
                    plane.ProjectOnto(line.GetEndPoint(0)),
                    plane.ProjectOnto(line.GetEndPoint(1)));

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
        /// Получить концевые точки линии
        /// </summary>
        /// <param name="line">Линия</param>
        /// <returns>Концевые точки линии</returns>
        public static XYZ[] GetEndpoints(this Line line)
            => new XYZ[]
            {
                line.GetEndPoint(0),
                line.GetEndPoint(1)
            };

        /// <summary>
        /// Возвращает расстояние между точками
        /// </summary>
        /// <param name="p1">Эта точка</param>
        /// <param name="p2">Точка, до которой нужно померить расстояние</param>
        /// <returns>Растояние между точками</returns>
        public static double GetLengthBetweenPoints(this XYZ p1, XYZ p2)
            => GetVectorLength(GetVectorFromTwoPoints(p1, p2));

        /// <summary>
        /// Возвращает длину вектора
        /// </summary>
        /// <param name="v">Вектор</param>
        /// <returns>Длина вектора</returns>
        public static double GetVectorLength(this XYZ v)
            => Math.Sqrt(Math.Pow(v.X, 2) + Math.Pow(v.Y, 2) + Math.Pow(v.Z, 2));

        /// <summary>
        /// Возвращает вектор напрвления между точками
        /// </summary>
        /// <param name="p1">Эта точка</param>
        /// <param name="p2">Точка, до которой необходимо построить вектор</param>
        /// <returns>Вектор направления между точками</returns>
        public static XYZ GetVectorFromTwoPoints(this XYZ p1, XYZ p2)
            => new XYZ(p2.X - p1.X, p2.Y - p1.Y, p2.Z - p1.Z);

        /// <summary>
        /// Изменяет длину вектора по его направлению
        /// </summary>
        /// <param name="vector">Вектор</param>
        /// <param name="length">Новая длина вектора</param>
        /// <returns>Вектор с новой длиной</returns>
        public static XYZ GetTransformedVector(this XYZ vector, double length)
        {
            var lengthFactor = length / vector.GetVectorLength();
            return new XYZ(
                vector.X * lengthFactor,
                vector.Y * lengthFactor,
                vector.Z * lengthFactor);
        }

        /// <summary>
        /// Получить тело элемента
        /// </summary>
        /// <param name="element">Элемент Revit</param>
        /// <param name="options">Опции</param>
        /// <returns>Тело элемента</returns>
        public static Solid GetSolid(
            this Element element,
            Options options = null)
        {
            var op = options ?? Options;
            var geometryObject = element.get_Geometry(op).First();

            switch (geometryObject)
            {
                case Solid solid:
                    return solid;

                case GeometryInstance _:
                {
                    var bb = element.get_BoundingBox(null);

                    var pt1 = bb.Min;
                    var pt2 = new XYZ(bb.Max.X, bb.Min.Y, bb.Min.Z);
                    var pt3 = new XYZ(bb.Max.X, bb.Max.Y, bb.Min.Z);
                    var pt4 = new XYZ(bb.Min.X, bb.Max.Y, bb.Min.Z);

                    var l12 = Line.CreateBound(pt1, pt2);
                    var l23 = Line.CreateBound(pt2, pt3);
                    var l34 = Line.CreateBound(pt3, pt4);
                    var l41 = Line.CreateBound(pt4, pt1);

                    var curveLoop = CurveLoop.Create(new List<Curve> { l12, l23, l34, l41 });
                    var height = bb.Max.Z - bb.Min.Z;

                    return GeometryCreationUtilities
                        .CreateExtrusionGeometry(
                            new List<CurveLoop> { curveLoop },
                            XYZ.BasisZ,
                            height);
                }

                default:
                    return null;
            }
        }

        /// <summary>
        /// Получить вертикали
        /// </summary>
        /// <param name="element">Элемент Revit</param>
        /// <param name="options">Опции</param>
        /// <returns>Вертикали</returns>
        public static ICollection<XYZ> GetVertices(
            this Element element,
            Options options = null)
        {
            var op = options ?? Options;

            var solid = element.GetSolid(op);

            var verticalFaces = solid
                .Faces
                .OfType<PlanarFace>()
                .Where(pf => Math.Abs(pf.FaceNormal.DotProduct(XYZ.BasisZ)) < LigthTolerance
                             && pf.Area > 0)
                .OrderByDescending(pf => pf.Area)
                .ToArray();

            var mainVerticalFace = verticalFaces.FirstOrDefault();
            if (mainVerticalFace == null)
                return null;

            double angle = 0;

            if (Math.Abs(mainVerticalFace.FaceNormal.X) > LigthTolerance
                && Math.Abs(mainVerticalFace.FaceNormal.Y) > LigthTolerance)
                angle = Math.Acos(mainVerticalFace.FaceNormal.X);

            Transform transform = Transform.Identity;

            if (angle > Tolerance)
                transform = Transform.CreateRotation(XYZ.BasisZ, -angle);

            var edgeArrays = verticalFaces
                .SelectMany(pf => pf.EdgeLoops.Cast<EdgeArray>())
                .ToArray();

            var points = edgeArrays
                .SelectMany(ea => ea.Cast<Edge>()
                                    .Select(e => e.AsCurve() as Line)
                                    .Where(l => l != null))
                .SelectMany(l => new XYZ[]
                {
                    l.GetEndPoint(0),
                    l.GetEndPoint(1)
                })
                .GetUnique()
                .OrderBy(pt => pt.Z)
                .ToArray();

            var minZ = points.First().Z;
            var maxZ = points.Last().Z;

            var tPoints = points
                .Select(pt => transform.OfPoint(pt))
                .ToArray();

            var tPointsByX = tPoints
                .OrderBy(pt => pt.X)
                .ToArray();

            var minX = tPointsByX.First().X;
            var maxX = tPointsByX.Last().X;

            var tPointsByY = tPoints
                .OrderBy(pt => pt.Y)
                .ToArray();

            var minY = tPointsByY.First().Y;
            var maxY = tPointsByY.Last().Y;

            var extremesT = new XYZ[]
            {
                new XYZ(minX, minY, minZ),
                new XYZ(minX, maxY, minZ),
                new XYZ(maxX, minY, minZ),
                new XYZ(maxX, maxY, minZ),
                new XYZ(minX, minY, maxZ),
                new XYZ(minX, maxY, maxZ),
                new XYZ(maxX, minY, maxZ),
                new XYZ(maxX, maxY, maxZ),
            };

            var extremes = extremesT
                .Select(pt => transform.Inverse.OfPoint(pt))
                .ToArray();

            return extremes;
        }

        /// <summary>
        /// Получить уникальные XYZ
        /// </summary>
        /// <param name="xyzs">Набор XYZ</param>
        /// <returns>Уникальные XYZ</returns>
        public static ICollection<XYZ> GetUnique(
            this IEnumerable<XYZ> xyzs)
        {
            var unique = new List<XYZ>();
            foreach (var xyz in xyzs)
            {
                if (unique.All(x => x.DistanceTo(xyz) > Tolerance))
                    unique.Add(xyz);
            }

            return unique;
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
            => tolerance > Math.Abs(a);

        private static bool IsEqual(double a, double b)
            => IsZero(b - a);
    }
}
