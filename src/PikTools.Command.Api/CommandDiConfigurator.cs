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
            Container.RegisterInstance(_commandData);
            Container.RegisterInstance(_commandData.Application);
            Container.RegisterInstance(_commandData.Application.Application);
            Container.Register(() => _commandData.Application.ActiveUIDocument);
            Container.Register(() => _commandData.Application.ActiveUIDocument.Document);
            Container.Register<IMethodCaller<PluginResult>>(() => new MethodCaller<PluginResult>(_commandObject));
        }
    }
}