namespace PikTools.Application.Example
{
    using Api;
    using Autodesk.Revit.UI;

    /// <summary>
    /// app
    /// </summary>
    public class App : PikToolsApplication
    {
        /// <summary>
        /// start
        /// </summary>
        /// <param name="service">service</param>
        public Result Start(IService service)
        {
            service.Go();
            return Result.Succeeded;
        }

        /// <summary>
        /// shutdown
        /// </summary>
        public Result ShutDown()
        {
            TaskDialog.Show("ddd", "Googby");
            return Result.Succeeded;
        }
    }
}