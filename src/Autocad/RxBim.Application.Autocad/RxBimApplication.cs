namespace RxBim.Application.Autocad
{
    using System;
    using System.IO;
    using Autodesk.AutoCAD.AcInfoCenterConn;
    using Autodesk.AutoCAD.ApplicationServices.Core;
    using Autodesk.AutoCAD.Runtime;
    using Autodesk.Internal.InfoCenter;
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
            catch (FileNotFoundException ex) when (ex.FileName.StartsWith("AeccDbMgd"))
            {
                // Приложение для Civil запускается в AutoCAD
                Application.Idle -= ApplicationOnIdle;
                _diConfigurator = null;

                var resultItem = new ResultItem
                {
                    Title = $"Приложение {GetType().Assembly.GetName().Name} работает, только в Civil 3D.",
                    Type = ResultType.Error
                };
                resultItem.ResultClicked += (_, _) => Application.ShowAlertDialog(ex.ToString());

                new InfoCenterManager().PaletteManager.ShowBalloon(resultItem);
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