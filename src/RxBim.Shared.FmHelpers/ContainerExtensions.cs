namespace RxBim.Shared.FmHelpers
{
    using System;
    using System.IO;
    using Abstractions;
    using Bimlab.Security.Client;
    using Di;
    using Microsoft.Extensions.Configuration;
    using Models;
    using Services;

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
        public static void AddFmHelpers(this IContainer container, Func<IConfiguration> cfg = null)
        {
            container.AddSingleton(() => cfg == null
                ? new FmSettings()
                : cfg().GetSection("FmSettings").Get<FmSettings>());
            container.AddSingleton(() =>
            {
                var settings = container.GetService<FmSettings>();
                return new TokenManager(new UtilityOptions
                {
                    ClientSecret = settings.ClientSecret,
                    ClientId = settings.ClientId,
                    AuthorityUri = settings.AuthorityUri,
                    Scope = settings.Scope,
                    UtilityPath = Path.Combine(
                        Path.GetDirectoryName(typeof(ContainerExtensions).Assembly.Location),
                        "Bimlab.Security.AuthUtility.exe")
                });
            });

            container.AddSingleton<IFamilyManagerService, FamilyManagerService>();
        }
    }
}