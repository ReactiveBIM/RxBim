namespace RxBim.Command.Autocad
{
    using System;
    using System.Reflection;
    using Di;
    using Microsoft.Extensions.DependencyInjection;
    using Shared;

    /// <summary>
    /// Autocad command.
    /// </summary>
    public abstract class RxBimCommand
    {
#if ACAD2025
        /// <summary>
        /// Allows you to turn off plugin execution in separated context.
        /// </summary>
        protected virtual bool RunInSeparatedContext => true;
#endif

        /// <summary>
        /// Executes a command.
        /// </summary>
        public virtual void Execute()
        {
            var type = GetType();
            var assembly = type.Assembly;
#if ACAD2025
            if (RunInSeparatedContext && PluginContext.IsCurrentContextDefault(type))
            {
                var newInstance = PluginContext.CreateInstanceInNewContext(type);
                if (newInstance is RxBimCommand rxBimCommand)
                {
                    rxBimCommand.Execute();
                    return;
                }
            }
#endif

            CallCommandMethod(assembly);
        }

        private IServiceProvider Configure(Assembly assembly)
        {
            var di = new CommandDiConfigurator(this);
            di.Configure(assembly);
            return di.Build();
        }

        private void CallCommandMethod(Assembly assembly)
        {
            var di = Configure(assembly);
            var methodCaller = di.GetService<IMethodCaller<PluginResult>>();
            methodCaller.InvokeMethod(di, Constants.ExecuteMethodName);
        }
    }
}