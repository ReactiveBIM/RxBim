namespace PikTools.Application.Ui.About
{
    using Di;
    using PikTools.Shared.Abstractions;
    using Services;

    /// <summary>
    /// Расширения для контейнера
    /// </summary>
    public static class ContainerExtensions
    {
        /// <summary>
        /// Добавляет сервисы UI в контейнер
        /// </summary>
        /// <param name="container">контейнер</param>
        public static void AddAbout(this IContainer container)
        {
            container.AddSingleton<IAboutShowService, AboutDialogService>();
        }
    }
}