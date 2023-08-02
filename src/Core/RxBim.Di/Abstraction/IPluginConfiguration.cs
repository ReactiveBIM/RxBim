namespace RxBim.Di
{
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// A plugin configuration abstraction.
    /// </summary>
    public interface IPluginConfiguration
    {
        /// <summary>
        /// Configures DI <paramref name="services"/>
        /// </summary>
        /// <param name="services">A container.</param>
        void Configure(IServiceCollection services);
    }
}