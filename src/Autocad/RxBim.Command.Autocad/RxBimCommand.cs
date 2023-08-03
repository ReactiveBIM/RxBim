namespace RxBim.Command.Autocad
{
    using System.Reflection;
    using Di;
    using Microsoft.Extensions.DependencyInjection;
    using Shared;

    /// <summary>
    /// Autocad command.
    /// </summary>
    public abstract class RxBimCommand
    {
        /// <summary>
        /// Executes a command.
        /// </summary>
        // ReSharper disable once MemberCanBeProtected.Global
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
            var methodCaller = di.Container.ServiceProvider.GetRequiredService<IMethodCaller<PluginResult>>();
            methodCaller.InvokeMethod(di.Container, Constants.ExecuteMethodName);
        }
    }
}