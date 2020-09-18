namespace PikTools.Shared.RevitExtensions.Extensions
{
    using System.Linq;
    using Autodesk.Revit.DB;

    /// <summary>
    /// Расширения для геометрии в Revit
    /// </summary>
    public static class GeometryExtensions
    {
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
    }
}
