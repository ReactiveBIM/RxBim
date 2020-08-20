namespace PikTools.Application.Api
{
    using System;
    using System.Reflection;
    using Autodesk.Revit.UI;
    using Autodesk.Revit.UI.Events;
    using Di;
    using Shared;
    using Result = Shared.Result;

    /// <summary>
    /// Revit application
    /// </summary>
    public abstract class PikToolsApplication : IExternalApplication
    {
        private bool _contextCreated;
        private UIControlledApplication _application;
        private ApplicationDiConfigurator _diConfigurator;

        /// <inheritdoc />
        public Autodesk.Revit.UI.Result OnStartup(UIControlledApplication application)
        {
            _application = application;
            /*AppDomain.CurrentDomain.AssemblyResolve +=
                (sender, args) =>
                {
                    if (args.Name.Contains("Tasks.Extensions"))
                    {
                    }

                    return Assembly.Load(args.RequestingAssembly.GetName());
                };*/
            application.Idling += ApplicationIdling;

            return Autodesk.Revit.UI.Result.Succeeded;
        }

        /// <inheritdoc />
        public Autodesk.Revit.UI.Result OnShutdown(UIControlledApplication application)
        {
            var methodCaller = _diConfigurator.Container.GetInstance<IMethodCaller<PluginResult>>();
            var result = methodCaller.InvokeCommand(_diConfigurator.Container, "Shutdown");
            return result.MapResultToRevitResult();
        }

        private void ApplicationIdling(object sender, IdlingEventArgs e)
        {
            if (sender is UIApplication uiApp && !_contextCreated)
            {
                try
                {
                    _diConfigurator = new ApplicationDiConfigurator(this, uiApp);
                    _diConfigurator.Configure(GetType().Assembly);

                    var methodCaller = _diConfigurator.Container.GetInstance<IMethodCaller<PluginResult>>();
                    methodCaller.InvokeCommand(_diConfigurator.Container, "Start");

                    _contextCreated = true;
                }
                catch (Exception exception)
                {
                    TaskDialog.Show("Error", exception.ToString());
                    throw;
                }
                finally
                {
                    _application.Idling -= ApplicationIdling;
                }
            }
        }
    }
}