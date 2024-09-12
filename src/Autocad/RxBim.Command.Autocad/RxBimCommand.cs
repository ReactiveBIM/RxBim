namespace RxBim.Command.Autocad
{
    using System;
    using System.Reflection;
    using Di;
    using Shared;

    /// <summary>
    /// Autocad command.
    /// </summary>
    public abstract class RxBimCommand
    {
        /// <summary>
        /// Executes a command.
        /// </summary>
        public virtual void Execute()
        {
#if NETCOREAPP
            var type = GetType();
            if (PluginContext.IsCurrentContextDefault(type))
            {
                var instance = PluginContext.CreateInstance(type);
                if (instance != null)
                {
                    var instanceType = instance.GetType();
                    var method = instanceType.GetMethod(nameof(Execute));
                    method?.Invoke(instance, Array.Empty<object>());
                    return;
                }
            }
#endif

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