namespace RxBim.Application.Ribbon.Abstractions
{
    using Di;

    /// <summary>
    /// Сервис построения меню
    /// </summary>
    public interface IMenuBuildService
    {
        /// <summary>
        /// Построение меню
        /// </summary>
        /// <param name="container">DI-Контейнер</param>
        void BuildMenu(IContainer container);
    }
}