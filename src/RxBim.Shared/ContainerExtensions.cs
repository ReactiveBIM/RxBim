namespace RxBim.Shared
{
    using Abstractions;
    using Di;

    /// <summary>
    /// The DI container extensions.
    /// </summary>
    public static class ContainerExtensions
    {
        /// <summary>
        /// Adds shared tools in a <paramref name="container"/>.
        /// </summary>
        /// <param name="container">The DI container.</param>
        public static void AddSharedTools(this IContainer container)
        {
            container.AddSingleton<IUserSettings, UserSettings>();
            container.AddSingleton<IModelFactory>(() => new ModelFactory(container));
        }
    }
}
