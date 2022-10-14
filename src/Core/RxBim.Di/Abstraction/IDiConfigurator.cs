namespace RxBim.Di
{
    using System.Reflection;

    /// <summary>
    /// DI container configurator abstraction.
    /// </summary>
    public interface IDiConfigurator<T>
    {
        /// <summary>
        /// Configures a container using <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">An assembly.</param>
        void Configure(Assembly assembly);
    }
}