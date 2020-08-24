namespace PikTools.Shared.Ui
{
    using PikTools.Shared.RevitExtensions.Abstractions;
    using PikTools.Shared.RevitExtensions.Collectors;
    using PikTools.Shared.RevitExtensions.Storages;
    using SimpleInjector;

    /// <summary>
    /// Расширения для контейнера
    /// </summary>
    public static class ContainerExtensions
    {
        /// <summary>
        /// Добавляет сервисы работы с Revit в контейнер
        /// </summary>
        /// <param name="container">контейнер</param>
        public static void AddRevitHelpers(this Container container)
        {
            container.Register<IProblemElementsStorage, ProblemElementsStorage>(Lifestyle.Singleton);
            container.Register<IDocumentsCollector, DocumentsCollector>(Lifestyle.Singleton);
            var collectorRegistration = Lifestyle.Singleton.CreateRegistration<ScopedElementsCollector>(container);
            container.AddRegistration<IElementsCollector>(collectorRegistration);
            container.AddRegistration<IScopedElementsCollector>(collectorRegistration);
            container.Register<RevitTask>(Lifestyle.Singleton);
        }
    }
}