namespace RxBim.Shared.RevitExtensions.Helpers
{
    using Autodesk.Revit.DB;

    /// <summary>
    /// Опции загрузки семейства в документ
    /// </summary>
    internal class FamilyLoadOptions : IFamilyLoadOptions
    {
        private readonly bool _isOverWrite;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="isOverWrite">Перезаписать значения параметров</param>
        public FamilyLoadOptions(bool isOverWrite = true)
        {
            _isOverWrite = isOverWrite;
        }

        /// <inheritdoc/>
        public bool OnFamilyFound(bool familyInUse, out bool overwriteParameterValues)
        {
            overwriteParameterValues = _isOverWrite;
            return true;
        }

        /// <inheritdoc/>
        public bool OnSharedFamilyFound(
            Family sharedFamily,
            bool familyInUse,
            out FamilySource source,
            out bool overwriteParameterValues)
        {
            source = FamilySource.Family;
            overwriteParameterValues = true;
            return true;
        }
    }
}
