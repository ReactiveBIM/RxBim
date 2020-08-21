namespace PikTools.Shared.RevitExtensions.Abstractions
{
    using System.Collections.Generic;

    /// <summary>
    /// Интерфейс коллектора документов
    /// </summary>
    public interface IDocumentsCollector
    {
        /// <summary>
        /// Получить заголовок основного документа
        /// </summary>
        /// <returns></returns>
        string GetMainDocumentTitle();

        /// <summary>
        /// Получить заголовка всех документов, связанны с заданным (и его самого)
        /// </summary>
        /// <returns>Заголовки документов</returns>
        IEnumerable<string> GetDocumentsTitles();
    }
}
