namespace RxBim.Di
{
    /// <summary>
    /// A plugin configuration abstraction.
    /// </summary>
    public interface IPluginConfiguration
    {
        /// <summary>
        /// Configures DI <paramref name="container"/>
        /// </summary>
        /// <param name="container">A container.</param>
        void Configure(IContainer container);
    }
}