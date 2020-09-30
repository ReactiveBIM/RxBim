namespace PikTools.Shared.RevitExtensions.Abstractions
{
    using Autodesk.Revit.DB;

    /// <summary>
    /// Варианты частей
    /// </summary>
    public enum ScopeType
    {
        /// <summary>
        /// Вся модель
        /// </summary>
        AllModel = 0,

        /// <summary>
        /// Активный вид
        /// </summary>
        ActiveView = 1,

        /// <summary>
        /// Выбранные элементы
        /// </summary>
        SelectedElements = 2
    }

    /// <summary>
    /// Интерфейс коллектора части элементов
    /// </summary>
    public interface IScopedElementsCollector : IElementsCollector
    {
        /// <summary>
        /// Какая часть элементов выбрана
        /// </summary>
        ScopeType Scope { get; }

        /// <summary>
        /// Задать тип части для коллектора
        /// </summary>
        /// <param name="scope">Вариант части</param>
        void SetScope(ScopeType scope);

        /// <summary>
        /// Имеются ли элементы
        /// </summary>
        /// <param name="doc">Текущий документ</param>
        /// <returns>true - есть элементы, иначе - false</returns>
        bool HasElements(Document doc);

        /// <summary>
        /// Вернуть выделение сохраненным ранее элементам
        /// </summary>
        void SetBackSelectedElements();
    }
}
