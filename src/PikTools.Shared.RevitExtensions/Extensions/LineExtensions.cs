namespace PikTools.Shared.RevitExtensions.Extensions
{
    using System;
    using Autodesk.Revit.DB;

    /// <summary> Расширения для <see cref="Line"/> </summary>
    public static class LineExtensions
    {
        private const double Tolerance = 0.0001;

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
        /// Возвращает отрезок, спроецированный на плоскость
        /// </summary>
        /// <param name="plane">Плоскость</param>
        /// <param name="line">Исходный отрезок</param>
        public static Line ProjectOnto(this Plane plane, Line line)
            => Line.CreateBound(
                    plane.ProjectOnto(line.GetEndPoint(0)),
                    plane.ProjectOnto(line.GetEndPoint(1)));

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
        /// Лежат ли текущий и проверяемый отрезок на одной прямой.
        /// </summary>
        /// <param name="firstLine">Текущий отрезок</param>
        /// <param name="secondLine">Проверяемый отрезок</param>
        /// <param name="tolerance">Допуск на сравнение чисел</param>
        public static bool IsLieOnSameStraightLine(this Line firstLine, Line secondLine, double tolerance = Tolerance)
        {
            // Если два отрезка не параллельны, то и дальнейшая проверка не требуется
            if (!firstLine.IsParallelTo(secondLine))
                return false;

            var firstLinePoints = firstLine.GetEndpoints();
            var secondLinePoints = secondLine.GetEndpoints();

            // Вектор первого отрезка будем использовать как эталон проверки.
            // Свойство Direction всегда содержит единичный вектор
            var fv = firstLine.Direction;

            // Нам требуется проверять попарно концевые точки первого отрезка с концевыми точками второго.
            // Это удобно сделать двумя итерациями
            foreach (var firstLinePoint in firstLinePoints)
            {
                foreach (var secondLinePoint in secondLinePoints)
                {
                    // Если два отрезка будут будут иметь общую концевую точку, то проверка сработает не верно,
                    // так как произведение векторов даст 0.0. Такие пары просто пропускаем
                    if (Math.Abs(firstLinePoint.DistanceTo(secondLinePoint)) < tolerance)
                        continue;

                    // Не важно из какой какую точку отнимать. Главное, привести к единичному вектору
                    var v = (secondLinePoint - firstLinePoint).Normalize();

                    // Если вектора не параллельны, то и отрезки не лежат на одной прямой
                    if (!fv.IsParallelTo(v))
                        return false;
                }
            }

            // Если в предыдущих итерациях мы не вышли из метода, значит два отрезка лежат на одной прямой
            return true;
        }

        /// <summary>
        /// Проверка параллельности двух векторов
        /// </summary>
        /// <param name="vector">Первый вектор</param>
        /// <param name="checkedVector">Второй вектор</param>
        /// <param name="tolerance">Допуск расстояния при проверке</param>
        private static bool IsParallelTo(this XYZ vector, XYZ checkedVector, double tolerance = Tolerance)
        {
            return Math.Abs(Math.Abs(vector.DotProduct(checkedVector)) - 1.0) < tolerance;
        }
    }
}
