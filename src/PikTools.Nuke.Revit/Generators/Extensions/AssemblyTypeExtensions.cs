namespace PikTools.Nuke.Revit.Generators.Extensions
{
    using Nuke.Extensions;
    using Nuke.Models;

    /// <summary>
    /// Расширения для типов сборок
    /// </summary>
    public static class AssemblyTypeExtensions
    {
        /// <summary>
        /// Возвращает название свойства, соответствующее типу
        /// </summary>
        /// <param name="type">Тип</param>
        public static string ToPropertyName(this AssemblyType type)
        {
            return $"{type.BaseTypeName.ToPluginType()}__{type.FullName.ToPropertyName()}";
        }

        /// <summary>
        /// Преобразует строку в имя свойства для csproj
        /// </summary>
        /// <param name="str">исходная строка</param>
        private static string ToPropertyName(this string str)
        {
            return str.Replace(".", "_");
        }
    }
}