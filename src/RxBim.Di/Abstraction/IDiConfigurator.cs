namespace RxBim.Di
{
    using System.Reflection;

    /// <summary>
    /// Конфигурато di контейнера
    /// </summary>
    public interface IDiConfigurator<T>
    {
        /// <summary>
        /// Configure
        /// </summary>
        /// <param name="assembly">assembly</param>
        void Configure(Assembly assembly);
    }
}