namespace PikTools.Shared.Ui
{
    using PikTools.Shared.Ui.Abstractions;
    using PikTools.Shared.Ui.Services;
    using PikTools.Shared.Ui.ViewModels;
    using SimpleInjector;

    /// <summary>
    /// Расширения для контейнера
    /// </summary>
    public static class ContainerExtensions
    {
        /// <summary>
        /// Добавляет сервисы UI в контейнер
        /// </summary>
        /// <param name="container">контейнер</param>
        public static void AddUi(this Container container)
        {
            container.Register<IUIDispatcher, UIDispatcher>(Lifestyle.Singleton);
            container.Register<INotificationService, NotificationService>(Lifestyle.Singleton);
            container.RegisterInstance<INotificationViewModel>(new NotificationViewModel());
        }
    }
}