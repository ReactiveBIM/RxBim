namespace RxBim.Application.Ribbon.Abstractions
{
    /// <summary>
    /// Лента
    /// </summary>
    public interface IRibbon
    {
        /// <summary>
        /// Создает или возвращает существующую вкладку />
        /// </summary>
        /// <param name="tabTitle">Заголовок вкладки</param>
        ITab Tab(string tabTitle = null);
    }
}