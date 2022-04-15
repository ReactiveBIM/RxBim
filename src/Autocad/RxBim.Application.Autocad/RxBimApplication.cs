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
        private ApplicationDiConfigurator _diConfigurator;

        /// <inheritdoc />
        public void Initialize()
        {
            Application.Idle += ApplicationOnIdle;
            Application.QuitWillStart += ApplicationOnQuitWillStart;
        }

        /// <inheritdoc />
        public void Terminate()
        {
            // ignore
        }

        private void ApplicationOnIdle(object sender, EventArgs e)
        {
            try
            {
                if (_diConfigurator == null)
                {
                    _diConfigurator = new ApplicationDiConfigurator(this);
                    _diConfigurator.Configure(GetType().Assembly);

                    var methodCaller = _diConfigurator.Container.GetService<IMethodCaller<PluginResult>>();
                    methodCaller.InvokeMethod(_diConfigurator.Container, Constants.StartMethodName);
                }
            }
            catch (Exception exception)
            {
                Application.ShowAlertDialog($"Error: {exception}");
            }
            finally
            {
                Application.Idle -= ApplicationOnIdle;
            }
        }

        private void ApplicationOnQuitWillStart(object sender, EventArgs e)
        {
            if (_diConfigurator == null)
            {
                return;
            }

            var methodCaller = _diConfigurator.Container.GetService<IMethodCaller<PluginResult>>();
            methodCaller.InvokeMethod(_diConfigurator.Container, Constants.ShutdownMethodName);
        }
    }
}