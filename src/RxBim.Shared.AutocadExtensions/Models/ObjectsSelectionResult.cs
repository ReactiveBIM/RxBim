namespace RxBim.Shared.AutocadExtensions.Models
{
    using System.Collections.Generic;
    using Abstractions;
    using Autodesk.AutoCAD.DatabaseServices;

    /// <summary>
    /// Результат выбора
    /// </summary>
    public class ObjectsSelectionResult : IObjectsSelectionResult
    {
        /// <summary>
        /// Пустой результат
        /// </summary>
        public static ObjectsSelectionResult Empty { get; } = new ObjectsSelectionResult();

        /// <summary>
        /// Флаг того, что было введено ключевое слово вместо выбора объектов
        /// </summary>
        public bool IsKeyword { get; set; }

        /// <summary>
        /// Коллекция ObjectId выбранных объектов - если были выбраны объекты
        /// </summary>
        public IEnumerable<ObjectId> SelectedObjects { get; set; } = new ObjectId[0];

        /// <summary>
        /// Ключевое слово - если было введено ключевое слово
        /// </summary>
        public string Keyword { get; set; } = string.Empty;

        /// <summary>
        /// Выбор пустой
        /// </summary>
        public bool IsEmpty => ReferenceEquals(this, Empty);
    }
}