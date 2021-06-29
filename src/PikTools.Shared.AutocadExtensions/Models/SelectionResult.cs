namespace PikTools.Shared.AutocadExtensions.Models
{
    using Abstractions;
    using Autodesk.AutoCAD.DatabaseServices;

    /// <summary>
    /// Результат выбора
    /// </summary>
    public class SelectionResult : ISelectionResult
    {
        /// <summary>
        /// Пустой результат
        /// </summary>
        public static SelectionResult Empty { get; } = new SelectionResult();

        /// <summary>
        /// Флаг того, что было введено ключевое слово вместо выбора объектов
        /// </summary>
        public bool IsKeyword { get; set; }

        /// <summary>
        /// Коллекция ObjectId выбранных объектов - если были выбраны объекты
        /// </summary>
        public ObjectId[] SelectedObjects { get; set; } = new ObjectId[0];

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