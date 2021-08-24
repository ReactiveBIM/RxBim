namespace RxBim.Shared.RevitExtensions.Abstractions
{
    using System.Collections.Generic;

    /// <summary>
    /// Интерфейс хранилища проблемных элементов
    /// </summary>
    public interface IProblemElementsStorage
    {
        /// <summary>
        /// Добавить проблемный элемент в хранилище
        /// </summary>
        /// <param name="id">Идентификатор элемента</param>
        /// <param name="problem">Описание проблемы</param>
        void AddProblemElement(int id, string problem);

        /// <summary>
        /// Получить скомбинированный по проблемам список id элементов
        /// </summary>
        /// <returns>Скомбинированный по проблемам список id элементов</returns>
        IDictionary<string, IEnumerable<int>> GetCombineProblems();

        /// <summary>
        /// Получить список проблем из хранилища
        /// </summary>
        /// <returns>Список проблем</returns>
        IEnumerable<KeyValuePair<int, string>> GetProblems();

        /// <summary>
        /// Есть ли проблемные элементы в хранилище
        /// </summary>
        /// <returns>true - есть, иначе - false</returns>
        bool HasProblems();

        /// <summary>
        /// Очистить хранилище
        /// </summary>
        void Clear();
    }
}
