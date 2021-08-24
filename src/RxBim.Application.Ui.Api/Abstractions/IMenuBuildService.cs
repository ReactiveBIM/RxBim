namespace RxBim.Application.Ui.Api.Abstractions
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