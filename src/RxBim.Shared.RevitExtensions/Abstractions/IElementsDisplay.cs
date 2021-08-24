namespace RxBim.Shared.RevitExtensions.Abstractions
{
    using System.Collections.Generic;

    /// <summary>
    /// Отображение элементов Revit в модели проекта
    /// </summary>
    public interface IElementsDisplay
    {
        /// <summary>
        /// Выбрать элементы в модели
        /// </summary>
        /// <param name="elementIds">Коллекция идентификаторов элементов</param>
        void SetSelectedElements(IList<int> elementIds);
        
        /// <summary>
        /// Выбрать элемент в модели
        /// </summary>
        /// <param name="elementId">Идентификатор элемента</param>
        void SetSelectedElement(int elementId);
        
        /// <summary>
        /// Сбросить текущее выделение элементов в модели
        /// </summary>
        void ResetSelection();

        /// <summary>
        /// Приблизить элемент на виде
        /// </summary>
        /// <param name="elementId">Идентификатор элемента</param>
        /// <param name="zoomFactor">Фактор увеличения или уменьшения масштаба.
        /// Значения больше 1 - увеличение, меньше 1 - уменьшение.</param>
        void ZoomElement(int elementId, double zoomFactor = 0.25);
    }
}
