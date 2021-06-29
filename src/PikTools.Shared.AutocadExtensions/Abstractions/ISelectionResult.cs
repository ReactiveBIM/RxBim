namespace PikTools.Shared.AutocadExtensions.Abstractions
{
    /// <summary>
    /// Результат выбора
    /// </summary>
    public interface ISelectionResult
    {
        /// <summary>
        /// Выбор пустой
        /// </summary>
        bool IsEmpty { get; }
    }
}