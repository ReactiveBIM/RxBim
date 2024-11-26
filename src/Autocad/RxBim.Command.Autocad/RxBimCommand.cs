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
        /// <summary>
        /// Executes a command.
        /// </summary>
        public virtual void Execute()
        {
            var assembly = GetType().Assembly;
            var di = Configure(assembly);
            CallCommandMethod(di);
        }

        private IServiceProvider Configure(Assembly assembly)
        {
            var di = new CommandDiConfigurator(this);
            di.Configure(assembly);
            return di.Build();
        }

        private void CallCommandMethod(IServiceProvider serviceProvider)
        {
            var methodCaller = serviceProvider.GetService<IMethodCaller<PluginResult>>();
            methodCaller.InvokeMethod(serviceProvider, Constants.ExecuteMethodName);
        }
    }
}