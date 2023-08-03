﻿namespace RxBim.Di.Tests
{
    using Microsoft.Extensions.DependencyInjection;
    using TestDependencies;

    public class TestDiConfigurator : DiConfigurator<IPluginConfiguration>
    {
        protected override void ConfigureBaseDependencies()
        {
            Container.Services.AddTransient<IBaseService, BaseService>();
        }
    }
}