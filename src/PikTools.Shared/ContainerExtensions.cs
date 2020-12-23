namespace PikTools.Shared
{
    using Abstractions;
    using SimpleInjector;

    /// <summary>
    /// Расширения для контейнера
    /// </summary>
    public static class ContainerExtensions
    {
        /// <summary>
        /// Добавляет общие вспомогательные сервисы
        /// </summary>
        /// <param name="container">DI контейнер</param>
        public static void AddSharedTools(this Container container)
        {
            container.Register<IUserSettings, UserSettings>(Lifestyle.Singleton);
            container.Register<IModelFactory>(() => new ModelFactory(container), Lifestyle.Singleton);
        }
    }
}
