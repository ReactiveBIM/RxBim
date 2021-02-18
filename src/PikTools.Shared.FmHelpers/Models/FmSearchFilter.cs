namespace PikTools.Shared.FmHelpers.Models
{
    /// <summary>
    /// Фильтр поиска семейств Fm
    /// </summary>
    public class FmSearchFilter
    {
        /// <summary> Имя семейства </summary>
        public string Name { get; set; }

        /// <summary> Имя типоразмера </summary>
        public string SymbolName { get; set; }

        /// <summary> Имя функционального типа </summary>
        public string FunctionalTypeName { get; set; }

        /// <summary> Категория </summary>
        public string CategoryName { get; set; }

        /// <summary> Лимит </summary>
        public int? Limit { get; set; }
    }
}
