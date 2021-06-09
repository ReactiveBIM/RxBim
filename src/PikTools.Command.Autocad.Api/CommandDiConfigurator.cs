namespace PikTools.Command.Autocad.Api
{
    using Autodesk.AutoCAD.ApplicationServices.Core;
    using Di;
    using Shared;

    /// <summary>
    /// Конфигуратор зависимостей команды
    /// </summary>
    internal class CommandDiConfigurator : DiConfigurator<ICommandConfiguration>
    {
        private readonly object _commandObject;

        /// <summary>
        /// Создает экземпляр класса <see cref="CommandDiConfigurator"/>
        /// </summary>
        /// <param name="commandObject">Объект команды</param>
        public CommandDiConfigurator(object commandObject)
        {
            _commandObject = commandObject;
        }

        /// <inheritdoc />
        protected override void ConfigureBaseDependencies()
        {
            Container
                 //// Регистрация документа, базы данных и редактора для текущего чертежа
                .AddInstance(Application.DocumentManager.MdiActiveDocument)
                .AddInstance(Application.DocumentManager.MdiActiveDocument.Database)
                .AddInstance(Application.DocumentManager.MdiActiveDocument.Editor)
                .AddTransient<IMethodCaller<PluginResult>>(() => new MethodCaller<PluginResult>(_commandObject));
        }
    }
}