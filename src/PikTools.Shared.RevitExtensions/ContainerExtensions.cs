namespace PikTools.Shared.RevitExtensions
{
    using Abstractions;
    using Collectors;
    using Di;
    using Services;
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
        public static void AddRevitHelpers(this IContainer container)
        {
            container.AddSingleton<IProblemElementsStorage, ProblemElementsStorage>();
            container.AddSingleton<IDocumentsCollector, DocumentsCollector>();
            container.AddSingleton<ISheetsCollector, SheetsCollector>();
            container.AddSingleton<IElementsDisplay, ElementsDisplayService>();
            container.AddSingleton<ISharedParameterService, SharedParameterService>();
            container.AddSingleton<IElementsCollector, ScopedElementsCollector>();
            container.AddSingleton<IScopedElementsCollector, ScopedElementsCollector>();
            container.AddSingleton<ITransactionService, TransactionService>();
            container.AddInstance(new RevitTask());
        }
    }
}