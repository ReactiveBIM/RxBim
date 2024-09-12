#pragma warning disable CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).
namespace RxBim.Application.Autocad
{
    using System;
    using Autodesk.AutoCAD.ApplicationServices.Core;
    using Autodesk.AutoCAD.Runtime;
    using Di;
    using Shared;
    using Exception = System.Exception;

    /// <inheritdoc />
    public abstract class RxBimApplication : IExtensionApplication
    {
        private ApplicationDiConfigurator? _diConfigurator;

        /// <inheritdoc />
        public void Initialize()
        {
#if NETCOREAPP
            var type = GetType();
            if (PluginContext.IsCurrentContextDefault(type))
            {
                _ = PluginContext.CreateInstance(type)!;
                return;
            }
#endif

            Application.Idle += ApplicationOnIdle;
            Application.QuitWillStart += ApplicationOnQuitWillStart;
        }

        /// <inheritdoc />
        public void Terminate()
        {
            // ignore
        }

        /// <summary>
        /// If it returns true, the application will run. Otherwise, the application will not run.
        /// </summary>
        protected virtual bool CanBeStarted() => true;

        private void ApplicationOnIdle(object sender, EventArgs e)
        {
            Application.Idle -= ApplicationOnIdle;

            try
            {
                if (_diConfigurator is not null || !CanBeStarted())
                    return;

                _diConfigurator = new ApplicationDiConfigurator(this);
                _diConfigurator.Configure(GetType().Assembly);

                var methodCaller = _diConfigurator.Container.GetService<IMethodCaller<PluginResult>>();
                methodCaller.InvokeMethod(_diConfigurator.Container, Constants.StartMethodName);
            }
            catch (Exception exception)
            {
                Application.ShowAlertDialog($"Error: {exception}");
            }
        }

        private void ApplicationOnQuitWillStart(object sender, EventArgs e)
        {
            Application.QuitWillStart -= ApplicationOnQuitWillStart;

            if (_diConfigurator is null)
                return;

            try
            {
                var methodCaller = _diConfigurator.Container.GetService<IMethodCaller<PluginResult>>();
                methodCaller.InvokeMethod(_diConfigurator.Container, Constants.ShutdownMethodName);
            }
            catch (Exception exception)
            {
                Application.ShowAlertDialog($"Error: {exception}");
            }
        }
    }
}