namespace RxBim.Shared.AutocadExtensions.Abstractions
{
    using System.Collections.Generic;
    using Autodesk.AutoCAD.DatabaseServices;

    /// <summary>
    /// Результат выбора объектов
    /// </summary>
    public interface IObjectsSelectionResult
    {
        /// <summary>
        /// Возвращает истину если было введено ключевое слово вместо выбора объектов
        /// </summary>
        bool IsKeyword { get; }

        /// <summary>
        /// Возвращает коллекцию ObjectId выбранных объектов
        /// </summary>
        IEnumerable<ObjectId> SelectedObjects { get; }

        /// <summary>
        /// Возвращает введённое ключевое слово
        /// </summary>
        string Keyword { get; }

        /// <summary>
        /// Возвращает истину, если не были выбраны объекты и не было введено ключевое слово
        /// </summary>
        bool IsEmpty { get; }
    }
}