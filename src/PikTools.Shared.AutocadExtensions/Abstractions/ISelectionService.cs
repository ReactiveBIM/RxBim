namespace PikTools.Shared.AutocadExtensions.Abstractions
{
    /// <summary>
    /// Сервис выбора объектов
    /// </summary>
    /// <typeparam name="T">Тип результата выбора</typeparam>
    public interface ISelectionService<out T>
        where T : ISelectionResult
    {
        /// <summary>
        /// Выбор объектов
        /// </summary>
        T Select();
    }
}