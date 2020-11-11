namespace PikTools.Shared.FmHelpers
{
    using System;
    using System.IO;
    using Bimlab.Security.Client;
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
        public static void AddFmHelpers(this Container container, Func<IConfiguration> cfg = null)
        {
            container.Register(() => cfg == null
                ? new FmSettings()
                : cfg().GetSection("FmSettings").Get<FmSettings>(), Lifestyle.Singleton);
            container.Register(() =>
            {
                var settings = container.GetInstance<FmSettings>();
                return new TokenManager(new UtilityOptions
                {
                    ClientSecret = settings.ClientSecret,
                    ClientId = settings.ClientId,
                    AuthorityUri = settings.AuthorityUri,
                    Scope = settings.Scope,
                    UtilityPath = Path.Combine(
                        Path.GetDirectoryName(typeof(ContainerExtensions).Assembly.Location), "Bimlab.Security.AuthUtility.exe")
                });
            }, Lifestyle.Singleton);

            container.Register<IFamilyManagerService, FamilyManagerService>(Lifestyle.Singleton);
        }
    }
}