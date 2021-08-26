namespace RxBim.Application.Ribbon.Abstractions
{
    /// <summary>
    /// Строитель ленты
    /// </summary>
    public interface IRibbonBuilder
    {
        /// <summary>
        /// Возвращает ленту
        /// </summary>
        IRibbon And();
    }
}