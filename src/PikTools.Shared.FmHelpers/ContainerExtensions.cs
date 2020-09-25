namespace PikTools.Shared.FmHelpers
{
    using PikTools.Shared.FmHelpers.Abstractions;
    using PikTools.Shared.FmHelpers.Services;
    using SimpleInjector;

    /// <summary>
    /// Расширения для контейнера
    /// </summary>
    public static class ContainerExtensions
    {
        /// <summary>
        /// Добавляет сервисы работы с Family Manager в контейнер
        /// </summary>
        /// <param name="container">контейнер</param>
        public static void AddFmHelpers(this Container container)
        {
            container.Register<IFamilyManagerService, FamilyManagerService>(Lifestyle.Singleton);
        }
    }
}