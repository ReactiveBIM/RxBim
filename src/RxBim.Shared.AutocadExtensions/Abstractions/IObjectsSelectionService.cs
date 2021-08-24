namespace RxBim.Shared.AutocadExtensions.Abstractions
{
    using System;
    using System.Collections.Generic;
    using Autodesk.AutoCAD.DatabaseServices;

    /// <summary>
    /// Сервис выбора объектов
    /// </summary>
    public interface IObjectsSelectionService
    {
        /// <summary>
        /// Проверка объекта по идентификатору. Если возвращает истину - объект может быть выбран.
        /// </summary>
        Func<ObjectId, bool> CanBeSelected { get; set; }

        /// <summary>
        /// Задаёт сообщение и ключевые слова в опции выбора
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="keywordGlobalAndLocalNames">Глобальные и локализованные имена ключевых слов</param>
        void SetMessageAndKeywords(string message, Dictionary<string, string>? keywordGlobalAndLocalNames = null);

        /// <summary>
        /// Запускает выбор объектов и возвращает результат выбора
        /// </summary>
        IObjectsSelectionResult RunSelection();
    }
}