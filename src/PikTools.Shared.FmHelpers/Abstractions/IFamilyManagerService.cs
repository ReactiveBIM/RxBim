namespace PikTools.Shared.FmHelpers.Abstractions
{
    using System;
    using System.Collections.Generic;
    using Autodesk.Revit.DB;
    using CSharpFunctionalExtensions;
    using Models;

    /// <summary>
    /// Интерфейс для сервиса работы с FM
    /// </summary>
    public interface IFamilyManagerService
    {
        /// <summary>
        /// Загрузить типоразмер семейства из FM в заданный документ
        /// </summary>
        /// <param name="doc">Документ Revit</param>
        /// <param name="familyName">Название семейства</param>
        /// <param name="symbolName">Название типоразмера</param>
        /// <param name="useTransaction">Оборачивать ли в транзакцию</param>
        /// <param name="familyLoadOptions">Настройки загрузки семейства</param>
        /// <returns>Тип загруженного семейства</returns>
        Result<FamilySymbol> GetTargetFamilySymbol(
            Document doc,
            string familyName,
            string symbolName,
            bool useTransaction = true,
            IFamilyLoadOptions familyLoadOptions = null);

        /// <summary>
        /// Загрузить семейство из FM в заданный документ
        /// </summary>
        /// <param name="doc">Документ Revit</param>
        /// <param name="familyName">Название семейства</param>
        /// <param name="useTransaction">Оборачивать ли в транзакцию</param>
        /// <param name="familyLoadOptions">Настройки загрузки семейства</param>
        /// <returns>Загруженное семейство</returns>
        [Obsolete]
        Result<Family> GetTargetFamily(
            Document doc,
            string familyName,
            bool useTransaction = true,
            IFamilyLoadOptions familyLoadOptions = null);

        /// <summary>
        /// Возвращает семейства по фильтру
        /// </summary>
        /// <param name="doc">Документ Revit</param>
        /// <param name="filter">Фильтр</param>
        /// <param name="familyLoadOptions">Настройки загрузки семейства</param>
        /// <returns>Список загруженных семейств</returns>
        Result<List<Family>> Search(
            Document doc,
            FmSearchFilter filter,
            IFamilyLoadOptions familyLoadOptions = null);
    }
}
