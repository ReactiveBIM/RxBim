namespace RxBim.Command.Autocad.Base
{
    using System.Reflection;
    using Di;
    using Shared;

    /// <summary>
    /// Base Autocad command.
    /// </summary>
    public abstract class RxBimCommandBase
    {
        /// <summary>
        /// Executes a command.
        /// </summary>
        public virtual void Execute()
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
            methodCaller.InvokeMethod(di.Container, Constants.ExecuteMethodName);
        }
    }
}