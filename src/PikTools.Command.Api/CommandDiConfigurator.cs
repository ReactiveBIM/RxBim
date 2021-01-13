namespace PikTools.Command.Api
{
    using Autodesk.Revit.UI;
    using Di;
    using Shared;

    /// <summary>
    /// Конфигуратор зависимостей команды
    /// </summary>
    internal class CommandDiConfigurator : DiConfigurator<ICommandConfiguration>
    {
        private readonly object _commandObject;
        private readonly ExternalCommandData _commandData;

        /// <summary>
        /// Создает экземпляр класса <see cref="CommandDiConfigurator"/>
        /// </summary>
        /// <param name="commandObject">Объект команды</param>
        /// <param name="commandData">Объект, содержащий ссылки на приложение и интерфейс Revit</param>
        public CommandDiConfigurator(object commandObject, ExternalCommandData commandData)
        {
            _commandObject = commandObject;
            _commandData = commandData;
        }

        /// <inheritdoc />
        protected override void ConfigureBaseDependencies()
        {
            Container
                .AddInstance(_commandData)
                .AddInstance(_commandData.Application)
                .AddInstance(_commandData.Application.Application)
                .AddTransient(() => _commandData.Application.ActiveUIDocument)
                .AddTransient(() => _commandData.Application.ActiveUIDocument?.Document)
                .AddTransient<IMethodCaller<PluginResult>>(() => new MethodCaller<PluginResult>(_commandObject));
        }
    }
}