namespace RxBim.Di
{
    /// <summary>
    /// A plugin configuration abstraction.
    /// </summary>
    public interface IPluginConfiguration
    {
        /// <summary>
        /// Configures DI <param name="container">container</param>.
        /// </summary>
        /// <param name="container">A container.</param>
        void Configure(IContainer container);
    }
}