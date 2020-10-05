namespace PikTools.Shared.RevitExtensions.Abstractions
{
    using Autodesk.Revit.DB;
    using Models;

    /// <summary>
    /// Сервис по работе с общими параметрами
    /// </summary>
    public interface ISharedParameterService
    {
        /// <summary>
        /// Попытка создания параметра. Если параметр с указанным именем уже существует, то метод вернет True без
        /// каких-либо действий над параметром
        /// </summary>
        /// <remarks>Метод создает транзакцию</remarks>
        /// <param name="sharedParameterInfo">Данные об общем параметре</param>
        /// <param name="fullMatch">True - параметр ФОП должен совпасть со всеми заполненными
        /// значениями sharedParameterInfo. False - параметр ищется только по имени
        /// <para/>При поиске в ФОП, если задано true, происходит проверка по всем свойствам sharedParameterInfo, которые имеют значение
        /// <para/>При поиске в текущем документе, если задано true, происходит проверка только по свойствам:
        /// Имя, Guid, DataType. Если последние два имеют значение у sharedParameterInfo
        /// </param>
        void AddSharedParameter(SharedParameterInfo sharedParameterInfo, bool fullMatch);

        /// <summary>
        /// Проверка параметра, представленного экземпляром <see cref="SharedParameterElement"/>, на существование
        /// в файле общих параметров
        /// </summary>
        /// <param name="sharedParameterInfo">Данные об общем параметре</param>
        /// <param name="fullMatch">True - параметр ФОП должен совпасть со всеми заполненными
        /// значениями sharedParameterInfo. False - параметр ищется только по имени</param>
        bool IsParameterExistInSpf(SharedParameterInfo sharedParameterInfo, bool fullMatch);
    }
}
