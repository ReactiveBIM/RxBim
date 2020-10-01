namespace PikTools.Shared.FmHelpers
{
    using Microsoft.Extensions.Configuration;
    using PikTools.Shared.FmHelpers.Abstractions;
    using PikTools.Shared.FmHelpers.Models;
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
        /// <param name="cfg">Конфигурация</param>
        public static void AddFmHelpers(this Container container, IConfiguration cfg = null)
        {
            container.Register(() => cfg == null
                ? new FmSettings()
                : cfg.GetSection("FmSettings").Get<FmSettings>(), Lifestyle.Singleton);

            container.Register<IFamilyManagerService, FamilyManagerService>(Lifestyle.Singleton);
        }
    }
}