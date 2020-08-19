namespace PikTools.CommandExample
{
    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI;

    /// <summary>
    /// my service
    /// </summary>
    public class MyService : IMyService
    {
        private readonly Document _doc;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="doc">doc</param>
        public MyService(Document doc)
        {
            _doc = doc;
        }

        /// <summary>
        /// go
        /// </summary>
        public void Go()
        {
            TaskDialog.Show(GetType().FullName, _doc.Title + $" slnsajnsdanlk");
        }
    }
}