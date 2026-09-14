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
#if NETCOREAPP
        /// <summary>
        /// Allows you to turn off plugin execution in separated context.
        /// </summary>
        protected virtual bool RunInSeparatedContext => false;

        /// <summary>
        /// Reuses the context for the application's DLL directory until AutoCAD exits.
        /// Applies only when <see cref="RunInSeparatedContext"/> is enabled.
        /// A new command instance and DI container are created for each execution.
        /// </summary>
        protected virtual bool ReuseSeparatedContext => false;
#endif

        /// <summary>
        /// Executes a command.
        /// </summary>
        public virtual void Execute()
        {
            var type = GetType();
            var assembly = type.Assembly;
#if NETCOREAPP
            if (RunInSeparatedContext && !PluginContext.IsCurrentContextRxBim(type))
            {
                if (ReuseSeparatedContext)
                {
                    ExecuteInReusedContext(type);
                    return;
                }

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

#if NETCOREAPP
        private void ExecuteInReusedContext(Type type)
        {
            Action execute;

            try
            {
                var command = PluginContext.CreateInstanceInReusedContext(type);

                // The command's base type may belong to another context; Action is shared by the runtime.
                execute = (Action)Delegate.CreateDelegate(typeof(Action), command, nameof(Execute));
            }
            catch (Exception exception)
            {
                Autodesk.AutoCAD.ApplicationServices.Core.Application.ShowAlertDialog($"Error: {exception}");
                return;
            }

            execute();
        }
#endif

        private IServiceProvider Configure(Assembly assembly)
        {
            var di = new CommandDiConfigurator(this);
            di.Configure(assembly);
            return di.Build();
        }

        private void CallCommandMethod(Assembly assembly)
        {
            var di = Configure(assembly);
            var methodCaller = di.GetRequiredService<IMethodCaller<PluginResult>>();
            methodCaller.InvokeMethod(di, Constants.ExecuteMethodName);
        }
    }
}