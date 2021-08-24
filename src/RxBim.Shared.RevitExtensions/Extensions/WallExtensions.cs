namespace RxBim.Shared.RevitExtensions.Extensions
{
    using System;
    using System.Linq;
    using Autodesk.Revit.DB;

    /// <summary>
    /// Расширения для стен
    /// </summary>
    public static class WallExtensions
    {
        private const double Tolerance = 0.0001;

        /// <summary>
        /// Получить линию расположения стены
        /// </summary>
        /// <param name="wall">Стена</param>
        /// <returns>Линию расположения стены</returns>
        public static Line GetLocationLine(this Wall wall)
        {
            if (!(wall.Location is LocationCurve lc)
                || !(lc.Curve is Line line))
                return null;

            return line;
        }

        /// <summary>
        /// Получить лини стены
        /// </summary>
        /// <param name="element">Стена</param>
        /// <returns>Линии стены</returns>
        public static Line[] GetLines(this Wall element)
        {
            var vs = element.GetVertices();
            if (vs == null)
                return null;

            var minZ = vs.OrderBy(v => v.Z).First().Z;

            var vsAtZ = vs
                .Select(v => new XYZ(v.X, v.Y, minZ))
                .GetUnique()
                .ToArray();

            if (vsAtZ.Length != 4)
                return null;

            var vsByX = vsAtZ
                .OrderBy(pt => pt.X)
                .ToArray();

            var left = Line.CreateBound(vsByX[0], vsByX[1]);

            var right = Line.CreateBound(vsByX[2], vsByX[3]);

            var vsByY = vsAtZ
                .OrderBy(pt => pt.Y)
                .ToArray();

            var top = Line.CreateBound(vsByY[0], vsByY[1]);

            var bottom = Line.CreateBound(vsByY[2], vsByY[3]);

            var ls = new Line[]
            {
                left,
                right,
                top,
                bottom,
            };

            return ls;
        }

        /// <summary>
        /// Получить длину стены
        /// </summary>
        /// <param name="wall">Стена</param>
        /// <returns>Длина стены</returns>
        public static double GetLength(this Wall wall)
        {
            var axis = wall.GetLocationLine();

            var longs = wall
                ?.GetLines()
                .FirstOrDefault(line => axis.Direction.CrossProduct(line.Direction).GetLength() < Tolerance);

            if (longs == null)
                return 0d;

            return longs.Length.FtToMm();
        }

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
    }
}
