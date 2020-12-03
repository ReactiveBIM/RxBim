namespace PikTools.Shared.RevitExtensions
{
    using Abstractions;
    using Collectors;
    using Services;
    using SimpleInjector;
    using Storages;

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
            container.Register<ISheetsCollector, SheetsCollector>(Lifestyle.Singleton);
            container.Register<IElementsDisplay, ElementsDisplayService>(Lifestyle.Singleton);
            container.Register<ISharedParameterService, SharedParameterService>(Lifestyle.Singleton);
            var collectorRegistration = Lifestyle.Singleton.CreateRegistration<ScopedElementsCollector>(container);
            container.AddRegistration<IElementsCollector>(collectorRegistration);
            container.AddRegistration<IScopedElementsCollector>(collectorRegistration);
            container.RegisterInstance(new RevitTask());
        }
    }
}