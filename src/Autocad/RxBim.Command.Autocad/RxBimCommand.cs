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