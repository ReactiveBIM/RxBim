namespace PikTools.Shared.RevitExtensions.Abstractions
{
    using Autodesk.Revit.DB;

    /// <summary>
    /// Интерфейс коллектора элементов
    /// </summary>
    public interface IElementsCollector
    {
        /// <summary>
        /// Получить отфильтрованную коллекцию элементов
        /// </summary>
        /// <param name="doc">Текущий документ</param>
        /// <param name="ignoreFilter">Игнорировать внутренние фильтры</param>
        /// <returns>Отфильтрованная коллекция элементов</returns>
        FilteredElementCollector GetFilteredElementCollector(Document doc, bool ignoreFilter = false);

        /// <summary>
        /// Выбрать элемент на модели
        /// </summary>
        /// <param name="id">Идентификатор элемента</param>
        void SelectElement(int id);

        /// <summary>
        /// Приблизить элемент на виде
        /// </summary>
        /// <param name="id">Идентификатор элемента</param>
        void ZoomElement(int id);
    }
}
