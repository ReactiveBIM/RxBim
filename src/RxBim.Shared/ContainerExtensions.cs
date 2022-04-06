namespace RxBim.Shared
{
    using System.Linq;
    using System.Reflection;
    using Abstractions;
    using Di;

    /// <summary>
    /// <see cref="IContainer"/> extensions for shared services.
    /// </summary>
    public static class ContainerExtensions
    {
        /// <summary>
        /// Добавляет общие вспомогательные сервисы
        /// </summary>
        /// <param name="container">DI контейнер</param>
        public static void AddSharedTools(this IContainer container)
        {
            container.AddSingleton<IUserSettings, UserSettings>();
            container.AddSingleton<IModelFactory>(() => new ModelFactory(container));
        }
    }
}