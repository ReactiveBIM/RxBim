namespace PikTools.CommandExample
{
    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI;
    using Serilog;

    /// <summary>
    /// my service
    /// </summary>
    public class MyService : IMyService
    {
        private readonly Document _doc;
        private readonly ILogger _logger;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="doc">doc</param>
        /// <param name="logger">logger</param>
        public MyService(Document doc, ILogger logger)
        {
            _doc = doc;
            _logger = logger;
        }

        /// <summary>
        /// go
        /// </summary>
        public void Go()
        {
            TaskDialog.Show(GetType().FullName, _doc.Title + $" slnsajnsdanlk");
            _logger.Information(nameof(MyService.Go));
        }
    }
}