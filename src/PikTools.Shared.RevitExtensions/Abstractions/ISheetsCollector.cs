namespace PikTools.Shared.RevitExtensions.Abstractions
{
    using System.Collections.Generic;

    /// <summary>
    /// Интерфейс коллектора листов
    /// </summary>
    public interface ISheetsCollector
    {
        /// <summary>
        /// Получить список названий листов, разбитый по документам
        /// </summary>
        /// <returns>Список названий листов, разбитый по документам</returns>
        Dictionary<string, List<string>> GetSheets();

        /// <summary>
        /// Получить список названий листов, разбитый по документам и дополнительному параметру
        /// </summary>
        /// <param name="groupSheetParam">Дополнительный параметр грцппировики листов</param>
        /// <returns>Список названий листов, разбитый по документам и дополнительному параметру</returns>
        Dictionary<string, Dictionary<string, List<string>>> GetSheets(string groupSheetParam);

        /// <summary>
        /// Получить список выбранных листов в основном документе
        /// </summary>
        /// <returns>Список выбранных листов</returns>
        IEnumerable<string> GetSelectedSheets();
    }
}
