namespace RxBim.Application.Ribbon.Abstractions
{
    using Di;

    /// <summary>
    /// Plugin Ribbon Menu Build Service
    /// </summary>
    public interface IMenuBuildService
    {
        /// <summary>
        /// Builds a ribbon menu for a plugin
        /// </summary>
        /// <param name="container">DI container</param>
        void BuildMenu(IContainer container);
    }
}