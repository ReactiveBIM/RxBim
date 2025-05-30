﻿namespace RxBim.Command.Revit
{
    using Autodesk.Revit.UI;
    using Di;
    using Microsoft.Extensions.DependencyInjection;
    using Shared;

    /// <summary>
    /// Revit command DI container configurator.
    /// </summary>
    internal class CommandDiConfigurator : DiConfigurator<ICommandConfiguration>
    {
        private readonly object _commandObject;
        private readonly ExternalCommandData _commandData;

        /// <summary>
        /// Initialize a new instance of <see cref="CommandDiConfigurator"/>.
        /// </summary>
        /// <param name="commandObject">Command object.</param>
        /// <param name="commandData">Revit command data.</param>
        public CommandDiConfigurator(object commandObject, ExternalCommandData commandData)
        {
            _commandObject = commandObject;
            _commandData = commandData;
        }

        /// <inheritdoc />
        protected override void ConfigureBaseDependencies()
        {
            Services
                .AddSingleton(_commandData)
                .AddSingleton(_commandData.Application)
                .AddSingleton(_commandData.Application.Application)
                .AddTransient(_ => _commandData.Application.ActiveUIDocument)
                .AddTransient(_ => _commandData.Application.ActiveUIDocument?.Document!)
                .AddTransient<IMethodCaller<PluginResult>>(_ => new MethodCaller<PluginResult>(_commandObject));
        }
    }
}