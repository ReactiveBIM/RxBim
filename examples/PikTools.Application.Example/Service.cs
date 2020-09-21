namespace PikTools.Application.Example
{
    using Autodesk.Revit.UI;

    /// <inheritdoc />
    public class Service : IService
    {
        private readonly UIApplication _app;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="app">uiapp</param>
        public Service(UIApplication app)
        {
            _app = app;
        }

        /// <inheritdoc />
        public void Go()
        {
            TaskDialog.Show(nameof(Service), _app.Application.VersionName);
        }
    }
}