namespace RxBim.Command.Autocad
{
    using System;
#if NETCOREAPP
    using System.IO;
    using System.Linq;
#endif
    using System.Reflection;
#if NETCOREAPP
    using System.Runtime.Loader;
#endif
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
            var type = GetType();
            var assembly = type.Assembly;
#if NETCOREAPP
            if (PluginContext.IsCurrentContextDefault(type))
            {
                // Attempt to find already exist context. If there is no exist context - create new.
                var appName = Path.GetFileNameWithoutExtension(assembly.Location);
                var existAssemblies = AppDomain.CurrentDomain.GetAssemblies()
                    .Where(a => a.FullName?.Contains(appName) ?? false)
                    .ToList();
                var existContext = existAssemblies.Select(AssemblyLoadContext.GetLoadContext)
                    .FirstOrDefault(c => !AssemblyLoadContext.Default.Equals(c));
                if (existContext is PluginContext context)
                {
                    var instance = context.CreateInstanceInContext(type);
                    if (instance is RxBimCommand command)
                    {
                        command.CallCommandMethod(assembly);
                        return;
                    }
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