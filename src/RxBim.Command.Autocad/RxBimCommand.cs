namespace RxBim.Command.Autocad
{
    using System.Reflection;
    using Di;
    using Shared;

    /// <summary>
    /// Команда Autocad
    /// </summary>
    public abstract class RxBimCommand
    {
        /// <summary>
        /// Настройка и выполнение команды
        /// </summary>
        public void Execute()
        {
            var assembly = GetType().Assembly;
            var di = Configure(assembly);
            CallCommandMethod(di);
        }

        private CommandDiConfigurator Configure(Assembly assembly)
        {
            var di = new CommandDiConfigurator(this);
            di.Configure(assembly);
            return di;
        }

        private void CallCommandMethod(CommandDiConfigurator di)
        {
            var methodCaller = di.Container.GetService<IMethodCaller<PluginResult>>();
            methodCaller.InvokeCommand(di.Container, Constants.ExecuteMethodName);
        }
    }
}