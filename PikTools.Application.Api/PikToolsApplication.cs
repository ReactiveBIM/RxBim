namespace PikTools.Application.Api
{
    using System;
    using Autodesk.Revit.UI;
    using Autodesk.Revit.UI.Events;
    using Di;

    /// <summary>
    /// Revit application
    /// </summary>
    public class PikToolsApplication : IExternalApplication
    {
        private bool _contextCreated;
        private UIControlledApplication _application;
        private ApplicationDiConfigurator _diConfigurator;

        /// <inheritdoc />
        public Result OnStartup(UIControlledApplication application)
        {
            _application = application;
            application.Idling += ApplicationIdling;

            return Result.Succeeded;
        }

        /// <inheritdoc />
        public Result OnShutdown(UIControlledApplication application)
        {
            var methodCaller = _diConfigurator.Container.GetInstance<IMethodCaller<Result>>();
            methodCaller.InvokeCommand(_diConfigurator.Container, "Shutdown");
            return Result.Succeeded;
        }

        private void ApplicationIdling(object sender, IdlingEventArgs e)
        {
            if (sender is UIApplication uiApp && !_contextCreated)
            {
                try
                {
                    _diConfigurator = new ApplicationDiConfigurator(this, uiApp);
                    _diConfigurator.Configure(GetType().Assembly);

                    var methodCaller = _diConfigurator.Container.GetInstance<IMethodCaller<Result>>();
                    methodCaller.InvokeCommand(_diConfigurator.Container, "Start");

                    _contextCreated = true;
                    _application.Idling -= ApplicationIdling;
                }
                catch (Exception exception)
                {
                    TaskDialog.Show("Error", exception.ToString());
                }
            }
        }
    }
}