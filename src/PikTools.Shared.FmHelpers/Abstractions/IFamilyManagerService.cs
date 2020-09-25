namespace PikTools.Shared.FmHelpers.Abstractions
{
    using Autodesk.Revit.DB;

    /// <summary>
    /// Интерфейс для сервиса работы с FM
    /// </summary>
    public interface IFamilyManagerService
    {
        /// <summary>
        /// Загрузить семейство из FM в заданный документ
        /// </summary>
        /// <param name="doc">Документ Revit</param>
        /// <param name="familyName">Название семейства</param>
        /// <returns>Тип загруженного семейства</returns>
        FamilySymbol GetTargetFamilySymbol(Document doc, string familyName);
    }
}
