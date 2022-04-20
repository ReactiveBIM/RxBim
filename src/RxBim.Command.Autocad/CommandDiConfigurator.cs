namespace RxBim.Command.Autocad
{
    using Autodesk.AutoCAD.ApplicationServices.Core;
    using Di;
    using Shared;

    /// <summary>
    /// Command DI configurator.
    /// </summary>
    internal class CommandDiConfigurator : DiConfigurator<ICommandConfiguration>
    {
        private readonly object _commandObject;

        /// <summary>
        /// Initialize a new instance of the <see cref="CommandDiConfigurator"/>.
        /// </summary>
        /// <param name="commandObject">The command object.</param>
        public CommandDiConfigurator(object commandObject)
        {
            _commandObject = commandObject;
        }

        /// <inheritdoc />
        protected override void ConfigureBaseDependencies()
        {
            Container
                .AddInstance(Application.DocumentManager.MdiActiveDocument)
                .AddInstance(Application.DocumentManager.MdiActiveDocument.Database)
                .AddInstance(Application.DocumentManager.MdiActiveDocument.Editor)
                .AddTransient<IMethodCaller<PluginResult>>(() => new MethodCaller<PluginResult>(_commandObject));
        }
    }
}