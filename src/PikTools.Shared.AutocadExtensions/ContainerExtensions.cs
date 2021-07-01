namespace PikTools.Shared.AutocadExtensions
{
    using Abstractions;
    using Di;
    using Services;

    /// <summary>
    /// Расширения для контейнера
    /// </summary>
    public static class ContainerExtensions
    {
        /// <summary>
        /// Добавляет сервисы работы с AutoCAD в контейнер
        /// </summary>
        /// <param name="container">Контейнер</param>
        public static void AddAutocadHelpers(this IContainer container)
        {
            container.AddSingleton<IObjectsSelectionService, ObjectsSelectionService>();
            container.AddSingleton<ICommandLineService, CommandLineService>();
        }
    }
}